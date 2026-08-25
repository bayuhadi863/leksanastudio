# 06b — Design System: Leksana Studio

> **Status:** spesifikasi implementasi. Dokumen ini adalah sumber kebenaran untuk
> token, tipografi, komponen, dan pola situs studio.
>
> **Sudah diimplementasikan** di [`code/`](code/) — lihat `src/app/globals.css`.
> Di mana implementasi mengoreksi spesifikasi, angka di dokumen ini sudah diperbarui
> dan alasannya ditulis di tempatnya.
> Prinsip dan arah strategisnya: [06-sistem-desain.md](06-sistem-desain.md).
> Batasan teknis & anggaran performa: [07-arsitektur-teknis.md](07-arsitektur-teknis.md).

---

## 6b.1 Tesis Desain

Satu kalimat yang menjadi alasan setiap keputusan di bawah:

> ### Situs ini terlihat seperti dokumen kerja yang ditulis seseorang, ditandatangani, dan dipertanggungjawabkan.

**Kenapa ini, dan bukan yang lain.** Yang membedakan Leksana bukan tampilan, bukan
stack, bukan harga. Yang membedakan adalah **alasan setiap keputusan ditulis dan
diserahkan** — 13 dokumen teknis dan satu buku PDF yang tidak dibuat pesaing mana pun.
Kalau itu pembedanya, itu juga yang harus terlihat sebelum satu kalimat pun dibaca.

Maka bahasa visualnya diambil dari benda yang benar-benar kamu serahkan: **berkas
kerja** — bukan brosur agensi, bukan dasbor, bukan portofolio desainer.

### Tiga arah yang dipertimbangkan dan ditolak

| Arah | Kenapa menarik | Kenapa ditolak |
|---|---|---|
| **Cetak biru / grid teknik** | Sesuai "dibangun untuk bertahan" | Estetika kertas milimeter sudah jadi klise portofolio developer. Meniru *gambar* insinyur, bukan *kerja* insinyur |
| **Dasbor / status sistem** | Sesuai "sistem yang jalan" | Angka berjalan dan lampu hijau membaca sebagai produk SaaS. Kamu menjual jasa, bukan langganan |
| **Editorial surat kabar** | Rapi, dewasa | Garis rambut + serif kontras tinggi + nol radius adalah tampilan generik yang muncul di ribuan situs. Ia tidak mengatakan apa pun tentang Leksana |

### Yang ditolak secara khusus

- **Krem hangat + serif kontras tinggi + aksen terakota** — seragam paling umum saat ini
- **Gradien ungu-biru, blob mengambang, ilustrasi isometrik** — seragam agensi
- **Inter + JetBrains Mono** — pasangan bawaan. Bukan salah, tapi tidak mengatakan apa-apa
- **Eyebrow bernomor `01 / TENTANG` di setiap bagian** — penomoran hanya sah kalau isinya
  memang urutan. Aturan lengkap di §6b.5

---

## 6b.2 Elemen Tanda Tangan — "Catatan Pinggir"

Satu hal yang membuat situs ini diingat. Seluruh keberanian desain dibelanjakan di sini;
semua yang lain tenang.

**Apa itu:** anotasi pendek bersuara orang pertama yang muncul di margin luar, sejajar
dengan klaim yang dianotasinya — seperti seorang insinyur memberi catatan di pinggir
speknya sendiri.

```
 desktop ≥1024px

 ┌──────────────────────────────────────┬──────────────────────────┐
 │                                      │                          │
 │  Paket A2 — Website Lengkap          │                          │
 │  Rp 8.500.000                        │  ── Catatan              │
 │                                      │  Saya menolak proyek di  │
 │  Halaman layanan terpisah, panel      │  bawah Rp 3,5 juta. Pada │
 │  edit sendiri, pelacakan konversi,   │  rentang itu yang bisa   │
 │  pelatihan satu sesi.                │  dikerjakan cuma pasang  │
 │                                      │  template — dan saya     │
 │  [ Diskusi lewat WhatsApp ]          │  tidak bisa              │
 │                                      │  mempertanggungjawabkan  │
 │                                      │  hasilnya.               │
 │                                      │                          │
 └──────────────────────────────────────┴──────────────────────────┘
   jalur utama  ·  max 55ch                jalur catatan  ·  260px
```

**Kenapa ini benar untuk Leksana, bukan sekadar unik:**

1. **Ia adalah brand yang dijadikan struktur.** Positioning-mu adalah "saya
   dokumentasikan alasan tiap keputusan". Catatan pinggir membuat itu terlihat,
   bukan diklaim.
2. **Ia melayani dua audiens sekaligus.** Pemilik klinik mendapat kehangatan dan
   kejujuran; pengambil keputusan institusi mendapat kedalaman — tanpa jalur utama
   menjadi padat.
3. **Ia jarang dipakai.** Pola sidenote hampir tidak ada di situs jasa web Indonesia.

**Aturan pemakaian — ini yang mencegahnya jadi hiasan:**

