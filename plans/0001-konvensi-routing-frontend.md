# 0001 — Konvensi routing frontend

- **Status:** Sebagian digantikan oleh [0002](0002-arsitektur-multibahasa.md)
- **Tanggal:** 2026-08-24
- **Menyentuh:** frontend (config, router, halaman, dokumentasi), blueprint (3 berkas)
- **Revisi:** v2 — rekomendasi berubah dari "publik tetap Indonesia" menjadi
  "struktur Inggris, slug konten Indonesia". Alasannya di §0.

---

> **Catatan (v3).** Pertanyaan "path Indonesia atau Inggris" di dokumen ini **gugur**
> setelah diputuskan situs jadi dwibahasa: dengan dua bahasa, tidak ada lagi path
> tanpa bahasa — yang ada `/harga` (ID) **dan** `/en/pricing` (EN), dua-duanya.
> Keputusan routing dipindahkan ke [0002 §D1–D3](0002-arsitektur-multibahasa.md).
>
> Yang **masih berlaku** dari dokumen ini: lima cacat struktural C1–C5 di §8.
> Semuanya diserap ke Fase 1 di plan 0002.

---

## 0. Catatan revisi

Draf pertama merekomendasikan mempertahankan seluruh rute publik dalam bahasa
Indonesia dengan alasan SEO. Rekomendasi itu ditantang dengan dua pertanyaan yang
benar, dan setelah diperiksa ulang **dua argumen saya tidak bertahan**:

1. **Saya membesar-besarkan dampak SEO.** Saya memperlakukan seluruh rute publik
   seolah sama-sama bernilai kata kunci. Setelah diperiksa satu per satu, yang
   benar-benar menargetkan kueri komersial hanyalah halaman vertikal dan slug
   layanan — dan keduanya **tetap berbahasa Indonesia** di kedua opsi. Sisanya
   (`/proses`, `/tentang`, `/kontak`, `/catatan`, `/kebijakan-privasi`) tidak
   pernah jadi kueri siapa pun. Rinciannya di §4.

2. **Saya salah menyebut campuran itu "tidak konsisten".** Ada garis yang jelas
   dan lazim dipakai: **struktur** memakai bahasa Inggris, **slug konten** memakai
   bahasa isinya. `/blog/cara-memilih-jasa-web` bukan kelalaian — itu pola standar
   di ribuan situs. Berarti `/portfolio/p3m-pens` berdampingan dengan
   `/jasa-website-klinik-gigi` bukan cacat konsistensi, melainkan penerapan aturan
   yang sama.

Yang tidak berubah: slug vertikal dan slug konten tetap Indonesia. Itu bukan
preferensi gaya, itu syarat fungsional (§4).

---

## 1. Masalahnya apa

Rute frontend perlu mengikuti praktik terbaik. Saat ini seluruh path — publik
maupun aplikasi — berbahasa Indonesia.

Ada tiga hal terpisah di dalam permintaan ini, dan lebih jernih diputuskan
sendiri-sendiri:

- **(a)** Bahasa path **struktural** (`/layanan`, `/harga`, `/panel/dasbor`).
- **(b)** Bahasa **slug konten** (`website-bisnis`, `p3m-pens`, `jasa-website-klinik-gigi`).
- **(c)** Lima cacat struktural di lapisan routing yang tidak ada hubungannya
  dengan bahasa, tetapi lebih layak diperbaiki daripada soal bahasanya sendiri.

---

## 2. Keadaan sekarang

### 2.1 Rute publik

Sumber: [`frontend/src/config/routes.ts`](../frontend/src/config/routes.ts)

| Path | Halaman | Slug konten |
| --- | --- | --- |
| `/` | Beranda | — |
| `/layanan` · `/layanan/:slug` | Layanan | `website-bisnis`, `website-perusahaan`, `sistem-informasi` |
| `/portofolio` · `/portofolio/:slug` | Portofolio | `p3m-pens`, `situs-ini` |
| `/proses` | Proses | — |
| `/harga` | Harga | — |
| `/tentang` | Tentang | — |
| `/kontak` · `/kontak/terima-kasih` | Kontak | — |
| `/catatan` · `/catatan/:slug` | Catatan | `proxy-hanya-get-dan-post` |
| `/kebijakan-privasi` | Privasi | — |
| `/:vertical` | Halaman vertikal | `jasa-website-klinik-gigi`, `jasa-website-klinik-kecantikan`, `jasa-sistem-informasi-kampus` |

