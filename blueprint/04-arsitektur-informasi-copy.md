# 04 — Arsitektur Informasi & Bank Copywriting

## 4.1 Sitemap

```
/                                   Homepage
│
├── /layanan                        Ringkasan 3 lini + yang tidak dikerjakan
│   ├── /layanan/website-bisnis         Lini A — untuk UKM & bisnis jasa
│   ├── /layanan/website-perusahaan     Lini B — company profile & institusi
│   └── /layanan/sistem-informasi       Lini C — aplikasi web multi-peran
│
├── /portofolio                     Daftar semua proyek
│   ├── /portofolio/p3m-pens            Studi kasus — klien nyata
│   └── /portofolio/klinik-gigi         Studi kasus — demo produk
│
├── /proses                         Bagaimana proyek berjalan, tahap demi tahap
├── /harga                          Rentang harga tiap lini + termin + FAQ harga
├── /tentang                        Siapa kamu, kenapa bisa dipercaya
├── /kontak                         Form + WhatsApp + jadwal panggilan
│
├── /blog                           SEO + bukti cara berpikir
│   └── /blog/[slug]
│
├── HALAMAN VERTIKAL  (menangkap pencarian niche — lihat 08-seo)
│   ├── /jasa-website-klinik-gigi
│   ├── /jasa-website-klinik-kecantikan
│   └── /jasa-sistem-informasi-kampus
│
└── /kebijakan-privasi   /syarat-ketentuan
```

**Aturan navigasi utama — maksimal 5 item:**
`Layanan · Portofolio · Proses · Harga · Kontak`
"Tentang" dan "Blog" masuk footer. Navigasi yang penuh membuat prospek memilih
"jelajah-jelajah" alih-alih memilih tindakan.

**Prioritas pembangunan:** homepage, portofolio + 1 studi kasus, layanan, harga, kontak.
Sisanya menyusul. Detail: [11-roadmap-eksekusi.md](11-roadmap-eksekusi.md).

---

## 4.2 Homepage — Wireframe

Urutan bagian bukan selera. Ini corong: setiap blok menjawab keberatan yang muncul
tepat setelah blok sebelumnya.

```
┌──────────────────────────────────────────────────────────────┐
│ 1. HERO                                                      │
│    H1  Judul janji (bukan nama studio)                       │
│    Sub  Satu kalimat: untuk siapa + apa hasilnya             │
│    [ Lihat portofolio ]  [ Diskusi lewat WhatsApp ]          │
│    Baris bukti tipis: "Terakhir dikirim: sistem 30 modul     │
│    untuk unit penelitian Politeknik Negeri"                  │
└──────────────────────────────────────────────────────────────┘
        ↑ Keberatan yang lahir di sini: "memang bisa apa?"
┌──────────────────────────────────────────────────────────────┐
│ 2. BUKTI DULUAN — 2 kartu proyek besar, berdampingan         │
│    Setiap kartu: tangkapan layar, 1 kalimat masalah,         │
│    1 angka hasil, tautan "Baca studi kasus →"                │
└──────────────────────────────────────────────────────────────┘
        ↑ "oke, tapi saya butuhnya apa?"
┌──────────────────────────────────────────────────────────────┐
│ 3. TIGA LINI LAYANAN — 3 kolom                               │
│    Judul · untuk siapa · harga mulai · tautan halaman        │
└──────────────────────────────────────────────────────────────┘
        ↑ "kenapa kamu, bukan yang lain?"
┌──────────────────────────────────────────────────────────────┐
│ 4. TIGA PILAR PEMBEDA                                        │
│    Kedalaman sistem · Kecepatan produk · Serah terima bersih │
│    Setiap pilar WAJIB menautkan bukti di halaman ini juga    │
└──────────────────────────────────────────────────────────────┘
        ↑ "prosesnya gimana? ribet nggak?"
┌──────────────────────────────────────────────────────────────┐
│ 5. PROSES 4 LANGKAH — horizontal, dengan durasi tiap langkah │
└──────────────────────────────────────────────────────────────┘
        ↑ "mahal nggak?"
┌──────────────────────────────────────────────────────────────┐
│ 6. HARGA MULAI — 3 angka, tautan ke /harga                   │
└──────────────────────────────────────────────────────────────┘
        ↑ "ada yang pernah puas?"
┌──────────────────────────────────────────────────────────────┐
│ 7. TESTIMONI / KUTIPAN KLIEN                                 │
│    ⚠ Kalau belum ada testimoni asli: JANGAN karang.          │
│       Ganti blok ini dengan "Hasil terukur" (angka nyata).   │
└──────────────────────────────────────────────────────────────┘
        ↑ "masih ragu"
┌──────────────────────────────────────────────────────────────┐
│ 8. FAQ — 6 pertanyaan, jawaban jujur                         │
└──────────────────────────────────────────────────────────────┘
┌──────────────────────────────────────────────────────────────┐
│ 9. CTA PENUTUP + FORM RINGKAS (3 field)                      │
└──────────────────────────────────────────────────────────────┘
┌──────────────────────────────────────────────────────────────┐
│ FOOTER  Nama · kontak · lokasi · tautan · legal              │
└──────────────────────────────────────────────────────────────┘
```

