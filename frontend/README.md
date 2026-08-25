# Leksana Studio — situs studio

Situs web dan sistem web Leksana Studio. Statis penuh, tanpa skrip pihak ketiga.

Dibangun dengan **React 19 + Vite 7 + TypeScript**, React Router untuk routing, Tailwind
CSS 4 untuk gaya, dan MDX untuk konten. Situs publiknya port dari versi Next.js —
tampilan, copy, dan perilakunya sama; yang berubah hanya lapisan kerangkanya.

Di repositori yang sama juga ada **panel pengelolaan** di `/panel`, dengan halaman
masuk di `/auth/masuk`, yang berbicara dengan API di [`../backend/`](../backend/).

Blueprint yang mendasari kode ini ada di [`../blueprint/`](../blueprint/):

| Dokumen                                                                                        | Isi                                                        |
| ---------------------------------------------------------------------------------------------- | ---------------------------------------------------------- |
| [`../blueprint/06-sistem-desain.md`](../blueprint/06-sistem-desain.md)                         | Arah desain: tesis, arah yang ditolak, anggaran keberanian |
| [`../blueprint/06b-design-system.md`](../blueprint/06b-design-system.md)                       | Spesifikasi: token, skala, komponen, state                 |
| [`../blueprint/04-arsitektur-informasi-copy.md`](../blueprint/04-arsitektur-informasi-copy.md) | Sitemap, wireframe, bank copywriting                       |
| [`../blueprint/07-arsitektur-teknis.md`](../blueprint/07-arsitektur-teknis.md)                 | Stack, anggaran performa, alur lead                        |

---

## Menjalankan

```bash
npm install
cp .env.example .env.local     # isi seperlunya; situs tetap jalan tanpa itu
npm run dev                    # http://localhost:3000
```

Situs publiknya jalan sendirian. Halaman masuk dan panel butuh API di
[`../backend/`](../backend/) hidup di `http://localhost:5180` — cara
menjalankannya ada di README repositori.

Perintah lain:

```bash
npm run build      # typecheck + build produksi ke dist/
npm run preview    # jalankan hasil build
npm run check      # typecheck + lint
npm run format     # prettier
```

Variabel lingkungan berawalan `VITE_` ikut terbawa ke bundel peramban dan **tidak
rahasia**. Kredensial Resend dan Fonnte sengaja tidak berawalan itu — nilainya hanya
dibaca di sisi server.

---

## Arsitektur

Lapisannya sengaja tipis dan searah. Yang di bawah tidak pernah mengimpor yang di atas.

```
src/content/       berkas MDX — tidak ada JSX, tidak ada logika
  studi-kasus/     satu berkas per proyek portofolio
  catatan/         satu berkas per tulisan

src/config/        satu-satunya sumber kebenaran untuk data & angka
  site.ts          identitas studio + format mata uang
  routes.ts        peta URL, navigasi utama, navigasi footer
  packages.ts      paket, tahapan, add-on, termin, lantai harga
  services.ts      tiga lini layanan
  verticals.ts     data halaman vertikal (mesin SEO)
  process.ts       empat langkah proses + tiga ketakutan
  copy.ts          FAQ, daftar yang ditolak, pilar bukti, halaman tentang

src/lib/           fungsi murni, tanpa JSX
  content.ts       pembaca MDX + validasi frontmatter (Zod)
  seo.ts           hook metadata halaman (title, canonical, OG, Twitter)
  structured-data.ts  JSON-LD
  whatsapp.ts      tautan wa.me + pesan pembuka per halaman
  contact-schema.ts   skema yang dipakai bersama formulir dan endpoint
  theme.ts         status tampilan + skrip pra-cat
  href.ts          menentukan sebuah tautan jadi <Link> atau <a>
  format.ts · cn.ts

src/components/
  ui/              primitif: Button, AppLink, Note, Label, ArrowLink, Rule,
                   ContactForm, ThemeSwitch
  layout/          Header, Footer, WhatsAppBar, Document, Annotation, SkipLink,
                   JsonLd, ScrollManager
  blocks/          blok halaman: Hero, ProjectCard, PricingTable, PhaseList, …
  mdx/             komponen yang boleh dipakai di dalam MDX

src/pages/         satu berkas per halaman. Komposisi saja; tidak ada data yang
                   lahir di sini.
  auth/            halaman masuk + pemilih peran, beserta rutenya
  panel/           kerangka panel + dasbor, beserta rutenya
src/App.tsx        tiga dunia, satu router: situs, masuk, panel

--- lapisan integrasi API (dipakai panel, bukan situs publik) ---

src/config/
  api.ts           alamat dasar API
  panel.ts         rute panel, kode menu, navigasi, resolusi halaman pendaratan
src/lib/
  http.ts          klien terautentikasi: token, X-Role-Active, refresh otomatis
  http-public.ts   klien tanpa token — login dan endpoint publik
  tokens.ts        penyimpanan sesi di cookie
  active-role.ts   peran aktif + peran terakhir dipakai
  api-error.ts     klasifikasi galat + toast yang di-throttle
src/repositories/  BaseRepository (5 endpoint CRUD) + Auth, Menu
src/store/         menu-store, active-role-store (zustand)
src/hooks/         useApi, usePermission, useUserMenus
src/components/route/  ProtectedRoute, MenuRoute
src/types/         bentuk yang dibagi dengan API

server/            kode yang berjalan di Node, bukan di peramban
  contact.ts       penanganan lead (validasi, rate limit, notifikasi)
  contact-middleware.ts   memasang endpoint itu di server dev & preview
  sitemap.ts       pembentuk sitemap.xml + robots.txt
  reading-time.ts  hitung estimasi waktu baca saat build

api/kontak.ts      titik masuk serverless untuk produksi
```

