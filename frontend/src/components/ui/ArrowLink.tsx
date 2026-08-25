import type { ReactNode } from 'react'

import { AppLink } from '@/components/ui/AppLink'
import { cn } from '@/lib/cn'

type Props = {
  readonly href: string
  readonly children: ReactNode
  readonly className?: string
}

/**
 * Tertiary action used inside cards and at the end of sections.
 * The arrow nudges on hover; nothing else moves.
 */
export function ArrowLink({ href, children, className }: Props) {
  return (
    <AppLink
      href={href}
      className={cn(
        'group/arrow text-accent inline-flex items-center gap-2 font-semibold',
        'transition-colors duration-150 ease-out',
        className,
      )}
    >
      <span className="underline decoration-transparent decoration-1 underline-offset-[0.22em] transition-[text-decoration-color] duration-150 ease-out group-hover/arrow:decoration-current">
        {children}
      </span>
      <span
        aria-hidden="true"
        className="transition-transform duration-150 ease-out group-hover/arrow:translate-x-0.5"
      >
        &rarr;
      </span>
    </AppLink>
  )
}
