# 0002 — Arsitektur multibahasa (Indonesia + Inggris)

- **Status:** Fase 1–3 disetujui · konten Inggris **ditunda tanpa batas waktu**
  (pasar sasaran Indonesia). Arsitektur tetap wajib menampung bahasa kedua.
- **Tanggal:** 2026-08-24
- **Menyentuh:** frontend, backend, CMS, SEO, konten, blueprint
- **Menggantikan:** [0001](0001-konvensi-routing-frontend.md) untuk bagian bahasa
  rute. Cacat C1–C5 di 0001 tetap berlaku dan diserap ke Fase 1.

---

## 0. Ringkasan keputusan

Sepuluh keputusan, semuanya dengan rekomendasi. Alasannya di §3.

| # | Pertanyaan | Rekomendasi |
| --- | --- | --- |
| D1 | Bentuk URL antar bahasa | Subdirektori pada satu domain |
| D2 | Bahasa bawaan diberi prefix? | Tidak — `/harga` (ID), `/en/pricing` (EN) |
| D3 | Slug ikut diterjemahkan? | Ya, struktur maupun slug konten |
| D4 | Bagaimana dua bahasa dipasangkan | `contentKey` stabil, slug per bahasa |
| D5 | Terjemahan belum ada → apa yang terjadi | 404 di bahasa itu. **Tanpa fallback diam-diam** |
| D6 | Pustaka i18n | Lapisan tipis buatan sendiri untuk UI; konten lewat lapisan konten |
| D7 | Susunan berkas MDX | Satu folder per entri, satu berkas per bahasa |
| D8 | Skema CMS | Tabel terjemahan terpisah (bukan kolom per bahasa) |
| D9 | Pre-render | **Wajib**, dan jadi prasyarat, bukan pelengkap |
| D10 | Deteksi bahasa otomatis | Tidak ada. Pengalih eksplisit, tanpa redirect otomatis |

Dan satu pertanyaan yang **hanya Anda yang bisa menjawab**, di §12.

---

## 1. Tujuan dan lingkup

Menjadikan situs dwibahasa (Indonesia + Inggris) dengan cara yang benar secara
teknis dan bisa dipelihara — sekaligus memakai proyek ini sebagai studi kasus
penerapan multibahasa yang layak ditunjukkan ke calon klien.

Lingkupnya menyentuh empat lapisan sekaligus, dan itulah kenapa dokumen ini panjang:

1. **Routing** — bentuk URL per bahasa
2. **Konten** — bagaimana teks disimpan, dipasangkan, dan diterbitkan per bahasa
3. **CMS** — skema basis data yang menampung terjemahan sejak hari pertama
4. **SEO** — kontrak teknis yang membuat Google memperlakukan dua versi sebagai
   pasangan, bukan duplikat

---

## 2. Keadaan sekarang

### 2.1 Yang harus diterjemahkan — angka nyata

Dihitung dari kode, bukan diperkirakan:

| Sumber | Kata | Isi |
| --- | ---: | --- |
| Konten MDX | 3.354 | 2 studi kasus + 1 catatan teknis |
| Config terstruktur | 3.099 | `services` 788 · `verticals` 931 · `packages` 479 · `copy` 291 · `process` 229 · sisanya |
| Prosa tertanam di JSX | ±8.300 | 51 berkas — judul bagian, paragraf pengantar, label |
| **Total** | **±14.750** | setara ±60 halaman A4 |

Angka JSX adalah perkiraan (heuristik, sudah dikurangi false positive dari
`Schematic.tsx` yang isinya SVG). Ordenya yang penting: **belasan ribu kata**,
bukan ratusan.

### 2.2 Tiga tingkat teks — dan ini yang menentukan arsitekturnya

Angka di atas terlihat menakutkan sampai dipisah menurut **siapa pemiliknya**:

| Tingkat | Perkiraan | Contoh | Pemilik | Tempatnya nanti |
| --- | ---: | --- | --- | --- |
| **1. Kerangka antarmuka** | ±1.500 kata | "Masuk", "Kirim pesan", "Nomor WhatsApp perlu diisi", label panel | Pengembang | Katalog pesan |
| **2. Copy halaman** | ±7.000 kata | Judul bagian, paragraf pengantar, isi halaman privasi | Klien/pemilik situs | Lapisan konten → CMS |
| **3. Konten panjang** | ±6.400 kata | Studi kasus, catatan, layanan, paket, vertikal | Klien/pemilik situs | Lapisan konten → CMS |

