# Studi Kasus 01 — Sistem Informasi P3M PENS

> **Status naskah:** draf siap sunting. Bagian bertanda 🔶 butuh verifikasi/izin
> sebelum dipublikasikan. Semua angka tanpa tanda sudah diverifikasi langsung dari
> repositori pada 2026-08-23.

---

## Metadata

| Field | Nilai |
|---|---|
| **Label** | `Klien` |
| **Klien** | Pusat Penelitian dan Pengabdian kepada Masyarakat (P3M), Politeknik Elektronika Negeri Surabaya — ✅ **izin nama & tangkapan layar diberikan** |
| **Jenis** | Sistem informasi web — situs publik + panel administrasi |
| **Peran** | Pengembang tunggal — analisis, basis data, backend, frontend, infrastruktur, deployment, dokumentasi |
| **Durasi** | ± 6 minggu (Juli – Agustus 2026) |
| **Stack** | React 19 · TypeScript 5.9 · Vite 8 · Tailwind CSS 4 · ASP.NET Core 9 · EF Core 9 · PostgreSQL 16 · Redis 7 · MinIO · Docker · Caddy · Woodpecker CI |
| **Tautan** | 🔶 URL produksi — tanyakan apakah boleh ditautkan publik |

---

## Tiga Angka Utama (kotak besar di atas halaman)

| **30** | **34** | **~73.000** |
|---|---|---|
| modul aplikasi | entitas basis data | baris kode |

**Angka pendukung** (dipakai di badan naskah, bukan di kotak):
27 menu · 37 controller API · 54 migrasi basis data · 4 peran pengguna ·
5 alur pengajuan dengan verifikasi · 13 dokumen teknis + satu buku PDF

---

## Masalah

P3M adalah unit yang mengurus seluruh penelitian, publikasi, dan pengabdian masyarakat
di sebuah politeknik negeri. Sebelum sistem ini ada, dua hal berjalan terpisah dan
keduanya bermasalah.

**Pertama, wajah publiknya.** Capaian penelitian, publikasi, katalog luaran, berita,
panduan kegiatan, dan struktur organisasi tersebar — sebagian di dokumen, sebagian di
halaman yang tidak bisa diperbarui sendiri oleh staf. Setiap perubahan kecil harus
lewat orang lain.

**Kedua, alur internalnya.** Pengajuan dana registrasi konferensi, pengajuan dana
publikasi, pengajuan SK mandiri, klaim buku, dan klaim HKI — lima alur berbeda,
masing-masing dengan berkas pendukung, verifikator, dan status revisi sendiri.
Semuanya berjalan lewat berkas dan pesan pribadi. Tidak ada satu tempat pun yang bisa
menjawab pertanyaan sederhana: *"pengajuan saya sekarang di tahap mana?"*

🔶 **Butuh dari klien:** satu-dua kalimat dari pengelola tentang kondisi sebelum
sistem ini — berapa lama satu pengajuan biasanya berputar, berapa berkas yang
tercecer. Satu kutipan asli di sini bernilai lebih dari tiga paragraf tulisanmu.

---

## Batasan yang Membuat Proyek Ini Tidak Biasa

Bagian ini adalah inti studi kasus. Bukan fiturnya yang sulit — melainkan tempat
sistem ini harus hidup.

### 1. Reverse proxy kampus hanya meneruskan GET, HEAD, dan POST

Sistem harus berjalan di balik proxy institusi yang **membuang PUT, PATCH, dan DELETE.**
Untuk aplikasi REST, ini berarti seluruh operasi ubah dan hapus tidak bisa dilakukan
sama sekali.

Menuntut perubahan konfigurasi proxy kampus bukan pilihan yang realistis — kebijakan
itu berlaku untuk semua aplikasi di lingkungan tersebut, bukan hanya milikmu.

### 2. WAF proxy menolak badan permintaan yang mengandung HTML