### 2.2 Rute aplikasi

Sumber: [`frontend/src/config/panel.ts`](../frontend/src/config/panel.ts).
Seluruhnya `noindex`, tidak pernah terindeks.

`/auth/masuk` · `/auth/pilih-peran` · `/panel/dasbor` · `/akses-ditolak`

### 2.3 Identifier di kode — sudah Inggris

`routes.services`, `routes.caseStudy`, `panelRoutes.dashboard`, `authRoutes.login`,
`menuCodes.dashboard`. Nama variabel, berkas, dan komentar seluruhnya Inggris.

**Ini perlu ditegaskan:** bagian yang paling sering dimaksud orang ketika bilang
"rute harus bahasa Inggris" — penamaan di dalam kode — sudah benar sejak awal.
Yang sedang dibicarakan di sini murni string URL-nya.

---

## 3. Garis yang benar: struktur vs konten

Ada dua garis, bukan satu.

**Garis pertama — identifier vs URL.** Identifier kode wajib Inggris; itu tidak
bisa ditawar dan sudah dipatuhi. URL adalah antarmuka, bukan identifier, jadi
aturannya berbeda.

**Garis kedua — struktur vs konten.** Ini yang saya lewatkan di draf pertama, dan
ini yang sebenarnya dipakai praktisi:

| | Contoh | Bahasa | Alasan |
| --- | --- | --- | --- |
| **Struktur** | `/services`, `/portfolio`, `/pricing`, `/panel/dashboard` | Inggris | Nama arsitektur, bukan konten. Sederajat dengan nama tabel dan endpoint API. Jumlahnya tetap, ditulis sekali, dibaca terus oleh pengembang. |
| **Slug konten** | `website-bisnis`, `p3m-pens`, `jasa-website-klinik-gigi` | Bahasa isinya | Diturunkan dari judul konten. Tumbuh seiring isi bertambah. Ditulis oleh penulis konten, bukan pengembang. |

Pola `/blog/cara-memilih-jasa-web` — struktur Inggris, slug Indonesia — adalah
bentuk yang paling umum ditemui, dan tidak seorang pun menganggapnya campur aduk.
Aturannya konsisten; yang berbeda hanya sumber katanya.

Dengan garis ini, aturan yang saya usulkan untuk ditulis ke README:

> **Path struktural memakai bahasa Inggris.** Sama seperti identifier, nama tabel,
> dan endpoint API — itu nama arsitektur.
>
> **Slug konten memakai bahasa kontennya.** Diturunkan dari judul, bukan dari kode.
>
> **Path aplikasi (auth, panel) selalu Inggris** dan tidak pernah terindeks.

---

## 4. SEO — angka yang jujur

Draf pertama memakai SEO sebagai argumen utama. Setelah diperiksa per halaman,
argumen itu jauh lebih sempit dari yang saya klaim.

**Apa yang sebenarnya menentukan peringkat** untuk halaman-halaman ini: judul
halaman, H1, isi teks, tautan internal, dan backlink. Semuanya berbahasa Indonesia
dan **tidak tersentuh** oleh perubahan path. Kata di dalam URL adalah sinyal yang
kecil — Google sendiri menyatakannya berulang kali, dan bobotnya jauh di bawah
yang biasa diasumsikan.

**Per halaman, nilai kata kuncinya:**

| Path | Menargetkan kueri komersial? | Kalau jadi Inggris |
| --- | --- | --- |
| `/jasa-website-klinik-gigi` | **Ya — ini produknya** | ❌ hancur total |
| `/layanan/website-bisnis` | Ya, sebagian (`website bisnis`) | slug tetap Indonesia → aman |
| `/layanan/sistem-informasi` | Ya, sebagian | slug tetap Indonesia → aman |
| `/harga` | Lemah (`harga jasa website` tidak match `harga` saja) | dampak dapat diabaikan |
| `/layanan` | Tidak — bukan kueri | nihil |
| `/portofolio` `/proses` `/tentang` `/kontak` `/catatan` `/kebijakan-privasi` | Tidak | nihil |

Jadi: **satu-satunya kelompok yang benar-benar bernilai SEO adalah slug konten, dan
slug konten tidak ikut diterjemahkan di opsi mana pun.**

Sisa dampaknya bukan peringkat melainkan **CTR dan keterbacaan** — pengunjung
Indonesia yang melihat `leksana.id/harga` sedikit lebih paham daripada
`leksana.id/pricing`, terutama ketika URL ditempel mentah di WhatsApp. Nyata,
tetapi kecil, dan ditukar dengan keuntungan yang lebih besar di §5.