**Konsekuensi penting:** tingkat 2 saat ini tertanam di JSX, dan itu **sudah
menjadi masalah sebelum multibahasa masuk**. Selama judul dan paragraf halaman
harga hidup di dalam `PricingPage.tsx`, klien tidak akan pernah bisa mengubahnya
lewat panel — padahal itu justru alasan CMS ini dibangun.

Artinya: memindahkan tingkat 2 keluar dari JSX **bukan biaya multibahasa.** Itu
pekerjaan yang tetap harus dilakukan untuk CMS. Multibahasa hanya membuatnya
terjadi lebih awal, dan itu keberuntungan, bukan beban.

### 2.3 Yang sudah benar

- Identifier, nama berkas, komentar: seluruhnya Inggris.
- Kode menu backend (`dashboard`, `user`, `role`): Inggris.
- `routes.ts` sudah memakai kunci Inggris (`routes.services`) — separuh dari peta
  `kunci → slug` yang dibutuhkan i18n sebenarnya sudah ada.
- Sitemap dan robots dibangun saat build dari konfigurasi yang sama dengan halaman.

### 2.4 Yang akan menghalangi

- `usePageMeta` menulis tag lewat `useLayoutEffect` — meta tidak ada di HTML awal.
- `useActiveRoleStore` membaca `document.cookie` saat modul dimuat — akan meledak
  saat pre-render di Node.
- `formatDate` mengunci `'id-ID'`.
- `App.tsx` menulis 13 path sebagai literal (cacat C1 di plan 0001).

---

## 3. Keputusan

### D1 — Bentuk URL antar bahasa: **subdirektori**

| Bentuk | Contoh | Kenapa tidak |
| --- | --- | --- |
| ccTLD | `leksana.id` / `leksana.com` | Dua domain, dua tagihan, dan **authority tidak nyambung** — backlink ke satu tidak menolong yang lain. Masuk akal untuk perusahaan multinegara, bukan studio satu orang. |
| Subdomain | `id.leksana.id` / `en.leksana.id` | DNS + sertifikat tambahan tanpa keuntungan yang sepadan di skala ini. |
| **Subdirektori** ✅ | `leksana.id/harga` · `leksana.id/en/pricing` | Satu domain, satu kolam authority, satu deploy, satu sertifikat. Semua backlink menumpuk di tempat yang sama. |

### D2 — Bahasa bawaan **tanpa prefix**

```
leksana.id/harga                    ← Indonesia (bawaan)
leksana.id/en/pricing               ← Inggris
leksana.id/jasa-website-klinik-gigi ← tetap di akar
```

Alternatifnya memberi prefix ke semua bahasa (`/id/harga` + `/en/pricing`).
Keduanya sah menurut Google. Tiga alasan memilih yang ini:

1. **Domainnya `.id`.** `leksana.id/id/harga` gagap dibaca dan gagap diucapkan.
   URL ini akan ditempel mentah di WhatsApp dan kartu nama.
2. **Halaman vertikal dirancang di akar.** Blueprint 08 menetapkan
   `/jasa-website-klinik-gigi` sebagai target kata kunci. Prefix-semua akan
   memindahkannya ke `/id/jasa-website-klinik-gigi` tanpa satu pun keuntungan.
3. **Tidak perlu redirect di `/`.** Mayoritas pengunjung berbahasa Indonesia dan
   langsung mendarat di halaman sungguhan. `leksana.id` sendiri jadi halaman
   nyata — dan halaman itulah yang paling banyak menerima backlink.

Ini mode kelas satu di semua pustaka i18n arus utama (`prefix_except_default` di
Nuxt, `localePrefix: 'as-needed'` di next-intl), bukan akal-akalan.

**Harganya, ditulis terus terang:** kalau suatu hari bahasa bawaan berganti,
seluruh URL Indonesia harus pindah dan butuh 301 massal. Untuk studio Indonesia,
itu skenario yang tidak akan terjadi.

### D3 — Slug **ikut diterjemahkan**, struktur maupun konten

```
/layanan/website-bisnis     ⇄  /en/services/business-website
/portofolio/p3m-pens        ⇄  /en/portfolio/p3m-pens-research-system
/catatan/proxy-hanya-...    ⇄  /en/notes/when-a-proxy-only-allows-...
```