Sistem punya editor teks kaya untuk berita, pengumuman, dan halaman publik. Isinya
HTML. Firewall aplikasi di depan proxy membaca HTML dalam badan permintaan sebagai
percobaan injeksi dan menolaknya — jadi menyimpan satu berita saja gagal di produksi,
padahal berhasil sempurna di lokal.

**Kombinasi kedua batasan ini adalah kelas masalah yang tidak pernah muncul di tutorial:**
kode benar, tes lolos, lokal jalan, produksi mati.

### 3. Autentikasi harus menumpang SSO kampus, tanpa menggantikannya

Dosen dan karyawan sudah punya akun institusi. Membuat mereka mendaftar ulang akan
membunuh adopsi. Tapi sistem juga butuh pengguna lokal (admin, anggota P3M) yang
tidak ada di direktori kampus.

### 4. Hak akses harus bisa diubah tanpa deploy ulang

Struktur organisasi unit seperti ini berubah. Kalau setiap perubahan hak akses berarti
mengubah kode dan menunggu deploy, sistemnya akan ditinggalkan dalam setahun.

### 5. Dikerjakan sendiri, dari basis data sampai TLS

Tidak ada tim backend, tidak ada DevOps, tidak ada QA. Setiap keputusan arsitektur
harus dinilai juga dari sisi: *"apakah saya sanggup merawat ini sendirian setahun lagi?"*

---

## Keputusan

### Keputusan 1 — Menyelundupkan verb yang diblokir lewat POST, bukan mengubah desain API

Saya memilih **mempertahankan REST yang benar dan menambahkan lapisan penerjemah
di kedua ujung**, walaupun itu berarti dua mekanisme yang harus selalu diubah
berpasangan.

Frontend mengirim PUT/PATCH/DELETE sebagai POST dengan header `X-HTTP-Method-Override`;
backend memasang `UseHttpMethodOverride()` sebelum routing, sehingga controller tetap
melihat verb aslinya.

**Alternatif yang ditolak:** mengubah semua endpoint jadi POST (`/user/update`,
`/user/delete`). Lebih sederhana untuk dikirim, tapi merusak semantik API selamanya
demi satu kendala lingkungan yang mungkin hilang tahun depan. Lapisan penerjemah bisa
dicabut dalam satu jam; API yang sudah terlanjur salah bentuk tidak bisa.

**Harganya, dan saya menuliskannya di dokumentasi:** mengubah satu sisi tanpa sisi
lain akan mematikan **seluruh** operasi tulis di produksi — dan gejalanya tidak akan
muncul saat pengembangan lokal.

### Keputusan 2 — Meng-encode badan permintaan sebagai base64 saat mengandung HTML

Saya memilih **base64 di seluruh badan JSON** dengan penanda header `X-Body-Encoding`,
walaupun itu menambah ~33% ukuran permintaan dan membuat badan permintaan tidak
terbaca di alat inspeksi.

**Alternatif yang ditolak:** meminta pengecualian WAF untuk endpoint tertentu
(bergantung pada pihak lain, dan pengecualian WAF adalah utang keamanan), atau
menyimpan konten sebagai teks polos (menghapus fitur editor kaya yang justru
menjadi alasan sistem ini menggantikan proses lama).

### Keputusan 3 — Hak akses sebagai data, bukan sebagai kode

Izin disimpan per pasangan peran × menu, dengan lima bendera (`lihat`, `tambah`, `ubah`,
`hapus`, `verifikasi`) ditambah daftar *custom event* seperti `verify` dan `delete-any`.
Backend menegakkannya lewat filter otorisasi global; frontend membaca izin yang sama
hanya untuk menyembunyikan tombol.

**Konsekuensi yang saya inginkan:** administrator mengubah hak akses lewat antarmuka.
Tidak ada deploy, tidak ada saya.

**Alternatif yang ditolak:** atribut peran di controller (`[Authorize(Roles="admin")]`).
Jauh lebih cepat ditulis, dan setiap perubahan organisasi berubah jadi tiket untuk saya.