---

## 5. Multibahasa — pertanyaan yang menentukan

Kalau situs ini suatu saat punya versi Inggris, bentuk yang benar adalah prefiks
per bahasa dengan `hreflang`:

```
/id/layanan            /en/services
/id/portofolio         /en/portfolio
/id/jasa-website-klinik-gigi   /en/dental-clinic-website-service
```

Pertanyaannya: apakah memulai dengan path Inggris membuat migrasi itu lebih murah?

**Jawaban jujurnya: bukan path-nya yang menentukan, melainkan apakah ada peta
`kunci → slug`.** Dua-duanya sama-sama butuh redirect. Yang membedakan biaya
migrasi adalah bentuk `routes.ts`:

```ts
// Bentuk sekarang — kunci sudah Inggris, tapi slug menempel langsung.
export const routes = { services: '/layanan', … }

// Bentuk yang siap i18n — nilai jadi berdimensi bahasa.
const segment = {
  services: { id: 'layanan', en: 'services' },
  work:     { id: 'portofolio', en: 'portfolio' },
}
```

Menambahkan dimensi bahasa itu adalah perubahan **satu berkas**, dan bisa
dikerjakan hari ini tanpa membangun i18n-nya (YAGNI: dimensinya jangan dibuat
sekarang, tetapi bentuknya harus memungkinkan).

Di sinilah argumen Anda menang, dan ini alasan sebenarnya saya berpindah:

> Kalau path dasarnya Inggris, **bahasa kanonik sistem hanya satu**: Inggris di
> kode, Inggris di API, Inggris di path. Bahasa Indonesia menjadi *salah satu*
> terjemahan — sederajat dengan bahasa lain yang mungkin menyusul.
>
> Kalau path dasarnya Indonesia, bahasa Indonesia terpanggang menjadi *default
> yang istimewa*. Menambah bahasa berarti mempromosikan Indonesia dari "bawaan"
> menjadi "salah satu" — pekerjaan konseptual tambahan yang tidak perlu ada.

Itu argumen arsitektur, bukan argumen selera, dan bobotnya melebihi selisih CTR
di §4.

---

## 6. Pilihan

### Opsi A — Publik tetap Indonesia, aplikasi jadi Inggris

- **+** CTR sedikit lebih baik untuk pembaca Indonesia.
- **−** Bahasa Indonesia terpanggang sebagai default istimewa (§5).
- **−** Dua konvensi dengan garis yang lebih sulit dijelaskan ("terindeks vs tidak").

### Opsi B — Struktur Inggris, slug konten Indonesia ✅ **rekomendasi**

- **+** Satu bahasa kanonik: kode, API, path. Garisnya tunggal dan mudah dijelaskan.
- **+** Siap i18n tanpa mempromosikan bahasa mana pun (§5).
- **+** `/panel/dashboard` ⇄ kode menu `dashboard` tidak perlu diterjemahkan di kepala.
- **+** Boilerplate panel ini akan dipakai ulang untuk klien lain; permukaan netral lebih mudah dibawa.
- **+** Nilai SEO utuh — slug konten tidak tersentuh (§4).
- **−** Selisih CTR kecil untuk pembaca Indonesia.
- **−** Tiga berkas blueprint perlu disinkronkan (§7.4).

### Opsi C — Semua tetap Indonesia

- **+** Tanpa pekerjaan.
- **−** Kehilangan seluruh keuntungan di atas; cacat §8 tetap harus dibereskan.

---

## 7. Rencana eksekusi — Opsi B

### 7.1 Peta perubahan URL

**Struktur → Inggris**

| Sekarang | Sesudah |
| --- | --- |
| `/layanan` · `/layanan/:slug` | `/services` · `/services/:slug` |
| `/portofolio` · `/portofolio/:slug` | `/portfolio` · `/portfolio/:slug` |
| `/proses` | `/process` |
| `/harga` | `/pricing` |
| `/tentang` | `/about` |
| `/kontak` | `/contact` |
| `/kontak/terima-kasih` | `/contact/thank-you` |
| `/catatan` · `/catatan/:slug` | `/notes` · `/notes/:slug` |
| `/kebijakan-privasi` | `/privacy-policy` |
| `/auth/masuk` | `/auth/login` |
| `/auth/pilih-peran` | `/auth/select-role` |
| `/panel/dasbor` | `/panel/dashboard` |
| `/akses-ditolak` | `/access-denied` |