Alternatifnya hanya menerjemahkan prefix (`/en/layanan/website-bisnis`). Itu
memberi pembaca Inggris URL berbahasa Indonesia — terbaca seperti hasil mesin,
dan membuang kecocokan kata kunci di bahasa kedua. Situs multibahasa yang serius
menerjemahkan seluruh path.

### D4 — Pasangan bahasa lewat **`contentKey`**, bukan lewat slug

Ini keputusan paling teknis di dokumen ini, dan paling menentukan.

Kalau slug diterjemahkan, `/portofolio/p3m-pens` dan
`/en/portfolio/p3m-pens-research-system` tidak lagi punya kesamaan tekstual.
Sistem tetap harus tahu keduanya halaman yang sama — untuk `hreflang`, dan supaya
pengalih bahasa mendarat di halaman yang setara, bukan di beranda.

Jawabannya: **identitas konten dipisahkan dari URL-nya.**

```
contentKey: "p3m-pens"        ← stabil, tidak pernah muncul di URL
  ├── id → slug "p3m-pens"
  └── en → slug "p3m-pens-research-system"
```

Slug boleh berubah (dan boleh diubah klien lewat CMS) tanpa memutus pasangan
bahasanya. Aturan turunannya, yang harus dipegang di seluruh sistem:

> **Jangan pernah memakai slug sebagai kunci.** Slug adalah alamat; `contentKey`
> adalah identitas.

### D5 — Terjemahan belum ada → **404**, tanpa fallback diam-diam

Kalau `/en/pricing` belum ditulis, tiga pilihan:

| | Perilaku | Nilai |
| --- | --- | --- |
| a | Sajikan konten Indonesia di URL `/en/` | ❌ Persis yang dihukum Google — bahasa tidak cocok dengan yang dijanjikan `hreflang`, dan pembaca kecewa |
| b | Redirect ke versi Indonesia | ⚠️ Bisa diterima, tetapi menyamarkan lubang dan mengotori laporan |
| c | **404, dan halaman itu tidak ada di dunia Inggris** ✅ | Jujur. Tidak masuk sitemap EN, tidak muncul di navigasi EN, tidak punya `hreflang` |

Pilih **(c)**. Yang membuatnya bisa dijalankan adalah **status terbit per bahasa** —
lihat D8. Sebuah entri boleh terbit di Indonesia dan masih draf di Inggris.

Konsekuensi yang disengaja: **cakupan bahasa adalah keputusan per halaman, bukan
global.** Halaman vertikal (`/jasa-website-klinik-gigi`) tidak perlu versi
Inggris sama sekali — tidak ada orang yang mencari jasa klinik gigi Surabaya
dalam bahasa Inggris. Memaksakannya justru menciptakan halaman tipis.

### D6 — i18n: **lapisan tipis buatan sendiri untuk kerangka antarmuka**

Karena tingkat 2 dan 3 (§2.2) masuk lapisan konten, yang tersisa untuk pustaka
i18n hanya tingkat 1: ±1.500 kata kerangka antarmuka, seluruhnya milik pengembang.

| | react-i18next / Lingui | Lapisan sendiri ✅ |
| --- | --- | --- |
| Ukuran | +15–20 kB gzip di bundel publik (+15%) | ±120 baris, 0 kB dependensi |
| Kunci bertipe | Perlu plugin/generator | Bawaan dari `as const` TypeScript |
| Jamak | Bawaan | `Intl.PluralRules` (sudah ada di peramban) |
| Tanggal/angka | Bawaan | `Intl.*` (sudah dipakai) |
| Alat terjemahan pihak ketiga | Terintegrasi | Perlu ekspor manual |

Untuk 1.500 kata yang hanya disentuh pengembang, pustaka penuh membawa lebih
banyak berat daripada manfaat. **Tetapi** katalognya disimpan dalam bentuk JSON
datar berkunci titik — bentuk yang sama persis dipakai i18next — supaya pindah ke
pustaka penuh nanti adalah perubahan konfigurasi, bukan penulisan ulang.

**Ambang pindah, ditulis sekarang supaya tidak jadi debat nanti:** kalau jumlah
kunci melewati ±500, atau kalau ada penerjemah non-pengembang yang perlu
menyuntingnya, pindah ke i18next. Semua akses lewat satu fungsi `t()`, jadi
peralihannya terkurung.

### D7 — MDX: satu folder per entri, satu berkas per bahasa

```
src/content/
  case-studies/
    p3m-pens/
      id.mdx          frontmatter: slug, title, summary, status
      en.mdx
    this-site/
      id.mdx
  notes/
    proxy-only-allows-get-and-post/
      id.mdx
```

