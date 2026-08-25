import type { ReactNode } from 'react'

import { Annotation } from '@/components/layout/Annotation'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

type Props = {
  readonly eyebrow?: string
  readonly headline: string
  readonly lead: string
  readonly note?: string
  readonly actions?: ReactNode
  readonly footnoteLabel?: string
  readonly footnote?: string
  readonly className?: string
}

/**
 * The hero is a thesis, not a cover.
 *
 * No image and no slider. Making the largest paint a block of text keeps LCP
 * around the time it takes to render a paragraph, and it forces the headline
 * to do its own work.
 */
export function Hero({
  eyebrow,
  headline,
  lead,
  note,
  actions,
  footnoteLabel,
  footnote,
  className,
}: Props) {
  const body = (
    <>
      {eyebrow ? <Label as="p">{eyebrow}</Label> : null}

      <h1 className={cn('type-display', eyebrow && 'mt-5')}>{headline}</h1>

      <p className="type-lead mt-7">{lead}</p>

      {actions ? <div className="mt-9 flex flex-col gap-3 sm:flex-row">{actions}</div> : null}

      {footnote ? (
        <div className="border-line mt-12 border-t pt-6">
          {footnoteLabel ? <Label as="p">{footnoteLabel}</Label> : null}
          <p className="type-small text-muted mt-2.5 max-w-lg">{footnote}</p>
        </div>
      ) : null}
    </>
  )

  return (
    <section className={cn('shell pt-14 pb-20 lg:pt-24 lg:pb-28', className)}>
      {note ? <Annotation note={note}>{body}</Annotation> : <div className="document">{body}</div>}
    </section>
  )
}
