import { useLayoutEffect } from 'react'

import { site } from '@/config/site'

export type PageMetaInput = {
  readonly title: string
  readonly description: string
  readonly path: string
  readonly type?: 'website' | 'article'
  readonly publishedTime?: string
  readonly modifiedTime?: string
  readonly noIndex?: boolean
  /** Skips the title suffix. Only the homepage sets this. */
  readonly exactTitle?: boolean
}

const absolute = (path: string): string => new URL(path, site.url).toString()

/**
 * Titles read "Halaman — Leksana Studio". The suffix is applied here, so page
 * titles must never include it themselves.
 */
export const TITLE_TEMPLATE = (title: string): string => `${title} — ${site.legalName}`

/** Share card. Referenced absolutely so scrapers resolve it without a base. */
const OG_IMAGE = absolute('/og.svg')

const MARK = 'data-page-meta'

type Tag =
  | {
      readonly kind: 'meta'
      readonly key: 'name' | 'property'
      readonly value: string
      readonly content: string
    }
  | { readonly kind: 'link'; readonly rel: string; readonly href: string }

const buildTags = (input: PageMetaInput): readonly Tag[] => {
  const {
    title,
    description,
    path,
    type = 'website',
    publishedTime,
    modifiedTime,
    noIndex = false,
  } = input

  const url = absolute(path)

  const tags: Tag[] = [
    { kind: 'link', rel: 'canonical', href: url },
    { kind: 'meta', key: 'name', value: 'description', content: description },

    { kind: 'meta', key: 'property', value: 'og:type', content: type },
    { kind: 'meta', key: 'property', value: 'og:url', content: url },
    { kind: 'meta', key: 'property', value: 'og:title', content: title },
    { kind: 'meta', key: 'property', value: 'og:description', content: description },
    { kind: 'meta', key: 'property', value: 'og:site_name', content: site.legalName },
    { kind: 'meta', key: 'property', value: 'og:locale', content: 'id_ID' },
    { kind: 'meta', key: 'property', value: 'og:image', content: OG_IMAGE },

    { kind: 'meta', key: 'name', value: 'twitter:card', content: 'summary_large_image' },
    { kind: 'meta', key: 'name', value: 'twitter:title', content: title },
    { kind: 'meta', key: 'name', value: 'twitter:description', content: description },
    { kind: 'meta', key: 'name', value: 'twitter:image', content: OG_IMAGE },

    { kind: 'meta', key: 'name', value: 'author', content: site.ownerName },
  ]

  if (publishedTime) {
    tags.push({
      kind: 'meta',
      key: 'property',
      value: 'article:published_time',
      content: publishedTime,
    })
  }

  if (modifiedTime) {
    tags.push({
      kind: 'meta',
      key: 'property',
      value: 'article:modified_time',
      content: modifiedTime,
    })
  }

  if (noIndex) {
    tags.push({ kind: 'meta', key: 'name', value: 'robots', content: 'noindex, nofollow' })
  }

  return tags
}

/**
 * Every page declares its metadata through this hook, so canonical URLs, OG
 * tags and title suffixes can never drift apart.
 *
 * Next did this on the server. Here the tags are written into <head> on
 * navigation: crawlers that execute JavaScript read them, and the ones that do
 * not fall back to the defaults in index.html. Pre-rendering is the next step
 * (see README — "SEO").
 */
export const usePageMeta = (input: PageMetaInput): void => {
  const { title, description, path, type, publishedTime, modifiedTime, noIndex, exactTitle } = input

  useLayoutEffect(() => {
    const head = document.head

    // Whatever the previous page wrote goes first; anything left behind would
    // describe a page the reader is no longer on.
    head.querySelectorAll(`[${MARK}]`).forEach((node) => node.remove())

    document.title = exactTitle ? title : TITLE_TEMPLATE(title)

    for (const tag of buildTags({
      title,
      description,
      path,
      type,
      publishedTime,
      modifiedTime,
      noIndex,
    })) {
      if (tag.kind === 'link') {
        const link = document.createElement('link')
        link.rel = tag.rel
        link.href = tag.href
        link.setAttribute(MARK, '')
        head.appendChild(link)
      } else {
        const meta = document.createElement('meta')
        meta.setAttribute(tag.key, tag.value)
        meta.content = tag.content
        meta.setAttribute(MARK, '')
        head.appendChild(meta)
      }
    }
  }, [title, description, path, type, publishedTime, modifiedTime, noIndex, exactTitle])
}
