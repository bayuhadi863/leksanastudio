import type { ReactNode } from 'react'

import { cn } from '@/lib/cn'

type Props = {
  readonly children: ReactNode
  readonly className?: string
}

/**
 * The document track: main column capped at the measure, annotation track
 * beside it from 1024px up. The asymmetry gives the page its shape before a
 * single word is read.
 */
export function Document({ children, className }: Props) {
  return <div className={cn('document', className)}>{children}</div>
}

/** Opts a child out of the measure cap — screenshots, tables, card grids. */
export function DocumentWide({ children, className }: Props) {
  return <div className={cn('document-wide', className)}>{children}</div>
}
