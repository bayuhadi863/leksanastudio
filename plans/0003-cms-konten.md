# 0003 — CMS konten

- **Status:** Dikerjakan — F1 selesai · `case-study` selesai backend→panel→impor · 8 modul konten selesai di backend · `page-copy`, `site-profile`, dan situs publik belum
- **Tanggal:** 2026-08-24
- **Menyentuh:** backend (skema + modul), panel, frontend (sumber konten), infra (webhook)
- **Bergantung pada:** [0002](0002-arsitektur-multibahasa.md) Fase 1–3 (routing, copy keluar dari JSX, pre-render)
- **Sudah disepakati dalam diskusi:** garis "isi milik klien, bentuk milik pengembang";
  body = blok bertipe, sisanya field bertipe; editor blok v1 memakai tombol naik/turun

---

## 0. Ringkasan keputusan

| # | Pertanyaan | Keputusan |
| --- | --- | --- |
| D1 | Batas kendali klien | Isi ✅ · Bentuk ❌ — empat tingkat di §1.2 |
| D2 | Format body tulisan panjang | Deretan **blok bertipe** (JSONB), bukan HTML, bukan MDX |
| D3 | Daftar milik satu entri (FAQ, masalah, deliverable) | **Repeater di dalam entri** (JSONB), bukan tabel sendiri |
| D4 | Identitas vs alamat | `id` identitas · `slug` alamat · `content_key` opsional untuk yang ditunjuk kode |
| D5 | Slug berubah | Masuk `*_slug_history` → dijawab **301** |
| D6 | Copy halaman (hero, pengantar bagian) | **Slot bernama** yang dideklarasikan kode, bukan blok bebas |
| D7 | Struktur skema | Tabel induk + tabel terjemahan, **siap multibahasa sejak awal** |
| D8 | Cara situs publik mendapat konten | **Tarik saat build → pre-render.** Pratinjau ambil langsung dari API |
| D9 | Terbit | Webhook → build ulang (~1–2 menit). Bukan instan, dan itu disengaja |
| D10 | MDX | Jadi **sumber impor sekali jalan**, lalu dicabut dari runtime |
| D11 | Editor blok v1 | Naik/turun + lipat blok. Drag-drop menyusul |
| D12 | Tipe konten | Ditulis **di kode** per proyek, bukan dikonfigurasi klien |

---

## 1. Prinsip

### 1.1 Satu kalimat yang mengatur semuanya

> **CMS memberi kendali atas APA YANG DIKATAKAN, bukan atas BAGAIMANA TAMPILNYA.**

Klien memiliki isi. Pengembang memiliki bentuk. Penolakan ini bukan keterbatasan
yang perlu diminta maaf — itu bagian dari yang dibeli klien, dan yang membedakan
Leksana Studio dari jasa web yang menyerahkan page-builder lalu mencuci tangan.

### 1.2 Empat tingkat kendali

| Tingkat | Isi | Klien | Bentuk di panel |
| --- | --- | --- | --- |
| **1. Data** | Studi kasus · Catatan · Layanan · Paket & harga · Vertikal · FAQ · Kontak & janji | ✅ Penuh | Formulir berfield, tervalidasi, berbatas |
| **2. Copy** | Headline hero · Pengantar tiap bagian · Teks tentang | ✅ Dengan pagar | Slot bernama + batas karakter + pratinjau |
| **3. Struktur** | Bagian mana ada di halaman mana · Urutannya · Navigasi | ⚠️ Hanya tampil/sembunyi untuk bagian yang **ditandai opsional**. Urutan tidak | Sakelar, bukan drag-drop |
| **4. Bentuk** | Warna · Huruf · Jarak · Tata letak · Gaya komponen | ❌ Tidak ada | Tidak ada di panel |

### 1.3 Aturan turunan yang dipegang di seluruh sistem

1. **Slug tidak pernah jadi kunci.** Slug adalah alamat, `id` adalah identitas.
2. **Body panjang = blok. Sisanya = field bertipe.** Kalau semuanya dijadikan blok,
   yang sedang dibangun adalah page-builder.
3. **Tidak ada HTML mentah, tidak ada custom CSS, tidak ada eksekusi kode.** Satu
   field bebas saja membatalkan seluruh pagar di dokumen ini.
4. **Skema siap multibahasa sejak baris pertama**, meski hanya `id` yang diisi.
   Menambahkannya belakangan berarti migrasi seluruh tabel konten.

---

## 2. Inventaris: apa yang jadi apa

Seluruh konten yang hari ini hidup di repo, dan ke mana perginya.

| Sumber sekarang | Modul CMS | Bentuk | Prioritas |
| --- | --- | --- | --- |
| `content/studi-kasus/*.mdx` | `case-study` | Field + **body blok** | **1** |
| `config/site.ts` | `site-profile` (singleton) | Field | **2** |
| `config/services.ts` | `service` | Field + repeater | **3** |
| `config/packages.ts` → `businessPackages`, `corporatePackages` | `service-package` | Field + repeater fitur | **3** |
| `config/packages.ts` → `systemPhases` | `project-phase` | Field | 3 |
| `config/packages.ts` → `addOns`, `paymentTerms` | `add-on`, `payment-term` | Field | 3 |
| `config/packages.ts` → `PRICE_FLOOR` | `site-profile` | Angka | 3 |
| `config/verticals.ts` | `vertical` | Field + repeater | **4** |
| `content/catatan/*.mdx` | `note` | Field + **body blok** | 5 |
| `config/process.ts` → `processSteps` | `process-step` | Field + repeater | 6 |
| `config/process.ts` → `commonFears` | Repeater di `page-copy` (proses) | QA | 6 |
| `config/copy.ts` → `homeFaq` | Repeater di `page-copy` (beranda) | QA | 6 |
| `config/copy.ts` → `outOfScope`, `proofPillars` | Repeater di `page-copy` | Field | 6 |
| `config/copy.ts` → `home`, `about` | `page-copy` (slot bernama) | Slot | 6 |
| Prosa tertanam di JSX (±7.000 kata) | `page-copy` per halaman | Slot | 6 |
| `pages/PrivacyPage.tsx` | `page-document` (privasi) | **Body blok** | 7 |