### Aturan yang dipegang

1. **Tidak ada nilai identitas atau angka janji di dalam komponen.** Nomor WhatsApp,
   harga, jumlah revisi, lama garansi — semuanya dari `src/config/`. Mengubahnya harus
   satu baris, bukan pencarian di empat belas berkas.
2. **Tidak ada nilai warna di dalam komponen.** Semua lewat token semantik
   (`bg-bg`, `text-muted`, `border-line`, `bg-accent`). Palet mentah hanya hidup di
   `src/styles/globals.css`.
3. **Penanda bahasa:** identifier, nama berkas, dan komentar berbahasa Inggris; URL,
   teks antarmuka, dan konten berbahasa Indonesia.
4. **Halaman vertikal tidak pernah disalin.** Menambah vertikal berarti menambah satu
   entri di `src/config/verticals.ts`. Satu template merender semuanya.
5. **Frontmatter divalidasi.** Berkas MDX yang cacat melempar galat saat modul dimuat,
   bukan diam-diam lolos ke produksi.

### Yang berubah dari versi Next.js

| Urusan          | Next.js                     | Di sini                                                              |
| --------------- | --------------------------- | -------------------------------------------------------------------- |
| Routing         | App Router berbasis folder  | React Router, tabel rute di `src/App.tsx`                            |
| Tautan          | `next/link`                 | `components/ui/AppLink` — memilih `<Link>` atau `<a>` sendiri        |
| Gambar          | `next/image`                | `<img>` biasa; posisinya diatur di `ProjectFigure`                   |
| Metadata        | `export const metadata`     | hook `usePageMeta` di `src/lib/seo.ts`                               |
| Huruf           | `next/font/google`          | berkas `.woff2` di `public/fonts/` + `@font-face` di `globals.css`   |
| MDX             | `next-mdx-remote`           | `@mdx-js/rollup`, dikompilasi saat build                             |
| `sitemap.ts`    | route metadata              | plugin Vite di `vite.config.ts` → `dist/sitemap.xml`                 |
| `robots.ts`     | route metadata              | plugin yang sama → `dist/robots.txt`                                 |
| `/api/kontak`   | route handler               | `server/contact.ts`, dipasang di dev/preview **dan** `api/kontak.ts` |
| Header keamanan | `next.config.ts`            | `vercel.json` untuk produksi, plugin Vite untuk dev                  |
| Kartu OG        | `opengraph-image.tsx` (PNG) | `public/og.svg` — lihat catatan SEO di bawah                         |

Rute apa pun di luar tabel dilayani `NotFoundPage`, termasuk slug layanan, studi kasus,
catatan, dan vertikal yang tidak ada di konfigurasi. Itu pengganti `dynamicParams = false`.

---

## Design system

Implementasi lengkap ada di [`src/styles/globals.css`](src/styles/globals.css), terbagi
tujuh lapisan: huruf → palet → token semantik → base → peran tipografi → tata letak →
komponen.

|               | Nilai               | Catatan                                                     |
| ------------- | ------------------- | ----------------------------------------------------------- |
| Latar         | `#EFF1F2`           | Kertas abu kebiruan. Bukan putih, sengaja bukan krem hangat |
| Teks          | `#1B2430`           | Tinta biru-hitam, tidak pernah hitam murni                  |
| Aksen         | `#7A2230`           | Oxblood — warna cap dokumen resmi. Dipakai < 5% permukaan   |
| Aksen (gelap) | `#D0685E`           | Dijaga tetap hangat supaya tidak melayang jadi salmon       |
| Judul         | Spectral 500/600    | Serif teks untuk layar                                      |
| Antarmuka     | Public Sans 400/600 | Huruf sistem desain pemerintah                              |
| Data & label  | DM Mono 400         | Satu-satunya tempat mono dipakai                            |
| Radius        | 2px / 4px           | Kertas yang dipotong rapi, bukan aplikasi konsumen          |

