# 08 — SEO & Mesin Konten

## 8.1 Masalah SEO Khas Studio Generalis

Spesialis punya jalan mudah: satu kata kunci, satu halaman, selesai.
Generalis tidak — dan ini konsekuensi nyata dari pilihan positioning di
[01-strategi-usaha.md](01-strategi-usaha.md) §1.3.

| Kata kunci | Volume | Kesulitan | Kualitas prospek | Putusan |
|---|---|---|---|---|
| `jasa pembuatan website` | Sangat tinggi | **Brutal** — agensi dengan anggaran SEO besar | Rendah (pemburu harga) | ❌ Jangan dikejar |
| `jasa pembuatan website murah` | Tinggi | Sedang | **Sangat buruk** | ❌ Jangan pernah |
| `jasa pembuatan website klinik gigi` | Rendah | **Rendah** | **Sangat tinggi** | ✅ Kejar |
| `jasa website klinik gigi surabaya` | Sangat rendah | Sangat rendah | Sangat tinggi | ✅ Kejar |
| `jasa pembuatan sistem informasi kampus` | Rendah | Rendah | Tinggi | ✅ Kejar |
| `contoh website klinik gigi` | Sedang | Rendah | Tinggi (riset pra-beli) | ✅ Kejar — lewat demo |
| `berapa biaya bikin website perusahaan` | Sedang | Sedang | Sedang | ✅ Kejar — lewat artikel |

**Kesimpulan strategis:** jangan bertarung di kepala kurva. Kuasai **ratusan ekor
panjang** yang masing-masing kecil tapi hampir tanpa pesaing dan hampir seluruhnya
prospek serius.

```
   Volume
     │  ╱╲   "jasa pembuatan website"  ← mustahil, prospek buruk
     │ ╱  ╲
     │╱    ╲___
     │         ╲______
     │                ╲___________  ← ekor panjang: kamu di sini
     └──────────────────────────────────
            Kesulitan menurun →
            Kualitas prospek naik →
```

---

## 8.2 Tiga Mesin Trafik

```
┌──────────────────────┬──────────────────────┬──────────────────────┐
│ MESIN 1              │ MESIN 2              │ MESIN 3              │
│ Halaman vertikal     │ Blog teknis          │ Demo yang ter-index  │
├──────────────────────┼──────────────────────┼──────────────────────┤
│ "jasa website        │ "reverse proxy hanya │ "contoh website      │
│  klinik gigi"        │  izinkan GET & POST" │  klinik gigi"        │
├──────────────────────┼──────────────────────┼──────────────────────┤
│ Menangkap prospek    │ Menangkap sesama     │ Menangkap prospek    │
│ Lini A yang sedang   │ teknis & pengambil   │ yang sedang riset,   │
│ mencari              │ keputusan institusi  │ belum siap bicara    │
├──────────────────────┼──────────────────────┼──────────────────────┤
│ Konversi TINGGI      │ Konversi rendah,     │ Konversi sedang,     │
│ Volume rendah        │ otoritas TINGGI      │ volume sedang        │
├──────────────────────┼──────────────────────┼──────────────────────┤
│ Bangun lebih dulu ★  │ Bangun berkelanjutan │ Datang gratis dari   │
│                      │                      │ produk klinik gigi   │
└──────────────────────┴──────────────────────┴──────────────────────┘
```

**Urutan pembangunan: Mesin 1 → Mesin 3 → Mesin 2.** Mesin 2 memberi hasil terbesar
tapi paling lambat (3–6 bulan). Mulai menulis lebih awal, jangan menunggu hasilnya
untuk memvalidasi apa pun.

---

## 8.3 Mesin 1 — Halaman Vertikal

Satu template, banyak data. Teknis: [07-arsitektur-teknis.md](07-arsitektur-teknis.md) §7.5.

### Gelombang 1 (bangun bersama situs v1)

| Slug | Sasaran |
|---|---|
| `/jasa-website-klinik-gigi` | ⭐ prioritas — produk sudah ada |
| `/jasa-website-klinik-kecantikan` | vertikal ekspansi pertama |
| `/jasa-sistem-informasi-kampus` | Lini C, memanfaatkan studi kasus P3M |
| `/jasa-website-perusahaan` | Lini B, umum |