Yang **tidak** masuk CMS: `routes.ts`, `panel.ts`, `i18n.ts` — itu struktur
(tingkat 3–4), milik pengembang.

### 2.1 Relasi yang sekalian diperbaiki

Hari ini dua hubungan disimpan sebagai slug bebas yang tidak dijamin siapa pun:

```ts
service.caseStudySlug = 'p3m-pens'    → jadi FK case_study_id
vertical.serviceSlug  = 'website-bisnis' → jadi FK service_id
```

Menjadi kunci asing berarti basis data yang menolak referensi menggantung, dan
slug boleh berubah tanpa memutus tautan.

---

## 3. Model konten

### 3.1 Field bertipe vs blok — garisnya

| Bagian | Bentuk | Alasan |
| --- | --- | --- |
| Judul, ringkasan, masalah, peran, klien, durasi, tahun | Field bertipe | Selalu ada, selalu satu, bentuknya tetap |
| Metrik (tepat 3) | Repeater bertipe, **dikunci 3** | Blueprint 05 menetapkan tiga. Formulir yang menegakkannya |
| Stack, deliverable, exclusion | Repeater teks | Daftar sederhana |
| Masalah (judul + isi), FAQ (tanya + jawab) | Repeater objek | Bentuk tetap, jumlah bebas |
| **Badan tulisan panjang** | **Blok** | Hanya di sini urutan dan jenis isi benar-benar bebas |

**Blok hanya untuk badan tulisan.** Studi kasus, catatan, dan halaman dokumen.
Tidak ada lagi.

### 3.2 Kenapa blok, bukan HTML dari WYSIWYG

Isi studi kasus sekarang memakai komponen rumah — `<Decision>`, `<Figure>`,
`<Metrics>`, `<Note>` — dan itulah yang membuatnya bukan blog post biasa.
Blueprint 05 bahkan mengunci formatnya: *"Saya memilih X karena Y, walaupun Z.
Keputusan tanpa alternatif yang ditolak bukan keputusan."*

HTML blob membuang semuanya. Blok justru **menegakkannya**:

| | MDX sekarang | Blok |
| --- | --- | --- |
| Format rumah | Bergantung disiplin penulis | Dipaksa formulir — `despite` wajib diisi |
| Eksekusi kode | Ya (MDX = JSX) — tidak aman untuk klien | Tidak. Blok adalah data |
| Validasi | Frontmatter saja | Skema per blok, ditegakkan server |
| Dipakai ulang | Hanya render React | HTML, PDF, RSS, ringkasan OG, indeks pencarian |

---

## 4. Katalog blok

Delapan tipe. Setiap tipe punya komponen React yang **sudah ada**.

| Tipe | Field | Komponen | Batas |
| --- | --- | --- | --- |
| `richText` | `html` | prosa `.copy` | Daftar putih tag; maks 4.000 karakter |
| `heading` | `level` (2\|3), `text` | `h2`/`h3` | Maks 90 karakter |
| `decision` | `step`, `title`, `chose`, `because`, `despite`, `body[]` | `Decision` | Semua wajib. `body[]` hanya `richText` |
| `figure` | `variant` (system\|website\|catalog), `mediaId?`, `alt`, `caption` | `Figure` / `Schematic` | `alt` wajib, min 10 karakter |
| `metrics` | `items[{ value, label }]` | `Metrics` → `MetricBlock` | **Tepat 3** |
| `note` | `html` | `Note` | **Maks 4 per dokumen** · 20–45 kata |
| `codeBlock` | `language`, `code` | `pre > code` | — |
| `table` | `head[]`, `rows[][]` | `Table` (`.scroll-x`) | Maks 8 kolom |

Perhatikan baris `note` dan `metrics`: aturan yang selama ini hanya catatan di
blueprint 06b dan 05 sekarang **ditegakkan mesin**. Itu peningkatan kualitas
nyata, bukan sekadar pemindahan penyimpanan.

### 4.1 Bentuk tersimpan

```json
[
  { "id": "b1", "type": "heading", "level": 2, "text": "Masalah" },
  { "id": "b2", "type": "richText",
    "html": "<p>P3M adalah unit yang mengurus seluruh penelitian…</p>" },
  { "id": "b3", "type": "metrics", "items": [
      { "value": "30", "label": "Modul" },
      { "value": "34", "label": "Entitas" },
      { "value": "~73.000", "label": "Baris kode" } ] },
  { "id": "b4", "type": "decision",
    "step": 1,
    "title": "Menyelundupkan metode yang diblokir lewat POST",
    "chose": "mempertahankan REST yang benar dan menambah lapisan penerjemah",
    "because": "kendala ini milik satu lingkungan, bukan milik sistemnya",
    "despite": "dua mekanisme harus selalu diubah berpasangan",
    "body": [ { "id": "b4a", "type": "richText", "html": "<p>Frontend mengirim PUT…</p>" } ] },
  { "id": "b5", "type": "note",
    "html": "<p>Angka mobile-nya 68. Penyebabnya SPA di server institusi tanpa CDN.</p>" }
]
```

`id` per blok stabil — dipakai React sebagai `key`, dan nanti untuk menautkan
terjemahan blok per blok.

### 4.2 Menulis prosa panjang tetap cepat

Keberatan yang sah terhadap block editor: menulis paragraf panjang jadi lambat.
Karena itu `richText` menerima **pintasan markdown** di editornya — ketik `## `
jadi heading, `**tebal**` jadi tebal, `- ` jadi daftar. Kecepatan menulisnya
setara markdown.

Yang **tidak** ada di `richText`: ukuran huruf, warna, rata tengah, tabel, HTML
mentah, tempel bergaya dari Word. Daftar putih tag, disaring **di server** —
bukan hanya di editor, karena editor bisa dilewati.

---

## 5. Skema basis data

### 5.1 Fondasi

```
locale
  code            'id' | 'en'        PK
  name, native_name
  is_default      bool               tepat satu true
  is_active       bool
  order           int

media
  id, object_path (MinIO), mime, size_bytes, width, height
  original_name, …audit + soft delete
  -- alt TIDAK di sini: alt bergantung konteks pemakaian, bukan berkas
```

`locale` jadi **tabel, bukan enum**: menambah bahasa jadi satu baris data, bukan
migrasi + deploy.