### Tiga aturan homepage

1. **Bukti sebelum penawaran.** Blok 2 ada sebelum blok 3. Prospek yang belum percaya
   tidak membaca daftar layanan.
2. **Tidak ada slider berputar di hero.** Slider adalah cara menghindari keputusan
   soal pesan mana yang paling penting. Pilih satu.
3. **CTA yang sama di seluruh halaman.** Satu tindakan utama (WhatsApp), satu tindakan
   sekunder (lihat portofolio). Jangan menambah tindakan ketiga.

---

## 4.3 Halaman Layanan (Template)

Dipakai untuk ketiga lini, dengan isi berbeda.

```
1. H1: nama layanan + untuk siapa
2. Blok masalah — 3 keluhan spesifik yang dialami audiens ini
3. Apa yang didapat — daftar konkret, bukan sifat
4. Contoh nyata — 1 studi kasus relevan, tersemat, bukan sekadar tautan
5. Paket & harga (Lini A) ATAU tahapan & rentang (Lini B/C)
6. Yang tidak termasuk — daftar eksplisit
7. Proses & durasi
8. FAQ khusus layanan ini — 4-6 pertanyaan
9. CTA
```

**Kesalahan paling umum di halaman layanan:** menulis daftar fitur teknis.
Klien tidak membeli "React 19 + Tailwind 4". Klien membeli "halaman layananmu bisa
diiklankan sendiri-sendiri, jadi biaya iklan tidak terbuang ke pengunjung yang salah".
Teknologi disebut di **bagian bawah**, satu baris, sebagai bukti — bukan sebagai janji.

---

## 4.4 Halaman Portofolio & Studi Kasus

Struktur studi kasus, aturan angka, dan aturan izin klien: dokumen terpisah,
[05-portofolio-studi-kasus.md](05-portofolio-studi-kasus.md).

Untuk halaman indeks `/portofolio`:

- Kartu besar, maksimal 2 per baris di desktop. Portofolio tipis terlihat lebih baik
  dengan sedikit kartu besar daripada banyak kartu kecil.
- Setiap kartu memuat: tangkapan layar, jenis proyek, satu kalimat masalah,
  satu angka hasil.
- **Label jujur.** Proyek klien nyata diberi label `Klien`; demo produk sendiri diberi
  label `Produk sendiri`. Menyamarkan demo sebagai pekerjaan klien adalah cara tercepat
  kehilangan kepercayaan saat ditanya detailnya.

---

## 4.5 Halaman Proses

Halaman ini terlihat sepele dan sering dilewati. Ia menutup keberatan yang paling
jarang diucapkan tapi paling sering membunuh deal: **"saya takut prosesnya berantakan
dan saya tidak tahu harus ngapain."**

```
Langkah 1 — Obrolan awal (30 menit, gratis)
  Yang saya tanyakan · yang perlu Anda siapkan · hasilnya apa

Langkah 2 — Ruang lingkup & penawaran (2-3 hari)
  Dokumen tertulis: apa yang dikerjakan, apa yang tidak,
  berapa lama, berapa biaya, kapan dibayar

Langkah 3 — Pengerjaan (durasi sesuai paket)
  Kabar setiap [X] hari · tautan pratinjau langsung ·
  [N] putaran revisi · apa yang saya butuhkan dari Anda

Langkah 4 — Peluncuran & serah terima
  Uji sebelum live · pelatihan · dokumentasi ·
  akun & kode atas nama Anda · garansi [N] hari
```