**Detail yang saya anggap penting:** ketika pengguna punya beberapa peran, akses
dilingkupi ke **satu peran aktif** yang dipilih saat login, bukan gabungan semuanya.
Gabungan peran membuat pengguna tidak pernah bisa menjawab pertanyaan "kenapa saya
bisa lihat ini?", dan membuat audit hampir mustahil.

### Keputusan 4 — Satu aplikasi React untuk situs publik dan panel admin

Situs publik di `/`, panel di `/app`, satu basis kode. Komponen yang merender halaman
publik **persis sama** dengan yang merender pratinjau di editor halaman.

**Yang didapat:** pratinjau yang benar-benar akurat. Admin melihat hasil final saat
menyunting, bukan perkiraan.

**Harganya:** bundel awal lebih besar. Ditangani dengan `lazy()` per modul, sehingga
pengunjung publik tidak pernah mengunduh kode panel administrasi.

### Keputusan 5 — Kelas dasar mengerjakan pekerjaan berulang, bukan generator kode

Modul CRUD baru mewarisi `BaseCrudController` + `BaseCrudService` + `BaseRepository`;
modul pengajuan mewarisi turunan yang sudah membawa alur verifikasi. Registrasi
dependensi dipindai otomatis, jadi modul baru tidak butuh satu baris pun konfigurasi
manual.

**Kenapa ini keputusan bisnis, bukan cuma keputusan gaya:** dengan 27 menu dan lima
alur pengajuan yang mirip, selisih antara "menyalin 400 baris per modul" dan
"mewarisi lalu menimpa 3 metode" adalah selisih antara proyek yang selesai dalam
enam minggu dan proyek yang tidak selesai.

### Keputusan 6 — Berkas tidak pernah disajikan lewat URL penyimpanan

Semua berkas (PDF pengajuan, gambar, katalog) tersimpan di MinIO dan hanya bisa
diambil lewat endpoint yang mengalirkan byte-nya. URL penyimpanan tidak pernah sampai
ke browser.

**Kenapa:** URL penyimpanan yang bocor berarti dokumen pengajuan bisa diakses siapa
pun yang punya tautannya, selamanya. Untuk berkas berisi data pribadi dosen,
itu bukan risiko yang bisa diterima.

---

## Hasil

**Yang berjalan hari ini:**

- Situs publik lengkap — profil, capaian penelitian & publikasi, katalog, berita,
  pengumuman, panduan & kalender kegiatan, research group, layanan, struktur anggota,
  kerja sama industri — seluruhnya dapat disunting sendiri oleh staf, termasuk susunan
  halaman depannya.
- Panel administrasi dengan 27 menu dan 4 peran.
- Lima alur pengajuan (dana registrasi, dana publikasi, SK mandiri, klaim buku,
  klaim HKI) dengan status, catatan verifikator, revisi, dan lampiran berkas.
- Login lewat akun kampus untuk dosen & karyawan; akun lokal untuk pengelola.
- Ekspor tabel ke PDF/Excel/CSV dan impor massal dari Excel untuk data penelitian,
  publikasi, dan pengabdian.
- Migrasi dan pengisian data awal berjalan otomatis saat aplikasi dinyalakan —
  memasang ulang dari nol tidak butuh langkah manual.
- Deployment ter-otomasi lewat CI mandiri, TLS otomatis, dan pemeriksaan kesehatan
  untuk basis data, cache, dan penyimpanan berkas.

**Serah terima:**
13 dokumen teknis berbahasa Indonesia — arsitektur, panduan memulai, referensi API,
model data, keamanan, deployment, operasional, konvensi kode, pemecahan masalah,
glosarium — plus versi buku PDF tunggal. Termasuk tutorial "cara menambah modul baru"
untuk backend dan frontend, sehingga pengembang berikutnya tidak perlu bertanya
kepada saya.

### Kebijakan Skor Lighthouse — DITETAPKAN 2026-08-23