| Aturan | Isi |
|---|---|
| Maksimal **4 catatan per halaman** | Lebih dari itu, ia berhenti menjadi tekanan dan mulai menjadi kebisingan |
| Selalu **orang pertama** | "Saya menolak…", "Saya salah memperkirakan…". Kalau bisa ditulis oleh siapa saja, hapus |
| Isinya **alasan, keberatan, atau batas** — bukan fitur | Catatan yang mengulang jalur utama adalah catatan yang dibuang |
| Panjang **20-45 kata** | Lebih panjang = paragraf yang salah tempat |
| Di bawah 1024px: **inline**, di dalam jalur utama, dengan garis vertikal aksen di kiri | Bukan disembunyikan. Isinya penting |
| Tidak pernah memuat CTA | Catatan menjelaskan, tidak menjual |

**Cara memakainya.** Ada dua bentuk, dan keduanya sudah ada di
[`code/src/components`](code/src/components).

`<Annotation>` memasangkan satu blok konten dengan catatannya dalam **satu baris grid**,
sehingga catatan sejajar dengan paragraf yang dianotasi:

```mdx
<Annotation note="Saya menolak proyek di bawah Rp 3,5 juta. Pada rentang itu yang bisa
dikerjakan cuma pasang template — dan saya tidak bisa mempertanggungjawabkan hasilnya.">

Paragraf yang sedang dianotasi.

</Annotation>
```

`<Note>` berdiri sendiri, dipakai saat catatan tidak perlu sejajar dengan apa pun:

```mdx
<Note>
Angka mobile-nya 68. Saya tulis di sini supaya tidak terlihat seperti ada yang
disembunyikan.
</Note>
```

> **Kenapa satu baris grid, bukan `float`.** Pendekatan float yang lazim dipakai untuk
> sidenote membuat catatan mendarat di sebelah paragraf **berikutnya**, bukan paragraf
> yang dianotasi. Grid menyelesaikan itu, dan urutan DOM-nya tetap terbaca
> berurutan: konten dulu, `<aside>` menyusul.

---

## 6b.3 Palet

Berhenti pada enam nilai. Palet yang lebih besar bukan tanda kecanggihan — ia tanda
belum ada keputusan.

| Nama token | Terang | Gelap | Peran |
|---|---|---|---|
| `kertas` | `#EFF1F2` | `#151B22` | Latar halaman — abu kebiruan, stok dokumen. Bukan putih, bukan krem |
| `kertas-atas` | `#F8F9FA` | `#1D242C` | Permukaan naik: tabel, kartu, blok kode |
| `tinta` | `#1B2430` | `#E6E9EC` | Teks utama — biru-hitam. **Tidak pernah `#000`** |
| `tinta-samar` | `#55606D` | `#9BA6B2` | Teks sekunder, label, keterangan |
| `garis` | `#D5DAE0` | `#2C3641` | Garis rambut, pembatas tabel, tepi kartu |
| `stempel` | `#7A2230` | `#D0685E` | **Aksen tunggal** — merah oxblood. Warna cap & stempel resmi |

**Kenapa oxblood, bukan biru korporat atau hijau tua.** Merah tua adalah warna cap
dan stempel pada dokumen resmi Indonesia — tanda bahwa sesuatu telah disahkan
seseorang. Ia serius tanpa jadi dingin, hangat tanpa jadi ramah berlebihan, dan ia
membedakan diri dari hampir seluruh situs jasa web lokal yang memakai biru gradasi.

**Kenapa latarnya abu kebiruan, bukan krem.** Krem hangat adalah pilihan bawaan yang
muncul di mana-mana sekarang. Abu kebiruan membaca sebagai kertas kerja/fotokopi —
lebih dekat ke dokumen teknis — dan memberi ketegangan yang enak melawan aksen hangat.

### Token semantik (yang dipakai komponen)

Komponen **tidak pernah** menyebut nilai palet langsung. Selalu lewat lapisan ini:

```
--bg              → kertas
--surface         → kertas-atas
--text            → tinta
--text-muted      → tinta-samar
--border          → garis
--accent          → stempel
--accent-fg       → #FFFFFF (terang) / #151B22 (gelap)
--accent-soft     → #F3E4E3 (terang) / #2C1E1D (gelap)   latar tenang untuk sorotan
--focus           → stempel
--ok              → #1B6E4F / #6FBF9B
--warn            → #8A5A00 / #D9A64A
--danger          → #A02418 / #E58579
```

**Anggaran pemakaian aksen: di bawah 5% permukaan halaman.** Kalau warna `stempel`
terlihat di lebih dari tiga tempat pada satu layar, ada yang harus dilepas.

### Kontras (perkiraan — verifikasi ulang sebelum live)

| Pasangan | Rasio | Lolos |
|---|---|---|
| `tinta` di atas `kertas` | ≈13:1 | AAA |
| `tinta-samar` di atas `kertas` | ≈5,2:1 | AA teks normal |
| `stempel` di atas `kertas` | ≈9,6:1 | AAA |
| putih di atas `stempel` | ≈11:1 | AAA — tombol utama aman |
| `garis` di atas `kertas` | ≈1,3:1 | Dekoratif saja. **Jangan** dipakai sebagai batas kontrol yang bermakna |

Baris terakhir penting: kalau sebuah garis menandai batas interaktif (tepi input,
tepi tombol sekunder), ia wajib naik ke `tinta-samar` agar mencapai 3:1.

---

## 6b.4 Tipografi

### Tiga peran, tiga huruf

