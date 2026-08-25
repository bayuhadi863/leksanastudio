# 07 — Arsitektur Teknis Situs Studio

> **Sudah diimplementasikan** di [`code/`](code/). Dokumen ini menjelaskan *kenapa*;
> [`code/README.md`](code/README.md) menjelaskan *cara menjalankannya*. Di mana
> implementasi mengoreksi rencana, angka di sini sudah diperbarui.

## 7.1 Kriteria Pemilihan

Situs studio dinilai dengan kriteria yang **berbeda** dari situs klien:

| Kriteria | Bobot | Alasan |
|---|---|---|
| Skor performa sempurna | **Tertinggi** | Skor ini dipamerkan. Situs studio yang lambat membatalkan seluruh klaim |
| Cepat menerbitkan artikel & studi kasus | Tinggi | Konten adalah mesin lead jangka panjang. Gesekan menulis = konten mati |
| SEO teknis kuat bawaan | Tinggi | Halaman vertikal & blog adalah saluran akuisisi utama |
| Biaya operasional mendekati nol | Sedang | Belum ada pendapatan saat dibangun |
| Bisa dipakai ulang jadi kerangka Lini B | Sedang | Apa pun yang dibangun di sini sebaiknya menjadi aset |
| Kemudahan bagi non-teknis | **Rendah** | Hanya kamu yang menyunting. Jangan bayar kompleksitas CMS untuk satu penyunting |

Kriteria terakhir itu yang menentukan: **jangan pasang CMS di awal.**

---

## 7.2 Stack

| Lapisan | Pilihan | Alasan |
|---|---|---|
| Framework | **Next.js 15 (App Router)** | Statis + server dalam satu framework; sama dengan stack produk klien sehingga komponen bisa saling pinjam |
| Bahasa | **TypeScript** | |
| Styling | **Tailwind CSS 4** + CSS variables | CSS variables jadi lapisan token ([06-sistem-desain.md](06-sistem-desain.md) §6.3) |
| Komponen | **Ditulis sendiri** | Situs ini butuh sedikit sekali komponen. Menarik pustaka komponen untuk tombol dan tabel menambah berat tanpa menambah apa pun |
| Konten | **MDX + file** (`next-mdx-remote`) | Studi kasus & artikel sebagai berkas `.mdx`, frontmatter divalidasi Zod saat build. Nol biaya, nol layanan eksternal, versi terlacak git |
| Form | **Zod + `<form>` biasa** | Tiga isian tidak perlu pustaka form. Skema Zod dipakai bersama oleh peramban dan server, jadi keduanya tidak bisa berbeda pendapat |
| Ikon | **Tidak ada** | Situs ini memakai ≤ 12 bentuk SVG inline. Pustaka ikon adalah berat tanpa imbalan |
| Animasi | **CSS saja** | Batasi sesuai §6.6. Jangan menambah pustaka animasi untuk fade |
| Hosting | **Vercel** (tier gratis) | Deploy dari git, CDN global, HTTPS otomatis, pratinjau per-branch |
| Analitik | **Vercel Analytics** + **GA4** | GA4 dibutuhkan kalau nanti beriklan |
| Form backend | **Resend** (email) + notifikasi WhatsApp | Gratis 3.000 email/bulan |
| Notifikasi WA | **Fonnte / Wablas** | Lead masuk harus sampai ke HP-mu dalam hitungan detik |
| Gambar | **Skema SVG inline** | Tidak ada foto sama sekali; figur digambar dengan token warna. `next/image` disiapkan untuk saat tangkapan layar asli masuk |

### Kenapa MDX, bukan CMS

| | MDX + file | CMS (Sanity/Payload/WordPress) |
|---|---|---|
| Biaya | Rp 0 | Rp 0–500 rb/bln + kompleksitas |
| Kecepatan | Statis penuh | Butuh cache & tuning |
| Menyunting dari HP | Tidak | Ya |
| Diagram & komponen kustom di dalam artikel | **Ya, bebas** | Terbatas |
| Riwayat perubahan | Git | Bergantung layanan |

Studi kasusmu berisi diagram, blok kode, dan komponen metrik. MDX menangani itu
secara alami; CMS akan melawanmu. Dan penyuntingnya hanya satu orang — kamu.

**Kapan pindah ke CMS:** saat ada orang lain yang harus menulis, atau saat kamu
benar-benar butuh menerbitkan dari HP. Bukan sebelum itu.

---

## 7.3 Arsitektur

```
                    ┌───────────────────────────┐
                    │   Repositori Git          │
                    │   kode + konten MDX       │
                    └─────────────┬─────────────┘
                                  │  push
                                  ▼
                    ┌───────────────────────────┐
                    │   Vercel — build statis    │
                    │   SSG + ISR untuk blog     │
                    └─────────────┬─────────────┘
                                  ▼
     ┌────────────────────────────────────────────────────┐
     │            namastudio.com  (CDN global)            │
     └───┬──────────────┬───────────────┬─────────────────┘
         │              │               │
         ▼              ▼               ▼
    Halaman        Studi kasus     Halaman vertikal
    statis         dari MDX        (data-driven, satu template)
         │
         ▼
    ┌──────────────────────────────────────┐
    │ Route handler /api/kontak            │
    │  → Resend (email ke kamu)            │
    │  → Fonnte  (WhatsApp ke kamu)        │
    │  → simpan ke berkas/DB ringan (ops.) │
    └──────────────────────────────────────┘
```

