import { businessPackages, corporatePackages, systemPhases } from './packages'

export type Problem = {
  readonly title: string
  readonly body: string
}

export type QA = {
  readonly question: string
  readonly answer: string
}

export type PricingShape = 'business-packages' | 'corporate-packages' | 'phases'

export type Service = {
  readonly slug: string
  /** Heading form. Used as a title, never inside a sentence. */
  readonly name: string
  /**
   * Sentence form, lower case. "Lihat layanan {shortName} selengkapnya" has to
   * read like Indonesian, which a four-word heading does not.
   */
  readonly shortName: string
  readonly audience: string
  readonly headline: string
  readonly summary: string
  readonly startingPrice: number
  readonly startingPriceLabel: string
  /** Problems specific to this audience. Never generic complaints. */
  readonly problems: readonly Problem[]
  readonly deliverables: readonly string[]
  readonly exclusions: readonly string[]
  /** Slug of the most relevant case study, or null while none exists yet. */
  readonly caseStudySlug: string | null
  readonly pricingShape: PricingShape
  readonly faq: readonly QA[]
}

export const services: readonly Service[] = [
  {
    slug: 'website-bisnis',
    name: 'Website bisnis',
    shortName: 'website bisnis',
    audience: 'Klinik, jasa lokal, dan UKM',
    headline: 'Website yang mendatangkan pelanggan, bukan sekadar ada',
    summary:
      'Untuk bisnis jasa lokal yang pelanggannya mencari lewat HP dan memutuskan dalam hitungan detik.',
    startingPrice: businessPackages[0]?.price ?? 3_500_000,
    startingPriceLabel: 'Mulai Rp 3,5 juta',
    problems: [
      {
        title: 'Nomor WhatsApp harus dicari dulu',
        body: 'Pengunjung membuka situs di HP, tidak langsung menemukan cara menghubungi, lalu kembali ke hasil pencarian. Kalah bukan karena kalah bagus — kalah karena kalah cepat.',
      },
      {
        title: 'Iklan mendarat di halaman depan',
        body: 'Anda membayar iklan untuk satu layanan tertentu, tetapi pengunjung mendarat di beranda yang bicara tentang semuanya. Biaya iklan terbuang di setiap klik.',
      },
      {
        title: 'Tidak bisa diperbarui sendiri',
        body: 'Mengganti jam buka atau menambah satu layanan berarti menunggu orang lain. Akhirnya tidak pernah diperbarui, dan situsnya perlahan menjadi salah.',
      },
    ],
    deliverables: [
      'Halaman terpisah untuk tiap layanan, sehingga tiap layanan bisa diiklankan dan ditemukan sendiri',
      'Tombol WhatsApp yang membawa pesan awal berbeda sesuai halaman asalnya',
      'Struktur SEO lokal dan data terstruktur, supaya muncul benar di pencarian dan di peta',
      'Pelacakan konversi yang siap dipakai Google Ads dan Meta',
      'Panel untuk mengubah konten sendiri, tanpa perlu menghubungi saya',
      'Domain, hosting, dan seluruh akun atas nama Anda sejak hari pertama',
    ],
    exclusions: [
      'Penulisan seluruh konten dan pengadaan foto — tersedia sebagai tambahan',
      'Pengelolaan kampanye iklan berbulan-bulan. Saya menyiapkan pelacakannya, bukan menjalankan kampanyenya',
      'Integrasi dengan sistem kasir atau rekam medis yang belum disebut di ruang lingkup',
    ],
    caseStudySlug: null,
    pricingShape: 'business-packages',
    faq: [
      {
        question: 'Berapa lama sampai situsnya bisa dipakai?',
        answer:
          'Landing konversi 5–7 hari, website lengkap 2–3 minggu. Penyebab molor paling sering bukan pengerjaan, melainkan konten dari klien yang belum lengkap — jadi daftar kebutuhan konten saya kirim di hari pertama.',
      },
      {
        question: 'Saya belum punya foto dan teks. Bagaimana?',
        answer:
          'Bisa dibantu. Penulisan konten dihitung terpisah per halaman, dan untuk foto saya bantu koordinasikan dengan fotografer. Yang tidak saya lakukan adalah memakai foto stok — itu terlihat, dan justru menurunkan kepercayaan.',
      },
      {
        question: 'Apakah nanti saya bisa mengubah isinya sendiri?',
        answer:
          'Ya, mulai paket Website lengkap. Anda bisa mengubah teks, harga, jam buka, layanan, dan galeri. Yang tidak bisa diubah sendiri adalah struktur halaman — itu disengaja, supaya tata letaknya tidak rusak.',
      },
      {
        question: 'Kenapa tidak pakai WordPress saja?',
        answer:
          'WordPress bukan pilihan yang buruk. Tapi untuk situs jasa lokal, ia menuntut perawatan plugin terus-menerus dan biaya hosting bulanan yang tidak perlu. Kebutuhan sebenarnya — "biar saya bisa edit sendiri" — tetap saya penuhi tanpa itu.',
      },
    ],
  },
  {
    // Slug stays `website-perusahaan`: it carries the "jasa website perusahaan"
    // query while the visible name carries "company profile". Two real search
    // phrases, one page.
    slug: 'website-perusahaan',
    name: 'Company profile & situs institusi',
    shortName: 'company profile',
    audience: 'Perusahaan dan institusi',
    headline: 'Wajah resmi yang tidak perlu dijelaskan lagi',
    summary:
      'Untuk perusahaan dan institusi yang situsnya akan dibaca calon mitra, calon karyawan, dan pihak yang sedang menilai Anda.',
    startingPrice: corporatePackages[0]?.price ?? 15_000_000,
    startingPriceLabel: 'Mulai Rp 15 juta',
    problems: [
      {
        title: 'Situs lama tidak lagi mewakili',
        body: 'Perusahaan tumbuh, layanan bertambah, tetapi situsnya berhenti di lima tahun lalu. Setiap kali dikirim ke calon mitra, ada yang perlu dijelaskan lebih dulu.',
      },
      {
        title: 'Informasi ada, tetapi tidak bisa ditemukan',
        body: 'Katalog produk, daftar layanan, atau direktori anggota tersimpan di dokumen. Orang yang mencarinya harus bertanya, dan pertanyaan itu berulang setiap minggu.',
      },
      {
        title: 'Terikat pada vendor sebelumnya',
        body: 'Domain atas nama orang lain, kode tidak pernah diserahkan, dan setiap perubahan kecil harus mengantre. Ini masalah kepemilikan, bukan masalah teknis.',
      },
    ],
    deliverables: [
      'Desain dibuat dari nol untuk perusahaan Anda — bukan tema yang diganti warnanya',
      'Struktur konten yang bisa tumbuh: kategori, katalog, direktori, halaman dinamis',
      'Panel konten untuk tim Anda, dengan pratinjau yang benar-benar sama dengan hasil akhirnya',
      'Dukungan dua bahasa bila diperlukan',
      'Dokumentasi serah terima: cara mengelola, cara deploy, dan apa yang sebaiknya tidak disentuh',
      'Domain, hosting, dan repositori kode atas nama perusahaan Anda',
    ],
    exclusions: [
      'Penulisan seluruh konten korporat dan penerjemahan — tersedia sebagai tambahan',
      'Fotografi dan videografi profil perusahaan',
      'Migrasi dari sistem lama yang belum diperiksa isinya. Itu masuk lokakarya terpisah',
    ],
    caseStudySlug: 'p3m-pens',
    pricingShape: 'corporate-packages',
    faq: [
      {
        question: 'Apa bedanya dengan paket website bisnis?',
        answer:
          'Desainnya dibuat dari nol, bukan dari inti produk yang sudah ada. Itu yang membuat selisih harga dan selisih waktunya. Kalau kebutuhan Anda sebenarnya sudah tercakup paket bisnis, saya akan mengatakannya — bukan menjual yang lebih mahal.',
      },
      {
        question: 'Bisakah situsnya dua bahasa?',
        answer:
          'Bisa. Yang perlu disiapkan bukan sisi teknisnya, melainkan siapa yang menulis dan meninjau versi bahasa kedua. Tanpa itu, versi kedua akan usang dalam tiga bulan.',
      },
      {
        question: 'Bagaimana kalau kami sudah punya desain dari agensi lain?',
        answer:
          'Bisa saya bangun dari desain yang sudah ada, dengan harga lebih rendah. Yang saya minta adalah berkas desain yang lengkap sampai tampilan HP — desain yang hanya selesai untuk desktop biasanya menambah waktu, bukan mengurangi.',
      },
    ],
  },
  {
    slug: 'sistem-informasi',
    name: 'Sistem & aplikasi web',
    shortName: 'sistem web',
    audience: 'Institusi & proses internal',
    headline: 'Sistem yang menggantikan pekerjaan manual, dan tetap jalan setelah saya pergi',
    summary:
      'Untuk proses internal yang sekarang berjalan lewat berkas, spreadsheet, dan pesan pribadi — dengan peran, persetujuan, dan lampiran yang harus terlacak.',
    startingPrice: 40_000_000,
    startingPriceLabel: 'Mulai Rp 40 juta',
    problems: [
      {
        title: 'Tidak ada yang bisa menjawab "sekarang di tahap mana"',
        body: 'Pengajuan berjalan lewat berkas dan pesan pribadi. Statusnya hanya ada di kepala satu-dua orang, dan hilang saat orang itu cuti.',
      },
      {
        title: 'Hak akses diurus dengan saling percaya',
        body: 'Semua orang bisa melihat semua hal karena memisahkannya terlalu merepotkan. Saat struktur organisasi berubah, tidak ada yang berani menyentuhnya.',
      },
      {
        title: 'Sistem sebelumnya mati karena tidak ada yang bisa merawatnya',
        body: 'Dibangun sekali, tidak didokumentasikan, lalu pengembangnya pergi. Yang tersisa adalah sesuatu yang tidak berani diubah siapa pun.',
      },
    ],
    deliverables: [
      'Peran dan izin per-aksi yang bisa diubah administrator lewat antarmuka, tanpa deploy ulang',
      'Alur pengajuan dan verifikasi lengkap dengan status, catatan pemeriksa, revisi, dan lampiran',
      'Integrasi dengan akun institusi yang sudah ada, sehingga pengguna tidak perlu mendaftar ulang',
      'Ekspor ke PDF, Excel, dan CSV, serta impor massal dari Excel',
      'Deployment, backup, dan pemeriksaan kesehatan yang berjalan sendiri',
      'Dokumentasi teknis tertulis, termasuk tutorial menambah modul baru untuk pengembang berikutnya',
    ],
    exclusions: [
      'Pengadaan server dan lisensi pihak ketiga',
      'Pemeliharaan setelah masa garansi, kecuali lewat retainer terpisah',
      'Perubahan ruang lingkup tanpa proses persetujuan tertulis. Ini yang membuat proyek tidak pernah selesai',
    ],
    caseStudySlug: 'p3m-pens',
    pricingShape: 'phases',
    faq: [
      {
        question: 'Kenapa lokakarya ruang lingkup harus berbayar?',
        answer:
          'Karena hasilnya adalah dokumen yang bisa Anda pakai — termasuk untuk meminta penawaran ke pihak lain. Lokakarya gratis menghasilkan estimasi asal-asalan, dan estimasi asal-asalan adalah awal dari proyek yang meleset dua kali lipat.',
      },
      {
        question: 'Kami belum tahu persis kebutuhannya. Apakah tetap bisa mulai?',
        answer:
          'Justru itu gunanya Tahap 0. Yang saya minta bukan spesifikasi, melainkan akses ke orang yang menjalankan prosesnya sekarang.',
      },
      {
        question: 'Kalau di tengah jalan ada kebutuhan baru?',
        answer:
          'Ada proses tertulis: usulan, estimasi, persetujuan, baru dikerjakan. Tidak ada pekerjaan tambahan yang dikerjakan diam-diam lalu ditagihkan belakangan.',
      },
      {
        question: 'Siapa yang memiliki kodenya?',
        answer:
          'Anda, setelah pelunasan. Termasuk repositori, dokumentasi, dan seluruh kredensial. Yang saya pertahankan hanya komponen generik yang tidak khas organisasi Anda.',
      },
      {
        question: 'Apakah Anda mengerjakan ini sendirian?',
        answer:
          'Ya. Artinya tidak ada yang hilang di antara oper-operan, dan Anda selalu bicara dengan orang yang benar-benar mengerjakan. Konsekuensinya juga jujur saya sampaikan: saya hanya menerima satu proyek sistem dalam satu waktu.',
      },
    ],
  },
]

export const getServiceBySlug = (slug: string): Service | undefined =>
  services.find((service) => service.slug === slug)

export const pricingShapeData = {
  'business-packages': businessPackages,
  'corporate-packages': corporatePackages,
  phases: systemPhases,
} as const