| Peran | Huruf | Kenapa |
|---|---|---|
| **Judul** | **Spectral** (500, 600) | Serif teks yang dirancang untuk layar. Karakternya kering dan tegas — huruf dokumen, bukan huruf iklan. Dipakai pada ukuran besar, ia terasa berwibawa tanpa menjadi dramatis |
| **Antarmuka & isi** | **Public Sans** (400, 600) | Huruf sistem desain pemerintah AS — dibuat untuk keterbacaan formulir dan dokumen publik. Register "resmi tapi jelas" persis yang dibutuhkan |
| **Data & catatan** | **DM Mono** (400) | Angka, label, metrik, dan penanda catatan. Sinyal teknis yang murah dan efektif |

**Kenapa serif untuk judul dan sans untuk isi — bukan sebaliknya.** Dokumen resmi
Indonesia dibaca dengan serif dan diisi dengan sans. Susunan ini terasa benar bagi
pembaca lokal tanpa perlu dijelaskan.

**Anggaran huruf: maksimal 5 berkas.** Spectral 500 + 600, Public Sans 400 + 600,
DM Mono 400. Subset `latin` saja. Preload dua yang muncul di layar pertama
(Spectral 600, Public Sans 400). Kalau anggaran performa terancam,
**DM Mono adalah yang pertama dikorbankan** — gantikan dengan Public Sans 600
+ `font-variant-numeric: tabular-nums` + `letter-spacing: 0.08em`.

### Skala

| Peran | Desktop | Mobile | Huruf & bobot | Tinggi baris | Spasi huruf |
|---|---|---|---|---|---|
| Display | 60px | 36px | Spectral 600 | 1.02 | −0.025em |
| H1 | 44 | 30 | Spectral 600 | 1.08 | −0.02em |
| H2 | 32 | 25 | Spectral 600 | 1.15 | −0.015em |
| H3 | 23 | 20 | Spectral 500 | 1.30 | −0.01em |
| Lead | 21 | 18.5 | Public Sans 400 | 1.55 | 0 |
| Isi | 18 | **17** | Public Sans 400 | 1.65 | 0 |
| Kecil | 15.5 | 15 | Public Sans 400 | 1.55 | 0 |
| Catatan pinggir | 15 | 15 | Public Sans 400 | 1.50 | 0 |
| Label / metrik | 12.5 | 12 | DM Mono 400 | 1.20 | **0.08em**, huruf besar |

**17px adalah lantai mutlak untuk isi di HP.** Sebagian besar prospek Lini A membaca
di HP kelas menengah di bawah cahaya matahari.

### Aturan yang tidak dilanggar

- **Lebar baris teks maksimal 62 karakter.** Ini yang menentukan lebar jalur utama,
  bukan sebaliknya. Dalam kode nilainya `55ch`, bukan `62ch`: satuan `ch` adalah lebar
  glif "0", dan di Public Sans itu lebih lebar dari rata-rata huruf kecil — `62ch`
  menghasilkan sekitar 76 karakter nyata. `55ch` menghasilkan sekitar 65.
- **Judul rapat, label renggang.** Kebalikannya terlihat amatir seketika.
- **Tanpa pemenggalan kata otomatis** (`hyphens: none`). Bahasa Indonesia punya kata
  panjang; pemenggalan otomatis sering salah dan mengganggu.
- **`text-wrap: balance`** pada judul, **`text-wrap: pretty`** pada paragraf —
  mencegah satu kata yatim di baris terakhir.
- **Angka selalu `tabular-nums`** di tabel dan blok metrik. Harga yang tidak sejajar
  membaca sebagai ceroboh.
- **Sentence case, bukan Title Case.** Bahasa Indonesia tidak mengenal kapitalisasi
  judul; `Website Klinik Gigi Yang Cepat` terlihat seperti terjemahan mesin.
- **Tidak ada teks di atas gambar** kecuali ada lapisan yang menjamin kontras 4.5:1.

---

## 6b.5 Penomoran & Perangkat Struktural

Perangkat struktural harus **mengkodekan sesuatu yang benar**, bukan menghias.

| Perangkat | Boleh dipakai di | Dilarang di |
|---|---|---|
| **Nomor urut** (1, 2, 3) | Langkah proses (memang berurutan) · Keputusan di studi kasus (memang terhitung) · Termin pembayaran | Bagian homepage · Kartu layanan · Daftar fitur |
| **Tanggal revisi** (`Diperbarui 23 Agu 2026`) | Studi kasus · Artikel · Halaman harga | Di mana-mana lainnya |
| **Garis rambut** | Pemisah antar-bagian · Baris tabel · Batas kartu | Sebagai hiasan di ruang kosong |
| **Label mono** | Kategori kartu (`KLIEN`) · Satuan metrik · Penanda catatan | Sebagai eyebrow di setiap heading |

**Uji kelayakan:** kalau nomor `01` diganti `07` dan tidak ada yang salah, nomor itu
hiasan. Hapus.

---

## 6b.6 Tata Letak & Grid

```
 breakpoint       jalur utama        jalur catatan     gutter
 ─────────────────────────────────────────────────────────────
 < 680px          penuh − 20px       inline            20px
 680 – 1023px     max 55ch, tengah   inline, indent    32px
 ≥ 1024px         55ch (~65 kar.)    260px             48px

 lebar halaman maksimal: 1088px  (68rem)

 Lebar halaman ditetapkan agar jalur catatan mendarat dekat tepi cangkang,
 bukan meninggalkan pita kertas kosong di kanannya.
```