### 5.2 Pola induk + terjemahan

Diterapkan sama persis ke setiap modul yang punya teks:

```
case_study                          ← tidak bergantung bahasa
  id            uuid       PK
  content_key   text NULL  UNIQUE   hanya untuk yang ditunjuk kode/seeder
  label         'klien' | 'produk-sendiri'
  figure        'system' | 'website' | 'catalog'
  cover_media_id → media.id NULL
  year          int
  order         int
  stack         jsonb      ['React 19', 'TypeScript', …]
  metrics       jsonb      [{ value, label }] — tepat 3
  …audit + soft delete

case_study_translation              ← bergantung bahasa
  id            uuid       PK
  case_study_id → case_study.id
  locale_code   → locale.code
  slug          text
  title, summary, problem, client, kind, duration, role   text
  cover_alt     text
  body          jsonb      ← deretan blok
  status        'draft' | 'published'
  published_at  timestamptz NULL
  …audit + soft delete

  UNIQUE (case_study_id, locale_code)   satu terjemahan per bahasa
  UNIQUE (locale_code, slug)            slug unik dalam satu bahasa
```

Pembagiannya konsisten: **apa pun yang bunyinya sama di semua bahasa** (angka
tahun, urutan, nama teknologi, referensi berkas) tinggal di induk. Sisanya di
terjemahan.

### 5.3 Riwayat slug — 301 yang biasanya dilupakan

```
case_study_slug_history
  id, case_study_id, locale_code, old_slug, changed_at, changed_by
  INDEX (locale_code, old_slug)
```

Slug diubah → slug lama tercatat → permintaan ke slug lama dijawab **301** ke slug
baru. Tanpa ini, tiap kali klien merapikan judul, satu URL yang sudah diindeks
Google mati diam-diam.

### 5.4 Modul lain

Pola yang sama, isinya berbeda:

| Modul | Induk (tak bergantung bahasa) | Terjemahan |
| --- | --- | --- |
| `note` | `order`, `pillar` | slug, title, summary, published, **body blok** |
| `service` | `slug_key`, `starting_price`, `pricing_shape`, `case_study_id` FK, `order` | slug, name, short_name, audience, headline, summary, starting_price_label, `problems[]`, `deliverables[]`, `exclusions[]`, `faq[]` |
| `vertical` | `service_id` FK, `pricing_shape`, `order` | slug, industry, headline, intro, note, whatsapp_intro, `problems[]`, `deliverables[]`, `faq[]` |
| `service_package` | `group` (business\|corporate), `code`, `price`, `highlighted`, `order` | name, audience, summary, price_note, duration, `features[{label,value}]` |
| `project_phase` | `step`, `order` | name, price, duration, scope, note |
| `add_on` | `order` | name, price, applies_to |
| `payment_term` | `order` | scope, schedule |
| `process_step` | `step`, `order` | title, duration, summary, `details[]`, client_input |
| `page_copy` | `page_code` (home\|pricing\|about\|…) | `slots` jsonb, meta_title, meta_description |
| `page_document` | `page_code` (privacy\|terms) | slug, title, **body blok**, updated_label |
| `site_profile` | singleton: whatsapp_number, email, social{}, promises{}, price_floor | tagline, description, city, region |

Repeater (`problems[]`, `faq[]`, `features[]`) disimpan **JSONB di baris
terjemahan**, bukan tabel sendiri (D3). Alasannya: daftar itu dimiliki satu entri,
tidak pernah dicari lintas entri, dan menyuntingnya harus terjadi di layar yang
sama dengan induknya. Tabel terpisah hanya menambah join dan memaksa klien pindah
layar.

### 5.5 Copy halaman = slot bernama (D6)

Tata letak tidak boleh diubah klien (tingkat 3), jadi copy halaman **bukan** blok
bebas — melainkan slot yang dideklarasikan kode:

```
page_copy_slot_definition        ← di-seed dari manifes di kode, seperti MenuSeeder
  page_code   'home'
  slot_key    'hero.headline'
  label       'Judul utama'
  kind        'text' | 'textarea' | 'richText' | 'list'
  max_length  90
  required    true
  order       int
```

Panel membangun formulirnya dari deklarasi ini. Menambah slot = mengubah manifes
+ deploy — karena menambah slot **adalah keputusan desain**, dan itu pekerjaan
pengembang. Sama persis dengan cara `menu` sudah bekerja sekarang.

---

## 6. Backend

### 6.1 Kelas dasar baru — sejajar dengan yang sudah ada

Mengikuti pola yang ada, bukan menyaingi:

| Baru | Sejajar dengan |
| --- | --- |
| `BaseTranslatableEntity` / `BaseTranslationEntity` | `BaseEntity` |
| `ITranslatableRepository<TEntity, TTranslation>` | `IBaseRepository<T>` |
| `BaseTranslatableRepository<…>` | `BaseRepository<T>` |
| `BaseTranslatableCrudService<…>` | `BaseCrudService<…>` |
| `BaseTranslatableCrudController<…>` | `BaseCrudController<…>` |
| `LocaleController` + `LocaleSeeder` | `MenuController` + `MenuSeeder` |
| `MediaController` (pakai `FileService`+`StorageService` yang ada) | — |
| `BlockValidator` (FluentValidation per tipe blok) | `BaseRequestValidator<T>` |

Tanpa kelas dasar ini, tiap modul akan menulis ulang join, aturan slug, dan
penanganan status terbitnya sendiri — dan salah satunya pasti akan salah.

### 6.2 Dua permukaan API, sengaja dipisah

```
/api/v1/case-study/**            PANEL — [JwtAuthorize] + [MenuCode] + [RequirePermission]
                                  mengembalikan SEMUA terjemahan, draf maupun terbit

/api/v1/public/case-study**      PUBLIK — [AllowAnonymous]
                                  ?locale=id, hanya status = published
                                  dipakai skrip build

/api/v1/preview/case-study/{id}  PRATINJAU — [JwtAuthorize]
                                  draf pun dikembalikan, untuk layar pratinjau
```

Pemisahan ini bukan gaya. Endpoint publik yang bisa menampilkan draf berarti
konten yang belum siap bisa ditemukan orang — dan itu jenis kebocoran yang tidak
terlihat sampai terjadi.

