import { routes } from './routes'
import type { QA } from './services'
import { site } from './site'

/** Homepage FAQ. Six questions, honest answers. The rest live on service pages. */
export const homeFaq: readonly QA[] = [
  {
    question: 'Anda kerja sendiri atau punya tim?',
    answer:
      'Sendiri. Artinya tidak ada yang hilang di antara oper-operan, dan Anda selalu bicara dengan orang yang benar-benar mengerjakan. Konsekuensinya juga saya sampaikan terus terang: saya hanya menerima satu proyek besar dalam satu waktu.',
  },
  {
    question: 'Berapa lama pengerjaannya?',
    answer:
      'Landing page 5–7 hari, website lengkap 2–3 minggu, company profile 4–10 minggu, sistem web dihitung per tahap. Penyebab molor paling sering bukan pengerjaan, melainkan konten dari klien yang datang terlambat — jadi daftar kebutuhannya saya kirim di hari pertama.',
  },
  {
    question: 'Website-nya nanti milik siapa?',
    answer:
      'Milik Anda. Domain, hosting, dan repositori kode didaftarkan atas nama Anda sejak hari pertama, dibayar langsung oleh Anda. Saya tidak menalangi keduanya — itu sumber sengketa nomor satu di jasa web.',
  },
  {
    question: 'Kalau nanti saya mau tambah fitur?',
    answer:
      'Dihitung terpisah dan selalu saya sampaikan sebelum dikerjakan. Tidak ada pekerjaan tambahan yang dikerjakan diam-diam lalu muncul di tagihan.',
  },
  {
    question: 'Kenapa tidak ada paket di bawah Rp 3,5 juta?',
    answer:
      'Karena pada harga di bawah itu, yang bisa saya kerjakan hanyalah memasang template dan mengganti teksnya. Hasilnya mungkin terlihat baik dan tetap tidak mendatangkan siapa pun. Kalau anggaran Anda memang di sana, saya lebih suka mengatakannya sekarang daripada mengecewakan Anda tiga bulan lagi.',
  },
  {
    question: 'Bisakah sekalian mengurus iklan dan SEO?',
    answer:
      'Struktur SEO dan pelacakan konversi sudah termasuk. Pengelolaan kampanye iklan berbulan-bulan bukan bidang saya — saya menyiapkan fondasinya, dan dengan senang hati merekomendasikan orang yang memang mengerjakan itu.',
  },
]

export type OutOfScopeItem = {
  readonly title: string
  readonly reason: string
}

/** The list of refused work. It feels like turning down money; its job is to filter. */
export const outOfScope: readonly OutOfScopeItem[] = [
  {
    title: 'Memperbaiki atau melanjutkan situs WordPress warisan vendor lain',
    reason: 'Waktunya tidak bisa diperkirakan, dan saya mewarisi masalah yang bukan buatan saya.',
  },
  {
    title: 'Proyek di bawah Rp 3,5 juta',
    reason: 'Pada rentang itu saya tidak bisa memberi hasil yang layak dipertanggungjawabkan.',
  },
  {
    title: 'Redesign tanpa tujuan bisnis yang jelas',
    reason:
      'Kalau tidak ada jawaban untuk “apa yang harus berubah setelah ini jadi”, proyeknya akan berputar tanpa akhir.',
  },
  {
    title: 'Pekerjaan yang dibayar penuh setelah jadi',
    reason: 'Tanpa pembayaran di muka, tidak mulai. Termasuk untuk kenalan.',
  },
  {
    title: 'Klaim medis dan klaim hasil yang tidak bisa dibuktikan',
    reason: 'Meski diminta klien. Ini juga melindungi klien.',
  },
  {
    title: 'Aplikasi mobile native, game, dan desain grafis murni',
    reason: 'Bukan bidang saya. Saya lebih suka merekomendasikan orang lain.',
  },
]

export type ProofPillar = {
  readonly title: string
  readonly body: string
  readonly proofLabel: string
  readonly proofHref: string
}