**Grid asimetris, bukan simetris.** Jalur utama tidak berada di tengah halaman —
ia digeser ke kiri untuk memberi ruang jalur catatan. Ini memberi halaman bentuk
yang khas bahkan sebelum kontennya dibaca.

```
 ≥1024px

 │←─── 1240px ────────────────────────────────────────────────→│
 │                                                              │
 │   ┌───────────────────────────────┐  ┌──────────────┐        │
 │   │  JALUR UTAMA                  │  │ JALUR        │        │
 │   │  62ch                         │  │ CATATAN      │        │
 │   │                               │  │ 260px        │        │
 │   └───────────────────────────────┘  └──────────────┘        │
 │                                                              │
 │   ┌────────────────────────────────────────────────┐         │
 │   │  LEBAR PENUH — tangkapan layar, tabel, kartu   │         │
 │   │  proyek. Boleh melewati kedua jalur.           │         │
 │   └────────────────────────────────────────────────┘         │
```

**Skala ruang** (kelipatan 4, tanpa nilai di luar daftar):
`4 · 8 · 12 · 16 · 24 · 32 · 48 · 64 · 96 · 128 · 160`

**Ritme vertikal antar-bagian:** 96px di HP, 160px di desktop. Ruang kosong yang
berani adalah cara termurah membuat situs terasa mahal — dan satu-satunya cara
membuat desain minimal terasa disengaja, bukan kosong.

**Radius: 2px.** Bukan 0 (nol radius adalah tampilan surat kabar generik), bukan 8-12px
(membaca sebagai aplikasi konsumen). 2px membaca seperti kertas yang dipotong rapi.
Tombol dan input: 4px.

**Bayangan: hampir tidak ada.** Struktur dibentuk garis rambut dan ruang, bukan
bayangan. Satu-satunya bayangan yang diizinkan adalah untuk elemen yang benar-benar
melayang di atas konten: bar WhatsApp HP dan menu tarik-turun.
`0 1px 2px rgb(27 36 48 / .06), 0 8px 24px rgb(27 36 48 / .08)`

---

## 6b.7 Hero Homepage

Hero adalah tesis, bukan sampul. Ia harus memuat hal paling khas dari dunia subjeknya —
di sini: **klaim yang langsung disertai bukti dan catatan penulisnya.**

```
 ┌─────────────────────────────────────────┬────────────────────┐
 │                                         │                    │
 │  Website dan sistem web                 │                    │
 │  yang dibangun untuk                    │                    │
 │  dipakai bertahun-tahun.                │  ── Catatan        │
 │                       ↑ Spectral 600    │  Klien terakhir    │
 │                         60px            │  saya menerima 13  │
 │                                         │  dokumen teknis    │
 │  Untuk pemilik bisnis dan unit          │  bersama sistemnya.│
 │  institusi yang sudah pernah kecewa     │  Supaya kalau      │
 │  dengan website yang cantik di awal     │  suatu hari ganti  │
 │  lalu ditinggalkan.                     │  pengembang,       │
 │                       ↑ Public Sans 21px│  tidak ada yang    │
 │                                         │  perlu diminta     │
 │  [ Diskusi lewat WhatsApp ]  Portofolio │  dari saya.        │
 │                                         │                    │
 │  ────────────────────────────────────   │                    │
 │  TERAKHIR DIKIRIM                       │                    │
 │  Sistem informasi 30 modul untuk unit   │                    │
 │  penelitian Politeknik Negeri Surabaya  │                    │
 │                    ↑ DM Mono 12.5 + isi │                    │
 └─────────────────────────────────────────┴────────────────────┘
```

**Tanpa gambar di hero.** Tangkapan layar produk muncul di blok bukti tepat di
bawahnya, di mana ia punya konteks. Hero tanpa gambar memuat lebih cepat (LCP adalah
teks) dan memaksa judulnya bekerja.

**Tanpa slider.** Slider adalah cara menghindari keputusan tentang pesan mana yang
paling penting.

---

## 6b.8 Komponen

Setiap komponen dispesifikasikan lengkap dengan seluruh state-nya. Komponen tanpa
state fokus yang terlihat dianggap belum selesai.

### Tombol

| Jenis | Terisi | Tepi | Teks | Kapan |
|---|---|---|---|---|
| **Utama** | `accent` | – | `accent-fg` | Satu per layar. Selalu tindakan yang sama |
| **Sekunder** | transparan | 1px `text-muted` | `text` | Pendamping tombol utama |
| **Hantu** | transparan | – | `text-muted` | Tindakan tersier di dalam kartu |
| **Tautan-panah** | – | – | `accent` + `→` | Di dalam kartu dan paragraf |

```
ukuran    tinggi   padding-x   font              radius
sedang    44px     24px        Public Sans 600 16px   4px
besar     52px     32px        Public Sans 600 17px   4px

state
default   sesuai tabel di atas
hover     terang/gelap 8% · transisi 150ms ease-out · TANPA terangkat, TANPA skala
active    terang/gelap 14% · translateY(1px)
fokus     outline 2px accent · offset 2px · TIDAK PERNAH outline:none tanpa pengganti
disabled  opacity .45 · cursor not-allowed · tetap terbaca (kontras ≥ 4.5:1)
loading   label diganti "Mengirim…" · lebar tombol DIKUNCI agar tata letak tidak lompat
```