**Semua halaman statis kecuali route handler form.** Tidak ada basis data untuk v0.
Kalau nanti butuh menyimpan lead, tambahkan tabel di Postgres gratis (Neon/Supabase) —
tapi email + WhatsApp sudah cukup untuk 50 lead pertama.

---

## 7.4 Struktur Folder

```
01-studio/code/
├── content/                      ← SEMUA konten, terpisah dari kode
│   ├── studi-kasus/              satu berkas per proyek portofolio
│   └── catatan/                  satu berkas per tulisan
│
├── src/config/                   ← satu-satunya sumber kebenaran data & angka
│   ├── site.ts                   identitas studio + format rupiah
│   ├── routes.ts                 peta URL + navigasi
│   ├── packages.ts               paket, tahapan, add-on, termin, lantai harga
│   ├── services.ts               tiga lini layanan
│   ├── verticals.ts              data halaman vertikal
│   ├── process.ts                empat langkah + tiga ketakutan
│   └── copy.ts                   FAQ, daftar yang ditolak, pilar bukti, tentang
│
├── src/lib/                      ← fungsi murni, tanpa JSX
│   ├── content.ts                pembaca MDX + validasi frontmatter
│   ├── seo.ts · structured-data.ts
│   ├── whatsapp.ts · contact-schema.ts · format.ts · cn.ts
│
├── src/components/
│   ├── ui/                       Button · Note · Label · ArrowLink · ContactForm
│   ├── layout/                   Header · Footer · WhatsAppBar · Document · Annotation
│   ├── blocks/                   Hero · ProjectCard · PricingTable · PhaseList · …
│   └── mdx/                      komponen yang boleh dipakai di dalam MDX
│
├── src/app/                      ← routing + komposisi saja
│   ├── page.tsx · layanan/ · portofolio/ · catatan/ · [vertical]/
│   ├── proses · harga · tentang · kontak · kebijakan-privasi
│   ├── api/kontak/route.ts
│   ├── globals.css               ← design system
│   └── sitemap.ts · robots.ts · opengraph-image.tsx · icon.svg
└── public/
```

### `src/config/site.ts` — aturan wajib

```ts
export const site = {
  nama: "[NAMA STUDIO]",
  tagline: "Dibangun untuk dipakai bertahun-tahun",
  domain: "https://namastudio.com",
  email: "halo@namastudio.com",
  wa: { nomor: "62812xxxxxxx", pesanDefault: "Halo, saya ingin diskusi soal ..." },
  kota: "Surabaya",
  sosial: { instagram: "", linkedin: "", github: "" },
  harga: { mulaiA: 3_500_000, mulaiB: 15_000_000, mulaiC: 40_000_000 },
  janji: { revisi: 2, hariGaransi: 60, jedaKabarHari: 3 },
} as const
```

> **Aturan yang sama seperti di produk klien:** kalau ada nilai identitas atau angka
> janji yang ditulis langsung di dalam komponen, itu **bug arsitektur**, bukan
> sekadar kurang rapi. Mengganti nomor WhatsApp harus satu baris.

---

## 7.5 Halaman Vertikal dari Data

Halaman vertikal (`/jasa-website-klinik-gigi`, `/jasa-website-klinik-kecantikan`, …)
adalah mesin SEO utama untuk posisi generalis. Jangan menulisnya satu per satu.

```ts
// src/config/verticals.ts — satu entri di dalam array
{
  slug: 'jasa-website-klinik-gigi',
  industry: 'Klinik gigi',
  headline: 'Jasa pembuatan website klinik gigi',
  problems: [
    "Pasien mencari lewat HP, lalu menyerah karena nomor WhatsApp tidak terlihat",
    "Iklan diarahkan ke halaman depan, bukan ke halaman layanan yang diiklankan",
    "Website lama tidak bisa diperbarui sendiri",
  ],
  deliverables: ['Halaman terpisah per perawatan', '…'],
  note: '…',                       // catatan pinggir khusus vertikal ini
  serviceSlug: 'website-bisnis',
  pricingShape: 'business-packages',
  whatsappIntro: 'Halo, saya dari klinik gigi. …',
  faq: [ /* khusus vertikal ini */ ],
}
```

Satu template `app/[vertical]/page.tsx` merender semuanya, dengan `dynamicParams = false`
sehingga slug yang tidak terdaftar menghasilkan 404 yang bersih. Menambah vertikal baru =
menambah satu entri. **Bukan** menyalin halaman.

Strategi kata kunci & daftar vertikal: [08-seo-mesin-konten.md](08-seo-mesin-konten.md).

---

## 7.6 Anggaran Performa

Situs studio memakai target lebih ketat dari situs klien, karena skornya dipamerkan.