### Elemen tanda tangan: catatan pinggir

`<Annotation note="…">` memasangkan satu blok konten dengan anotasinya dalam satu baris
grid, sehingga catatan sejajar dengan paragraf yang dianotasinya. Di bawah 1024px ia
tampil inline dengan garis aksen — isinya terlalu penting untuk disembunyikan di HP.

Aturan pemakaian (bukan sekadar saran): maksimal empat per halaman, selalu orang
pertama, isinya alasan/keberatan/batas — tidak pernah fitur, tidak pernah ajakan membeli.

### Gerak

Seluruh anggaran gerak dibelanjakan pada satu hal: kemunculan catatan pinggir.
Dikerjakan dengan `animation-timeline: view()` — **nol JavaScript.** Di peramban yang
belum mendukungnya, catatan sekadar terlihat sejak awal.

`prefers-reduced-motion: reduce` mematikan seluruh transisi.

### Dua skala: dokumen dan alat

Repositori ini berisi dua produk, dan keduanya tidak boleh berbagi ukuran.

**Situs adalah dokumen.** Dibaca sekali, berurutan, dari jarak baca. Skala longgar
itulah yang membuatnya terbaca sebagai sesuatu yang dipikirkan.

**Panel adalah alat.** Dibuka dua puluh kali sehari, dipindai bukan dibaca, dan dinilai
dari seberapa banyak pekerjaan yang muat dalam satu layar laptop. Skala situs di sana
menukar satu layar kerja dengan satu layar kertas — dua baris tabel, tiga field.

Panel mempertahankan huruf, warna, dan garis rambutnya; yang dilepas adalah ukurannya.

| | Dokumen | Alat |
| --- | --- | --- |
| Teks isi | 17–18px | 15px |
| Judul halaman | 30–44px | 22px |
| Judul bagian | 25–32px | 18px |
| Label mono | 12–12.5px | 11px |
| Tinggi kontrol | 44px | 36px (44px di layar sentuh) |

Keduanya ditulis sebagai variabel yang dibaca peran tipografi (`--type-h1-size` dan
kawan-kawan), jadi pergantian skala adalah **satu kelas** dan tidak ada komponen di
bawahnya yang perlu tahu: `.type-scale-tool` di cangkang panel, `.type-scale-site` di
lembar pratinjau — karena pratinjau berukuran alat adalah kebohongan tentang apa yang
akan terbit.

Angka yang berubah pada satu layar 1440×900:

```
bilah atas        81px → 44px
rel navigasi     288px → 240px
butir menu       103px → 40px      (keterangan hanya untuk menu yang sedang dibuka)
judul halaman     32px → 22px
kepala → field   106px → 58px
tinggi dokumen 10.241px → 8.083px
```

Butir menu adalah yang terpenting: dengan 17 menu, keterangan satu baris di tiap butir
berarti rel setinggi 1.751px — rel yang harus digulir sebelum satu menu pun diklik.

Yang **tidak** ikut memadat: target sentuh. Di `pointer: coarse` semua kontrol kembali
ke 44px. Kepadatan itu untuk laptop, bukan untuk jari.

### Dua field sebaris

Field yang berdampingan dulunya dua tumpukan mandiri: begitu keterangan salah satunya
memakan dua baris, kotak isiannya turun setengah baris dan pasangan itu terlihat rusak —
persis hal yang dinilai orang dari sebuah formulir sebelum satu katanya dibaca.

`FieldRow` memberi baris itu empat jalur — **label · keterangan · kontrol · galat** — dan
tiap field menyerahkan empat barisnya sendiri ke `subgrid`. Jalur keterangan setinggi
keterangan terpanjang, dan kedua kotak isian tetap mulai di piksel yang sama.

```tsx
<FieldRow>                                  {/* dua kolom sama lebar */}
<FieldRow columns="7rem minmax(0, 1fr)">    {/* nomor langkah + judul */}
```

Syaratnya satu: setiap field selalu merender keempat bagiannya, termasuk keterangan yang
kosong. Elemen yang muncul-hilang akan menggeser semua baris sesudahnya. Di bawah 40rem
field-nya menumpuk dan persoalannya hilang dengan sendirinya; peramban tanpa `subgrid`
kembali ke perilaku lama — rata atas, tidak sejajar, tetapi tidak pernah rusak.

### Bilah gulir

