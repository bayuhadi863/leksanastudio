# Blueprint Studio — Usaha & Website Sendiri

> **Yang dibangun:** brand studio dan website studio — aset yang menjual seluruh
> jasa, menampung portofolio, dan menangkap prospek.
>
> **Yang dijual di sana:** jasa pembuatan website & sistem web. Bukan website-nya.

---

## Posisi Dokumen Ini

Blueprint ini mengurus **usaha**, bukan salah satu produk.
Blueprint per lini produk hidup di foldernya sendiri:

| Folder | Isi |
|---|---|
| `01-studio/` (di sini) | Brand, positioning, harga, situs agensi, lead, operasional, legal |
| [`../02-produk-klinik-gigi/`](../02-produk-klinik-gigi/) | Demo induk klinik gigi + playbook personalisasi + outreach niche |

---

## Peta Dokumen

| File | Isi | Kapan dibaca |
|---|---|---|
| [01-strategi-usaha.md](01-strategi-usaha.md) | Model bisnis, tiga lini pendapatan, ekonomi unit, jebakan generalis & cara keluar, ekspansi, risiko | **Pertama** |
| [02-positioning-penawaran.md](02-positioning-penawaran.md) | Kalimat positioning, paket & harga tingkat studio, batas layanan, kualifikasi prospek | Kedua |
| [03-identitas-brand.md](03-identitas-brand.md) | Kandidat nama, checklist verifikasi merek & domain, tagline, tone of voice, aset brand minimum | Sebelum apa pun dipublikasikan |
| [04-arsitektur-informasi-copy.md](04-arsitektur-informasi-copy.md) | Sitemap, wireframe tiap halaman, bank copywriting siap pakai | Saat membangun |
| [05-portofolio-studi-kasus.md](05-portofolio-studi-kasus.md) | Anatomi studi kasus yang menjual, aturan angka & izin klien, template | Saat membangun |
| [06-sistem-desain.md](06-sistem-desain.md) | **Arah desain:** tesis visual, arah yang ditolak & alasannya, anggaran keberanian | Sebelum membangun |
| [06b-design-system.md](06b-design-system.md) | **Spesifikasi:** palet, tipografi, grid, elemen tanda tangan, seluruh komponen + state, suara antarmuka, gerak, aksesibilitas, kode Tailwind | Saat membangun |
| [07-arsitektur-teknis.md](07-arsitektur-teknis.md) | Stack, model konten, anggaran performa, form, analitik, deployment | Saat membangun |
| [08-seo-mesin-konten.md](08-seo-mesin-konten.md) | Peta kata kunci untuk studio generalis, halaman vertikal, blog, distribusi LinkedIn/IG | Setelah situs live |
| [09-mesin-lead-closing.md](09-mesin-lead-closing.md) | Corong, CTA, form, alur WA, skrip discovery call, proposal, follow-up | Eksekusi |
| [10-operasional-legal.md](10-operasional-legal.md) | Kontrak, ruang lingkup, termin, kepemilikan kode, NDA, badan usaha, invoice, serah terima | **Sebelum klien pertama** |
| [11-roadmap-eksekusi.md](11-roadmap-eksekusi.md) | Urutan pembangunan, anggaran waktu, definisi selesai | Eksekusi |
| **[code/](code/)** | **Implementasi situs studio** — Next.js 15 + Tailwind 4, 27 halaman statis. Lihat [code/README.md](code/README.md) | Saat membangun |
| [portofolio/01-p3m-pens.md](portofolio/01-p3m-pens.md) | Naskah studi kasus proyek klien nyata | Saat menulis konten |
| [portofolio/02-demo-klinik-gigi.md](portofolio/02-demo-klinik-gigi.md) | Naskah studi kasus demo produk | Setelah demo dibangun |

---

## Aturan Emas Situs Studio

1. **Situs studio adalah barang bukti pertama.** Kalau situsmu lambat, tidak rapi di HP,
   atau copy-nya berantakan, tidak ada satu pun klaim di dalamnya yang dipercaya.
   Target: Lighthouse 100/100/100/100 di halaman utama. Tidak ada tawar-menawar.
2. **Bukti mendahului klaim.** Setiap klaim kemampuan harus punya tautan ke bukti
   di halaman yang sama — studi kasus, demo hidup, atau angka yang bisa dicek.
3. **Jangan pernah menulis "kami" kalau baru satu orang.** Bahasa jamak yang palsu
   ketahuan dan merusak kepercayaan. Lihat [03-identitas-brand.md](03-identitas-brand.md) §3.6.
4. **Satu halaman = satu keputusan.** Setiap halaman punya satu tindakan berikutnya
   yang jelas. Halaman tanpa CTA adalah halaman yang membuang prospek.
5. **Tampilkan harga.** Menyembunyikan harga menyaring prospek serius, bukan prospek murah.
   Rentang harga menyaring prospek murah. Lihat [02-positioning-penawaran.md](02-positioning-penawaran.md) §2.5.
6. **Portofolio berbicara tentang masalah klien, bukan tentang teknologi.**
   Nama framework masuk di bagian bawah, bukan judul.
7. **Situs studio tidak boleh terlihat seperti situs klien.** Kalau demo klinik gigi
   dan situs studio terasa dari cetakan yang sama, kemampuanmu terlihat sempit.

---

## Definisi Sukses Bertingkat

| Tingkat | Kriteria | Kenapa penting |
|---|---|---|
| **Ada** | Domain aktif, 5 halaman, 1 studi kasus nyata, form & WA jalan | Bisa dikirim lewat tautan. Tanpa ini, outreach tidak punya tujuan |
| **Kredibel** | 2 studi kasus, halaman proses, halaman harga, Lighthouse 100 | Prospek bisa menilai sendiri tanpa bertanya |
| **Ditemukan** | Ranking halaman 1 untuk ≥3 kata kunci lokal/vertikal | Prospek datang tanpa outreach |
| **Menghasilkan** | ≥1 lead masuk per minggu dari situs, bukan dari outreach manual | Corong mulai bekerja sendiri |
| **Berulang** | ≥2 klien retainer, ≥1 klien datang karena rujukan | Pendapatan tidak bergantung pada perburuan baru |
