/**
 * Which element a link should become.
 *
 * Under Next every one of these was `next/link`, which quietly downgraded to a
 * plain anchor for anything it could not route. The router used here does not,
 * so the decision is made once, here, instead of at fourteen call sites.
 */

/** Opens in a new tab. */
export const isExternalHref = (href: string): boolean => /^https?:\/\//.test(href)

/** Handed to the operating system: mail client, dialler. Same tab. */
export const isProtocolHref = (href: string): boolean => /^(mailto|tel|sms):/.test(href)

/** In-page anchor. Routing it would throw away the current page. */
export const isHashHref = (href: string): boolean => href.startsWith('#')

/** Everything the router owns. */
export const isRouteHref = (href: string): boolean =>
  !isExternalHref(href) && !isProtocolHref(href) && !isHashHref(href)
