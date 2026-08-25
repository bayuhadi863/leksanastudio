/**
 * Packages and pricing. These numbers surface on the pricing page, service
 * pages, vertical landing pages and JSON-LD. Change once, changes everywhere.
 */

/** `true` = included · `false` = not included · string = qualifier. */
export type FeatureValue = boolean | string

export type PackageFeature = {
  readonly label: string
  readonly value: FeatureValue
}

export type ServicePackage = {
  readonly code: string
  readonly name: string
  readonly audience: string
  readonly price: number
  readonly priceNote?: string
  readonly duration: string
  readonly summary: string
  readonly highlighted?: boolean
  readonly features: readonly PackageFeature[]
}

export const businessPackages: readonly ServicePackage[] = [
  {
    code: 'A1',
    name: 'Landing konversi',
    audience: 'Satu layanan, satu tujuan, siap diiklankan',
    price: 3_500_000,
    duration: '5–7 hari',
    summary:
      'Satu halaman panjang yang dirancang untuk satu tindakan: pengunjung menghubungi Anda.',
    features: [
      { label: 'Jumlah halaman', value: '1 halaman panjang' },
      { label: 'Booking WhatsApp kontekstual', value: true },
      { label: 'Halaman layanan terpisah', value: false },
      { label: 'SEO lokal & data terstruktur', value: 'Dasar' },
      { label: 'Pelacakan konversi', value: true },
      { label: 'Panel ubah konten sendiri', value: false },
      { label: 'Landing page khusus iklan', value: false },
      { label: 'Blog & artikel awal', value: false },
      { label: 'Sesi pelatihan', value: false },
      { label: 'Garansi perbaikan', value: '30 hari' },
    ],
  },
  {
    code: 'A2',
    name: 'Website lengkap',
    audience: 'Bisnis dengan beberapa layanan yang perlu ditemukan',
    price: 8_500_000,
    duration: '2–3 minggu',
    highlighted: true,
    summary:
      'Tiap layanan punya halamannya sendiri, sehingga tiap layanan bisa diiklankan dan ditemukan sendiri.',
    features: [
      { label: 'Jumlah halaman', value: '8–12 halaman' },
      { label: 'Booking WhatsApp kontekstual', value: true },
      { label: 'Halaman layanan terpisah', value: true },
      { label: 'SEO lokal & data terstruktur', value: true },
      { label: 'Pelacakan konversi', value: true },
      { label: 'Panel ubah konten sendiri', value: true },
      { label: 'Landing page khusus iklan', value: false },
      { label: 'Blog & artikel awal', value: false },
      { label: 'Sesi pelatihan', value: '1 sesi' },
      { label: 'Garansi perbaikan', value: '60 hari' },
    ],
  },
  {
    code: 'A3',
    name: 'Lengkap + growth',
    audience: 'Bisnis yang sudah beriklan dan ingin biayanya bekerja',
    price: 15_000_000,
    duration: '3–4 minggu',
    summary:
      'Website lengkap ditambah halaman iklan khusus dan blog, supaya trafik berbayar dan trafik pencarian punya tempat mendarat yang benar.',
    features: [
      { label: 'Jumlah halaman', value: '12–18 halaman' },
      { label: 'Booking WhatsApp kontekstual', value: true },
      { label: 'Halaman layanan terpisah', value: true },
      { label: 'SEO lokal & data terstruktur', value: true },
      { label: 'Pelacakan konversi', value: true },
      { label: 'Panel ubah konten sendiri', value: true },
      { label: 'Landing page khusus iklan', value: '2 halaman' },
      { label: 'Blog & artikel awal', value: '3 artikel' },
      { label: 'Sesi pelatihan', value: '2 sesi' },
      { label: 'Garansi perbaikan', value: '90 hari' },
    ],
  },
]