### 6.3 Validasi blok

Blok divalidasi **di server**, bukan hanya di editor:

- Tipe blok harus dikenal; tipe asing ditolak, bukan diabaikan.
- Field wajib per tipe (`decision.despite` tidak boleh kosong).
- `metrics` tepat 3 · `note` maks 4 per dokumen · batas karakter.
- `richText.html` disaring daftar putih: `p, strong, em, a, ul, ol, li, code, br`.
  Atribut hanya `href` (dan `href` wajib `http(s):`, `mailto:`, atau `/`).

**Duplikasi yang harus diakui:** skema blok hidup dua kali — FluentValidation di
C#, tipe TypeScript di frontend. Tidak ada cara menghindarinya tanpa membangun
generator. Penanganannya: kontrak ditulis di satu dokumen (§4), dan ada uji
kontrak yang menjalankan berkas JSON contoh melewati kedua validator.

---

## 7. Panel

### 7.1 Editor blok v1

Sesuai kesepakatan: tombol naik/turun, bukan drag-drop.

```
Body

┌──────────────────────────────────────────────┐
│ ▾ HEADING                        ↑  ↓   ✕   │
│   Masalah                                    │
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│ ▾ TEKS                           ↑  ↓   ✕   │
│   [ B  I  🔗  •  1.  </> ]                   │
│   P3M adalah unit yang mengurus seluruh      │
│   penelitian, publikasi, dan pengabdian…     │
│                             1.204 / 4.000    │
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│ ▸ KEPUTUSAN 1 — Menyelundupkan metode…       │  ← dilipat
│                                  ↑  ↓   ✕   │
└──────────────────────────────────────────────┘

            [ + Tambah blok ▾ ]
     Teks · Heading · Keputusan · Gambar
     Metrik · Catatan · Kode · Tabel
```

- **Lipat/buka per blok** — ini yang menyelesaikan masalah dokumen panjang,
  bukan drag-drop.
- Pindah ke drag-drop nanti murni ganti UI: datanya tetap array berurutan, tidak
  ada migrasi dan tidak ada perubahan API.

### 7.2 Pagar pengaman — wajib ada

| Pagar | Kenapa |
| --- | --- |
| Penghitung karakter + batas ditegakkan **server** | Slot 12 kata diisi 80 kata merusak ritme tipografi |
| Field wajib tidak bisa dikosongkan (H1, meta description, `alt`) | Rusaknya SEO tidak terlihat sampai tiga bulan kemudian |
| Gambar: rasio + ukuran maksimal + `alt` wajib | Aksesibilitas dan tata letak |
| Rich text daftar putih, disaring server | Satu celah HTML membatalkan semua pagar lain |
| Draf → pratinjau → terbit | Tidak ada yang tayang tanpa pernah dilihat |
| Peringatan saat mengubah slug terbit | "Tautan lama akan dialihkan otomatis, tapi bagikan ulang tautan barunya" |
| Jejak audit (`CreatedBy`/`UpdatedBy` — sudah ada) | Tahu siapa mengubah apa |

### 7.3 Pratinjau

Tombol **Pratinjau** membuka situs dalam mode pratinjau:

```
/portofolio/p3m-pens?preview=<token>
```

- Token berumur pendek, terikat pengguna, dikeluarkan panel.
- Situs berjalan sebagai SPA di mode ini: ambil dari `/api/v1/preview/**`,
  **tanpa build**, jadi instan.
- Header `X-Robots-Tag: noindex` + meta `noindex`.

### 7.4 Klien Anda memakai HP

Pemilik usaha kecil di Surabaya, bukan pengembang. Konsekuensi nyata:

- Panel harus enak dipakai di layar 390px — termasuk editor blok.
- Label bahasa Indonesia, tanpa jargon. "Alamat halaman", bukan "slug".
- Pesan galat menyebutkan **apa yang harus dilakukan**, bukan apa yang salah.
- Tombol simpan selalu terlihat, tidak tersembunyi di bawah gulir panjang.

---

## 8. Bagaimana konten sampai ke pengunjung

### 8.1 Dua jalur, dua tujuan

```
TERBIT
  panel  ──►  status = published  ──►  webhook  ──►  build
                                                      │
                                    tarik /api/v1/public/**  (hanya published)
                                    unduh gambar MinIO → dist/
                                    pre-render HTML per halaman
                                    bangun sitemap + robots
                                                      │
                                                  ~1–2 menit  ──►  live

PRATINJAU
  panel  ──►  buka /portofolio/<slug>?preview=<token>
              SPA ambil /api/v1/preview/**  ──►  instan, noindex, butuh login
```

### 8.2 Kenapa build-time, bukan fetch saat halaman dibuka

| | SPA fetch saat dibuka | **Tarik saat build** ✅ |
| --- | --- | --- |
| HTML untuk perayap | Kosong | Penuh |
| Kecepatan buka | Menunggu API | Ratusan milidetik |
| Backend mati | **Situs publik ikut mati** | Situs tetap hidup |
| Terbit | Instan | ~1–2 menit |

Untuk situs yang inti bisnisnya SEO (blueprint 08), kolom kanan tidak bisa
ditawar. Dan konsekuensi "situs publik tidak bergantung backend sama sekali"
adalah argumen jualan yang kuat: server CMS boleh mati semalaman, situsnya tetap
melayani.

**Yang harus disampaikan ke klien sejak awal:** terbit butuh 1–2 menit. Itu
normal, dan pratinjau menutupi jedanya.

### 8.3 Kalau build gagal karena backend mati

Konten hasil tarikan terakhir disimpan sebagai berkas di repo build
(`.content-cache/`). Backend tidak bisa dihubungi saat build → pakai cache
terakhir dan **peringatkan**, jangan gagalkan build. Situs yang gagal deploy
karena CMS sedang restart adalah kerapuhan yang tidak perlu.

---

## 9. Migrasi

### 9.1 MDX → blok

MDX tidak dibuang — dipakai sekali sebagai sumber impor:

```
skrip impor
  ├─ baca frontmatter          → kolom induk + terjemahan
  ├─ ## heading                → { type: 'heading' }
  ├─ paragraf / daftar / kode  → { type: 'richText' } / { type: 'codeBlock' }
  ├─ <Decision …>              → { type: 'decision', … }
  ├─ <Figure …>                → { type: 'figure', … }
  ├─ <Metrics items={…}>       → { type: 'metrics', … }
  ├─ <Note>                    → { type: 'note' }
  ├─ <Annotation note="…">     → blok isinya + { type: 'note' } sesudahnya
  └─ tabel                     → { type: 'table' }
```

- Idempoten lewat `content_key` — bisa dijalankan berulang.
- Gambar di `public/portofolio/` diunggah ke MinIO, `cover_media_id` diisi.
- **Verifikasi:** render hasil impor, bandingkan teksnya dengan render MDX asli.
  Beda = impor salah, bukan "cukup mirip".

### 9.2 Config TS → basis data

`services.ts`, `verticals.ts`, `packages.ts`, `process.ts`, `copy.ts`, `site.ts`
jadi seeder — persis pola `MenuSeeder`/`RoleSeeder` yang sudah ada. Idempoten,
aman dijalankan ulang, dan **tidak menimpa** yang sudah disunting lewat panel.

### 9.3 Setelah migrasi

`import.meta.glob` MDX, `@mdx-js/rollup`, `remark-*`, dan modul virtual waktu-baca
**dicabut dari frontend**. Dua pipeline konten berarti tiap fitur render harus
dibangun dua kali, dan yang kedua selalu ketinggalan.

Berkas MDX-nya sendiri disimpan di `archive/` sebagai catatan sejarah, bukan
sebagai sumber.

---

## 10. Izin

Menu baru per modul, mengikuti mekanisme yang sudah jalan:

```
case-study · note · service · service-package · vertical
process · page-copy · page-document · media · site-profile · locale
```

| Peran | Akses |
| --- | --- |
| `super-admin` | Semua, termasuk `user`, `role`, `locale`, `site-profile` |
| `editor` | Seluruh menu konten. **Tanpa** `user`, `role`, `locale` |

Pemisahan itu yang membuat panel aman diserahkan: editor mengurus isi, dan tidak
bisa memberi dirinya sendiri akses tambahan. Ini sudah berlaku hari ini —
modul-modul baru tinggal masuk daftar.

Satu keputusan tambahan yang layak diambil: **`site-profile` hanya `super-admin`.**
Nomor WhatsApp dan email adalah saluran masuk lead; salah ketik di sana lebih
mahal daripada salah ketik di mana pun.

---

## 11. Fase eksekusi

Prasyarat: [0002](0002-arsitektur-multibahasa.md) Fase 1–3 (routing, copy keluar
dari JSX, pre-render). Fase 2 di sana memindahkan prosa JSX ke slot bernama di
berkas lokal — **slot yang sama** yang nanti diisi CMS di Fase F6 di bawah. Jadi
pekerjaan itu batu loncatan, bukan pekerjaan ganda.

| Fase | Isi | Situs publik | Nilai berdiri sendiri |
| --- | --- | --- | --- |
| **F1** | Fondasi: `locale`, kelas dasar translatable, `media`, `MediaController`, validator blok | Tidak berubah | Prasyarat semua |
| **F2** | Modul `case-study` penuh: entitas, repo, service, controller, panel + **editor blok** | Tidak berubah — masih baca MDX | Panel bisa dipakai, konten bisa disiapkan |
| **F3** | Impor MDX → DB · build tarik dari API · pratinjau · webhook terbit | **Berubah sumbernya**, tampilan identik | **Loop lengkap untuk satu modul** |
| **F4** | Modul `note` | Berubah sumbernya | Murah — pola sudah ada |
| **F5** | Modul terstruktur: `service`, `service-package`, `project-phase`, `add-on`, `payment-term`, `vertical` | Berubah sumbernya | Harga & layanan bisa diurus sendiri |
| **F6** | `page-copy` (slot) + `process-step` + repeater FAQ/outOfScope/proofPillar | Berubah sumbernya | Copy halaman bisa diurus sendiri |
| **F7** | `page-document` (privasi) + `site-profile` | Berubah sumbernya | Ganti nomor WhatsApp tanpa deploy |
| **F8** | Penyempurnaan: riwayat versi, drag-drop, kelengkapan terjemahan | — | Setelah dipakai nyata |

**F1–F3 adalah irisan vertikal lengkap** — satu modul, dari basis data sampai
halaman terbit. Itu sengaja: risiko terbesar rencana ini ada di editor blok, dan
irisan vertikal membuktikan atau menggugurkannya lebih awal, sebelum sebelas modul
lain dibangun di atas asumsi yang belum teruji.

Urutan F5 sebelum F6 disengaja: **harga dan layanan lebih sering berubah daripada
headline halaman.**

---

## 12. Risiko