### Gelombang 2 (setelah ada klien di gelombang 1)

`klinik-hewan` · `fisioterapi` · `optik` · `salon-barbershop` · `bimbel-kursus` ·
`bengkel` · `notaris-konsultan`

### Varian kota (setelah vertikal punya peringkat)

`/jasa-website-klinik-gigi-surabaya` · `-sidoarjo` · `-malang` · `-gresik`

⚠️ **Aturan anti-konten kembar:** varian kota **wajib** punya isi yang benar-benar
berbeda — daftar wilayah layanan, contoh lokal, nomor kontak yang sama tapi konteks
kota berbeda. Menyalin halaman dan hanya mengganti nama kota adalah *doorway page*,
dan Google menghukumnya. **Kalau tidak sanggup menulis isi yang berbeda, jangan
buat halamannya.**

### Isi wajib tiap halaman vertikal

1. H1 mengandung kata kunci utama, apa adanya
2. Tiga masalah spesifik industri itu — bukan masalah umum
3. Fitur yang relevan untuk industri itu saja
4. **Contoh nyata** — demo hidup atau studi kasus, tersemat
5. Paket & harga
6. FAQ khusus industri (JSON-LD `FAQPage`)
7. CTA WhatsApp dengan pesan awal yang menyebut industrinya

**Panjang target: 900–1.500 kata.** Kurang dari itu tidak akan mengungguli siapa pun.
Lebih dari itu jadi tidak terbaca.

---

## 8.4 Mesin 2 — Blog Teknis

### Tiga pilar artikel

| Pilar | Contoh judul | Untuk siapa | Frekuensi |
|---|---|---|---|
| **Keputusan teknis** ⭐ | "Ketika reverse proxy kampus hanya mengizinkan GET dan POST" | Sesama teknis, pengambil keputusan institusi | 2×/bulan |
| **Panduan pembeli** | "Berapa sebenarnya biaya bikin website perusahaan di Indonesia (2026)" | Prospek Lini A & B yang sedang riset | 1×/bulan |
| **Bedah industri** | "Kenapa 8 dari 10 website klinik gigi kehilangan pasien di layar pertama" | Prospek vertikal | 1×/bulan |

**Pilar 1 adalah aset paling bernilai dan paling sedikit pesaingnya.** Hampir tidak
ada orang Indonesia yang menulis keputusan arsitektur dengan alasan dan alternatif
yang ditolak. Artikel semacam ini terus menarik trafik bertahun-tahun dan menjadi
alasan orang mempercayakan proyek besar.

### Sepuluh artikel pertama — sudah ada bahannya

Semua berasal dari P3M PENS. Naskah setengah jadi tersedia di
[`p3m-pens/code/linkedin-plan/`](../../p3m-pens/code/linkedin-plan/).

| # | Judul kerja | Pilar |
|---|---|---|
| 1 | Ketika reverse proxy hanya mengizinkan GET dan POST | Keputusan |
| 2 | WAF menolak HTML di badan permintaan — dan editor teks kaya saya berhenti bekerja | Keputusan |
| 3 | Hak akses sebagai data, bukan sebagai kode | Keputusan |
| 4 | Kenapa peran aktif tunggal mengalahkan gabungan peran | Keputusan |
| 5 | Satu aplikasi React untuk situs publik dan panel admin | Keputusan |
| 6 | Berkas yang tidak pernah disajikan lewat URL penyimpanan | Keputusan |
| 7 | Berapa biaya bikin website perusahaan di Indonesia | Panduan pembeli |
| 8 | Yang harus ditanyakan sebelum menandatangani kontrak jasa web | Panduan pembeli |
| 9 | Website atau sistem informasi — mana yang sebenarnya Anda butuhkan | Panduan pembeli |
| 10 | Kenapa website klinik kehilangan pasien di layar pertama | Bedah industri |

**Aturan penerbitan:** satu artikel benar-benar bagus per dua minggu mengalahkan
empat artikel dangkal per bulan. Artikel dangkal merusak posisi otoritasmu — persis
aset yang sedang kamu bangun.

