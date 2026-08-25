import { site } from './site'
import type { QA } from './services'

export type ProcessStep = {
  readonly step: number
  readonly title: string
  readonly duration: string
  readonly summary: string
  readonly details: readonly string[]
  readonly clientInput: string
}

/**
 * Numbering is legitimate here: the process really is a sequence.
 * Numbering rules live in blueprint 06b §6b.5.
 */
export const processSteps: readonly ProcessStep[] = [
  {
    step: 1,
    title: 'Obrolan awal',
    duration: '30 menit, gratis',
    summary: 'Saya lebih banyak bertanya daripada menjelaskan.',
    details: [
      'Apa yang paling merepotkan sekarang, dan apa yang sudah pernah dicoba',
      'Apa yang harus berubah tiga bulan setelah ini jadi',
      'Siapa saja yang ikut memutuskan, dan kapan targetnya harus jalan',
      'Rentang anggaran, supaya saya tidak menawarkan sesuatu yang tidak masuk akal',
    ],
    clientInput: 'Waktu 30 menit dan kejujuran soal anggaran.',
  },
  {
    step: 2,
    title: 'Ruang lingkup & penawaran',
    duration: '2–3 hari',
    summary: 'Dokumen tertulis, bukan angka lewat pesan.',
    details: [
      'Ringkasan masalah dengan kata-kata Anda sendiri dari obrolan awal',
      'Daftar yang dikerjakan, dan daftar yang tidak termasuk',
      'Waktu pengerjaan, harga, dan termin pembayaran',
      `Berlaku ${site.promises.quoteValidDays} hari`,
    ],
    clientInput: 'Konfirmasi bahwa ringkasan masalahnya memang benar.',
  },
  {
    step: 3,
    title: 'Pengerjaan',
    duration: 'Sesuai paket',
    summary: 'Anda tidak perlu menagih kabar.',
    details: [
      `Kabar setiap ${site.promises.updateEveryDays} hari kerja, tanpa diminta`,
      'Tautan pratinjau langsung sejak minggu pertama — Anda melihat kemajuannya, bukan mendengarnya',
      `${site.promises.revisionRounds} putaran revisi termasuk, di tahap desain`,
      'Perubahan ruang lingkup selalu disampaikan dan disetujui sebelum dikerjakan',
    ],
    clientInput: 'Konten dan aset di tanggal yang disepakati. Ini penyebab molor nomor satu.',
  },
  {
    step: 4,
    title: 'Peluncuran & serah terima',
    duration: '1 minggu',
    summary: 'Selesai berarti Anda tidak bergantung pada saya.',
    details: [
      'Uji menyeluruh sebelum live, di HP dan desktop',
      'Sesi pelatihan yang direkam, supaya bisa diputar ulang tim Anda',
      'Dokumentasi: cara mengelola, cara deploy, dan apa yang sebaiknya tidak disentuh',
      `Semua akun atas nama Anda · garansi perbaikan ${site.promises.warrantyDays} hari`,
    ],
    clientInput: 'Satu jam untuk sesi pelatihan.',
  },
]

/** The three fears that are rarely spoken and most often kill a decision. */
export const commonFears: readonly QA[] = [
  {
    question: 'Bagaimana kalau Anda hilang di tengah jalan?',
    answer: `Kabar setiap ${site.promises.updateEveryDays} hari kerja, tanpa perlu ditagih. Kalau saya tidak mengabari lebih dari lima hari kerja, Anda berhak membatalkan dan pembayaran dikembalikan sebanding dengan pekerjaan yang belum selesai.`,
  },
  {
    question: 'Bagaimana kalau nanti saya tergantung terus pada Anda?',
    answer:
      'Domain, hosting, dan repositori kode atas nama Anda sejak hari pertama. Dokumentasi diserahkan tertulis. Kalau suatu saat Anda ganti pengembang, tidak ada satu pun hal yang perlu diminta dari saya.',
  },
  {
    question: 'Bagaimana kalau revisi ditagih terus?',
    answer: `${site.promises.revisionRounds} putaran revisi sudah termasuk di dalam harga, di tahap desain. Perubahan ruang lingkup dihitung terpisah dan selalu saya sampaikan sebelum dikerjakan — tidak pernah muncul di tagihan sebagai kejutan.`,
  },
]
