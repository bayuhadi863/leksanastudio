# Leksana Studio — backend

API untuk situs dan panel pengelolaan Leksana Studio. ASP.NET Core 9, PostgreSQL,
Redis, MinIO.

Kerangkanya diangkat dari backend P3M PENS — bagian yang umum saja. Yang khas P3M
(modul penelitian, publikasi, pengajuan, integrasi API kampus) tidak ikut, begitu
pula dua tambalan yang hanya masuk akal di lingkungan PENS: `X-HTTP-Method-Override`
dan badan permintaan ber-base64. Server di sini normal, jadi PUT/PATCH/DELETE dan
JSON biasa dipakai apa adanya.

---

## Menjalankan

```bash
# 1. layanan pendukung — hidup di ../infra, dipakai bersama frontend
cd ../infra
cp .env.example .env          # isi kata sandinya
docker compose up -d

# 2. konfigurasi aplikasi
cd ../backend/src
cp appsettings.Development.json.example appsettings.Development.json
#   → samakan kata sandi Postgres/Redis/MinIO dengan ../../infra/.env
#   → isi Jwt:Secret dengan string acak panjang
#   → isi Seeder:Users[0].Password untuk akun admin pertama

# 3. jalankan
dotnet run
```

Swagger di `http://localhost:5180/swagger`, health check di `/health`.

Migrasi dijalankan otomatis saat start (`Database:AutoMigrate`, default `true`).
Health check juga dijalankan saat start: kalau Postgres atau MinIO mati,
aplikasinya **sengaja gagal start** daripada melayani permintaan yang pasti error.

Port di [`../infra/.env`](../infra/) sengaja bukan port default (5437, 6381, 9010/9011) supaya
stack ini bisa hidup berdampingan dengan proyek lain di mesin yang sama.

Perintah lain:

```bash
dotnet build                                       # kompilasi
dotnet ef migrations add <Nama> --output-dir Migrations
dotnet ef database update
```

---

## Arsitektur

Lapisannya searah — yang di bawah tidak pernah mengimpor yang di atas.

```
src/
  API/                 permukaan HTTP. Tidak ada logika bisnis di sini.
    Attributes/        [JwtAuthorize] [MenuCode] [RequirePermission] [RequireCustomEvent]
    Controllers/       BaseCrudController + Auth, User, Role, Menu, File
    Filters/           JwtAuthorizationFilter, PermissionAuthorizationFilter
    Middlewares/       GlobalExceptionMiddleware
    Validators/        FluentValidation, satu per request DTO

  Application/         orkestrasi kasus penggunaan
    DTOs/              bentuk yang keluar-masuk API
    Interfaces/        kontrak repository, service, seeder
    Seeders/           Role → User → Menu → RoleMenu (urutan lewat ISeeder.Order)
    Services/          AuthService, UserService, RoleService, MenuService,
                       FileService, StorageService

  Common/              yang dipakai lintas modul
    Repositories/      BaseRepository<T> — paging, sorting, transaksi
    Services/          BaseCrudService<...>, BaseImportService, JwtService
    Export/ Import/    Excel & PDF, ClosedXML + QuestPDF
    Models/            BaseRequest, BaseResponse, PaginationResponse, MenuPermission
    Helpers/           PasswordHash (satu-satunya tempat BCrypt dipakai)

  Domain/Entities/     User, UserRefreshToken, Role, UserRole, Menu, RoleMenu
  Extensions/          satu berkas per hal yang dipasang ke DI
  Infrastructure/      EF Core, Redis, MinIO, implementasi repository
  Migrations/          dibuat EF, jangan disunting tangan
```

### Yang membuatnya bisa dipakai ulang

- **Satu modul CRUD = empat berkas.** Entity, repository, service (turunan
  `BaseCrudService`), controller (turunan `BaseCrudController`). Paging, sorting,
  pencarian, soft delete, audit, dan transaksi sudah ada di kelas dasarnya.
- **Registrasi otomatis.** `AddRepositories()`, `AddApplicationServices()`, dan
  `AddSeeders()` memindai assembly lewat Scrutor. Menambah modul tidak menambah
  satu baris pun di `Program.cs`.
- **Soft delete lewat konvensi.** Setiap `IBaseEntity` dapat query filter global
  `!IsDeleted` dan penamaan kolom audit `{tabel}_{properti}` di `AppDbContext` —
  bukan diulang di tiap entity.
- **Satu bentuk respons.** Semua endpoint menjawab `BaseResponse<T>`; semua galat
  lewat `GlobalExceptionMiddleware`. Frontend hanya perlu tahu satu bentuk.