**Tinggi sentuh minimum 44px di HP. Tanpa pengecualian.**

### Kartu proyek — komponen paling penting di situs

```
┌───────────────────────────────────────────────────────┐
│ ┌───────────────────────────────────────────────────┐ │
│ │        tangkapan layar produk · 16:10             │ │
│ │        tanpa bingkai laptop, tanpa mockup 3D      │ │
│ └───────────────────────────────────────────────────┘ │
│                                                       │
│ KLIEN · SISTEM INFORMASI          ← DM Mono 12.5      │
│                                                       │
│ Sistem informasi untuk unit                           │  ← Spectral 23px
│ penelitian politeknik negeri                          │     JUDUL MASALAH,
│                                                       │     bukan nama klien
│ Lima alur pengajuan yang sebelumnya berjalan          │
│ lewat berkas dan pesan pribadi.                       │  ← satu kalimat
│                                                       │
│ 30 modul · 34 entitas · 6 minggu   ← DM Mono, muted   │
│                                                       │
│ Baca studi kasus →                                    │
└───────────────────────────────────────────────────────┘
  tepi 1px garis · radius 2px · TANPA bayangan
  hover: tepi → accent, judul → accent. Tidak terangkat, tidak berskala.
```

**Judul kartu adalah masalah yang diselesaikan, bukan nama klien.** Prospek memindai
mencari masalahnya sendiri, bukan mencari nama institusi yang tidak ia kenal.

### Lencana label

`KLIEN` · `PRODUK SENDIRI` — DM Mono 11px, huruf besar, `letter-spacing .1em`,
teks `text-muted`, tanpa latar, dipisah titik-tengah dari kategori.

Tanpa latar berwarna: lencana berwarna menarik perhatian ke label administratif,
padahal fungsinya cuma kejujuran. Aturan pelabelan:
[05-portofolio-studi-kasus.md](05-portofolio-studi-kasus.md) §5.6.

### Blok metrik

```
   30            34            ~73.000
   ────          ────          ────
   MODUL         ENTITAS       BARIS KODE
```

Angka: Spectral 600, 44px desktop / 32px HP, `tabular-nums`.
Garis: 24px, 2px, warna `accent`.
Label: DM Mono 12.5px huruf besar, `text-muted`.

Tanpa kotak, tanpa ikon, tanpa gradien. Ini komponen yang paling sering dipakai ulang —
biarkan ia sunyi.

### Catatan pinggir

```
desktop                          mobile / tablet
┌──────────────────┐             ┌──────────────────────────────┐
│ ── Catatan       │             │ │ CATATAN                    │
│ Teks anotasi     │             │ │ Teks anotasi orang pertama │
│ orang pertama…   │             │ │ …                          │
└──────────────────┘             └──────────────────────────────┘
 penanda: garis 16px             garis kiri 2px accent
 warna accent + label            padding-left 16px
 DM Mono 11px                    latar accent-soft, radius 2px
 teks 15px text-muted
```

Muncul dengan fade + geser 6px saat paragraf induknya masuk layar. Ini satu-satunya
animasi masuk di seluruh situs (§6b.10).

### Tabel harga

Bukan tiga kartu melayang. **Satu tabel.** Dokumen menyajikan harga dalam tabel, dan
tabel lebih mudah dibandingkan daripada tiga kolom terpisah.

- Header baris kiri: `text-muted`, Public Sans 600, 15.5px
- Kolom paket yang disorot: latar `accent-soft`, tanpa tepi tambahan
- Harga: Spectral 600, `tabular-nums`
- Baris dipisah garis rambut 1px `border`
- Di bawah 680px: tabel berubah jadi tumpukan kartu per paket, urutan
  **A2 lebih dulu** (paket yang direkomendasikan muncul pertama di HP)

### Langkah proses

Penomoran sah di sini — prosesnya memang berurutan.

```
1 ── Obrolan awal                                    30 menit, gratis
     Yang saya tanyakan, yang perlu Anda siapkan.

2 ── Ruang lingkup & penawaran                       2–3 hari
     Dokumen tertulis: apa yang dikerjakan, apa yang tidak.

3 ── Pengerjaan                                      sesuai paket
     Kabar setiap 3 hari kerja, tanpa perlu ditagih.

4 ── Peluncuran & serah terima                       1 minggu
     Akun dan kode atas nama Anda. Dokumentasi diserahkan.
```

Nomor: Spectral 600 32px, warna `accent`. Garis penghubung vertikal 1px `border`
antar langkah di desktop.

### Formulir

```
Label       Public Sans 600 15.5px, text · SELALU <label>, bukan placeholder
Input       tinggi 48px · tepi 1px text-muted · radius 4px · latar surface
            fokus: tepi accent + ring 3px accent/20%
Bantuan     15px text-muted, di BAWAH input
Error       15px danger + ikon · muncul di bawah, TIDAK menggeser tata letak
            (ruangnya dicadangkan)
Wajib       tandai yang OPSIONAL, bukan yang wajib — kalau sebagian besar wajib
```

**Tiga field saja:** nama, WhatsApp, kebutuhan. Setiap field tambahan menurunkan
pengiriman. Kualifikasi terjadi di percakapan, bukan di formulir
([09-mesin-lead-closing.md](09-mesin-lead-closing.md) §9.4).