- **Nama folder = `contentKey`** (D4). Inggris, karena itu identifier.
- Berkas per bahasa membuat terjemahan yang hilang **terlihat sebagai berkas yang
  tidak ada** — bukan sebagai field kosong yang mudah terlewat.
- Frontmatter per bahasa memuat `slug`, `title`, `summary`, dan `status`.
- Field yang tidak bergantung bahasa (tahun, stack, metrik angka, urutan) tinggal
  di `entry.json` di folder yang sama — supaya tidak diduplikasi dan tidak bisa
  berbeda antar bahasa.

### D8 — CMS: **tabel terjemahan terpisah**

Tiga pola yang lazim:

| Pola | Bentuk | Vonis |
| --- | --- | --- |
| Kolom per bahasa | `title_id`, `title_en` di satu baris | ❌ Menambah bahasa = migrasi skema. Mati di bahasa ketiga. |
| JSONB per field | `title jsonb = {"id":…,"en":…}` | ⚠️ Fleksibel, tapi susah diindeks per bahasa, susah divalidasi, susah dibuat unik per slug |
| **Tabel terjemahan** ✅ | `case_study` + `case_study_translation` | Menambah bahasa = **baris data**, bukan skema. Status terbit per bahasa jadi alami. Slug unik per bahasa bisa ditegakkan basis data. |

Ini pola yang ditempuh Drupal, Strapi, dan Directus setelah masing-masing mencoba
yang lain lebih dulu. Skema konkretnya di §6.

**Bukan satu tabel generik untuk semua konten.** Godaan untuk membuat
`content_entry` + `content_translation` yang menampung segalanya harus ditolak:
itu membuang kolom bertipe, validasi, dan kemampuan query. Yang dibagi adalah
**polanya**, bukan tabelnya — persis seperti `BaseEntity`/`BaseCrudService` yang
sudah ada sekarang.

### D9 — Pre-render: **wajib, dan jadi prasyarat**

Ini naik dari "nanti" menjadi penghalang, karena `hreflang` dan `canonical`
adalah tag yang paling perlu ada di **HTML awal**:

- Googlebot memang menjalankan JavaScript — tetapi menunda render, dan `hreflang`
  yang muncul belakangan sering terlewat siklus pertama.
- Perayap lain **tidak menjalankan JavaScript sama sekali**: pratinjau tautan
  WhatsApp, LinkedIn, Facebook, Slack, dan Bing.

Bilingual tanpa pre-render menghasilkan situs yang mengklaim dwibahasa tetapi
tidak pernah dibaca begitu oleh mesin mana pun.

**Caranya:** pre-render buatan sendiri saat build, memakai `renderToString`.
Bukan pilihan spekulatif — pada Fase 1 sebelumnya, **seluruh 20 rute sudah
terbukti bisa dirender di Node**. Daftar rutenya bisa diturunkan penuh dari config
dan konten, jadi tidak perlu peramban headless.

Yang harus diperbaiki lebih dulu:

1. `usePageMeta` diganti pengumpul meta saat render (context), supaya `hreflang`
   masuk ke HTML — bukan ditempel `useLayoutEffect` setelahnya.
2. Modul yang membaca `document`/`localStorage` saat impor harus dijaga
   (`useActiveRoleStore` melanggar ini hari ini).
3. `formatDate` menerima locale, tidak lagi mengunci `'id-ID'`.

Panel dan halaman masuk **tidak** ikut di-pre-render: privat, `noindex`, dan
tidak ada gunanya. Itu menyederhanakan pekerjaan.

### D10 — **Tanpa deteksi bahasa otomatis**

Tidak ada redirect berdasarkan `Accept-Language` maupun cookie. Alasannya bukan
kemalasan:

- Google secara eksplisit menyarankan menghindari redirect otomatis berbasis
  bahasa — perayap datang dari satu wilayah dan akan terus dilempar ke versi yang
  sama, sehingga versi lain tidak pernah terindeks.
- Pengguna yang dilempar otomatis kehilangan kendali, dan sering terjebak.

Sebagai gantinya: **pengalih bahasa eksplisit** di header dan footer, yang
menautkan ke **halaman setara** (lewat `contentKey`), bukan ke beranda. Pilihan
boleh diingat untuk kenyamanan pengalihnya sendiri — tidak pernah untuk
memindahkan orang secara otomatis.

