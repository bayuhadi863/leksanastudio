# Leksana Studio

Situs studio Leksana — jasa pembuatan website dan sistem web, sekaligus barang
bukti pertamanya sendiri.

| Folder                     | Isi                                                                                            |
| -------------------------- | ---------------------------------------------------------------------------------------------- |
| [`blueprint/`](blueprint/) | Dokumen perencanaan: strategi, positioning, brand, arsitektur informasi, sistem desain, arsitektur teknis, SEO, operasional, roadmap |
| [`frontend/`](frontend/)   | Situs publik + panel pengelolaan — React 19, Vite 7, TypeScript. Lihat [`frontend/README.md`](frontend/README.md) |
| [`backend/`](backend/)     | API + basis data — ASP.NET Core 9, PostgreSQL, Redis, MinIO. Lihat [`backend/README.md`](backend/README.md) |
| [`infra/`](infra/)         | Layanan pendukung: `docker-compose.yml` untuk PostgreSQL, Redis, dan MinIO. Berdiri sendiri karena dipakai bersama backend maupun frontend, dan nanti tempat berkas deployment |

---

## Dua bentuk yang bisa dijalankan

Situs ini bisa berdiri sendiri, dan panel pengelolaannya bisa dipasang ketika
dibutuhkan. Yang menentukan hanya satu variabel saat build:

| `VITE_PANEL` | Yang dibangun | Butuh backend? |
| --- | --- | --- |
| kosong (bawaan) | Situs publik saja. Kode panel **tidak ikut** ke dalam bundel. | Tidak |
| `on` | Situs + panel + layar masuk. | Ya |

Bukan sakelar tampilan: `__PANEL_ENABLED__` diganti jadi literal saat build, jadi
`import()` panelnya lipat sendiri dan potongannya tidak pernah dibuat.

```
tanpa panel   122 modul → 430 kB, satu berkas JS
dengan panel  306 modul → 430 kB + 483 kB panel + 87 kB + 40 kB
```

Yang tayang di Vercel adalah bentuk pertama: situs berdiri sendiri, tanpa API,
tanpa basis data — kontennya berasal dari repositori ini. Formulir kontak tetap
hidup karena ia fungsi serverless (`frontend/api/kontak.ts`), bukan bagian dari
API .NET.

Alasan dan angkanya di [`plans/0005`](plans/0005-rilis-vercel-dan-mode-tanpa-backend.md).

---

## Rilis

Tayang di **https://leksanastudio.vercel.app**

```
git push  →  GitHub  →  ┬─  GitHub Actions   pemeriksaan (typecheck · lint · build ×2 · build backend)
                        └─  Vercel Git       deploy situs publik
```

Proyek Vercel-nya dikonfigurasi dengan **Root Directory = `frontend`**, jadi repo
banyak folder ini tetap bisa dibangun apa adanya. `frontend/vercel.json` membawa
`ignoreCommand`, sehingga perubahan yang hanya menyentuh `backend/`, `infra/`,
atau `plans/` tidak memicu build situs.

Deploy tidak lewat GitHub Actions dengan sengaja — integrasi Git milik Vercel sudah
melakukannya, dan menyimpan `VERCEL_TOKEN` di rahasia repo hanya menambah satu kunci
untuk dijaga tanpa menambah kemampuan.

---

## Menjalankan semuanya

Tiga terminal, urutannya penting — backend menolak start kalau layanannya belum
hidup.

```bash
# 1. layanan pendukung (PostgreSQL, Redis, MinIO)
cd infra
cp .env.example .env            # isi kata sandinya
docker compose up -d

# 2. API                        → http://localhost:5180  (Swagger di /swagger)
cd ../backend/src
cp appsettings.Development.json.example appsettings.Development.json
#   samakan kata sandi dengan infra/.env, isi Jwt:Secret dan
#   Seeder:Users[0].Password
dotnet run

# 3. situs + panel              → http://localhost:3000
cd ../../frontend
npm install
cp .env.example .env             # isi VITE_PANEL="on" supaya panelnya ikut
npm run dev
```

Masuk ke panel lewat `http://localhost:3000/auth/masuk` dengan email dan kata
sandi yang Anda isi di `Seeder:Users`.

---

## Bentuk sistemnya

```
                      ┌──────────────────────────────┐
  pengunjung  ──────► │  Situs publik   (React SPA)  │
                      │  /, /layanan, /portofolio, … │
                      └──────────────┬───────────────┘
                                     │  konten hari ini: MDX + config di repo
                                     │  konten nanti:    dari API
                      ┌──────────────▼───────────────┐
   pengelola  ──────► │  Panel          /panel/…     │
                      │  masuk di       /auth/masuk  │
                      └──────────────┬───────────────┘
                                     │  JWT + X-Role-Active
                      ┌──────────────▼───────────────┐
                      │  API ASP.NET Core            │
                      │  auth · pengguna · peran ·   │
                      │  menu · berkas               │
                      └──────┬───────┬───────┬───────┘
                        Postgres   Redis   MinIO
                        (data)   (sesi)  (berkas)
```

Situs publik masih membaca kontennya dari repositori (MDX untuk studi kasus dan
catatan, TypeScript untuk layanan/harga/vertikal). Itulah yang akan dipindahkan ke
CMS: sumbernya berganti, tampilannya tidak.

---

## Keadaan saat ini

**Sudah jalan**

- Situs publik lengkap: 13 halaman + halaman vertikal, MDX, terang/gelap,
  sitemap dan robots yang dibuat saat build.
- API: autentikasi (login, rotasi refresh token, logout dengan blacklist),
  pengguna, peran, menu, unggah berkas — semuanya di balik izin per-menu.
- Panel: `/auth/masuk`, pemilih peran untuk akun bermultiperan, layout `/panel`
  dengan navigasi yang dibangun dari izin sungguhan, dan dasbor yang menampilkan
  sesi beserta hak aksesnya.
- Kode panel dipisah dari bundel publik, jadi pengunjung tidak ikut mengunduhnya.

**Belum**

- Halaman CRUD pengguna dan peran (endpoint dan izinnya sudah ada).
- Modul CMS: portofolio, catatan, layanan, harga, teks halaman.
- Pra-render untuk SEO — lihat bagian "SEO" di `frontend/README.md`.
- Berkas Docker backend dan konfigurasi reverse proxy produksi.

---

## Catatan tentang blueprint

Berkas di `blueprint/` disalin apa adanya dari perencanaan awal, jadi isinya masih
menyebut implementasi Next.js di folder bernama `code/`. Sekarang implementasinya
ada di `frontend/` dan memakai React + Vite; tautan relatif ke `code/` di dalam
dokumen itu karenanya tidak lagi menunjuk ke mana-mana.

Isi keputusannya sendiri — desain, copy, arsitektur informasi, strategi SEO — tetap
berlaku dan tetap dipatuhi oleh kode di `frontend/`.