### Bar WhatsApp (HP, < 768px)

Muncul setelah gulir 400px. Tinggi 64px + `env(safe-area-inset-bottom)`.
Latar `surface`, garis atas 1px `border`, satu tombol utama lebar penuh.
Bayangan diizinkan di sini. Transisi masuk: `translateY` 200ms.

### Navigasi

- Desktop: kiri wordmark, kanan 5 tautan + satu tombol utama. Tinggi 72px.
  Tanpa latar sampai gulir 40px, lalu latar `kertas` + garis bawah 1px.
- HP: wordmark + tombol menu. Menu terbuka **penuh layar**, bukan tarik-turun —
  tautan besar (24px), mudah disentuh, dan tidak ada yang tersembunyi.
- Tautan aktif: garis bawah 2px `accent`. Bukan warna teks berubah (kontras terjaga).

### Blok kode & tabel data

- Latar `surface`, tepi 1px `border`, radius 2px, DM Mono 15px
- Maksimal 15 baris terlihat; sisanya dilipat dengan tombol "Tampilkan selengkapnya"
- **Semua tabel dan blok kode dibungkus `overflow-x: auto`.** Halaman tidak pernah
  bergulir horizontal, isinya yang bergulir
- Diagram: **SVG inline** dengan `currentColor` dan token — bukan gambar. Wajib
  terbaca di terang dan gelap

---

## 6b.9 Suara Antarmuka

Kata di antarmuka ada untuk memudahkan, bukan menghias.

| Aturan | ❌ | ✅ |
|---|---|---|
| Namai tindakannya, bukan mekanismenya | Submit · Kirim | Kirim pesan · Minta penawaran |
| Kata yang sama sepanjang alur | Tombol "Kirim" → toast "Data tersimpan" | Tombol "Kirim pesan" → "Pesan terkirim" |
| Tautan menjelaskan tujuannya | Klik di sini · Selengkapnya | Baca studi kasus P3M PENS |
| Error menyebut masalah dan jalan keluar | Terjadi kesalahan | Nomor WhatsApp perlu diawali 08 atau +62 |
| Error tidak meminta maaf | Maaf, ada kendala… | Pesan belum terkirim. Coba lagi, atau langsung lewat WhatsApp |
| Keadaan kosong mengajak bertindak | Belum ada data | Belum ada artikel di kategori ini. Lihat semua artikel → |
| Sentence case | Lihat Semua Portofolio | Lihat semua portofolio |
| Satu elemen satu tugas | Label yang juga menjelaskan | Label melabeli; keterangan menjelaskan |

**Register:** kalimat pendek, kata kerja aktif, tanpa jargon di jalur utama.
Istilah teknis boleh — di catatan pinggir, di studi kasus, dan di blog. Itu tempatnya.

**Kalibrasi per halaman:**
halaman vertikal Lini A → hangat dan konkret ·
halaman Lini C → terstruktur dan hati-hati ·
blog → terbuka soal trade-off, termasuk yang gagal.
Detail: [03-identitas-brand.md](03-identitas-brand.md) §3.6.

---

## 6b.10 Gerak

**Seluruh anggaran gerak dibelanjakan pada satu hal: catatan pinggir.** Sisanya diam.

| Diizinkan | Nilai |
|---|---|
| Catatan pinggir masuk | fade + `translateY(6px)`, 260ms `ease-out`, sekali saja |
| Transisi warna hover | 150ms `ease-out` |
| Garis bawah tautan tumbuh | 150ms |
| Bar WhatsApp muncul | `translateY`, 200ms |
| Menu HP membuka | fade + skala 0.98→1, 180ms |

| Dilarang |
|---|
| Elemen yang beranimasi ulang setiap kali digulir melewatinya |
| Parallax · angka menghitung naik · kursor kustom · partikel |
| Animasi urutan saat halaman dimuat (menunda LCP demi kesan) |
| Hover yang mengangkat atau memperbesar kartu |

**Wajib:**
```css
@media (prefers-reduced-motion: reduce) {
  *, *::before, *::after {
    animation-duration: .01ms !important;
    transition-duration: .01ms !important;
    scroll-behavior: auto !important;
  }
}
```

Hanya animasikan `transform` dan `opacity`. Menganimasikan `height`, `top`, atau
`box-shadow` merusak anggaran INP.

**Uji:** matikan seluruh animasi. Kalau halaman terasa kosong, masalahnya ada di
tata letak dan tipografi — bukan pada kurangnya gerak.

---

## 6b.11 Gambar

| Aturan | Alasan |
|---|---|
| **Nol foto stok.** Sama sekali | Foto stok menghapus kredibilitas dalam satu detik |
| Tangkapan layar produk nyata sebagai visual utama | Ini bukti, bukan hiasan |
| Tanpa mockup 3D, tanpa bingkai laptop mengambang | Bingkai perangkat adalah bahasa template |
| Satu foto asli dirimu di halaman Tentang | Wajah nyata menaikkan kepercayaan pada jasa personal |
| Tangkapan layar HP disandingkan dengan desktop | Sebagian besar prospek menilai dari HP |
| AVIF/WebP · `width` & `height` selalu ditulis · `loading="lazy"` kecuali hero | CLS = 0 dan Lighthouse 100 |
| Data pribadi di tangkapan layar **diganti nama fiktif**, bukan diblur | Blur terlihat seperti menyembunyikan sesuatu |

