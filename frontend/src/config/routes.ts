/**
 * Route map. The only place URL paths are written as strings.
 * Navigation, footer, sitemap and JSON-LD all read from here.
 */

export const routes = {
  home: '/',
  services: '/layanan',
  service: (slug: string) => `/layanan/${slug}`,
  work: '/portofolio',
  caseStudy: (slug: string) => `/portofolio/${slug}`,
  process: '/proses',
  pricing: '/harga',
  about: '/tentang',
  contact: '/kontak',
  contactThanks: '/kontak/terima-kasih',
  notes: '/catatan',
  note: (slug: string) => `/catatan/${slug}`,
  vertical: (slug: string) => `/${slug}`,
  privacy: '/kebijakan-privasi',
} as const

export type NavItem = {
  readonly label: string
  readonly href: string
}

/**
 * Primary navigation — five items. A crowded nav makes prospects browse
 * instead of act.
 */
export const mainNav: readonly NavItem[] = [
  { label: 'Layanan', href: routes.services },
  { label: 'Portofolio', href: routes.work },
  { label: 'Proses', href: routes.process },
  { label: 'Harga', href: routes.pricing },
  { label: 'Tentang', href: routes.about },
]

export type NavGroup = {
  readonly title: string
  readonly items: readonly NavItem[]
}

export const footerNav: readonly NavGroup[] = [
  {
    title: 'Layanan',
    items: [
      { label: 'Website bisnis', href: routes.service('website-bisnis') },
      { label: 'Company profile', href: routes.service('website-perusahaan') },
      { label: 'Sistem & aplikasi web', href: routes.service('sistem-informasi') },
      { label: 'Harga & termin', href: routes.pricing },
    ],
  },
  {
    title: 'Bukti',
    items: [
      { label: 'Portofolio', href: routes.work },
      { label: 'Studi kasus P3M PENS', href: routes.caseStudy('p3m-pens') },
      { label: 'Catatan teknis', href: routes.notes },
      { label: 'Proses kerja', href: routes.process },
    ],
  },
  {
    title: 'Studio',
    items: [
      { label: 'Tentang', href: routes.about },
      { label: 'Kontak', href: routes.contact },
      { label: 'Kebijakan privasi', href: routes.privacy },
    ],
  },
]
