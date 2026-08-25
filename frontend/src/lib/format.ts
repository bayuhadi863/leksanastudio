/** Date helpers. All dates render in Indonesian, always with an explicit month name. */

const longDate = new Intl.DateTimeFormat('id-ID', {
  day: 'numeric',
  month: 'long',
  year: 'numeric',
})

const shortDate = new Intl.DateTimeFormat('id-ID', {
  day: 'numeric',
  month: 'short',
  year: 'numeric',
})

export const formatDate = (iso: string): string => longDate.format(new Date(iso))
export const formatDateShort = (iso: string): string => shortDate.format(new Date(iso))

/** Rough reading time. Indonesian prose runs ~200 words per minute. */
export const readingMinutes = (text: string): number =>
  Math.max(1, Math.round(text.trim().split(/\s+/).length / 200))