---

## 6b.12 Aksesibilitas

Bukan formalitas — Lighthouse 100 adalah bukti yang kamu pamerkan.

- `<html lang="id">`. Salah bahasa membuat pembaca layar melafalkan teks Indonesia
  dengan fonetik Inggris
- Tautan lewati-ke-konten sebagai elemen fokus pertama
- Kontras: teks isi ≥ 4.5:1, teks besar ≥ 3:1, **batas kontrol bermakna ≥ 3:1**
- Fokus terlihat di setiap elemen interaktif. `outline: none` tanpa pengganti
  adalah bug, bukan pilihan gaya
- Satu `<h1>` per halaman, hierarki heading berurutan tanpa lompat
- Setiap input punya `<label>` sungguhan; placeholder bukan label
- Error diumumkan lewat `aria-live="polite"` dan ditautkan dengan `aria-describedby`
- Navigasi keyboard penuh, termasuk menu HP dan FAQ
- Gambar informatif punya `alt` deskriptif; gambar dekoratif `alt=""`
- Target sentuh ≥ 44×44px
- Catatan pinggir dapat diakses berurutan — di DOM ia berada **tepat setelah**
  paragraf yang dianotasinya, bukan di akhir halaman

---

## 6b.13 Performa sebagai Keputusan Desain

Setiap keputusan visual dinilai juga terhadap anggaran di
[07-arsitektur-teknis.md](07-arsitektur-teknis.md) §7.6. Target: **100 di keempat
kategori, di HP.**

| Keputusan desain | Konsekuensi performa |
|---|---|
| Hero berbasis teks | LCP adalah teks — hampir selalu di bawah 1 detik |
| Maksimal 5 berkas huruf, subset latin | ~90 KB total, dua di-preload |
| Struktur dibentuk garis rambut, bukan bayangan | Lebih sedikit lapisan komposit |
| Tanpa pustaka animasi | Nol JS untuk gerak; semuanya CSS |
| Tanpa pustaka ikon | ≤ 12 SVG inline, di-tree-shake secara alami |
| Tanpa sematan pihak ketiga | Tidak ada widget chat, tidak ada feed Instagram |
| Gerak hanya `transform`/`opacity` | INP terjaga |

**Kalau sebuah keputusan visual menurunkan skor di bawah 100, keputusan itu yang
diubah — bukan targetnya.** Situs ini adalah barang bukti pertamamu.

---

## 6b.14 Implementasi

### Tailwind 4 — blok `@theme`

```css
@import "tailwindcss";

@theme {
  --color-kertas:        #EFF1F2;
  --color-kertas-atas:   #F8F9FA;
  --color-tinta:         #1B2430;
  --color-tinta-samar:   #55606D;
  --color-garis:         #D5DAE0;
  --color-stempel:       #7A2230;
  --color-stempel-soft:  #F3E4E3;

  --font-judul:  "Spectral", Georgia, serif;
  --font-badan:  "Public Sans", system-ui, sans-serif;
  --font-data:   "DM Mono", ui-monospace, monospace;

  --radius-kertas: 2px;
  --radius-kontrol: 4px;

  --text-display: 3.75rem;
  --text-h1:      2.75rem;
  --text-h2:      2rem;
  --text-h3:      1.4375rem;
  --text-lead:    1.3125rem;
  --text-badan:   1.125rem;
  --text-kecil:   0.96875rem;
  --text-label:   0.78125rem;
}
```

### Kontrol tampilan

Situs memberi pembaca kendali atas mode bacanya. **Tiga status, bukan dua:**

```
  TAMPILAN   [ TERANG ][ SISTEM ][ GELAP ]
                          ▲ bawaan — atribut tidak dipasang sama sekali
```

| Aturan | Isi |
|---|---|
| **Tiga status** | "Sistem" adalah preferensi nyata. Saklar biner membuang pilihan yang sudah dibuat pembaca di perangkatnya |
| **Radio group, bukan tombol** | Tiga pilihan yang saling meniadakan persis definisi radio group. Navigasi panah dan pengumuman pembaca layar didapat gratis |
| **Label kata, bukan ikon** | Ikon matahari/bulan adalah bawaan yang muncul di mana-mana, dan menuntut pembaca menebak. Label mono tiga kata sesuai bahasa dokumen situs ini |
| **Skrip pra-cat wajib** | Satu baris di `<head>`, dibungkus `try/catch`. Tanpa ini, pembaca yang memilih gelap melihat kedip terang di tiap muat halaman |
| **"Sistem" menghapus, bukan menyimpan** | Atribut dicabut dan kunci penyimpanan dihapus, sehingga `prefers-color-scheme` benar-benar mengambil alih |
| **`color-scheme` ikut dipaksa** | Kalau tidak, scrollbar dan kontrol form bawaan peramban tertinggal di mode yang salah |
| **Letak: footer** | Header dikunci pada wordmark + navigasi + satu tombol utama (§6b.8). Kontrol ini melayani pembaca yang mencarinya, bukan menarik perhatian |

