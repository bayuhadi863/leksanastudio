# 11 — Roadmap Eksekusi

## 11.1 Urutan Besar & Alasannya

```
FASE A          FASE B              FASE C            FASE D
Brand +         Situs studio v0     Demo klinik       Situs studio v1
studi kasus     (bisa menjual)      gigi              + mesin konten
7 hari          10-14 hari          19 hari           7 hari
   │                 │                  │                  │
   ▼                 ▼                  ▼                  ▼
Aset yang       Tautan yang       Produk yang      Mesin yang
sudah ada       bisa dikirim      bisa dijual      mendatangkan
jadi terlihat   ke prospek        berulang         prospek sendiri
```

### Kenapa situs studio sebelum demo klinik gigi

Blueprint produk klinik gigi menyarankan "kalau waktu terbatas, bangun demo dulu".
**Saran itu berlaku saat kamu belum punya bukti apa pun.** Keadaanmu berbeda:

| | Kalau demo dulu | Kalau situs studio dulu ⭐ |
|---|---|---|
| Waktu sampai bisa menjual apa pun | ~19 hari | **~3 minggu, tapi bisa menjual Lini B & C sejak hari ke-21** |
| Aset P3M PENS | Tetap tidak terlihat selama 3 minggu | **Langsung menghasilkan** |
| Yang bisa dijual di akhir fase | Hanya klinik gigi | Website custom, sistem informasi, **dan** persiapan klinik gigi |
| Risiko kalau berhenti di tengah | Demo setengah jadi = nol | Situs 5 halaman tetap berfungsi |
| Konten medsos | Menunggu demo jadi | **Bisa mulai minggu ke-2** dari bahan P3M |

**Yang menentukan:** kamu sudah punya satu proyek klien nyata yang selesai. Aset itu
sedang tidak menghasilkan apa-apa. Mengubahnya jadi studi kasus yang bisa dikirim
lewat tautan memakan beberapa hari — dan langsung membuat setiap percakapan penjualan
berikutnya kredibel, termasuk percakapan tentang klinik gigi.

**Fase C tetap wajib dan tidak boleh ditunda tanpa batas.** Tanpa demo, Lini A
(mesin arus kas) tidak jalan.

---

## FASE A — Brand & Studi Kasus (7 hari)

**Tujuan:** identitas ditetapkan, dan aset terbesar yang sudah ada berubah jadi naskah.

> **Sudah selesai per 2026-08-23:** nama studio ditetapkan (**Leksana Studio**) dan
> izin portofolio P3M PENS diberikan. Fase ini menyusut jadi **~5 hari.**

| Hari | Pekerjaan | Rujukan |
|---|---|---|
| 1 | Jalankan checklist verifikasi untuk *Leksana*: PDKI kelas 42 & 35, domain, handle, uji telepon | [03-identitas-brand.md](03-identitas-brand.md) §3.4 |
| 1 | Beli domain (`leksana.id` → `leksanastudio.com` → `leksana.studio`). Daftarkan email domain, WhatsApp Business, handle medsos | §3.9 |
| 2 | Wordmark logo, favicon, foto profil, banner. **Maksimal satu hari** | §3.8 |
| 3-4 | Tulis studi kasus P3M PENS sampai tuntas | [portofolio/01-p3m-pens.md](portofolio/01-p3m-pens.md) |
| 4 | Kumpulkan visual: 6 tangkapan layar, 1 diagram arsitektur, 1 video 45 detik, **arsip tangkapan layar PSI desktop** | §5.4 |
| 5 | Siapkan template kontrak, invoice, dan penawaran | [10-operasional-legal.md](10-operasional-legal.md) |

**Selesai kalau:** domain aktif, logo ada, naskah studi kasus lengkap dengan visual,
dan template kontrak siap pakai.

**Kalau verifikasi hari 1 menggugurkan *Leksana*** (merek bentrok di kelas 42 —
kemungkinan kecil): ambil kandidat cadangan di [§3.3](03-identitas-brand.md) —
*Rangka* atau *Simpul*. **Beri batas satu hari, jangan lebih.** Nama yang cukup baik
dan sudah dipakai mengalahkan nama sempurna yang belum ada.

---

## FASE B — Situs Studio v0 (10-14 hari)

**Tujuan:** satu tautan yang bisa dikirim ke prospek mana pun, dan yang berdiri sendiri
sebagai bukti kualitas.