export const corporatePackages: readonly ServicePackage[] = [
  {
    code: 'B1',
    name: 'Company profile',
    audience: 'Perusahaan yang butuh wajah resmi di internet',
    price: 15_000_000,
    priceNote: 'Rp 15–22 juta, menyempit setelah ruang lingkup disepakati',
    duration: '4–6 minggu',
    summary:
      'Desain dibuat dari nol — bukan dari tema. Sepuluh sampai lima belas halaman dengan panel konten dan opsi dua bahasa.',
    features: [
      { label: 'Desain', value: 'Dari nol, bukan dari tema' },
      { label: 'Jumlah halaman', value: '10–15 halaman' },
      { label: 'Panel konten', value: true },
      { label: 'Dua bahasa', value: 'Opsional' },
      { label: 'Katalog / direktori terstruktur', value: false },
      { label: 'Dokumentasi serah terima', value: true },
    ],
  },
  {
    code: 'B2',
    name: 'Situs institusi / katalog',
    audience: 'Institusi dan perusahaan dengan data yang harus dicari',
    price: 22_000_000,
    priceNote: 'Rp 22–40 juta, tergantung jumlah jenis data',
    duration: '6–10 minggu',
    summary:
      'Semua yang ada di Company profile, ditambah katalog atau direktori dengan pencarian, penyaringan, dan halaman dinamis per entri.',
    features: [
      { label: 'Desain', value: 'Dari nol, bukan dari tema' },
      { label: 'Jumlah halaman', value: '15+ dan halaman dinamis' },
      { label: 'Panel konten', value: true },
      { label: 'Dua bahasa', value: 'Opsional' },
      { label: 'Katalog / direktori terstruktur', value: true },
      { label: 'Dokumentasi serah terima', value: true },
    ],
  },
]

export type ProjectPhase = {
  readonly step: number
  readonly name: string
  readonly price: string
  readonly duration: string
  readonly scope: string
  readonly note?: string
}

export const systemPhases: readonly ProjectPhase[] = [
  {
    step: 0,
    name: 'Lokakarya ruang lingkup',
    price: 'Rp 3–8 juta',
    duration: '1–2 minggu',
    scope:
      'Wawancara, peta proses, daftar peran dan izin, model data kasar, wireframe alur utama, dan estimasi berjenjang.',
    note: 'Berbayar, dan hasilnya milik Anda — bahkan kalau kita tidak lanjut ke tahap berikutnya.',
  },
  {
    step: 1,
    name: 'MVP fungsional',
    price: 'Rp 40–90 juta',
    duration: '2–4 bulan',
    scope:
      'Modul inti, autentikasi dan peran, alur utama dari ujung ke ujung, deployment, dan dokumentasi.',
  },
  {
    step: 2,
    name: 'Kelengkapan & integrasi',
    price: 'Rp 25–70 juta',
    duration: '1–3 bulan',
    scope:
      'Modul pendukung, laporan dan ekspor, impor massal, serta integrasi dengan sistem yang sudah ada.',
  },
  {
    step: 3,
    name: 'Retainer operasional',
    price: 'Rp 3–6 juta / bulan',
    duration: 'Berkelanjutan',
    scope: 'Monitoring, backup, perbaikan, iterasi kecil, dan waktu respons yang disepakati.',
  },
]

export type AddOn = {
  readonly name: string
  readonly price: string
  readonly appliesTo: string
}

export const addOns: readonly AddOn[] = [
  { name: 'Penulisan konten profesional', price: 'Rp 300–600 rb / halaman', appliesTo: 'Semua' },
  { name: 'Setup Google Business Profile', price: 'Rp 1–1,5 juta', appliesTo: 'Website bisnis' },
  {
    name: 'Setup & kelola Google Ads bulan pertama',
    price: 'Rp 2,5 juta',
    appliesTo: 'Website bisnis',
  },
  {
    name: 'Panel konten lanjutan',
    price: 'Rp 3–6 juta',
    appliesTo: 'Website bisnis & perusahaan',
  },
  {
    name: 'Dokumentasi teknis & serah terima terstruktur',
    price: 'Rp 4–10 juta',
    appliesTo: 'Sistem web',
  },
  { name: 'Migrasi data dari sistem lama', price: 'Rp 3–15 juta', appliesTo: 'Sistem web' },
  { name: 'Pelatihan tim (sesi 2 jam)', price: 'Rp 750 rb / sesi', appliesTo: 'Semua' },
]

export type PaymentTerm = {
  readonly scope: string
  readonly schedule: string
}

export const paymentTerms: readonly PaymentTerm[] = [
  { scope: 'Landing konversi', schedule: '50% di muka · 50% saat serah terima' },
  {
    scope: 'Website lengkap & growth',
    schedule: '50% di muka · 30% saat draf disetujui · 20% saat live',
  },
  {
    scope: 'Company profile & situs institusi',
    schedule: '40% di muka · 30% saat desain disetujui · 30% saat live',
  },
  {
    scope: 'Sistem web',
    schedule: 'Lokakarya lunas di muka · tiap tahap berikutnya 40/30/30',
  },
  { scope: 'Retainer', schedule: 'Di muka tiap bulan, atau tahunan dengan potongan 10%' },
]

/** The floor that is defended. Written on the pricing page, not hidden. */
export const PRICE_FLOOR = 3_500_000