---

## Autentikasi

```
POST /api/v1/auth/login        email + kata sandi → access + refresh token
POST /api/v1/auth/refresh      rotasi refresh token
POST /api/v1/auth/revoke       cabut satu refresh token
POST /api/v1/auth/logout       cabut semua + blacklist access token (Redis)
GET  /api/v1/auth/user-info    identitas pengguna saat ini
GET  /api/v1/auth/my-roles     peran yang dimiliki, untuk pemilih peran
```

Beberapa keputusan yang perlu diketahui sebelum menyentuh kodenya:

- **Tidak ada endpoint registrasi.** Akun pada panel pengelolaan dibuat oleh
  administrator atau seeder, bukan oleh pengunjung. Pendaftaran mandiri di sini
  hanya menambah permukaan serangan tanpa satu pun kegunaan.
- **Satu pesan untuk semua kegagalan login.** Email tidak dikenal dan kata sandi
  salah menjawab hal yang sama persis — membedakannya sama saja menyediakan alat
  untuk mendaftar akun mana yang ada.
- **Masa hidup sesi ganda.** Refresh token punya jendela geser (default 7 hari)
  *dan* batas mutlak (30 hari) yang diwariskan tanpa berubah setiap kali dirotasi.
  Sesi yang aktif tetap hidup, tetapi tidak bisa diperpanjang selamanya.
- **Rotasi sekali pakai.** Refresh token lama langsung dicabut saat dipakai;
  memakainya dua kali menghasilkan 401.
- **Logout benar-benar logout.** Access token yang masih berlaku dimasukkan ke
  daftar hitam Redis sampai kedaluwarsa, jadi tidak ada jeda "masih bisa dipakai
  satu jam lagi".

---

## Izin

Model yang sama seperti di P3M, karena memang inti yang membuat panel bisa
diserahkan ke klien tanpa rasa cemas:

```
User ──< UserRole >── Role ──< RoleMenu >── Menu
                                 │
                                 └─ canView / canCreate / canUpdate / canDelete
                                    canVerify + custom events
```

- Satu **menu** mewakili satu area yang bisa dikelola. Izin menempel padanya.
- Satu **peran** memegang sekumpulan grant per menu.
- Pengguna boleh punya banyak peran, tetapi **hanya satu yang aktif** per
  permintaan — dipilih lewat header `X-Role-Active`. Menggabungkan semua peran
  akan membuat "ganti peran" tidak ada artinya, dan membuat pengguna tidak pernah
  tahu sedang bertindak sebagai siapa.
- Peran aktif yang bukan milik pengguna dijawab **403**, bukan diam-diam diabaikan.

Menegakkannya cukup dua atribut:

```csharp
[MenuCode("user")]                       // di controller
[RequirePermission(PermissionAction.Create)]   // di action
```

`PermissionAuthorizationFilter` terdaftar global, jadi tidak ada endpoint yang
lolos karena lupa dipasangi filter.

Peran dan menu awal ada di `Application/Seeders/`. Seeder **tidak pernah menimpa**
grant yang sudah ada, sehingga izin yang disunting lewat panel selamat dari setiap
deploy ulang.

| Peran         | Akses awal                                        |
| ------------- | ------------------------------------------------- |
| `super-admin` | Dasbor (lihat) · Pengguna, Peran & Akses (penuh)  |
| `editor`      | Dasbor (lihat) · menu konten saat sudah ada       |

Pemisahan itu yang membuat panel aman diserahkan: editor mengurus isi situs, dan
tidak bisa memberi dirinya sendiri akses tambahan.

---

## Konfigurasi

Semua lewat `appsettings.json` + `appsettings.Development.json`, atau variabel
lingkungan dengan pemisah `__` (mis. `Jwt__Secret`). Bagian yang ada:
`PostgresqlSettings`, `Jwt`, `Redis`, `Minio`, `Cors`, `FileUpload`, `Seeder`,
`Database:AutoMigrate`.

`appsettings.Development.json` tidak masuk git dan tidak masuk image Docker.

Di produksi isi `Cors:AllowedOrigins`. Dibiarkan kosong, kebijakannya mengizinkan
semua origin — nyaman untuk lokal, tidak untuk produksi.

---

## Yang belum ada

- Halaman CRUD pengguna dan peran di frontend (endpointnya sudah ada dan sudah
  dijaga izin).
- Modul CMS: portofolio, catatan, layanan, harga, teks halaman.
- Berkas Docker untuk backend + konfigurasi reverse proxy produksi.
