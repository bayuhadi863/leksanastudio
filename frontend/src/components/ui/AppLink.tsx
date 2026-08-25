import type { ComponentPropsWithoutRef } from 'react'
import { Link } from 'react-router-dom'

import { isExternalHref, isRouteHref } from '@/lib/href'

type Props = Omit<ComponentPropsWithoutRef<'a'>, 'href'> & {
  readonly href: string
  /** Forces the new-tab treatment for a link the heuristic would keep in-tab. */
  readonly external?: boolean
}

/**
 * The site's only link primitive.
 *
 * Internal paths go through the router so navigation stays client-side;
 * everything else — external sites, `mailto:`, `tel:`, in-page anchors —
 * renders as a plain anchor, because the router has no business handling any
 * of them.
 */
export function AppLink({ href, external, children, ...rest }: Props) {
  if (external ?? isExternalHref(href)) {
    return (
      <a href={href} target="_blank" rel="noopener noreferrer" {...rest}>
        {children}
      </a>
    )
  }

  if (!isRouteHref(href)) {
    return (
      <a href={href} {...rest}>
        {children}
      </a>
    )
  }

  return (
    <Link to={href} {...rest}>
      {children}
    </Link>
  )
}