Bawaan Windows adalah palung tebal berikut tombol panah — perabot dari sistem desain lain,
dan cukup tebal untuk terbaca sebagai garis batas kedua di sisi tiap textarea. Di
`globals.css` semua bilah gulir dibuat tipis dengan warna dari token teks:

```css
*, *::before, *::after { scrollbar-width: thin; }
:root { scrollbar-color: var(--scrollbar-thumb) transparent; }
```

Empat keputusan di baliknya:

- **Warna diturunkan, bukan ditulis dua kali.** `--scrollbar-thumb` adalah campuran
  `--ink` dengan `--paper`, jadi tema gelap dapat pegangan yang benar tanpa deklarasi
  tambahan.
- **Tetap terbaca.** Kontras pegangan 3,89:1 (terang) dan 4,60:1 (gelap) terhadap latar —
  di atas ambang 3:1 untuk komponen non-teks. Yang membuatnya tidak mengganggu adalah
  lebarnya (10px, dari 15px), bukan warnanya yang dipudarkan.
- **Tidak disembunyikan sampai disentuh kursor.** Bilah yang baru muncul saat hover tidak
  terlihat oleh siapa pun yang tidak memakai kursor, dan berhenti memberi tahu semua orang
  bahwa masih ada yang bisa dibaca. Yang menguat saat kursor masuk hanyalah wadah gulir di
  dalam halaman — bukan bilah halaman itu sendiri, yang akan selalu dalam keadaan hover.
- **Menyerah pada sistem saat diminta.** Di `forced-colors: active`, `scrollbar-color`
  dikembalikan ke `auto`.

Blok `::-webkit-scrollbar` di bawahnya hanya untuk Safari lama; Chrome mengabaikannya
begitu `scrollbar-color` diset, jadi keduanya tidak pernah berebut.

### Tampilan terang / gelap

Tiga status, bukan dua: **Terang · Sistem · Gelap**. "Sistem" adalah preferensi nyata —
pembaca yang sudah mengatur perangkatnya sudah membuat pilihan, dan menggantinya dengan
saklar biner berarti membuang pilihan itu.

| Bagian                                | Berkas                                                                                                     |
| ------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| Status, penyimpanan, skrip pra-render | `src/lib/theme.ts`                                                                                         |
| Penyuntikan skrip ke `<head>`         | plugin `leksana-theme-script` di `vite.config.ts`                                                          |
| Kontrol (radio group)                 | `src/components/ui/ThemeSwitch.tsx` — `layout="inline"` di footer situs, `layout="stacked"` di rel panel   |
| Token per tema                        | `src/styles/globals.css` — blok `:root`, `@media (prefers-color-scheme: dark)`, `:root[data-theme="dark"]` |

Cara kerjanya:

- Skrip inline **141 byte** di `<head>` membaca `localStorage` dan memasang `data-theme`
  **sebelum cat pertama** — jadi pembaca yang memilih gelap tidak pernah melihat kedip
  terang lebih dulu. Dibungkus `try/catch`: kalau penyimpanan diblokir, halaman tetap jalan.
  Skripnya diambil langsung dari `src/lib/theme.ts` saat build, jadi kunci penyimpanannya
  tidak mungkin berbeda antara skrip dan aplikasi.
- Memilih **Sistem** menghapus atribut _dan_ menghapus kuncinya dari penyimpanan, sehingga
  `prefers-color-scheme` mengambil alih lagi. Tidak ada sisa yang tertinggal.
- `color-scheme` ikut dipaksa saat tema dipaksa, supaya scrollbar dan kontrol form bawaan
  peramban tidak tertinggal di mode yang salah.
- Kontrolnya `<fieldset>` + tiga `<input type="radio">`. Navigasi panah dan pengumuman
  pembaca layar didapat gratis; tidak ada `role` buatan.

Di panel, kontrolnya duduk di **dasar rel navigasi**, bukan di footer halaman. Formulir
konten bisa setinggi sepuluh ribu piksel, dan kontrol sejauh itu ke bawah adalah kontrol
yang tidak pernah ditemukan — persis yang terjadi sebelum ini dipindah. Di rel ia selalu
ada di layar, berpasangan dengan "Lihat situs →" sebagai sesama urusan ruang kerja, dan
tidak ikut bersaing dengan tombol milik halaman yang sedang dibuka. Di HP, pasangan yang
sama muncul di dasar drawer menu. Satu setelan, satu kontrol, satu tempat.

Yang **tidak** dipakai: ikon matahari/bulan. Itu bawaan yang muncul di mana-mana; label
mono tiga kata lebih sesuai bahasa dokumen situs ini dan tidak menuntut tebakan.

---

## Menambah konten

### Studi kasus baru

Buat `src/content/studi-kasus/<slug>.mdx`. Frontmatter divalidasi oleh
`caseStudyFrontmatterSchema` di `src/lib/content.ts`.

