import { readdirSync, readFileSync } from 'node:fs'
import path from 'node:path'

import matter from 'gray-matter'

import { routes } from '../src/config/routes'
import { services } from '../src/config/services'
import { site } from '../src/config/site'
import { verticals } from '../src/config/verticals'

/**
 * Sitemap and robots.txt.
 *
 * Next generated both from `app/sitemap.ts` and `app/robots.ts`. Vite emits a
 * static bundle, so they are written into `dist/` at build time instead — from
 * the same config the pages render from, so a new service, vertical or article
 * can never be missing here.
 */

type ChangeFrequency = 'weekly' | 'monthly' | 'yearly'

type SitemapEntry = {
  readonly url: string
  readonly lastModified: Date
  readonly changeFrequency: ChangeFrequency
  readonly priority: number
}

const absolute = (pathname: string): string => new URL(pathname, site.url).toString()

const readFrontmatter = (root: string, folder: string) => {
  const directory = path.join(root, 'src', 'content', folder)

  return readdirSync(directory)
    .filter((file) => file.endsWith('.mdx'))
    .map((file) => {
      const raw = readFileSync(path.join(directory, file), 'utf8')
      const { data } = matter(raw)
      return { slug: file.replace(/\.mdx$/, ''), data: data as Record<string, unknown> }
    })
    .filter((entry) => entry.data.draft !== true)
}

const isoDate = (value: unknown, fallback: Date): Date => {
  if (typeof value === 'string') {
    const parsed = new Date(value)
    if (!Number.isNaN(parsed.getTime())) return parsed
  }
  if (value instanceof Date) return value
  return fallback
}

export const buildSitemap = (root: string, now = new Date()): string => {
  const caseStudies = readFrontmatter(root, 'studi-kasus')
  const notes = readFrontmatter(root, 'catatan')

  const staticPages: readonly SitemapEntry[] = (
    [
      { url: absolute(routes.home), changeFrequency: 'monthly', priority: 1 },
      { url: absolute(routes.services), changeFrequency: 'monthly', priority: 0.9 },
      { url: absolute(routes.work), changeFrequency: 'monthly', priority: 0.9 },
      { url: absolute(routes.pricing), changeFrequency: 'monthly', priority: 0.8 },
      { url: absolute(routes.process), changeFrequency: 'yearly', priority: 0.7 },
      { url: absolute(routes.about), changeFrequency: 'yearly', priority: 0.6 },
      { url: absolute(routes.contact), changeFrequency: 'yearly', priority: 0.6 },
      { url: absolute(routes.notes), changeFrequency: 'weekly', priority: 0.7 },
      { url: absolute(routes.privacy), changeFrequency: 'yearly', priority: 0.2 },
    ] as const
  ).map((entry) => ({ ...entry, lastModified: now }))

  const entries: readonly SitemapEntry[] = [
    ...staticPages,
    ...services.map((service) => ({
      url: absolute(routes.service(service.slug)),
      lastModified: now,
      changeFrequency: 'monthly' as const,
      priority: 0.8,
    })),
    ...verticals.map((vertical) => ({
      url: absolute(routes.vertical(vertical.slug)),
      lastModified: now,
      changeFrequency: 'monthly' as const,
      priority: 0.8,
    })),
    ...caseStudies.map((entry) => ({
      url: absolute(routes.caseStudy(entry.slug)),
      lastModified: isoDate(entry.data.updated, now),
      changeFrequency: 'yearly' as const,
      priority: 0.9,
    })),
    ...notes.map((entry) => ({
      url: absolute(routes.note(entry.slug)),
      lastModified: isoDate(entry.data.updated ?? entry.data.published, now),
      changeFrequency: 'yearly' as const,
      priority: 0.6,
    })),
  ]

  const body = entries
    .map((entry) =>
      [
        '  <url>',
        `    <loc>${entry.url}</loc>`,
        `    <lastmod>${entry.lastModified.toISOString()}</lastmod>`,
        `    <changefreq>${entry.changeFrequency}</changefreq>`,
        `    <priority>${entry.priority}</priority>`,
        '  </url>',
      ].join('\n'),
    )
    .join('\n')

  return `<?xml version="1.0" encoding="UTF-8"?>\n<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">\n${body}\n</urlset>\n`
}

export const buildRobots = (): string =>
  [
    'User-Agent: *',
    'Allow: /',
    // A conversion page has no business in search results.
    `Disallow: ${routes.contactThanks}`,
    // Nor does anything behind sign-in — and a build without the panel simply
    // has nothing at these paths, so the lines cost nothing either way.
    'Disallow: /panel/',
    'Disallow: /auth/',
    '',
    `Host: ${site.url}`,
    `Sitemap: ${site.url}/sitemap.xml`,
    '',
  ].join('\n')