Karena bahasa bawaan tanpa prefix (D2), `/` **adalah** beranda Indonesia. Tidak
ada halaman pemilih bahasa, tidak ada redirect, tidak ada hop tambahan.

---

## 4. Arsitektur target

### 4.1 Peta URL

```
leksana.id/                          beranda           ⇄  /en
leksana.id/layanan                   layanan           ⇄  /en/services
leksana.id/layanan/website-bisnis                      ⇄  /en/services/business-website
leksana.id/portofolio                                  ⇄  /en/portfolio
leksana.id/portofolio/p3m-pens                         ⇄  /en/portfolio/p3m-pens-research-system
leksana.id/proses                                      ⇄  /en/process
leksana.id/harga                                       ⇄  /en/pricing
leksana.id/tentang                                     ⇄  /en/about
leksana.id/kontak                                      ⇄  /en/contact
leksana.id/catatan                                     ⇄  /en/notes
leksana.id/kebijakan-privasi                           ⇄  /en/privacy-policy
leksana.id/jasa-website-klinik-gigi  vertikal          — hanya ID (disengaja)

leksana.id/auth/login                aplikasi, satu bahasa antarmuka
leksana.id/panel/dashboard
```

**Catatan:** slug Inggris di atas contoh, ditetapkan bersama saat penulisan konten.

### 4.2 Lapisan

```
┌─ config/i18n.ts ──────────────────────────────────────────────┐
│  locales: ['id', 'en']   defaultLocale: 'id'                  │
│  segment: { services: { id: 'layanan', en: 'services' }, … }  │
└───────────────────────────────────────────────────────────────┘
        │                    │                      │
        ▼                    ▼                      ▼
  routes(locale)      messages/{locale}.json    content loader
  path builder        kerangka antarmuka        contentKey → per-locale
  + routePatterns     (±1.500 kata)             (MDX hari ini, API nanti)
        │                    │                      │
        └────────────────────┴──────────────────────┘
                             ▼
                    LocaleProvider (context)
                    t() · locale · alternates
                             │
              ┌──────────────┴──────────────┐
              ▼                             ▼
        Halaman publik              Skrip pre-render
        (dwibahasa)                 satu HTML per (bahasa × rute)
                                    meta + hreflang tertanam
```

### 4.3 Struktur berkas

```
frontend/src/
  config/
    i18n.ts            daftar bahasa, bawaan, peta segmen
    routes.ts          routes(locale) + routePatterns — diturunkan dari i18n.ts
  messages/
    id.json            kerangka antarmuka
    en.json
  content/
    case-studies/<contentKey>/{entry.json, id.mdx, en.mdx}
    notes/<contentKey>/{entry.json, id.mdx, en.mdx}
    pages/<contentKey>/{id.json, en.json}      copy halaman (tingkat 2)
  lib/
    i18n.ts            t(), useLocale(), Intl per bahasa
    content.ts         pemuat, berkunci contentKey, sadar bahasa
  components/i18n/
    LocaleProvider.tsx
    LocaleSwitcher.tsx
frontend/scripts/
  prerender.ts         satu HTML per (bahasa × rute)
```

---

## 5. Kontrak SEO

Bukan daftar keinginan — ini yang membuat Google memperlakukan dua versi sebagai
pasangan, dan tiap barisnya wajib.

### 5.1 Di setiap halaman

```html
<html lang="id">                                   <!-- ikut bahasa halaman -->

<link rel="canonical" href="https://leksana.id/harga" />
<!-- canonical menunjuk DIRI SENDIRI. Tidak pernah ke bahasa lain. -->

<link rel="alternate" hreflang="id" href="https://leksana.id/harga" />
<link rel="alternate" hreflang="en" href="https://leksana.id/en/pricing" />
<link rel="alternate" hreflang="x-default" href="https://leksana.id/harga" />
<!-- termasuk dirinya sendiri; timbal balik wajib -->

<meta property="og:locale" content="id_ID" />
<meta property="og:locale:alternate" content="en_US" />
```

Tiga aturan yang paling sering dilanggar orang:

1. **Timbal balik.** Kalau ID menunjuk EN, EN wajib menunjuk balik ke ID. Sepihak
   = seluruh kelompok diabaikan Google.
2. **Menunjuk diri sendiri.** Tiap halaman memuat `hreflang` untuk dirinya juga.
3. **Hanya untuk pasangan yang benar-benar ada.** Terjemahan belum terbit (D5) →
   `hreflang`-nya tidak dipancarkan sama sekali. Jangan pernah menunjuk ke 404.