```yaml
---
title: '…'
label: 'klien' # atau 'produk-sendiri' — wajib jujur
client: '…'
kind: 'Sistem informasi'
figure: 'system' # system | website | catalog
summary: '…'
problem: '…' # satu kalimat, muncul di kartu portofolio
year: 2026
duration: '6 minggu'
role: '…'
stack: ['…']
metrics: # tepat tiga
  - { value: '30', label: 'Modul' }
updated: '2026-08-23'
order: 1
---
```

Komponen yang tersedia di dalam MDX: `<Annotation>`, `<Note>`, `<Decision>`,
`<Figure>`, `<Metrics>`, `<Label>`.

`<Decision>` memaksa format rumah: _"Saya memilih X karena Y, walaupun Z."_ Keputusan
tanpa alternatif yang ditolak bukan keputusan — itu preferensi, dan tidak layak masuk
studi kasus.

### Catatan (tulisan) baru

Buat `src/content/catatan/<slug>.mdx` dengan `title`, `summary`, `published`, dan
`pillar` (`keputusan` | `panduan` | `industri`).

### Vertikal baru

Tambah satu entri di `src/config/verticals.ts`. Halaman, metadata, JSON-LD, dan sitemap
mengikuti sendiri.

⚠️ Setiap vertikal wajib punya masalah, deliverable, dan FAQ yang **benar-benar
berbeda**. Menyalin halaman dan mengganti nama industri adalah _doorway page_, dan
Google menghukumnya. Kalau tidak sanggup menulis isinya, jangan buat halamannya.

---

## Gambar

Situs ini tidak memakai foto — tidak ada foto stok, tidak ada mockup 3D, tidak ada bingkai
perangkat. Yang ada hanya dua hal:

**1. Tangkapan layar produk nyata.** Ini yang diutamakan. Taruh berkasnya di
[`public/portofolio/`](public/portofolio/) (aturan penamaan, ukuran, dan redaksi ada di
README folder itu), lalu daftarkan di frontmatter studi kasus:

```yaml
cover:
  src: '/portofolio/p3m-pens-cover.png'
  alt: 'Daftar pengajuan di panel administrasi dengan status verifikasi'
```

Begitu `cover` ada, ia otomatis dipakai di kartu portofolio, halaman layanan, dan halaman
vertikal — tanpa menyentuh satu halaman pun.

**2. Skema SVG sebagai pengganti sementara**
([`Schematic.tsx`](src/components/blocks/Schematic.tsx)). Dipakai otomatis selama `cover`
belum ada, digambar dengan token warna sehingga terbaca di kedua tema dan tidak memakan
permintaan jaringan.

> **Jangan mencampur.** Satu kartu dengan tangkapan layar asli di sebelah satu kartu
> berskema terbaca setengah jadi. Kalau satu studi kasus diberi `cover`, semuanya diberi.

Catatan: tanpa `next/image`, tidak ada pengoptimal gambar. Ekspor tangkapan layar pada
lebar yang benar-benar dipakai (≤ 1600px) dan kompres sebelum dimasukkan.

---

## Formulir kontak

```
Formulir (client)  →  validasi Zod  →  POST /api/kontak
                                          ├─ rate limit 3/menit per IP → 429
                                          ├─ validasi ulang (skema yang sama)
                                          ├─ honeypot terisi → ditolak, pesan dibuang
                                          ├─ Resend  (surel)      ┐ best-effort,
                                          └─ Fonnte  (WhatsApp)   ┘ paralel
                                          → /kontak/terima-kasih
```

Logikanya ada di [`server/contact.ts`](server/contact.ts) dan dipasang di dua tempat:

- **Lokal** — plugin `leksana-contact-api` di `vite.config.ts` memasangnya sebagai
  middleware, jadi `npm run dev` dan `npm run preview` melayani `/api/kontak` betulan.
- **Produksi** — [`api/kontak.ts`](api/kontak.ts) untuk host yang menjalankan fungsi
  serverless (Vercel, Netlify, Cloudflare).

Kalau situs dipasang di hosting statis murni tanpa fungsi serverless, endpoint itu tidak
ada dan formulir akan gagal terkirim. Formulirnya menangani kegagalan itu dengan
mengarahkan ke WhatsApp, tetapi lebih baik gunakan host yang mendukung fungsi.

Tanpa variabel lingkungan, endpoint tetap mengembalikan 200 dan mencatat isi pesan ke log
— supaya pengembangan lokal tidak butuh kredensial apa pun.

Halaman terima kasih punya URL sendiri (bukan sekadar state) supaya konversinya bisa
dilacak, dan ber-`noindex` karena tidak ada gunanya di hasil pencarian.

---