export const proofPillars: readonly ProofPillar[] = [
  {
    title: 'Kedalaman yang bisa dicek',
    body: 'Proyek terakhir saya: sistem informasi dengan 30 modul, empat peran pengguna, izin per-aksi yang bisa diubah tanpa deploy ulang, dan integrasi akun kampus. Dikerjakan sendiri, dari basis data sampai deployment.',
    proofLabel: 'Baca studi kasusnya',
    proofHref: routes.caseStudy('p3m-pens'),
  },
  {
    title: 'Cepat karena sudah dibangun sebelumnya',
    body: 'Untuk website bisnis saya tidak mulai dari nol. Ada inti produk yang sudah matang per jenis usaha, jadi waktu terpakai untuk hal yang khas bisnis Anda — bukan untuk membuat ulang tombol.',
    proofLabel: 'Lihat layanannya',
    proofHref: routes.service('website-bisnis'),
  },
  {
    title: 'Anda tidak tersandera',
    body: 'Kode, domain, hosting, dan akun atas nama Anda. Dokumentasi diserahkan tertulis, dan sesi pelatihannya direkam. Kalau suatu hari Anda ganti pengembang, tidak ada satu pun hal yang harus diminta dari saya.',
    proofLabel: 'Lihat cara kerjanya',
    proofHref: routes.process,
  },
]

export const home = {
  headline: 'Website dan sistem web yang dibangun untuk dipakai bertahun-tahun.',
  subheadline:
    'Untuk pemilik bisnis dan unit institusi yang sudah pernah kecewa dengan website yang cantik di awal lalu ditinggalkan. Saya bangun, saya dokumentasikan, dan saya serahkan sepenuhnya ke Anda.',
  heroNote:
    'Klien terakhir saya menerima 13 dokumen teknis bersama sistemnya. Supaya kalau suatu hari mereka ganti pengembang, tidak ada yang perlu diminta dari saya.',
  latestLabel: 'Terakhir dikirim',
  latestBody:
    'Sistem informasi 30 modul untuk unit penelitian Politeknik Elektronika Negeri Surabaya.',
  pricingNote:
    'Harga ditampilkan supaya Anda bisa menilai sendiri tanpa perlu bertanya. Kalau anggaran Anda di bawah angka terkecil di sini, saya lebih suka mengatakannya sekarang.',
} as const

export const about = {
  intro: `Saya ${site.ownerName}. Saya membangun website dan sistem web dari ${site.city}, sendirian, untuk pemilik bisnis dan unit institusi yang butuh sesuatu yang benar-benar dipakai — bukan sesuatu yang diluncurkan lalu ditinggalkan.`,
  note: 'Saya tidak menulis “kami”. Bahasa jamak yang palsu selalu ketahuan, dan kalau saya berbohong soal hal sekecil itu, wajar Anda meragukan hal yang lebih besar.',
  credibility: [
    {
      title: 'Sistem production yang sedang dipakai',
      body: 'Sistem informasi untuk unit penelitian sebuah politeknik negeri: 30 modul, 34 entitas basis data, empat peran, lima alur pengajuan dengan verifikasi, dan integrasi akun kampus. Enam minggu, dikerjakan sendiri dari basis data sampai TLS.',
    },
    {
      title: 'Dokumentasi yang benar-benar diserahkan',
      body: 'Tiga belas dokumen teknis berbahasa Indonesia plus satu buku PDF, termasuk tutorial menambah modul baru untuk pengembang berikutnya. Bukan lampiran formalitas — itu yang membuat klien tidak tersandera.',
    },
    {
      title: 'Bekerja di dalam batasan, bukan mengeluhkannya',
      body: 'Proxy kampus yang hanya meneruskan GET dan POST. Firewall yang menolak HTML di badan permintaan. Saya menyelesaikannya tanpa merusak desain API, lalu menuliskan alasannya supaya orang berikutnya tidak salah menyentuh.',
    },
  ],
  principles: [
    {
      title: 'Saya menolak lebih banyak daripada yang saya ambil',
      body: 'Ada daftar tertulis pekerjaan yang tidak saya kerjakan. Bukan karena sombong — karena di luar itu saya tidak bisa memberi hasil yang layak dipertanggungjawabkan.',
    },
    {
      title: 'Alasan keputusan selalu ditulis',
      body: 'Keputusan teknis yang tidak jelas alasannya akan jadi beban orang berikutnya. Jadi saya menulis: kenapa X, apa alternatifnya, dan apa harganya.',
    },
    {
      title: 'Satu proyek besar dalam satu waktu',
      body: 'Dua proyek besar bersamaan akan mematikan keduanya. Kalau saya sedang penuh, saya katakan sejak awal — bukan menerima lalu mengulur.',
    },
  ],
  notMyField: [
    'Aplikasi mobile native',
    'Game dan grafis 3D',
    'Desain grafis dan branding visual murni',
    'Pengelolaan kampanye iklan berbulan-bulan',
    'Pemeliharaan situs WordPress warisan vendor lain',
  ],
} as const