### Ruang lingkup v0 — lima halaman, tidak lebih

```
/                       Homepage
/portofolio             Indeks (1 kartu untuk sekarang)
/portofolio/p3m-pens    Studi kasus penuh
/layanan                Ringkasan 3 lini + yang tidak dikerjakan
/kontak                 Form 3 field + WhatsApp
```

`/harga`, `/proses`, `/tentang`, `/blog` — **ditunda ke v1.** Isi ringkasnya dulu
sebagai bagian dari homepage.

| Hari | Pekerjaan | Rujukan |
|---|---|---|
| 1-2 | Setup proyek, token desain, `config/site.ts`, layout, header/footer | [07-arsitektur-teknis.md](07-arsitektur-teknis.md) §7.4 |
| 3-5 | Homepage — semua 9 blok | [04-arsitektur-informasi-copy.md](04-arsitektur-informasi-copy.md) §4.2 |
| 6-7 | Pipeline MDX + halaman studi kasus + komponen metrik/diagram | §7.2 |
| 8 | `/portofolio`, `/layanan` | §4.3 |
| 9 | `/kontak` + route handler + Resend + notifikasi WhatsApp + event GA4 | §7.8 |
| 10 | SEO teknis: sitemap, robots, metadata, JSON-LD, OG image | §7.7 |
| 11 | Performa: kejar Lighthouse 100 di keempat kategori | §7.6 |
| 12 | QA: HP nyata, mode gelap, semua tautan, form ujung ke ujung | §7.10 |
| 13 | **Live.** Search Console, sitemap disubmit | |
| 14 | Cadangan | |

**Selesai kalau:** [checklist §7.10](07-arsitektur-teknis.md) lolos seluruhnya,
form terkirim dan notifikasi WhatsApp masuk ke HP-mu, dan Lighthouse 100/100/100/100
di HP dengan tangkapan layar tersimpan.

**Aturan keras:** kalau hari ke-14 tiba dan situs belum live, **luncurkan apa adanya**
selama checklist §7.10 lolos. Halaman yang belum ada tidak merugikan siapa pun.
Situs yang belum live merugikan setiap hari.

### Yang langsung dikerjakan setelah live (paralel dengan Fase C)

- Perbarui profil LinkedIn dengan tautan situs
- Terbitkan post LinkedIn pertama dari [`linkedin-plan/`](../../p3m-pens/code/linkedin-plan/), tautkan ke studi kasus
- Kirim tautan ke 10 orang di jaringanmu dengan pesan minta masukan — bukan minta proyek.
  Rujukan sering lahir dari sini

---

## FASE C — Demo Klinik Gigi (19 hari)

Blueprint dan roadmap detail sudah lengkap di
[`../02-produk-klinik-gigi/09-roadmap.md`](../02-produk-klinik-gigi/09-roadmap.md).
Ikuti apa adanya — jangan disusun ulang.

**Tiga penyesuaian karena situs studio sudah ada:**

1. **Pinjam token & komponen dari situs studio** untuk hal generik (tombol, tipografi,
   utilitas layout). Jangan pinjam **arah visualnya** — demo klinik harus terasa
   seperti klinik, bukan seperti studio. Lihat [06-sistem-desain.md](06-sistem-desain.md) §6.1.
2. **Demo dipasang di subdomain** `demo.[domain-studio]`, bukan domain terpisah —
   supaya menguatkan otoritas domain utamamu. [08-seo-mesin-konten.md](08-seo-mesin-konten.md) §8.5.
3. **Tulis studi kasusnya sambil membangun**, bukan setelahnya. Blok "keputusan"
   paling akurat ditulis saat keputusannya baru diambil.
   Kerangka: [portofolio/02-demo-klinik-gigi.md](portofolio/02-demo-klinik-gigi.md).

**Selesai kalau:** definisi selesai di roadmap produk terpenuhi, **dan** demo tertaut
dari situs studio, **dan** naskah studi kasusnya sudah jadi.

---

## FASE D — Situs Studio v1 (7 hari)

**Tujuan:** situs berhenti sekadar menjadi brosur dan mulai mendatangkan prospek sendiri.