### 5.2 Sitemap

Satu `sitemap.xml`, tiap URL membawa daftar saudaranya:

```xml
<url>
  <loc>https://leksana.id/harga</loc>
  <xhtml:link rel="alternate" hreflang="id" href="https://leksana.id/harga"/>
  <xhtml:link rel="alternate" hreflang="en" href="https://leksana.id/en/pricing"/>
</url>
```

Halaman yang belum diterjemahkan hanya muncul sekali, tanpa alternate.

### 5.3 Lainnya

- `robots.txt` — tambah `Disallow: /panel/` dan `/auth/` (cacat C5 dari plan 0001).
- JSON-LD — `inLanguage` per halaman; `Organization` cukup sekali dengan
  `knowsLanguage: ["id","en"]`.
- Kartu OG — idealnya per bahasa. Fase awal boleh satu kartu netral.
- Google Search Console — kedua versi dalam satu properti domain; laporan
  "International Targeting" dipakai untuk memverifikasi `hreflang`.

### 5.4 Yang **tidak** dilakukan

- ❌ Terjemahan mesin lalu diterbitkan begitu saja. Google menyebutnya konten
  hasil-otomatis bernilai rendah; risikonya lebih besar daripada tidak punya versi
  Inggris sama sekali.
- ❌ Redirect otomatis berbasis bahasa peramban (D10).
- ❌ `hreflang` ke halaman yang belum ada.
- ❌ Menyajikan teks Indonesia di URL `/en/`.

---

## 6. Skema CMS

### 6.1 Bentuk

Satu tabel bahasa, lalu tiap modul konten dipecah dua: bagian yang tidak
bergantung bahasa, dan bagian yang bergantung bahasa.

```
locale
  code            'id' | 'en'          PK logis
  name            'Bahasa Indonesia' | 'English'
  is_default      bool                 tepat satu true
  is_active       bool                 mematikan bahasa tanpa menghapus data
  order           int

case_study                              ← tidak bergantung bahasa
  id, content_key (unik), figure, cover_path, year,
  duration, stack, metrics, order, …audit + soft delete

case_study_translation                  ← bergantung bahasa
  id, case_study_id → case_study.id
  locale_code    → locale.code
  slug, title, summary, problem, role, client, body
  status         draft | published
  published_at
  …audit + soft delete

  UNIQUE (case_study_id, locale_code)   satu terjemahan per bahasa
  UNIQUE (locale_code, slug)            slug unik dalam satu bahasa
```

Pola yang sama untuk `note`, `service`, `vertical`, `package`, `page_copy`.

### 6.2 Kenapa `locale` jadi tabel, bukan enum

Menambah bahasa jadi **satu baris**, bukan migrasi + deploy. Panel bisa
menampilkan daftarnya. Bahasa bisa dinonaktifkan tanpa kehilangan terjemahannya.

### 6.3 Yang harus ditambahkan ke backend

Mengikuti pola yang sudah ada, bukan menyainginya:

| Baru | Sejajar dengan |
| --- | --- |
| `BaseTranslatableEntity` / `BaseTranslationEntity` | `BaseEntity` |
| `BaseTranslatableRepository<TEntity, TTranslation>` | `BaseRepository<T>` |
| `BaseTranslatableCrudService<…>` | `BaseCrudService<…>` |
| `BaseTranslatableCrudController<…>` | `BaseCrudController<…>` |
| `?locale=` pada endpoint publik; panel selalu mengembalikan semua terjemahan | — |
| `LocaleController` + `LocaleSeeder` | `MenuController`, `MenuSeeder` |

Tanpa kelas dasar ini, tiap modul akan menulis ulang join dan aturan
fallback-nya sendiri — dan salah satunya pasti akan salah.

### 6.4 Yang harus ditampilkan panel

- Pengalih bahasa di setiap formulir konten, dengan penanda "sudah/belum diterjemahkan".
- Terbit per bahasa, bukan sekali untuk semuanya (D5).
- Layar **kelengkapan terjemahan**: apa saja yang terbit di ID tapi belum di EN.
  Ini yang mencegah situs setengah jadi (§9), dan bukan sekadar hiasan.
- Slug per bahasa, bisa disunting, dengan peringatan bahwa mengubah slug yang
  sudah terbit menuntut redirect.

---

## 7. Dampak ke backend

Tidak ada perubahan pada autentikasi, izin, atau kode menu. Yang bertambah:
tabel `locale`, kelas dasar translatable, dan modul konten yang memang belum ada.