**Slug konten → tidak berubah**

```
website-bisnis · website-perusahaan · sistem-informasi
p3m-pens · situs-ini
proxy-hanya-get-dan-post
jasa-website-klinik-gigi · jasa-website-klinik-kecantikan · jasa-sistem-informasi-kampus
```

Hasil akhirnya, misalnya: `/services/sistem-informasi`,
`/portfolio/p3m-pens`, `/notes/proxy-hanya-get-dan-post`,
`/jasa-website-klinik-gigi`.

### 7.2 Perubahan kode

1. **`config/routes.ts`** — segmen ditulis sekali di satu tempat, path dan pola
   rute diturunkan darinya. Bentuknya dibuat siap menerima dimensi bahasa nanti
   tanpa membangunnya sekarang.

   ```ts
   const segment = {
     services: 'services',
     work: 'portfolio',
     process: 'process',
     pricing: 'pricing',
     about: 'about',
     contact: 'contact',
     contactThanks: 'thank-you',
     notes: 'notes',
     privacy: 'privacy-policy',
   } as const

   export const routes = {
     services: `/${segment.services}`,
     service: (slug: string) => `/${segment.services}/${slug}`,
     …
   } as const

   /** Pola untuk <Route path>. Segmen yang sama, parameternya berbeda. */
   export const routePatterns = {
     services: routes.services,
     service: `/${segment.services}/:slug`,
     …
   } as const
   ```

2. **`config/panel.ts`** — path aplikasi jadi Inggris; segmen relatif diekspor
   agar `AuthRoutes`/`PanelRoutes` tidak menulis ulang. Hapus `panelRoutes.users`
   dan `panelRoutes.roles` yang sudah mati (C3).

3. **`App.tsx`** — seluruh `path="..."` diganti `routePatterns.*` (C1).

4. **`AuthRoutes.tsx`, `PanelRoutes.tsx`** — segmen dibaca dari config (C2).

5. **`AccessDeniedPage.tsx`** — `window.location.href` → `useNavigate()` (C4).

6. **`server/sitemap.ts`** — tambah `Disallow: /panel/` dan `Disallow: /auth/`
   di `buildRobots()` (C5). Sitemap sendiri sudah membaca dari `routes`, jadi ikut
   berubah sendiri.

### 7.3 Berkas yang tersentuh

```
frontend/src/config/routes.ts
frontend/src/config/panel.ts
frontend/src/App.tsx
frontend/src/pages/auth/AuthRoutes.tsx
frontend/src/pages/panel/PanelRoutes.tsx
frontend/src/pages/AccessDeniedPage.tsx
frontend/server/sitemap.ts
frontend/README.md          aturan #3 + tabel rute panel
README.md                   contoh tautan
```

Halaman-halaman lain tidak tersentuh: semuanya sudah menautkan lewat `routes.*`,
tidak ada satu pun path literal di luar `config/` dan `App.tsx` (sudah diverifikasi
dengan grep).

**Backend tidak tersentuh sama sekali.** Kode menu (`dashboard`, `user`, `role`)
sudah Inggris, jadi kontrak izin tetap utuh.

### 7.4 Sinkronisasi blueprint

Tiga berkas menyebut URL lama dan akan bertentangan dengan implementasi:

| Berkas | Bagian |
| --- | --- |
| `blueprint/04-arsitektur-informasi-copy.md` | §4.1 pohon sitemap |
| `blueprint/09-mesin-lead-closing.md` | tabel pesan pembuka WhatsApp per URL |
| `blueprint/11-roadmap-eksekusi.md` | daftar halaman prioritas |

`blueprint/08-seo-mesin-konten.md` **tidak** perlu diubah — isinya slug vertikal,
yang tidak berubah.

Blueprint yang bertentangan dengan kode lebih berbahaya daripada blueprint yang
direvisi. Saya usulkan memperbarui ketiganya, dengan satu baris catatan revisi di
tiap berkas supaya jejak keputusannya tidak hilang.

---

## 8. Cacat routing lain — dikerjakan apa pun opsinya

**C1 — `App.tsx` menulis ulang path sebagai literal.**
[`routes.ts`](../frontend/src/config/routes.ts) menyatakan dirinya "the only place
URL paths are written as strings", tetapi
[`App.tsx:79–98`](../frontend/src/App.tsx) menulis `path="/layanan"`,
`path="/portofolio/:slug"`, dan sebelas lainnya sebagai literal. Sumber
kebenarannya jadi dua, dan yang kedua diam-diam tidak dipakai siapa pun.