> **Kenapa ini ada, padahal §6b.10 menyuruh melepas satu aksesori.** Aturan itu berlaku
> untuk **hiasan**, bukan untuk **kontrol**. Toggle tema adalah kontrol — ia milik pembaca.
> Dan untuk studio yang menjual "saya serahkan kendalinya ke Anda", menolak memberi
> pembaca kendali atas cara ia membaca adalah kontradiksi kecil yang tidak perlu.

### Lapisan semantik & mode gelap

```css
:root {
  --bg: var(--color-kertas);
  --surface: var(--color-kertas-atas);
  --text: var(--color-tinta);
  --text-muted: var(--color-tinta-samar);
  --border: var(--color-garis);
  --accent: var(--color-stempel);
  --accent-fg: #FFFFFF;
  --accent-soft: var(--color-stempel-soft);
}

@media (prefers-color-scheme: dark) {
  :root:not([data-theme="light"]) {
    --bg: #151B22;  --surface: #1D242C;
    --text: #E6E9EC; --text-muted: #9BA6B2;
    --border: #2C3641;
    --accent: #D0685E; --accent-fg: #151B22; --accent-soft: #2C1E1D;
  }
}

:root[data-theme="dark"] { /* nilai yang sama seperti blok di atas */ }
```

> **Aturan wajib:** setiap warna **harus** punya definisi di `:root` polos.
> Warna yang hanya didefinisikan di dalam blok media atau `[data-theme]` menghasilkan
> halaman yang rusak sebagian di salah satu mode.

### Jalur catatan

```css
.dokumen {
  display: grid;
  grid-template-columns: 1fr;
  gap: 0 var(--space-12);
}

@media (min-width: 1024px) {
  .dokumen {
    grid-template-columns: minmax(0, 55ch) 260px;
  }
  .dokumen > * { grid-column: 1; }
  .dokumen > .catatan { grid-column: 2; }
  .dokumen > .lebar-penuh { grid-column: 1 / -1; }
}
```

### Struktur berkas

```
src/styles/
├── tokens.css        @theme + lapisan semantik + mode gelap
├── base.css          reset, tipografi dasar, fokus, reduced-motion
└── dokumen.css       grid jalur utama + jalur catatan

src/components/
├── blocks/           Hero · KartuProyek · BlokMetrik · TabelHarga ·
│                     LangkahProses · Faq · CtaBlok
├── mdx/              Catatan · Metrik · Diagram · Kutipan · TabelData
└── layout/           Header · Footer · BarWhatsApp · Dokumen
```

---

## 6b.15 Boleh & Jangan

| ✅ Boleh | ❌ Jangan |
|---|---|
| Satu aksen, dipakai di bawah 5% permukaan | Aksen kedua "biar tidak monoton" |
| Garis rambut membentuk struktur | Kartu berbayang bertumpuk |
| Ruang kosong vertikal yang berani | Mengisi ruang dengan ikon dan garis hias |
| Tangkapan layar produk nyata | Mockup 3D, ilustrasi isometrik, foto stok |
| Nomor pada urutan yang memang berurutan | Eyebrow `01 / TENTANG` di setiap bagian |
| Catatan pinggir berisi alasan dan batas | Catatan pinggir berisi fitur atau CTA |
| Judul serif rapat, label mono renggang | Semua huruf besar untuk judul panjang |
| Sentence case | Title Case Pada Judul Bahasa Indonesia |
| Harga ditulis di tabel | Tiga kartu melayang dengan lencana "POPULER" berwarna |
| Kontras 4.5:1 minimum di teks isi | Abu muda di atas abu muda karena "terlihat kalem" |

---

## 6b.16 Kalau Ragu

Urutan bertanya, dari atas:

1. **Apakah ini membantu prospek memutuskan?** Kalau tidak, hapus.
2. **Apakah ini benar tentang Leksana**, atau cuma terlihat bagus? Perangkat struktural
   yang tidak mengkodekan apa pun adalah hiasan.
3. **Apakah ini bertahan di HP 360px dan di mode gelap?** Kalau belum diuji, belum selesai.
4. **Apakah ini menurunkan skor di bawah 100?** Kalau ya, ubah keputusannya.
5. **Kalau harus melepas satu elemen dari layar ini, mana?** Lepas.

---

## 6b.17 Definisi Selesai

- [ ] Lighthouse **100/100/100/100** di HP, mode throttle — tangkapan layar diarsipkan
- [ ] Terbaca benar di terang **dan** gelap; tidak ada warna yang hanya hidup di satu mode
- [ ] Diuji di lebar 360px — tanpa gulir horizontal di mana pun
- [ ] Seluruh warna berasal dari token semantik; nol nilai heksa di komponen
- [ ] Setiap elemen interaktif punya state fokus yang terlihat
- [ ] Setiap input punya `<label>`; setiap error menyebut jalan keluarnya
- [ ] Maksimal 5 berkas huruf; dua di-preload
- [ ] Maksimal 4 catatan pinggir per halaman, semuanya orang pertama, semuanya berisi alasan
- [ ] Nol foto stok, nol mockup 3D, nol gradien
- [ ] Setiap nomor urut lolos uji: ganti `01` dengan `07` — apakah ada yang salah?
- [ ] `prefers-reduced-motion` dihormati
- [ ] `<html lang="id">`
- [ ] Dibuka berdampingan dengan demo klinik gigi — **terasa jelas berbeda**