Satu keputusan yang perlu diambil sekarang meski CMS-nya belakangan: **antarmuka
panel sendiri satu bahasa atau dwibahasa?**

Rekomendasi: **satu bahasa (Indonesia) dulu.** Pengelolanya Anda dan klien
Indonesia. Menerjemahkan panel menggandakan pekerjaan tanpa pembaca. Kalau nanti
ada klien berbahasa Inggris, lapisan `t()` yang sama sudah tersedia.

---

## 8. Fase eksekusi

Tiap fase berdiri sendiri dan **memberi nilai meskipun fase berikutnya tidak
pernah dikerjakan**. Itu syaratnya, bukan gaya penulisan.

### Fase 1 — Kerangka routing & locale · situs tidak berubah

- `config/i18n.ts`: daftar bahasa, bawaan, peta segmen.
- `routes.ts` jadi `routes(locale)` + `routePatterns`.
- `LocaleProvider`, `useLocale()`.
- Router menerima `/en/*` tetapi belum ada halaman EN.
- **Sekalian: cacat C1–C5 dari plan 0001** (path literal, segmen literal, config
  mati, reload penuh, robots).
- **Gerbang:** situs berperilaku identik. Seluruh uji peramban tetap lolos.

### Fase 2 — Copy halaman keluar dari JSX · situs tidak berubah

- Prosa tingkat 2 (±7.000 kata) pindah ke `content/pages/<key>/id.json`.
- Kerangka antarmuka tingkat 1 (±1.500 kata) pindah ke `messages/id.json`.
- **Ini pekerjaan CMS, bukan pekerjaan i18n** (§2.2) — jadi tetap berharga
  meskipun Inggris dibatalkan.
- **Gerbang:** tidak ada prosa Indonesia tersisa di `pages/` dan `components/`
  (dibuktikan skrip). Rendering identik.

### Fase 3 — Pre-render · kemenangan SEO besar, masih satu bahasa

- Meta dikumpulkan saat render, bukan lewat efek.
- Modul yang menyentuh peramban saat impor dijaga.
- `scripts/prerender.ts`: satu HTML per rute publik.
- **Gerbang:** `curl` tanpa JavaScript mengembalikan HTML lengkap berisi judul,
  deskripsi, canonical, dan JSON-LD. Pratinjau WhatsApp menampilkan judul halaman
  yang benar — hari ini menampilkan judul beranda untuk semua halaman.

### Fase 4 — Infrastruktur dwibahasa · `/en` hidup, isinya masih sedikit

- `messages/en.json` (±1.500 kata — kerangka antarmuka).
- Pengalih bahasa lewat `contentKey`.
- `hreflang`, `canonical`, `og:locale`, sitemap dengan alternate.
- Pre-render per bahasa.
- **Gerbang:** validator `hreflang` bersih; hanya halaman yang benar-benar ada
  yang punya alternate.

### Fase 5 — Konten Inggris · bertahap, bergerbang

- Urutan yang disarankan: beranda → layanan (3) → harga → kontak → satu studi
  kasus → tentang → proses. Sisanya menyusul.
- **Gerbang per halaman:** sebuah halaman terbit di EN hanya jika seluruh
  teksnya sudah Inggris. Tidak ada terbit separuh.
- Vertikal **tidak** diterjemahkan (D5).

### Fase 6 — CMS dengan terjemahan sejak awal

- Tabel `locale` + kelas dasar translatable.
- Modul konten dimigrasi satu per satu; MDX jadi sumber awal impor.
- Panel: pengalih bahasa, terbit per bahasa, layar kelengkapan.

**Fase 1–3 layak dikerjakan bahkan seandainya Inggris dibatalkan besok.** Itu
bukan kebetulan — itu cara programnya disusun supaya tidak ada pekerjaan yang
terbuang kalau prioritas berubah.

---

## 9. Biaya konten — bagian yang membunuh proyek dwibahasa

±14.750 kata. Sekitar 60 halaman A4.

Dan ini **bukan penerjemahan.** Copy pemasaran harus di-*transcreate*: "Dibangun
untuk dipakai bertahun-tahun" tidak punya padanan harfiah yang tetap berbunyi
seperti janji. Studi kasus penuh idiom teknis Indonesia. Halaman harga memakai
rupiah dan istilah pasar lokal yang tidak berpindah utuh.

Perkiraan jujur, dikerjakan sendiri sambil mengerjakan yang lain: **beberapa
minggu**, bukan beberapa hari.