| Hari | Pekerjaan | Rujukan |
|---|---|---|
| 1 | `/harga` — ketiga lini, termin, FAQ harga | [02-positioning-penawaran.md](02-positioning-penawaran.md) |
| 1 | `/proses` — empat langkah + tiga ketakutan yang dijawab | [04-arsitektur-informasi-copy.md](04-arsitektur-informasi-copy.md) §4.5 |
| 2 | `/tentang` | §4.6 |
| 2 | Studi kasus ke-2 (demo klinik gigi) terbit | |
| 3-4 | Template halaman vertikal + 4 halaman gelombang 1 | [08-seo-mesin-konten.md](08-seo-mesin-konten.md) §8.3 |
| 5 | Blog: pipeline + 2 artikel pertama terbit | §8.4 |
| 6 | Halaman layanan per lini (3 halaman) | §4.3 |
| 7 | QA ulang, Lighthouse ulang, submit ulang sitemap | |

**Selesai kalau:** 4 halaman vertikal live, 2 artikel terbit, 2 studi kasus lengkap,
seluruh halaman di sitemap [§4.1](04-arsitektur-informasi-copy.md) ada kecuali blog
yang masih bertumbuh.

---

## 11.2 Berkelanjutan (Setelah Fase D)

| Ritme | Kegiatan |
|---|---|
| **Harian** | Balas lead < 2 jam. Ini mengalahkan semua kegiatan lain di daftar ini |
| **Mingguan** | 3 post LinkedIn · 3 konten IG · 1 postingan Google Business · perbarui papan lead |
| **2 mingguan** | 1 artikel blog |
| **Bulanan** | Periksa Search Console · tambah 1 halaman vertikal · tinjau alasan kalah |
| **Kuartalan** | Tinjau harga · perbarui studi kasus dengan hasil terbaru · putuskan ekspansi (satu sumbu saja) |

---

## 11.3 Aturan Anti-Molor

| Aturan | Penerapan |
|---|---|
| **Batas waktu keras per fase** | Fase yang lewat tenggat diluncurkan apa adanya, bukan diperpanjang |
| **Satu hari untuk memilih nama** | Nama cukup baik yang sudah dipakai > nama sempurna yang belum ada |
| **Tidak ada halaman baru sebelum yang lama live** | Ruang lingkup v0 dikunci di lima halaman |
| **Tidak ada ganti stack di tengah jalan** | Keputusan teknis dikunci di Fase B hari ke-1 |
| **Tidak ada desain ulang sebelum ada 10 lead** | Kamu belum punya data untuk tahu apa yang salah |
| **Kirim di 85%** | Yang tersisa 15% hampir selalu hal yang tidak diperhatikan siapa pun |

### Tanda kamu sedang menghindar dari pekerjaan yang sebenarnya

Semua ini terasa produktif dan tidak satu pun mendekatkanmu ke klien pertama:

- Mengganti palet warna untuk ketiga kalinya
- Meriset framework animasi
- Membuat brand guideline PDF
- Menambah halaman keenam sebelum lima yang pertama live
- Menyempurnakan naskah studi kasus yang sudah cukup baik
- Membaca ulang blueprint ini alih-alih mengerjakan langkah berikutnya

**Pekerjaan yang sebenarnya:** meluncurkan, mengirim tautan ke orang, membalas cepat,
dan menulis penawaran.

---

## 11.4 Definisi Selesai — Tingkat Usaha

| # | Tonggak | Tanda |
|---|---|---|
| 1 | **Terlihat** | Situs live, 1 studi kasus, tautan bisa dikirim |
| 2 | **Kredibel** | 2 studi kasus, halaman harga & proses, Lighthouse 100 |
| 3 | **Terdistribusi** | 10 post LinkedIn, 2 artikel, 4 halaman vertikal live |
| 4 | **Tervalidasi** | 5 percakapan penjualan nyata, ≥1 penawaran tertulis terkirim |
| 5 | **Menghasilkan** | Klien berbayar pertama, DP masuk |
| 6 | **Berulang** | Klien ke-3, waktu pengerjaan turun ≥40% dari yang pertama |
| 7 | **Stabil** | ≥2 retainer bulanan, ≥1 klien datang dari rujukan |

Perhatikan tonggak 1 dan 2: **proyek ini dirancang supaya tidak bisa gagal total.**
Kalau semua prospek menolak, kamu tetap memiliki brand, situs, produk, dan mesin
konten — aset yang tidak hilang dan makin bernilai.
