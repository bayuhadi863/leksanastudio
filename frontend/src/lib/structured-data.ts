import { routes } from '@/config/routes'
import type { QA } from '@/config/services'
import { site } from '@/config/site'

type JsonLd = Record<string, unknown>

const absolute = (path: string): string => new URL(path, site.url).toString()

export const organizationSchema = (): JsonLd => ({
  '@context': 'https://schema.org',
  '@type': 'ProfessionalService',
  '@id': `${site.url}#studio`,
  name: site.legalName,
  alternateName: site.name,
  description: site.description,
  url: site.url,
  email: site.email,
  telephone: `+${site.whatsapp.number}`,
  founder: { '@type': 'Person', name: site.ownerName },
  areaServed: { '@type': 'Country', name: 'Indonesia' },
  address: {
    '@type': 'PostalAddress',
    addressLocality: site.city,
    addressRegion: site.region,
    addressCountry: site.countryCode,
  },
  knowsLanguage: ['id', 'en'],
  sameAs: [site.social.linkedin, site.social.instagram, site.social.github],
})

export const serviceSchema = (input: {
  name: string
  description: string
  path: string
  startingPrice: number
}): JsonLd => ({
  '@context': 'https://schema.org',
  '@type': 'Service',
  name: input.name,
  description: input.description,
  url: absolute(input.path),
  provider: { '@id': `${site.url}#studio` },
  areaServed: { '@type': 'Country', name: 'Indonesia' },
  offers: {
    '@type': 'Offer',
    priceCurrency: 'IDR',
    price: input.startingPrice,
    priceSpecification: {
      '@type': 'PriceSpecification',
      priceCurrency: 'IDR',
      minPrice: input.startingPrice,
    },
  },
})

export const faqSchema = (items: readonly QA[]): JsonLd => ({
  '@context': 'https://schema.org',
  '@type': 'FAQPage',
  mainEntity: items.map((item) => ({
    '@type': 'Question',
    name: item.question,
    acceptedAnswer: { '@type': 'Answer', text: item.answer },
  })),
})

export const articleSchema = (input: {
  title: string
  description: string
  path: string
  published: string
  updated?: string
}): JsonLd => ({
  '@context': 'https://schema.org',
  '@type': 'Article',
  headline: input.title,
  description: input.description,
  url: absolute(input.path),
  datePublished: input.published,
  dateModified: input.updated ?? input.published,
  inLanguage: 'id-ID',
  author: { '@type': 'Person', name: site.ownerName },
  publisher: { '@id': `${site.url}#studio` },
})

export const breadcrumbSchema = (
  trail: readonly { readonly label: string; readonly path: string }[],
): JsonLd => ({
  '@context': 'https://schema.org',
  '@type': 'BreadcrumbList',
  itemListElement: [{ label: 'Beranda', path: routes.home }, ...trail].map((item, index) => ({
    '@type': 'ListItem',
    position: index + 1,
    name: item.label,
    item: absolute(item.path),
  })),
})