| Risiko | Tingkat | Penanganan |
| --- | --- | --- |
| **Editor blok jadi lubang tanpa dasar** | **Tinggi** | v1 sengaja jelek: naik/turun, tanpa drag-drop, tanpa animasi. Dibuktikan di F2–F3 sebelum modul lain menumpuk |
| Dua validator (C# + TS) melenceng | Sedang | Kontrak di §4 + uji kontrak berkas JSON lewat kedua sisi |
| Build gagal karena backend mati | Sedang | Cache konten + peringatan, bukan gagal (§8.3) |
| Waktu build tumbuh seiring konten | Rendah | Puluhan halaman, bukan ribuan. Ditinjau ulang di 200+ halaman |
| Klien menempel 400 kata di slot 20 kata | Sedang | Batas karakter ditegakkan server (§7.2) |
| Token pratinjau bocor → draf terbaca | Rendah | Umur pendek, terikat pengguna, `noindex` |
| Impor MDX diam-diam kehilangan isi | Sedang | Verifikasi banding teks, bukan "kelihatannya benar" (§9.1) |
| Slug diubah → tautan lama mati | Sedang | `*_slug_history` → 301 (§5.3) |

---

## 13. Yang tidak dikerjakan

- **Drag-drop blok** — v2, murni ganti UI.
- **Riwayat versi + rollback** — jejak audit dulu. Snapshot per terbit menyusul.
- **Tipe konten yang bisa dikonfigurasi klien** (ala Strapi) — model bisnisnya
  bespoke, bukan self-service (D12).
- **Terbit terjadwal**, alur persetujuan, komentar, penguncian saat disunting
  bersamaan — belum ada penggunanya (satu sampai dua editor per situs).
- **Kustomisasi warna/huruf/tata letak** — tingkat 4, tidak akan pernah ada.
- **Field HTML/CSS bebas** — sengaja, selamanya. Satu field bebas membatalkan
  seluruh dokumen ini.
- **Konten bahasa Inggris.** Skemanya siap; hanya baris `id` yang diisi.

---

## 14. Keputusan yang diminta

1. **Katalog blok §4 — delapan tipe.** Ada yang kurang, atau ada yang bisa
   dipangkas untuk v1? (`table` dan `codeBlock` hanya dipakai catatan teknis;
   bisa ditunda kalau mau memangkas F2.)
2. **Urutan F5 sebelum F6** — layanan & harga lebih dulu daripada copy halaman.
   Setuju?
3. **`site-profile` dikunci `super-admin`** (§10) — setuju?
4. **Batas karakter** perlu ditetapkan angkanya per field. Saya usulkan menurunkan
   dari panjang teks yang ada sekarang + 20%, lalu ditinjau setelah dipakai.

---

## 15. Catatan hasil

### Sudah selesai dan terverifikasi

**F1 — Fondasi**

- `locale` sebagai tabel (bukan enum), `media`, `slug_history`.
- `BaseTranslatableEntity` / `BaseTranslationEntity`, `BaseTranslatableCrudService`,
  `GenericRepository<T>` open-generic (tabel terjemahan tidak butuh berkas repo).
- `BlockDocumentProcessor`: validasi + sanitasi HTML lewat pustaka (Ganss.Xss 9.2.995,
  audit kerentanan bersih), normalisasi, id blok otomatis.
- Skema penuh: **35 tabel**, enum disimpan sebagai teks, kolom `jsonb` benar,
  indeks unik berfilter `(parentid, localecode) WHERE NOT isdeleted`.
- 17 menu + 30 grant peran ter-seed. `editor` dapat 12 menu konten,
  **tanpa** `user` / `role` / `locale` / `site-profile`.

**F2 — Modul `case-study` (backend)**

- Entitas, DTO, service, controller, validator, endpoint publik, reorder.
- `GET /api/v1/public/block-schema` menyajikan kontrak blok, jadi panel dan
  server tidak bisa melenceng.

**Terverifikasi otomatis — 19/19 lolos**

Termasuk: `<script>` di rich text tersapu bersih; metrik wajib tepat 3;
catatan pinggir maksimal 4; blok keputusan menolak `despite` kosong; slug unik
per bahasa (409); slug lama menjawab `MOVED_PERMANENTLY`; draf tidak pernah
muncul di endpoint publik; endpoint panel menolak anonim (401).

### Yang meleset dari rencana

**Satu bug arsitektural ditemukan saat pengujian, bukan saat penulisan.**
Mapster ikut memetakan koleksi `Translations` ke entitas induk, membuat baris
terjemahan baru dengan id acak yang lalu di-`UPDATE` dan tidak cocok baris mana
pun (`DbUpdateConcurrencyException`). Diperbaiki di kelas dasar dengan
`TypeAdapterConfig` yang mengecualikan `Translations` — sekali, di satu tempat,
sehingga 11 modul berikutnya tidak bisa mengulanginya.

Rencana tidak menyebut risiko ini sama sekali. Catatan untuk rencana berikutnya:
pemetaan otomatis ke entitas bernavigasi selalu perlu daftar pengecualian
eksplisit.

**F2 — Panel `case-study` + pustaka berkas**

Perangkat panel yang dipakai ulang oleh 11 modul berikutnya, bukan sekali pakai:
`PanelPage`, `DataTable`, `FormSection` (jalur catatan pinggir situs publik
dipakai lagi sebagai penjelasan field), `LanguageTabs`, `SaveBar`,
`ConfirmDialog`, kumpulan input, `SlugInput`, `Repeater`, `RichTextInput`
(Tiptap dibatasi persis ke daftar izin server), `BlockEditor` + `BlockForm`,
`MediaPicker` / `MediaField` / `UploadZone` / `MediaGrid`.

Halaman: daftar portofolio, formulir studi kasus (buat + sunting), pustaka
berkas dengan unggah, penamaan, dan hapus.

Keputusan UI/UX yang diambil di sini:

- **Ruang lingkup field terlihat.** Tiap bagian formulir diberi label
  "Semua bahasa" atau "Bahasa ID". Tanpa itu, panel multibahasa selalu berakhir
  dengan tahun yang berbeda-beda untuk proyek yang sama.
- **Tab bahasa selalu tampil, termasuk yang kosong.** Situs setengah
  diterjemahkan gagal diam-diam; satu-satunya tempat kegagalan itu harus
  berisik adalah layar tempat terjemahannya ditulis.
- **Gambar dipilih dari pratinjau, bukan dari nama berkas.** Field yang hanya
  menampilkan `cover.jpg` adalah cara gambar salah naik ke situs.
- **Batas blok dibaca dari server** (`/public/block-schema`), bukan disalin
  ulang di frontend.
- **Simpan tetap di tempat** (bar lengket) dan menolak jalan saat ada isian yang
  belum benar — validasi klien meniru aturan server, bukan menggantikannya.

**Terverifikasi otomatis — 27/27 lolos (Playwright, alur nyata di peramban)**

Unggah berkas → penamaan → daftar; buat studi kasus lengkap dengan dua blok →
tersimpan (201) → alamat pindah ke mode sunting; isi bertahan setelah muat
ulang; tombol simpan mati saat tidak ada perubahan; formulir menolak simpan
sebelum isian wajib lengkap **tanpa mengirim permintaan**; terbit tanpa metrik
ditolak; entri terbit langsung terbaca di endpoint publik lengkap dengan
bloknya; pemilih gambar mengisi sampul dan blok gambar (`mediaId` + `src`
tersimpan); tidak ada gulir horizontal di layar 390px.

### Yang meleset dari rencana (2)

**Dialog tertutup ternyata tampil.** `<dialog>` disembunyikan peramban lewat
`dialog:not([open]) { display: none }` di *user agent stylesheet* — dan aturan
penulis, sespesifik apa pun, selalu menang atas itu. Satu kelas `flex` pada
elemen dialognya membuat pemilih gambar tampil permanen di tengah formulir.
Ditemukan dari tangkapan layar, bukan dari test: semua pemeriksaan fungsional
lolos karena dialognya memang bekerja — hanya tidak pernah bersembunyi.

Diperbaiki dua lapis: tata letak flex dipindah satu tingkat ke dalam, dan
`dialog:not([open]) { display: none !important }` ditambahkan ke lapis dasar
supaya kesalahan yang sama tidak bisa terulang di dialog berikutnya.

**F2 — Delapan modul konten berikutnya (backend)**

`note`, `service`, `vertical`, `service-package`, `project-phase`, `add-on`,
`payment-term`, `process-step`, `page-document`.

Sebelum menulisnya, kelas dasar dinaikkan dulu supaya modul ke-9 sampai ke-12
tidak menyalin apa pun:

- `BaseTranslatableCrudService` sekarang mengisi sendiri ringkasan bahasa tiap
  baris daftar, memuat terjemahan untuk formulir, menyaring status, mengurutkan
  ulang (`IHasOrder`), mengambil entri terbit per bahasa, dan menyusun
  `alternates` untuk hreflang.
- `BaseTranslatableCrudController` menyediakan endpoint `reorder`.
- `ContentRules` memegang aturan yang berulang: bentuk slug, satu bahasa satu
  baris, dan bentuk-bentuk daftar JSON.

Hasilnya: satu modul baru = DTO + service (rata-rata 90 baris) + controller
(20 baris) + validator. `CaseStudyService` sendiri turun dari 290 ke 196 baris
setelah dipindah ke kelas dasar.

Aturan yang dipasang di validator adalah janji halaman, bukan gaya:

| Modul | Ditolak saat terbit |
| --- | --- |
| `service` | tanpa audiens, tanpa satu pun keluaran |
| `vertical` | tanpa satu pun masalah khas industri itu |
| `project-phase` · `add-on` | tanpa harga |
| `payment-term` | tanpa jadwal pembayaran |
| `process-step` | tanpa "yang perlu disiapkan klien" |
| `page-document` | tanpa isi tulisan |
| `service-package` | tanpa satu pun baris perbandingan |

Ditambah satu aturan yang dijaga saat menulis, bukan saat mengisi formulir:
**satu paket unggulan per tabel**. Dua kolom yang ditonjolkan sama saja dengan
tidak ada yang ditonjolkan.

Endpoint publik bertambah: `note`, `service`, `vertical`, `page-document`
(termasuk pencarian lewat kode halaman), `process`, dan `pricing` — satu
tanggapan berisi paket, tahap, tambahan, dan termin sekaligus, karena keempatnya
dibaca bersamaan dan build yang merakit satu halaman dari empat permintaan gagal
dengan empat cara.

**Terverifikasi otomatis — 32/32 lolos (API nyata, basis data nyata)**

Termasuk: draf tidak pernah muncul di endpoint publik; tiap aturan "ditolak saat
terbit" di tabel atas benar-benar menolak (400 dengan pesan yang bisa dibaca
editor); paket unggulan kedua mematikan yang pertama; alamat catatan yang
berubah menjawab `MOVED_PERMANENTLY`; `reorder` benar-benar mengubah urutan;
endpoint panel menolak anonim (401). Semua data uji dihapus lagi setelah lulus.

