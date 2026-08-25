/**
 * Single source of truth for studio identity.
 *
 * Architectural rule: no identity value or promise figure may be hard-coded
 * inside a component. Changing the WhatsApp number must be a one-line edit,
 * not a search across fourteen files.
 *
 * Note on language: identifiers are English, user-facing copy is Indonesian.
 */

/*
 * `import.meta.env` is guarded because this module is also imported by the
 * build-time sitemap generator, which runs in plain Node where Vite has not
 * substituted anything.
 */
const SITE_URL = (import.meta.env?.VITE_SITE_URL ?? 'https://leksana.id').replace(/\/$/, '')

export const site = {
  name: 'Leksana',
  legalName: 'Leksana Studio',
  ownerName: 'Bayu Hadi Leksana',
  tagline: 'Dibangun untuk dipakai bertahun-tahun',
  description:
    'Studio web dan sistem web di Surabaya. Saya membangun website dan aplikasi web yang dirancang untuk dipakai bertahun-tahun — lengkap dengan dokumentasi dan serah terima penuh.',
  url: SITE_URL,
  city: 'Surabaya',
  region: 'Jawa Timur',
  countryCode: 'ID',
  locale: 'id-ID',
  email: 'halo@leksana.id',
  whatsapp: {
    /** International format without the plus sign — used directly in wa.me links. */
    number: '6281234567890',
    display: '+62 812-3456-7890',
  },
  social: {
    linkedin: 'https://www.linkedin.com/in/bayuhadileksana',
    instagram: 'https://www.instagram.com/leksana.studio',
    github: 'https://github.com/bayuhadileksana',
  },
  /** Promise figures. They appear in many places; they live in one. */
  promises: {
    revisionRounds: 2,
    warrantyDays: 60,
    updateEveryDays: 3,
    replyWithinHours: 2,
    quoteValidDays: 14,
  },
} as const

export type Site = typeof site

const idrFormatter = new Intl.NumberFormat('id-ID', {
  style: 'currency',
  currency: 'IDR',
  maximumFractionDigits: 0,
})

/** "Rp 3.500.000" — for tables, where digits must line up. */
export const formatIDR = (amount: number): string => idrFormatter.format(amount)

/** "Rp 3,5 juta" — easier to read inside a sentence. */
export const formatIDRShort = (amount: number): string => {
  const millions = amount / 1_000_000
  const value = Number.isInteger(millions)
    ? String(millions)
    : millions.toFixed(1).replace('.', ',')
  return `Rp ${value} juta`
}