**Kegagalan yang paling lazim** bukan teknis: kodenya jadi, lalu konten Inggris
berhenti di 40%, dan situs punya `/en/` yang separuh berbahasa Indonesia. Itu
lebih buruk daripada tidak punya versi Inggris — Google menandainya konten tipis,
dan pengunjung Inggris menganggapnya situs terbengkalai.

Dua pengaman, keduanya sudah ada di rencana ini:

1. **Gerbang per halaman** (Fase 5) — halaman terbit hanya kalau lengkap.
2. **Layar kelengkapan di panel** (§6.4) — lubangnya kelihatan, bukan tersembunyi.

Dan rekomendasi lingkup: **jangan targetkan seluruh situs.** Mulai dari 5–6
halaman yang benar-benar dibutuhkan pembaca Inggris. Catatan teknis dan halaman
vertikal boleh selamanya hanya Indonesia — dan itu bukan cacat, itu keputusan.

---

## 10. Risiko

| Risiko | Tingkat | Penanganan |
| --- | --- | --- |
| Konten EN berhenti setengah jalan | **Tinggi** | Gerbang per halaman + layar kelengkapan (§9) |
| `hreflang` salah pasang → satu versi hilang dari indeks | Sedang | Timbal balik dibangkitkan otomatis dari `contentKey`, tidak ditulis tangan; diverifikasi lewat Search Console |
| Pre-render menyingkap asumsi khusus peramban | Sedang | Fase 3 berdiri sendiri, tidak bergantung Inggris; sudah dibuktikan 20 rute bisa dirender di Node |
| Slug berubah setelah terbit | Sedang | Peringatan di panel; setelah live wajib 301 di hosting — SPA tidak bisa mengembalikan 301 |
| Bundel membengkak | Rendah | Katalog dimuat per bahasa; tanpa pustaka i18n (D6) |
| URL berubah sebelum live | **Nihil** | Belum diindeks. Ini momen termurah — dan alasan mengerjakannya sekarang |
| Blueprint jadi usang | Rendah | 04, 09, 11 disinkronkan di Fase 1 |

---

## 11. Yang tidak dikerjakan

- **Bahasa ketiga.** Arsitekturnya menampung, datanya tidak dibuat.
- **Antarmuka panel dwibahasa** (§7).
- **Vertikal berbahasa Inggris** (D5).
- **Deteksi bahasa otomatis** (D10) — sengaja, bukan tertunda.
- **Mata uang & format regional.** Harga tetap rupiah di kedua bahasa; itu
  keputusan bisnis, bukan keputusan i18n. Kalau nanti perlu USD, itu rencana
  tersendiri.
- **Alat manajemen terjemahan** (Crowdin/Tolgee). Ambangnya di D6.

---

## 12. Keputusan yang diminta

**A. Sepuluh keputusan teknis D1–D10** — setuju, atau ada yang ingin diubah?

**B. Satu pertanyaan yang hanya Anda yang bisa jawab, dan ini yang paling
menentukan:**

> **Versi Inggrisnya untuk siapa?**

Jawabannya mengubah isi, bukan cuma bahasanya:

| Kalau audiensnya… | Maka versi Inggris… |
| --- | --- |
| Klien internasional / remote | Butuh copy berbeda: harga USD, cara kerja lintas zona waktu, referensi lokal Surabaya dikurangi. Bukan terjemahan — versi berbeda. |
| Perusahaan multinasional di Indonesia | Terjemahan cukup dekat, tetapi bukti dan referensinya digeser ke skala korporat. |
| Sekadar "supaya terlihat profesional" | ⚠️ Alasan paling lemah, dan yang paling sering berujung situs setengah jadi. Kalau ini alasannya, saya sarankan **tidak** dwibahasa — dan tetap mengerjakan Fase 1–3, yang memberi hampir seluruh manfaat teknisnya. |
| Studi kasus / portofolio kemampuan | Sah — tapi cukup 5–6 halaman inti, bukan seluruh situs. Halaman "Tentang" berbahasa Inggris yang rapi lebih meyakinkan daripada 20 halaman setengah jadi. |

**C. Urutan.** Fase 1–3 memberi nilai tanpa bergantung Inggris sama sekali. Saya
sarankan menyetujui Fase 1–3 sekarang, dan memutuskan Fase 4–5 setelah pertanyaan
B terjawab.

---

## 13. Catatan hasil

_Diisi setelah dikerjakan._
