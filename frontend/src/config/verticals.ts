import type { PricingShape, Problem, QA } from './services'

/**
 * Vertical landing pages — the SEO engine for a capability-led generalist.
 *
 * One template renders all of them (`app/[vertical]/page.tsx`). Adding a
 * vertical means adding one entry here, never copying a page.
 *
 * Rule against doorway pages: every entry must carry genuinely different
 * problems, deliverables and FAQ. If we cannot write them, we do not ship
 * the page.
 */
export type Vertical = {
  readonly slug: string
  readonly industry: string
  readonly headline: string
  readonly intro: string
  readonly problems: readonly Problem[]
  readonly deliverables: readonly string[]
  readonly note: string
  readonly serviceSlug: string
  readonly pricingShape: PricingShape
  readonly whatsappIntro: string
  readonly faq: readonly QA[]
}

export const verticals: readonly Vertical[] = [
  {
    slug: 'jasa-website-klinik-gigi',
    industry: 'Klinik gigi',
    headline: 'Jasa pembuatan website klinik gigi',
    intro:
      'Pasien gigi tidak membandingkan klinik. Mereka membuka dua atau tiga hasil pencarian di HP, lalu menghubungi yang paling cepat menjawab pertanyaan mereka. Website klinik yang baik adalah website yang menang di tiga puluh detik pertama.',
    problems: [
      {
        title: 'Pertanyaan yang sama masuk ke WhatsApp setiap hari',
        body: '“Buka jam berapa?”, “behel berapa?”, “bisa hari Minggu?”. Jawabannya selalu sama, dan admin mengetik ulang puluhan kali sehari. Itu bukan masalah admin — itu masalah halaman yang tidak menjawab.',
      },
      {
        title: 'Pasien veneer dan behel riset berminggu-minggu',
        body: 'Perawatan bernilai jutaan tidak diputuskan dalam satu kunjungan halaman. Kalau tidak ada halaman khusus yang menjelaskan prosedur, kisaran biaya, dan hasil sebelum–sesudah, klinik Anda tidak masuk daftar pertimbangan.',
      },
      {
        title: 'Rasa takut tidak pernah dijawab',
        body: 'Alasan nomor satu orang menunda ke dokter gigi adalah takut sakit dan takut biayanya tidak terduga. Website yang tidak menyentuh dua hal itu kehilangan pasien yang sebenarnya sudah siap datang.',
      },
    ],
    deliverables: [
      'Halaman terpisah per perawatan — veneer, behel, implan, scaling — supaya tiap perawatan bisa diiklankan sendiri',
      'Tombol WhatsApp yang pesan awalnya menyebut perawatan yang sedang dilihat pasien',
      'Status buka/tutup yang mengikuti jam praktik, sehingga pasien tahu kapan akan dibalas',
      'Profil dokter dengan kredensial dan nomor STR, karena kepercayaan dimulai dari orangnya',
      'Blok kisaran biaya dan penanganan rasa takut, ditulis tanpa satu pun klaim medis',
      'Struktur SEO lokal dan peta, supaya klinik muncul saat orang mencari “dokter gigi terdekat”',
    ],
    note: 'Saya tidak menulis “dijamin tidak sakit” atau “hasil permanen” di website klinik mana pun, meski diminta. Itu klaim medis, dan yang menanggung risikonya bukan saya — melainkan klinik Anda.',
    serviceSlug: 'website-bisnis',
    pricingShape: 'business-packages',
    whatsappIntro: 'Halo, saya dari klinik gigi. Ingin tanya soal pembuatan website klinik.',
    faq: [
      {
        question: 'Apakah harga perawatan harus ditampilkan?',
        answer:
          'Tidak harus angka pasti — kisaran sudah cukup, dan justru mengurangi pertanyaan berulang di WhatsApp. Pasien yang tahu kisarannya datang dengan ekspektasi yang benar; yang tidak cocok tersaring sebelum memakan waktu admin.',
      },
      {
        question: 'Bisakah pasien booking langsung dari website?',
        answer:
          'Bisa, tapi untuk mayoritas klinik saya menyarankan WhatsApp dengan pesan otomatis yang sudah terisi. Formulir booking terlihat modern dan sering diabaikan; WhatsApp adalah tempat pasien Indonesia benar-benar bertanya.',
      },
      {
        question: 'Apakah foto sebelum–sesudah boleh dipasang?',
        answer:
          'Boleh, dengan izin tertulis pasien dan tanpa menjanjikan hasil yang sama untuk orang lain. Saya bantu siapkan format izinnya, karena bagian ini sering dilewati dan risikonya nyata.',
      },
      {
        question: 'Klinik kami sudah punya Instagram. Masih perlu website?',
        answer:
          'Instagram bagus untuk ditemukan, tetapi buruk untuk menjawab. Orang yang mencari “dokter gigi terdekat” di Google tidak akan menemukan feed Anda, dan orang yang butuh jam praktik tidak akan menggulir 40 postingan untuk mencarinya.',
      },
    ],
  },
  {
    slug: 'jasa-website-klinik-kecantikan',
    industry: 'Klinik kecantikan',
    headline: 'Jasa pembuatan website klinik kecantikan',
    intro:
      'Di klinik kecantikan, keputusan dibuat dengan mata. Yang menentukan bukan daftar layanan, melainkan apakah calon pasien percaya hasil yang Anda tunjukkan itu nyata — dan apakah ia merasa aman menanyakan harganya.',
    problems: [
      {
        title: 'Galeri hasil terlihat seperti stok, bukan seperti klinik',
        body: 'Foto model yang jelas bukan pasien Anda menurunkan kepercayaan, bukan menaikkannya. Calon pasien di kota yang sama mengenali wajah yang bukan dari sini.',
      },
      {
        title: 'Harga tidak terlihat, jadi orang tidak bertanya',
        body: 'Di kategori ini, menyembunyikan kisaran biaya tidak membuat orang menghubungi. Justru sebaliknya — mereka pindah ke klinik yang berani menuliskannya.',
      },
      {
        title: 'Treatment banyak, halamannya satu',
        body: 'Facial, laser, filler, dan slimming punya calon pasien yang berbeda dan kata pencarian yang berbeda. Satu halaman untuk semuanya berarti tidak ditemukan oleh siapa pun.',
      },
    ],
    deliverables: [
      'Halaman per treatment dengan penjelasan prosedur, durasi, dan kisaran biaya',
      'Galeri hasil dengan pembanding sebelum–sesudah dan aturan izin yang jelas',
      'Profil dokter dan terapis, karena di kategori ini orangnya adalah produknya',
      'Tombol WhatsApp per treatment, sehingga percakapan dimulai dari konteks yang benar',
      'Tata letak yang tetap cepat meski galerinya berat',
      'Struktur SEO lokal untuk pencarian “klinik kecantikan” plus nama kota',
    ],
    note: 'Kategori ini paling sering meminta klaim yang tidak bisa dibuktikan — “permanen”, “tanpa efek samping”, “pasti putih”. Saya menolaknya. Bukan untuk sok bersih, tapi karena klaim seperti itu yang paling cepat menarik masalah ke klinik Anda.',
    serviceSlug: 'website-bisnis',
    pricingShape: 'business-packages',
    whatsappIntro: 'Halo, saya dari klinik kecantikan. Ingin tanya soal pembuatan websitenya.',
    faq: [
      {
        question: 'Foto sebelum–sesudah aman dipasang?',
        answer:
          'Aman kalau ada izin tertulis dari pasien dan tidak ada janji hasil yang sama untuk orang lain. Yang tidak aman adalah memakai foto dari internet — itu masalah hak cipta sekaligus masalah kepercayaan.',
      },
      {
        question: 'Bagaimana supaya situsnya tetap cepat padahal fotonya banyak?',
        answer:
          'Gambar dikonversi ke format modern, dimuat bertahap, dan ukurannya ditetapkan supaya tata letak tidak melompat. Galeri berat bukan alasan situs jadi lambat — itu hasil dari galeri yang tidak diurus.',
      },
      {
        question: 'Kami punya banyak promo bulanan. Bisa diubah sendiri?',
        answer:
          'Bisa, mulai paket Website lengkap. Promo, harga, dan jadwal bisa Anda ubah tanpa menghubungi saya.',
      },
    ],
  },
  {
    slug: 'jasa-sistem-informasi-kampus',
    industry: 'Kampus & institusi pendidikan',
    headline: 'Jasa pembuatan sistem informasi untuk kampus dan unit institusi',
    intro:
      'Unit di kampus punya masalah yang mirip di mana-mana: proses yang jelas di kepala orangnya, tetapi tidak ada di mana pun sebagai sistem. Saya sudah pernah membangun persis sistem seperti itu, di lingkungan kampus, dengan seluruh batasan yang menyertainya.',
    problems: [
      {
        title: 'Pengajuan berjalan lewat berkas dan pesan pribadi',
        body: 'Tidak ada satu tempat pun yang bisa menjawab “pengajuan saya sekarang di tahap mana”. Statusnya ada di kepala satu-dua orang, dan hilang saat mereka cuti.',
      },
      {
        title: 'Hak akses tidak bisa diubah tanpa memanggil vendor',
        body: 'Struktur unit berubah setiap periode. Kalau setiap perubahan hak akses berarti tiket ke pengembang, sistemnya akan ditinggalkan dalam setahun.',
      },
      {
        title: 'Sistem sebelumnya mati bersama pengembangnya',
        body: 'Dibangun sekali, tidak didokumentasikan, lalu orangnya pergi. Yang tersisa adalah sesuatu yang tidak berani diubah siapa pun — termasuk pengembang berikutnya.',
      },
    ],
    deliverables: [
      'Situs publik unit dan panel administrasi dalam satu sistem, dengan pratinjau yang benar-benar sama dengan hasil akhirnya',
      'Alur pengajuan dan verifikasi: status, catatan pemeriksa, revisi, dan lampiran berkas',
      'Izin per-aksi berbasis data — administrator mengubahnya lewat antarmuka, tanpa deploy ulang',
      'Login dengan akun kampus yang sudah ada, tanpa pendaftaran ulang',
      'Ekspor PDF, Excel, dan CSV, serta impor massal dari Excel',
      'Dokumentasi teknis berbahasa Indonesia, termasuk tutorial menambah modul baru',
    ],
    note: 'Sistem terakhir yang saya bangun harus jalan di balik proxy kampus yang hanya meneruskan GET dan POST, dan firewall yang menolak HTML di badan permintaan. Saya menyelesaikannya tanpa merusak desain API — dan menuliskan alasannya, supaya orang berikutnya tidak salah menyentuh.',
    serviceSlug: 'sistem-informasi',
    pricingShape: 'phases',
    whatsappIntro: 'Halo, saya dari unit di kampus. Ingin tanya soal pembuatan sistem informasi.',
    faq: [
      {
        question: 'Apakah bisa diintegrasikan dengan akun kampus?',
        answer:
          'Bisa, dan sudah pernah saya kerjakan. Dosen dan karyawan masuk dengan akun institusi, sementara pengguna lokal seperti admin unit tetap punya akun sendiri. Keduanya berjalan berdampingan.',
      },
      {
        question: 'Kami harus mengikuti proses pengadaan. Bisakah menyesuaikan?',
        answer:
          'Bisa. Yang saya sarankan adalah memulai dari lokakarya ruang lingkup berbayar, karena keluarannya adalah dokumen kebutuhan yang bisa langsung Anda pakai untuk proses pengadaan — termasuk kalau akhirnya dikerjakan pihak lain.',
      },
      {
        question: 'Sistemnya akan dipasang di server kampus. Apakah itu masalah?',
        answer:
          'Tidak, tapi harus diuji sejak minggu pertama, bukan menjelang serah terima. Batasan lingkungan kampus — proxy, firewall, port — hampir selalu baru terlihat saat mencoba, dan menemukannya di akhir jauh lebih mahal.',
      },
      {
        question: 'Siapa yang memiliki kodenya?',
        answer:
          'Institusi Anda, setelah pelunasan. Termasuk repositori, dokumentasi, dan seluruh kredensial.',
      },
    ],
  },
]

export const getVerticalBySlug = (slug: string): Vertical | undefined =>
  verticals.find((vertical) => vertical.slug === slug)

export const verticalSlugs = verticals.map((vertical) => vertical.slug)
