import { site } from '@/config/site'

/**
 * Builds a wa.me link with a pre-filled opening message.
 *
 * The opening message differs per page on purpose: it tells us which page the
 * prospect came from without having to ask, and it lets the conversation start
 * from the right context.
 */
export const whatsappLink = (message: string): string => {
  const url = new URL(`https://wa.me/${site.whatsapp.number}`)
  url.searchParams.set('text', message)
  return url.toString()
}

export const whatsappMessages = {
  default: 'Halo, saya ingin diskusi soal pembuatan website.',
  pricing: 'Halo, saya sudah lihat halaman harga. Ingin tanya untuk kebutuhan ',
  process: 'Halo, saya ingin tanya soal proses pengerjaannya.',
  caseStudy: (title: string) =>
    `Halo, saya baru baca studi kasus "${title}". Kebutuhan saya mirip.`,
  service: (name: string) => `Halo, saya ingin tanya soal layanan ${name}.`,
} as const