**Impor MDX → basis data (§9.1)**

`frontend/scripts/import-content.mjs` memindahkan dua studi kasus yang ada ke
CMS: 36 blok (1.494 kata) dan 17 blok (1.070 kata), termasuk tiga tangkapan layar
yang naik ke MinIO dan tertaut sebagai sampul dan blok gambar.

Skrip menulis lewat API panel, bukan lewat basis data — aturan yang menolak
seorang editor juga menolak skrip ini. Idempoten lewat `contentKey`; dijalankan
tiga kali, hasilnya tetap dua entri.

**Verifikasi yang dituntut rencana ini benar-benar dijalankan.** Bukan "cukup
mirip": kata-kata sumber dan kata-kata blok dibandingkan berurutan, lalu
dibandingkan sekali lagi setelah dibaca ulang dari basis data. Satu kata meleset
menghentikan impor dan menyebut kata keberapa. `<Annotation>` adalah satu-satunya
penyusunan ulang (isi dulu, catatan pinggir sesudahnya), dan pemeriksanya tahu.

**Dua cacat yang hanya muncul setelah ada data nyata:**

1. **Daftar konten tidak menghormati urutan tampil.** `ApplyFilter` mengurutkan
   dengan `OrderBy(Order)`, tetapi repositori menimpanya dengan "terbaru dulu"
   ketika `sortBy` kosong — jadi urutan yang disunting editor tidak pernah
   terlihat, dan kontrol pengurutan yang akan dibuat nanti akan berbohong.
   Diperbaiki di `BaseTranslatableCrudService`: tanpa `sortBy`, daftar konten
   diurutkan berdasarkan `Order` menaik. `OrderBy` yang sia-sia di sembilan modul
   ikut dibuang, karena kode mati yang tampak bekerja lebih berbahaya daripada
   tidak ada kode sama sekali.
2. **Dokumen panjang terbuka penuh.** Studi kasus 36 blok membuat halaman sunting
   setinggi 30.093 piksel. Sekarang dokumen di atas delapan blok terbuka dalam
   keadaan terlipat (10.137 piksel), sementara blok yang baru ditambahkan tetap
   terbuka sendiri.

**Tiga cacat tata letak, semuanya lebar — ditemukan dari satu keluhan**

Konten nyata membuat panel melebihi lebar layar. Tiga penyebab berbeda, semuanya
kelas yang sama: kotak yang menolak menyusut di bawah isinya.

1. **Kartu blok terlipat.** Ringkasan satu baris memakai `truncate`
   (`white-space: nowrap`), jadi minimum otomatisnya selebar seluruh paragraf.
   Trek grid ikut melebar, halaman ikut melebar. `grid-cols-1` + `min-w-0`.
2. **Kolom isi panel.** `lg:grid-cols-[18rem_1fr]` — `1fr` tidak boleh menyusut
   di bawah isinya, jadi tepat di lebar tempat rel muncul (1024px) halaman
   terdorong 16px. `minmax(0,1fr)`.