## Deploy

Hasil build adalah satu SPA di `dist/`. Dua hal wajib diatur di host:

1. **Fallback ke `index.html`** untuk semua rute selain `/api/*`. Tanpa ini,
   `/portofolio/p3m-pens` akan 404 saat dibuka langsung atau di-refresh.
2. **Header keamanan** yang dulu diatur `next.config.ts`.

[`vercel.json`](vercel.json) sudah memuat keduanya. Untuk host lain, empat header yang
harus ada: `X-Content-Type-Options: nosniff`, `Referrer-Policy:
strict-origin-when-cross-origin`, `X-Frame-Options: DENY`, dan `Permissions-Policy:
camera=(), microphone=(), geolocation=(), interest-cohort=()`.

---

## Panel pengelolaan

Situs publik dan panel tinggal di satu aplikasi, tetapi tidak berbagi kerangka:
`SiteLayout` hanya membungkus halaman publik. Panel membawa kerangkanya sendiri —
alat kerja tidak perlu membawa navigasi pemasaran.

```
/auth/masuk        masuk
/auth/pilih-peran  muncul hanya kalau akun punya lebih dari satu peran
/panel/dasbor      ringkasan sesi + hak akses
/akses-ditolak     peran tidak mencakup halaman yang diminta
```

### Dua gerbang, berurutan

1. `ProtectedRoute` — ada token? Kalau tidak, ke halaman masuk. Kalau ya, **menu
   dimuat lebih dulu**, sebelum satu pun halaman anak dirender. Tanpa itu, halaman
   sempat dirender dengan izin kosong dan menampilkan penolakan yang sedetik
   kemudian ditarik lagi.
2. `MenuRoute` — kode menu halaman ini ada di daftar yang boleh dilihat?

Gagal memuat menu **tidak** diperlakukan sebagai "tidak punya akses". Backend mati
dan izin dicabut adalah dua hal berbeda, dan menampilkannya sama akan menyesatkan.

Sidebar dibangun dari izin sungguhan, bukan daftar tetap: editor dan administrator
benar-benar melihat menu yang berbeda. Menyembunyikan tombol tetap sekadar
kenyamanan — server memeriksa izin yang sama di setiap permintaan.

### Sesi

`lib/http.ts` menempelkan token dan header `X-Role-Active`, lalu memperbarui sesi
yang kedaluwarsa **satu kali** secara transparan sebelum menyerah. Refresh-nya
memakai axios polos, bukan instance ini — kalau tidak, permintaan yang justru
seharusnya memperbaiki 401 akan masuk lagi ke interceptor yang sama.

Token disimpan di cookie `SameSite=Lax`, dan `secure` di luar localhost.

Akun bermultiperan memilih perannya secara sengaja di `/auth/pilih-peran`. Memilih
otomatis akan membuat seseorang menyunting sebagai peran yang tidak ia sadari
sedang dipakai.

### Ukuran bundel

Seluruh kode panel dan halaman masuk dipisah lewat `React.lazy`, jadi pengunjung
yang membaca studi kasus tidak ikut mengunduh formulir masuk, klien HTTP, maupun
pustaka formulirnya. Bundel publik tetap ~434 kB; panel dan auth jadi berkas
terpisah yang hanya diambil saat dibuka.

### Pratinjau langsung

Menyunting tanpa melihat hasilnya berarti menyimpan dulu, membuka situs, lalu kembali —
tiga langkah untuk satu kalimat. Formulir studi kasus punya tiga tampilan, dipilih lewat
kontrol di kepala halaman dan diingat antar sesi:

| Tampilan | Untuk |
| --- | --- |
| **Tulis** | Menulis serius. Catatan panduan tampil di jalur pinggir. |
| **Tulis + pratinjau** | Bawaan di layar ≥ 75rem. Formulir kiri, hasil kanan, hidup. |
| **Pratinjau** | Memeriksa keseluruhan sebelum menerbitkan. |

Yang membuatnya bisa dipercaya: **komponen yang dipakai pratinjau adalah komponen yang
dipakai halaman terbit.** `CaseStudyArticleView`, `ProjectCardView`, `Decision`, `Figure`,
`Metrics`, `Note` — satu definisi, dua pemanggil. Pratinjau yang digambar ulang dengan
tangan akan melenceng, dan pratinjau yang melenceng lebih buruk daripada tidak ada:
editornya tetap menyimpan lalu memeriksa di situs.

Keputusan lain:

- **Dua permukaan, bukan satu.** Kartu portofolio *dan* halaman artikel. Separuh field
  hanya muncul di salah satunya — "Masalah" tidak pernah tampil di artikel, ia yang
  memikul kartu. Editor yang tidak melihatnya akan terus menulisnya untuk tempat yang salah.