Sumber tunggal yang dipakai: **PageSpeed Insights**, `https://p3m.pens.ac.id/`,
diukur 2026-08-23.

| Kategori | Desktop | Mobile | Yang ditampilkan |
|---|---|---|---|
| Performance | **94** 🟢 | 68 🟠 | ✅ Desktop |
| Accessibility | **91** 🟢 | 89 🟠 | ✅ Desktop |
| Best Practices | **100** 🟢 | **100** 🟢 | ✅ Keduanya |
| SEO | **100** 🟢 | **100** 🟢 | ✅ Keduanya |

**Keputusan: tampilkan blok skor desktop, keempat kategori, berlabel lengkap.**
Keempatnya berada di pita hijau (90-100). Ini bukti kuat dan sepenuhnya jujur —
selama labelnya ada.

**Format wajib, jangan disingkat:**

> **PageSpeed Insights — Desktop, 23 Agustus 2026**
> Performa **94** · Aksesibilitas **91** · Praktik Terbaik **100** · SEO **100**
> Diukur pada lingkungan hosting institusi yang tidak saya kendalikan.

Baris ketiga bukan basa-basi. Ia menjelaskan kenapa angka ini bagus *meskipun* server,
proxy, dan CDN bukan pilihanmu — dan ia melindungimu kalau skornya berubah nanti
karena hal di luar kendalimu.

### Tiga aturan yang mengikat

**1. Kata "Desktop" tidak boleh hilang.** Standar industri membaca skor tanpa label
sebagai mobile. Menampilkan 94 tanpa menyebut desktop = menyesatkan. Dengan label =
jujur dan lengkap.

**2. Jangan pernah menulis klaim performa mobile** dengan studi kasus ini sebagai
buktinya. Dilarang: "cepat di semua perangkat", "Lighthouse hijau", "hijau di mobile
dan desktop". Prospek teknis akan menjalankan PSI sendiri dan menemukan 68.

**3. Angka mobile tidak dicantumkan, tapi jawabannya disiapkan.** Studi kasus adalah
dokumen penjualan, bukan laporan audit — tidak mencantumkan angka yang tidak ditanya
bukan ketidakjujuran. Tapi kalau ditanya, jawab langsung tanpa berkelit:

> Mobile-nya 68. Penyebabnya aplikasi SPA di server institusi tanpa CDN, ditambah
> throttling jaringan 4G yang dipakai PSI untuk pengujian mobile. Gambar hero dan
> pemecahan bundel adalah dua hal pertama yang akan saya kerjakan kalau ada
> kesempatan menyentuhnya lagi.

Jawaban ini justru menguatkan posisi — ia menunjukkan kamu tahu persis kenapa,
dan tahu apa yang akan dilakukan.

### Skor DevTools lokal dibuang — dengan alasan yang tercatat

| | Lokal (DevTools) | PSI | Selisih |
|---|---|---|---|
| Performance desktop | 78 | **94** | −16 |
| Best Practices | 77 | **100** | −23 |

Panel Lighthouse sendiri menyebutkan penyebabnya:
*"There may be stored data affecting loading performance in this location: IndexedDB.
Audit this page in an incognito window."* Aplikasi ini memang menyimpan state di
peramban, jadi run lokal di profil biasa selalu tercemar.

**Aturan umum yang berlaku untuk semua proyek berikutnya:** angka yang dipublikasikan
selalu berasal dari PSI, tidak pernah dari DevTools. Kalau butuh uji lokal, jalankan
di jendela penyamaran dengan ekstensi dimatikan — dan tetap jangan dipublikasikan.

### Peluang mobile 68 → 90

Prioritas **rendah** sekarang, karena angka desktop sudah membawa bukti yang
dibutuhkan. Kerjakan hanya kalau aksesmu masih ada dan Fase A-D sudah jalan.
Tersangka utama: gambar hero belum dioptimasi, bundel SPA belum dipecah untuk rute
publik, tidak ada CDN. Kalau berhasil, hasilnya masuk blok Hasil sebagai
**"mobile 68 → 9x"** dan jadi bahan artikel Pilar 1 — dibingkai sebagai batasan
lingkungan, **bukan** menyalahkan klien.