**C2 — Segmen anak juga literal.**
[`AuthRoutes.tsx:19–20`](../frontend/src/pages/auth/AuthRoutes.tsx) dan
[`PanelRoutes.tsx:25`](../frontend/src/pages/panel/PanelRoutes.tsx) menulis
`"masuk"`, `"pilih-peran"`, `"dasbor"` langsung.

**C3 — `panelRoutes.users` dan `panelRoutes.roles` mati.**
Dideklarasikan, tidak dipakai satu baris pun, tidak ada rutenya, tidak ada di
navigasi. Dihapus sekarang, ditambahkan kembali bersama halamannya.

**C4 — `AccessDeniedPage` memuat ulang seluruh halaman.**
[`AccessDeniedPage.tsx:38`](../frontend/src/pages/AccessDeniedPage.tsx) memakai
`window.location.href`; itu membuang state React dan mengunduh ulang bundel.
(`http.ts` memang harus memakainya karena berjalan di luar React; yang ini tidak.)

**C5 — `robots.txt` belum menutup permukaan aplikasi.**
Sekarang hanya `Disallow: /kontak/terima-kasih`. Panel dan auth sudah `noindex`
lewat meta tag, tetapi meta tag baru terbaca setelah JavaScript jalan; `Disallow`
menutupnya lebih awal.

---

## 9. Rencana verifikasi

1. `npm run check` — typecheck + lint bersih.
2. `npm run build` — build lolos; ukuran bundel publik tidak naik.
3. **Bukti C1/C2:** grep memastikan tidak ada literal path di luar `config/`.
4. **Uji peramban** (Playwright, 29 pemeriksaan yang sudah ada, path diperbarui):
   redirect saat belum masuk, validasi, login gagal, login berhasil, cookie,
   sidebar, tabel izin, muat ulang, tema, keluar, panel terkunci lagi.
5. **Regresi situs publik:** 13 halaman + 3 halaman vertikal masih merender;
   navigasi utama, footer, dan tautan di dalam halaman masih hidup.
6. **Path lama** (`/layanan`, `/panel/dasbor`) jatuh ke 404 yang benar — bukan
   halaman kosong. Catatan: `/layanan` akan tertangkap `/:vertical` lalu
   dikembalikan sebagai 404 karena bukan slug vertikal yang terdaftar.
7. **`robots.txt` + `sitemap.xml`** hasil build diperiksa isinya (18 URL, path baru).
8. Uji gulir horizontal 3 lebar × seluruh halaman — memastikan tidak ada regresi.

---

## 10. Risiko

| Risiko | Penilaian |
| --- | --- |
| URL lama sudah tersebar | **Nihil.** Situs belum live, belum ada backlink, belum terindeks. Ini momen termurah untuk mengubahnya — setelah live, tiap perubahan URL menuntut 301 di lapisan hosting, karena SPA tidak bisa mengembalikan 301 sendiri. |
| Sesi pengguna putus | **Nihil.** Cookie tidak terikat path selain `/`. |
| Kontrak backend bergeser | **Nihil.** Backend tidak tersentuh. |
| Tautan internal di MDX rusak | **Nihil.** Sudah diperiksa: tidak ada tautan internal di dalam MDX. |
| Blueprint jadi stale | **Nyata, dan dijawab** di §7.4 dengan memperbaruinya. |

---

## 11. Yang tidak dikerjakan

- **Slug konten tidak diterjemahkan.** Bukan gaya, tapi syarat fungsional (§4).
- **Lapisan redirect/alias tidak dibuat.** Belum ada URL lama yang perlu dijaga.
  Untuk dicatat: begitu live, perubahan URL apa pun wajib disertai 301 di
  Vercel/Caddy, bukan di React Router.
- **i18n tidak dibangun.** Bentuk `routes.ts` dibuat siap menerima dimensi bahasa,
  dimensinya sendiri tidak dibuat sekarang.
- **Halaman `/panel/users` dan `/panel/roles` tidak dibuat** — pekerjaan terpisah;
  di sini entri matinya justru dihapus.

---

## 12. Keputusan yang diminta

Opsi **A**, **B**, atau **C**. Rekomendasi saya **Opsi B**.

Bagian 8 (C1–C5) dikerjakan apa pun jawabannya — itu cacat, bukan preferensi.

---

## 13. Catatan hasil

_Diisi setelah dikerjakan._