**Yang wajib ditulis eksplisit di sini** — tiga hal yang paling ditakuti klien:

| Ketakutan | Kalimat yang menjawabnya |
|---|---|
| "Nanti hilang di tengah jalan" | "Kabar setiap 3 hari kerja, tanpa perlu ditagih. Kalau saya tidak mengabari lebih dari 5 hari, Anda berhak membatalkan dan DP dikembalikan proporsional." |
| "Nanti saya tergantung terus" | "Domain, hosting, dan repositori kode atas nama Anda sejak hari pertama. Kalau suatu saat Anda ganti pengembang, tidak ada yang perlu diminta dari saya." |
| "Nanti revisi ditagih terus" | "[N] putaran revisi termasuk di dalam harga, di tahap desain. Perubahan ruang lingkup dihitung terpisah dan selalu saya sampaikan sebelum dikerjakan." |

---

## 4.6 Halaman Tentang

Bukan biografi. Ini halaman **pengurangan risiko**.

```
1. Foto asli. Wajah, bukan logo, bukan avatar.
2. Satu paragraf: apa yang kamu kerjakan dan untuk siapa.
3. Kenapa bisa dipercaya — bukti konkret, bukan tahun pengalaman:
   proyek yang sudah selesai, ukurannya, tanggung jawabnya.
4. Cara kerja & nilai — 3 poin, masing-masing punya konsekuensi nyata
   ("Saya menolak proyek di bawah Rp 3,5 jt, karena ...").
5. Yang bukan bidang saya — daftar jujur.
6. Lokasi & cara dihubungi.
```

**Yang dihapus:** "passionate about technology", daftar sertifikat kursus online,
logo teknologi berjejer, "kami adalah tim muda yang dinamis".

---

## 4.7 Bank Copywriting

### Judul hero — pilih satu, jangan campur

| Sudut | Judul |
|---|---|
| **Ketahanan** ⭐ | Website dan sistem web yang dibangun untuk dipakai bertahun-tahun |
| Hasil bisnis | Website yang mendatangkan pelanggan, bukan sekadar ada |
| Kontras | Bukan template yang dipasangkan. Dibangun, didokumentasikan, diserahkan |
| Kapabilitas | Dari landing page sampai sistem multi-peran — satu orang, satu tanggung jawab |

### Sub-headline hero

> Untuk pemilik bisnis dan unit institusi yang sudah pernah kecewa dengan
> website yang cantik di awal lalu ditinggalkan. Saya bangun, saya dokumentasikan,
> dan saya serahkan sepenuhnya ke Anda.

### Teks tombol

| Situasi | Teks | Hindari |
|---|---|---|
| CTA utama | Diskusi Lewat WhatsApp | "Hubungi Kami" (dingin, tanpa arah) |
| CTA sekunder | Lihat Portofolio | "Selengkapnya" |
| Halaman harga | Minta Penawaran Tertulis | "Order Sekarang" |
| Studi kasus | Baca Studi Kasus Lengkap | "Read More" |
| Lini C | Jadwalkan Lokakarya Ruang Lingkup | "Konsultasi Gratis" (mengundang penjelajah) |

### Blok tiga pilar

> **Kedalaman yang bisa dicek**
> Proyek terakhir saya: sistem informasi dengan 30 modul, 4 peran pengguna, izin
> per-aksi yang bisa diubah tanpa deploy ulang, dan integrasi SSO institusi —
> dikerjakan sendiri, dari basis data sampai deployment.
> *PageSpeed Insights (Desktop): Performa 94 · Aksesibilitas 91 · Praktik Terbaik 100 · SEO 100.*
> [Baca studi kasusnya →]

> **Cepat karena sudah dibangun sebelumnya**
> Untuk website bisnis, saya tidak mulai dari nol. Ada inti produk yang sudah matang
> per jenis usaha, jadi waktu terpakai untuk hal yang khas bisnis Anda —
> bukan untuk membuat ulang tombol.
> [Lihat demo hidupnya →]