🔶 **Yang masih perlu dilengkapi sebelum publikasi:**
- Tangkapan layar hasil PSI desktop (arsipkan — skor berubah seiring waktu)
- Jumlah pengguna aktif / jumlah pengajuan yang sudah diproses, kalau boleh disebut
- Satu kutipan dari pengelola P3M
- 4–6 tangkapan layar: halaman depan publik, editor landing page, daftar pengajuan,
  dialog verifikasi, pengaturan hak akses peran, dashboard
- Satu diagram arsitektur (sudah ada di `docs/02-arsitektur-sistem.md` — tinggal
  disederhanakan untuk audiens non-teknis)
- Video 45 detik menelusuri satu alur pengajuan dari kirim sampai terverifikasi

---

## Yang Saya Lakukan Berbeda Kalau Mengulang

**1. Menguji di balik proxy sejak minggu pertama, bukan menjelang deployment.**
Dua batasan proxy baru terlihat setelah sebagian besar sistem selesai. Keduanya bisa
ditemukan dalam satu jam dengan satu permintaan uji ke lingkungan target di hari
pertama. Sekarang setiap proyek yang akan tinggal di infrastruktur orang lain saya
mulai dengan satu deploy kosong ke sana — sebelum fitur pertama ditulis.

**2. Menulis tes untuk lapisan izin lebih awal.**
Proyek berjalan dengan proyek tes yang masih kerangka. Untuk sistem dengan izin
per-aksi berbasis data, di situlah tes paling berharga — bukan di CRUD.

**3. Menyepakati aturan penamaan sebelum migrasi pertama.**
Satu nama folder salah eja ikut tertanam di pipeline CI, dua Dockerfile, dan seluruh
skrip deploy. Sekarang biaya memperbaikinya lebih besar daripada membiarkannya.
Pelajaran yang murah, tapi hanya kalau dipelajari sekali.

---

## Versi Anonim — Tidak Terpakai (Arsip)

> Izin nama sudah diberikan, jadi versi ini **tidak dipakai.** Disimpan sebagai
> cadangan kalau suatu saat klien menarik izinnya, dan sebagai pola untuk klien
> berikutnya yang tidak memberi izin.

Ganti bagian metadata dan paragraf pembuka:

> **Klien:** unit penelitian dan pengabdian masyarakat di sebuah politeknik negeri
> di Jawa Timur.
>
> Sistem menggabungkan situs publik lembaga dengan panel administrasi internal yang
> menangani lima alur pengajuan dana dan surat keputusan.

Semua blok **Batasan**, **Keputusan**, dan **Yang saya lakukan berbeda** dipakai utuh —
tidak ada satu pun yang membocorkan identitas klien, dan justru blok-blok itulah yang
menjual.

Tangkapan layar: ganti logo dan nama institusi dengan penanda netral, dan ganti nama
orang pada data contoh dengan nama fiktif. Jangan diblur.

---

## Cara Naskah Ini Dipakai Ulang

| Tempat | Bentuk |
|---|---|
| `/portofolio/p3m-pens` | Naskah penuh, ini |
| Homepage | Kartu bukti + tiga angka |
| `/layanan/sistem-informasi` | Blok Batasan + Keputusan 3, tersemat |
| LinkedIn | Satu post per keputusan → 6 post. Naskahnya sudah setengah jadi di [`p3m-pens/code/linkedin-plan/`](../../../p3m-pens/code/linkedin-plan/) |
| Blog | "Ketika reverse proxy hanya mengizinkan GET dan POST" — artikel teknis yang menarik trafik pencarian jangka panjang |
| Penawaran Lini C | Lampiran satu halaman sebagai bukti kapabilitas |