- **Pratinjau mengikuti kursor.** Menyunting blok ke-22 sambil melihat blok pertama bukan
  pratinjau, itu benda kedua yang harus digulir. Blok yang sedang disunting dibawa ke
  layar — hanya kalau memang sedang tidak terlihat, supaya tidak melompat saat mengetik.
- **Di dalam `iframe`, dan itu keharusan.** Versi pertama merender pratinjau langsung di
  halaman. Itu aman selama lebarnya sama dengan kolomnya, dan langsung salah begitu
  lembarnya disempitkan ke lebar HP: `@media (min-width: 64rem)` dan setiap `vw` di
  stylesheet menjawab viewport **peramban**, bukan kotak tempatnya berada. Hasilnya lembar
  390px yang masih memakai tata letak desktop — empat kolom tabel fakta dijejalkan ke
  layar HP — dan melaporkan pemeriksaan HP yang sebenarnya tidak pernah terjadi. Hanya
  `iframe` yang memberi sebuah kotak viewport-nya sendiri. Harganya satu salinan
  stylesheet dan satu cermin tema; keduanya murah untuk pratinjau yang jujur.
- **Lebar HP 390px** sebagai sakelar. Situs ini menjanjikan hasil yang benar di HP; itu
  juga lebar tempat sebagian besar pembacanya datang. Bingkainya di-*outline*, bukan
  di-*border* — border memakan dua piksel viewport, dan 390px harus benar-benar 390px.
- **Formulir memadat saat terbelah.** Catatan panduan mengalah lebih dulu — ia mengajar
  saat formulir pertama kali ditemui, dan satu klik jauhnya di mode Tulis. Jalur catatan
  memakai container query pada kolom formulirnya sendiri, jadi ia tahu kolomnya menyempit
  meski jendelanya tidak.
- **Dengan pratinjau terbuka, halamannya berhenti menggulir.** Yang menggulir adalah dua
  kolomnya: tulisan di kiri, hasil di kanan, keduanya berpaut ke jendela. Tingginya diukur
  saat runtime (`useFillHeight`) dari sisi atas kolom sampai bilah simpan — versi CSS murni
  harus menghardcode tinggi kepala panel, judul halaman, dan sakelar mode, lalu salah
  begitu salah satunya berubah. Sebelum ini pratinjau memakai `max-height` + `sticky`, dan
  tingginya bergantung pada seberapa jauh halaman sedang digulir; itu sebabnya ia terpotong
  jadi 384px.
- **`useDeferredValue`.** Pratinjau dirender di belakang ketikan, tidak di depannya.

### Menambah halaman panel

1. Tambah kode menu di `MenuSeeder` pada backend, lalu beri grant di
   `RoleMenuSeeder`.
2. Tambah satu entri di `panelNav` (`src/config/panel.ts`).
3. Tambah rutenya di `src/pages/panel/PanelRoutes.tsx`, bungkus `MenuRoute`.

Navigasi, halaman pendaratan setelah masuk, dan penjagaan izin mengikuti sendiri.

### Memindahkan konten MDX ke CMS

Studi kasus lahir sebagai berkas MDX. Sekarang basis datanya yang jadi sumber;
MDX dipakai sekali sebagai bahan impor.

```bash
npm run import:content            # impor lalu verifikasi
npm run import:content -- --dry   # konversi + verifikasi saja, tidak menulis
```

Yang dijamin skrip ini:

- **Idempoten.** Entri dicocokkan lewat `contentKey` (nama berkas MDX-nya), jadi
  menjalankannya dua kali memperbarui, bukan menggandakan. Gambar dicocokkan lewat
  nama berkas, jadi tidak diunggah ulang.
- **Memeriksa dirinya sendiri.** Kata-kata di berkas sumber dan kata-kata di blok
  yang tertulis harus sama persis — termasuk setelah dibaca ulang dari basis data.
  Beda satu kata: impor berhenti dan menyebut kata keberapa yang meleset.
- **Menolak yang tidak dikenal.** Komponen MDX di luar `Metrics`, `Decision`,
  `Figure`, `Note`, dan `Annotation` menghentikan impor. Isi yang hilang diam-diam
  jauh lebih mahal daripada impor yang gagal.
- **Lewat API, bukan basis data.** Skrip menulis lewat endpoint yang sama dengan
  editor, jadi aturan yang menolak seorang editor juga menolak skrip ini.

Kredensial diambil dari `LEKSANA_ADMIN_EMAIL` / `LEKSANA_ADMIN_PASSWORD`, atau —
kalau tidak diset — dari seeder di `backend/src/appsettings.Development.json`.