3. **Jalur catatan pinggir.** `.annotation` menyalakan dua kolom pada viewport
   64rem. Di situs publik itu benar; di panel, rel sudah mengambil 18rem lebih
   dulu, jadi dua kolom menyala di kolom yang tidak muat — 249px meluber. Di
   dalam panel jalurnya sekarang dikunci ke **container query** (`@container
   panel-content (min-width: 60rem)`), bukan ke viewport, dengan
   `minmax(0, var(--measure))` sebagai pengaman kalau aritmetikanya meleset.

Ditambah editor tabel di dalam blok: pembungkusnya diberi `min-w-0` supaya kotak
gulirnya benar-benar menggulir, bukan melebarkan kartunya.

**Terverifikasi — 15 lebar × 2 keadaan, semuanya 0px meluber**

320 · 360 · 390 · 414 · 640 · 768 · 1023 · 1024 · 1100 · 1200 · 1279 · 1280 ·
1366 · 1440 · 1920, dalam keadaan terlipat maupun semua blok terbuka; ditambah
daftar, pustaka berkas, dialog pemilih gambar (tetap terpusat — container query
tidak menariknya keluar dari top layer), dan halaman publik (jalur catatannya
tidak berubah: `605.875px 260px` di 1440).

**Cacat keempat: id blok kembar.**

Editor mengunci keadaan buka/tutup tiap kartu pada `id` bloknya. Server membuat id itu
dari 12 karakter pertama GUID v7 — yang isinya **stempel milidetik**, sama persis untuk
semua blok yang ditulis dalam milidetik yang sama. Impor 43 blok pulang membawa **2 id
unik**. Akibatnya satu kartu dibuka, dua puluh kartu ikut terbuka.

Diperbaiki dua lapis:

- **Server** membuat id dari keacakan, bukan dari jam, lalu memeriksanya terhadap daftar
  id yang sudah dipakai di dokumen itu — id kembar yang dikirim klien pun ikut diganti.
  Sekarang 43 blok = 43 id.
- **Klien** memperbaiki dokumen lama saat dibuka (`withUniqueBlockIds`), jadi baris yang
  tersimpan sebelum jaminan itu ada tetap berperilaku benar tanpa menunggu disimpan ulang.
  Blok pertama yang memakai sebuah id tetap memegangnya; hanya yang kembar yang diganti.

**Terverifikasi — impor 2/2 cocok kata per kata · panel 12/12 · lipat 5/5 · blok 9/9**

Termasuk: daftar membaca urutan tampil, sampul tampil, 36 blok termuat, metrik dan
teknologi terisi, menyunting ringkasan lalu memuat ulang tetap tersimpan.

**Pratinjau langsung — lebih awal dari rencana, dan bentuknya berbeda**

§8.1 membayangkan pratinjau sebagai halaman situs yang dibuka dengan token
(`/portofolio/<slug>?preview=…`). Yang dibangun justru pratinjau **di dalam
formulir**, hidup mengikuti ketikan, tanpa simpan dan tanpa permintaan ke server.

Alasannya: keluhan yang sebenarnya bukan "saya tidak bisa melihat draf", melainkan
"saya harus menyimpan dulu untuk tahu hasilnya". Pratinjau bertoken tetap butuh
simpan. Pratinjau bertoken masih akan berguna untuk **membagikan** draf ke orang
lain, dan itu tetap di daftar — tetapi bukan ini masalahnya.

Yang membuatnya bisa dipercaya: komponen yang dipakai pratinjau adalah komponen
yang dipakai halaman terbit. `CaseStudyArticleView` dan `ProjectCardView`
diekstrak dari halaman publik, `Decision`/`Figure`/`Metrics` dipindah keluar dari
pemetaan MDX ke `components/content/`. Satu definisi, dua pemanggil — dan
`BlockRenderer` yang lahir di sini memang yang dibutuhkan F3 nanti.

Tiga keputusan UI/UX yang diambil sadar:

- **Dua permukaan.** Kartu portofolio *dan* artikel. Separuh field hanya muncul di
  salah satunya; "Masalah" tidak pernah tampil di artikel.
- **Pratinjau mengikuti kursor**, dan hanya menggulir kalau blok yang disunting
  memang sedang tidak terlihat.
- **Formulir memadat saat terbelah**, dan yang mengalah lebih dulu adalah catatan
  panduan — bukan isian milik editor.

**Dua cacat yang muncul dan diperbaiki:**

1. Jalur catatan pinggir memakai container query pada `#panel-konten`. Saat layar
   terbelah, wadah itu tetap lebar sementara kolom formulirnya separuh — jadi dua
   kolom tetap menyala dan badan formulir terjepit ke 288px. Container dipindah ke
   kolom formulirnya sendiri (`.form-column`); wadah terdekat yang menang.
2. **Pratinjau HP berbohong.** Merender di dalam halaman berarti media query dan
   `vw` tetap membaca viewport peramban, jadi lembar 390px memakai tata letak
   desktop: tabel fakta empat kolom bertumpuk, judul berukuran 44px. Pratinjau
   dipindah ke dalam `iframe` — satu-satunya cara memberi sebuah kotak viewport
   sendiri. Stylesheet disalin (dan diikuti saat HMR), tema dicerminkan, dan
   bingkainya di-outline supaya 390px tetap 390px.

**Terverifikasi — 18/18**

Mengetik judul langsung terlihat di artikel *dan* di kartu; teks blok muncul tanpa
simpan; 36 blok dirender; pratinjau menggulir ke blok yang disunting (1773 → 5489);
lebar HP tepat 390px; catatan panduan hilang saat terbelah dan kembali di mode
Tulis; mode pratinjau menyembunyikan formulir; tanpa gulir horizontal. Halaman
publik setelah refaktor tetap utuh (8/8): judul, metrik, tabel fakta, 6 keputusan,
2 gambar, 3 catatan pinggir, 1 tabel.

### Berikutnya

`page-copy` (butuh manifes slot) dan `site-profile`, halaman panel untuk sembilan
modul di atas, seeder config TS (§9.2), lalu F3 (situs baca dari API + pra-render +
webhook) — pratinjau bertoken untuk berbagi draf ikut di sana.