---

## 8.5 Mesin 3 — Demo yang Ter-index

Demo klinik gigi dibangun untuk ranking di `contoh website klinik gigi` dan sejenisnya.
Strategi lengkapnya ada di blueprint produk:
[`../02-produk-klinik-gigi/README.md`](../02-produk-klinik-gigi/README.md).

**Yang perlu diurus dari sisi situs studio:**
- Demo di subdomain (`demo.namastudio.com`), **bukan** subdirektori — supaya
  otoritasnya terpisah dan tidak membingungkan sinyal situs studio
- Demo menautkan balik ke situs studio dari halaman "Tentang Demo Ini"
- Situs studio menautkan ke demo dari halaman vertikal klinik gigi
- **Pratinjau ber-brand prospek wajib `noindex`.** Ini aturan keras — lihat
  [`../02-produk-klinik-gigi/12-etika-legal.md`](../02-produk-klinik-gigi/12-etika-legal.md)

---

## 8.6 Distribusi — Di Mana Konten Disebar

| Saluran | Untuk lini | Bentuk | Frekuensi |
|---|---|---|---|
| **LinkedIn (pribadi)** ⭐ | C, B | Post keputusan teknis, versi pendek artikel | 3×/minggu |
| **Instagram** | A | Reel sebelum/sesudah, tur fitur, carousel edukasi | 3×/minggu |
| **TikTok** | A | Sama dengan Reel, dipotong ulang | ikut IG |
| **Blog sendiri** | Semua | Artikel penuh | 2–4×/bulan |
| **Google Business Profile** | A lokal | Postingan + foto | 1×/minggu |
| **Komunitas/forum** | C | Menjawab pertanyaan teknis, tanpa promosi | seperlunya |

**LinkedIn adalah saluran terpenting untuk Lini C** — di sanalah kepala unit,
manajer, dan dosen berada. Rencana LinkedIn yang sudah disusun untuk P3M PENS
([`../../p3m-pens/code/linkedin-plan/`](../../p3m-pens/code/linkedin-plan/))
bisa dipakai langsung; ia hanya perlu ditambahi tautan ke situs studio di setiap post.

**Instagram/TikTok untuk Lini A.** Rencana konten khusus vertikal klinik gigi sudah
lengkap di [`../02-produk-klinik-gigi/11-konten-medsos.md`](../02-produk-klinik-gigi/11-konten-medsos.md).

### Aturan daur ulang — satu pekerjaan, enam keluaran

```
Satu keputusan teknis di P3M PENS
        │
        ├──► Blok "Keputusan" di studi kasus
        ├──► Artikel blog 1.200 kata
        ├──► Post LinkedIn 250 kata
        ├──► Carousel Instagram 6 slide
        ├──► Reel 45 detik
        └──► Paragraf di halaman vertikal /jasa-sistem-informasi-kampus
```

**Jangan pernah membuat konten dari nol.** Setiap keping konten harus berasal dari
pekerjaan nyata yang sudah dilakukan. Ini yang membuat mesinnya berkelanjutan
untuk satu orang.

---

## 8.7 Pengukuran

Periksa **bulanan**, bukan harian. SEO bergerak lambat; memeriksa harian hanya
menghasilkan kecemasan dan keputusan buruk.

| Metrik | Alat | Target bulan 6 |
|---|---|---|
| Tayangan pencarian | Search Console | 3.000/bulan |
| Klik dari pencarian | Search Console | 150/bulan |
| Kata kunci di halaman 1 | Search Console | ≥ 5 |
| Lead dari form | GA4 event | 4/bulan |
| Lead dari WhatsApp langsung | Hitung manual | 4/bulan |
| Kunjungan halaman studi kasus | GA4 | 200/bulan |

**Metrik yang diabaikan:** jumlah pengikut, jumlah tayangan post, jumlah suka.
Tidak satu pun membayar tagihan.

**Satu metrik yang paling penting:** *berapa lead yang menyebut "saya baca artikel/
lihat demo Anda"*. Catat manual. Itu satu-satunya bukti bahwa mesin kontennya bekerja.