> **Anda tidak tersandera**
> Kode, domain, hosting, dan akun atas nama Anda. Dokumentasi diserahkan dalam
> bentuk tertulis. Kalau suatu hari Anda ganti pengembang, tidak ada satu pun hal
> yang harus diminta dari saya.
> [Lihat contoh dokumentasinya →]

### Blok penanganan keberatan harga

> **Kenapa tidak Rp 1 juta?**
> Karena pada harga itu yang bisa dikerjakan hanyalah memasang template dan mengganti
> teksnya. Hasilnya bisa jadi terlihat baik — dan tetap tidak mendatangkan siapa pun,
> karena tidak ada yang dirancang untuk itu. Kalau anggaran Anda memang di sana,
> saya lebih suka mengatakannya sekarang daripada mengecewakan Anda tiga bulan lagi.

### Blok jaminan / pengurang risiko

> **Yang Anda dapat, tertulis**
> Ruang lingkup tertulis sebelum dibayar · Kabar setiap 3 hari kerja ·
> Tautan pratinjau sejak minggu pertama · Semua akun atas nama Anda ·
> Garansi perbaikan [N] hari setelah live

### Footer

```
[NAMA STUDIO]
Website & sistem web · Surabaya, Indonesia

Layanan          Perusahaan        Kontak
Website Bisnis   Tentang           WhatsApp: +62 xxx
Company profile  Proses            halo@domain
Sistem Web       Blog              LinkedIn

© 2026 [Nama]. Kebijakan Privasi · Syarat & Ketentuan
```

---

## 4.8 Katalog FAQ Standar

Ambil 6 untuk homepage, sisanya sebar ke halaman layanan terkait.

| Pertanyaan | Arah jawaban |
|---|---|
| Berapa lama pengerjaannya? | Angka nyata per paket, plus penyebab molor yang paling sering (konten dari klien terlambat) |
| Saya belum punya konten/foto, gimana? | Bisa dibantu, sebutkan sebagai add-on berbayar dan jadwalnya |
| Apakah saya bisa update sendiri nanti? | Ya untuk paket dengan panel; jelaskan mana yang bisa dan mana yang tidak |
| Kalau nanti saya mau tambah fitur? | Dihitung terpisah, selalu disampaikan sebelum dikerjakan |
| Website-nya milik siapa? | Milik klien sepenuhnya — domain, hosting, kode. Jelaskan mekanismenya |
| Kenapa tidak pakai WordPress? | Jawab tanpa merendahkan WordPress. Fokus pada kecepatan, keamanan, biaya hosting, dan bahwa kebutuhan "biar bisa edit sendiri" tetap dipenuhi |
| Apakah bisa sekalian iklan / SEO? | Ya sebagai add-on; jelaskan batas — kamu bukan agensi iklan penuh |
| Bagaimana kalau saya tidak puas? | Termin pembayaran bertahap, revisi di tahap desain, dan syarat pembatalan yang jelas |
| Apakah bisa dicicil? | Termin sudah bertahap; di luar itu, jawab tegas |
| Kamu kerja sendiri atau tim? | **Jawab jujur.** "Sendiri" adalah jawaban yang menenangkan kalau diikuti "jadi tidak ada yang hilang di antara oper-operan" |

---

## 4.9 Aturan Variabel Copy

Semua nilai berikut hidup di satu file konfigurasi, **tidak pernah** ditulis langsung
di dalam komponen:

```
NAMA_STUDIO · TAGLINE · WA_NUMBER · EMAIL · KOTA ·
HARGA_MULAI_A · HARGA_MULAI_B · HARGA_MULAI_C ·
JUMLAH_REVISI · HARI_GARANSI · JEDA_KABAR_HARI ·
LINK_IG · LINK_LINKEDIN · LINK_GITHUB
```

Alasannya sama dengan alasan di produk klien: mengganti nomor WhatsApp seharusnya
satu baris, bukan pencarian di 14 berkas. Skema lengkap:
[07-arsitektur-teknis.md](07-arsitektur-teknis.md) §7.4.