| Metrik | Target | Batas keras |
|---|---|---|
| Lighthouse Performance (HP) | **100** | 98 |
| Accessibility · Best Practices · SEO | **100** | 100 |
| LCP | < 1,2 dtk | 1,8 dtk |
| CLS | 0 | 0,05 |
| INP | < 100 ms | 200 ms |
| JS awal (halaman depan) | ~103 KB terkompresi | 130 KB |
| Berat halaman depan | < 500 KB | 800 KB |

> **Soal angka 103 KB.** Target awal dokumen ini adalah < 90 KB, dan itu tidak
> tercapai. Penyebabnya bukan kode situs: ~103 KB adalah lantai runtime React + router
> Next.js App Router yang terkirim di tiap halaman apa pun isinya. Kode situs sendiri
> menyumbang ~2 KB per halaman. Menurunkannya lebih jauh berarti mengganti framework,
> bukan mengoptimasi halaman — dan itu bukan pertukaran yang sepadan untuk situs ini.

**Yang paling sering merusak skor 100, urut:**
1. Font yang dimuat dari luar tanpa `display: swap` atau terlalu banyak bobot
2. Gambar tanpa `width`/`height`
3. Skrip analitik yang dimuat sebelum interaktif
4. Sematan pihak ketiga (feed Instagram, widget chat) — **jangan pasang**

---

## 7.7 SEO Teknis (Wajib Ada Sejak v0)

- `sitemap.ts` dan `robots.ts` dibangkitkan otomatis dari daftar konten
- Metadata per halaman: `title`, `description`, canonical, OG image
- `opengraph-image.tsx` — OG image dibangkitkan otomatis per studi kasus & artikel
- **Structured data JSON-LD:**
  - `Organization` / `ProfessionalService` di seluruh situs
  - `Service` di tiap halaman layanan & vertikal
  - `Article` di tiap artikel blog
  - `FAQPage` di tiap blok FAQ
  - `BreadcrumbList` di halaman dalam
- Slug bahasa Indonesia, deskriptif, tanpa tanggal (`/blog/proxy-hanya-get-post`)
- Satu `<h1>` per halaman, hierarki heading berurutan

---

## 7.8 Form Kontak & Alur Lead

```
Pengunjung isi form (3 field: nama, WhatsApp, kebutuhan)
              │
              ▼
   /api/kontak  →  validasi Zod  →  honeypot + rate limit
              │
      ┌───────┴────────┬──────────────────┐
      ▼                ▼                  ▼
  Email (Resend)   WA ke HP-mu      Event GA4
  ke halo@...      (Fonnte)         "lead_masuk"
      │
      ▼
  Halaman terima kasih  →  ajak lanjut ke WhatsApp langsung
```

**Tiga field, tidak lebih.** Setiap field tambahan menurunkan pengiriman form.
Kualifikasi dilakukan di percakapan, bukan di form.

**Perlindungan spam:** honeypot + rate limit per IP. **Jangan pasang CAPTCHA** —
ia menurunkan konversi lebih banyak daripada spam yang dicegahnya pada volume sekecil ini.

**Halaman terima kasih wajib punya URL sendiri** (`/kontak/terima-kasih`) supaya
konversi bisa dilacak dan dijadikan target iklan.

---

## 7.9 Biaya Operasional

| Item | Biaya/bulan |
|---|---|
| Hosting Vercel (Hobby) | Rp 0 |
| Domain `.com` atau `.id` | ~Rp 15–30 rb (dari tahunan) |
| Resend (3.000 email) | Rp 0 |
| Fonnte (paket dasar) | ~Rp 20–50 rb |
| Vercel Analytics | Rp 0 di tier gratis |
| **Total** | **< Rp 100 rb/bulan** |

Naik ke Vercel Pro (~USD 20/bln) hanya kalau situs klien ikut di-host di sana secara
komersial — dan pada titik itu biaya sudah dibayar klien.

---

## 7.10 Checklist Sebelum Live

- [ ] Lighthouse 100/100/100/100 di HP (mode throttle), tangkapan layar disimpan
- [ ] Diuji nyata di HP, bukan hanya di emulator browser
- [ ] Semua tautan WhatsApp membuka aplikasi dengan pesan awal yang benar
- [ ] Form terkirim → email masuk → notifikasi WhatsApp masuk → event GA4 tercatat
- [ ] `sitemap.xml` dan `robots.txt` benar; **tidak ada `noindex` yang tertinggal**
- [ ] OG image tampil benar saat tautan dibagikan di WhatsApp, LinkedIn, Instagram
- [ ] Halaman 404 kustom dengan tautan kembali yang berguna
- [ ] Kebijakan privasi & syarat-ketentuan terisi (bukan lorem)
- [ ] Google Search Console terpasang & sitemap disubmit
- [ ] Tidak ada teks placeholder yang tertinggal — cari `[NAMA`, `lorem`, `TODO`, `xxx`
- [ ] Terbaca benar di mode gelap
- [ ] Semua studi kasus sudah lolos aturan izin di [05-portofolio-studi-kasus.md](05-portofolio-studi-kasus.md) §5.5