`<Annotation>` jadi dua hal: isinya lebih dulu, lalu catatan pinggirnya sebagai
blok `note` sesudahnya. Itu satu-satunya penyusunan ulang yang dilakukan, dan
pemeriksa teksnya tahu soal itu.

---

## SEO — status sekarang

**Ini bagian yang belum selesai, dan sengaja.**

Vite menghasilkan SPA: `dist/index.html` yang sama dilayani untuk setiap URL, lalu React
mengisi isinya di peramban. Konsekuensinya:

- **Yang sudah jalan.** `sitemap.xml` dan `robots.txt` dibuat saat build dari konfigurasi
  yang sama dengan halamannya. Setiap halaman memasang `<title>`, deskripsi, canonical,
  OG, Twitter card, dan JSON-LD lewat `usePageMeta` + `<JsonLd>`. Googlebot menjalankan
  JavaScript, jadi ia melihat semuanya.
- **Yang belum.** Perayap yang tidak menjalankan JavaScript — pratinjau tautan WhatsApp,
  Facebook, LinkedIn, sebagian besar bot — hanya melihat isi `index.html`, yaitu
  metadata bawaan beranda. Membagikan tautan studi kasus akan menampilkan judul beranda.
- **Kartu OG** masih `public/og.svg`. Desainnya sama persis dengan versi Next, tetapi
  sebagian besar perayap tidak merender SVG; perlu diekspor jadi PNG 1200×630.

Perbaikannya (untuk dibahas terpisah) adalah pre-render saat build: menghasilkan satu
berkas HTML per rute, lengkap dengan `<head>` dan isi halamannya, sambil tetap
menghidrasi jadi SPA. Karena semua rute sudah bisa diketahui dari `src/config/` dan
`src/content/`, dan seluruh halaman sudah terbukti bisa dirender di Node, langkah itu
tidak menuntut perombakan — hanya belum dikerjakan.

---

## Performa

| Metrik                       | Hasil build                 |
| ---------------------------- | --------------------------- |
| JS (satu bundel, semua rute) | 430 kB · 128 kB gzip        |
| CSS                          | 33 kB · 7,7 kB gzip         |
| Huruf                        | 4 berkas woff2, 87 kB total |
| Permintaan pihak ketiga      | 0                           |

**Soal angka JS.** SPA mengirim satu bundel untuk semua rute, jadi angkanya lebih besar
daripada per-halaman versi Next (~103 kB bersama + ~2 kB per halaman). Isinya React,
React Router, Zod, dan seluruh konten MDX yang sudah dikompilasi. Menurunkannya berarti
memecah rute dengan `React.lazy` dan memindahkan validasi Zod ke sisi server saja —
keduanya masuk akal dan belum dikerjakan.

Yang menjaga sisanya tetap ringan:

- Hero berbasis teks — LCP adalah paragraf, bukan gambar
- Empat berkas huruf, subset latin, di-_self-host_; nol permintaan ke Google Fonts
- Nol pustaka animasi, nol pustaka ikon, nol sematan pihak ketiga
- Figur berupa SVG inline, bukan berkas gambar
- Sumber MDX tidak ikut terkirim: estimasi waktu baca dihitung saat build
  (`server/reading-time.ts`)

---

## Sebelum live

- [ ] `VITE_SITE_URL` diisi domain sungguhan
- [ ] `VITE_API_BASE_URL` diarahkan ke API produksi (path relatif, mis. `/api/v1`)
- [ ] `Cors:AllowedOrigins` di backend diisi domain situs
- [ ] Alur masuk diuji di produksi: masuk, muat ulang, ganti peran, keluar
- [ ] Nomor WhatsApp, surel, dan tautan sosial di `src/config/site.ts` diperbarui
- [ ] `RESEND_*` dan `FONNTE_*` diisi, lalu formulir diuji ujung ke ujung
- [ ] Fallback SPA di host diuji: buka `/portofolio/p3m-pens` langsung, lalu refresh
- [ ] Kartu OG diekspor jadi PNG 1200×630 dan `og:image` diarahkan ke sana
- [ ] Lighthouse dijalankan di HP dengan throttle; tangkapan layarnya diarsipkan
- [ ] Diuji di lebar 360px — tidak boleh ada gulir horizontal
- [ ] Diperiksa di mode terang **dan** gelap
- [ ] `sitemap.xml` dan `robots.txt` benar; tidak ada `noindex` yang tertinggal
- [ ] Google Search Console dipasang, sitemap disubmit
- [ ] Dicari sisa placeholder: `leksana.id`, `6281234567890`, `bayuhadileksana`
- [ ] Tiga status tampilan dicoba, termasuk muat ulang halaman setelah memilih gelap

---

## Lisensi

Milik pribadi. Bukan template, tidak untuk didistribusikan ulang.
