import type { ReactNode } from 'react'

import { MetricBlock, type Metric } from '@/components/blocks/MetricBlock'
import { ProjectFigure } from '@/components/blocks/ProjectFigure'
import { type SchematicVariant } from '@/components/blocks/Schematic'
import { Label } from '@/components/ui/Label'

/**
 * The three composite pieces a case study is written from.
 *
 * They live here rather than inside the MDX mapping because two things render
 * them now: the MDX files that are still on disk, and the block renderer that
 * draws the same content out of the database. One definition means the panel's
 * preview cannot drift from the published page — which is the only thing that
 * makes a preview worth showing.
 */

/**
 * Structured decision block for case studies.
 *
 * The house format is fixed on purpose: "I chose X because Y, even though Z."
 * A decision without a rejected alternative is not a decision, it is a
 * preference — and it does not belong in a case study.
 */
export function Decision({
  step,
  title,
  chose,
  because,
  despite,
  children,
}: {
  readonly step: number
  readonly title: string
  readonly chose: string
  readonly because: string
  readonly despite: string
  readonly children: ReactNode
}) {
  return (
    <section className="border-line border-t pt-8">
      <Label as="p">Keputusan {step}</Label>
      <h3 className="type-h3 mt-3">{title}</h3>

      <p className="mt-4">
        Saya memilih <strong className="text-accent font-semibold">{chose}</strong> karena{' '}
        <strong className="font-semibold">{because}</strong>, walaupun {despite}.
      </p>

      <div className="text-muted mt-4 grid gap-4">{children}</div>
    </section>
  )
}

/**
 * Figure with a caption. Never stock imagery — either a real, redacted
 * screenshot (`src`) or the schematic that stands in until one exists.
 */
export function Figure({
  variant,
  caption,
  alt,
  src,
}: {
  readonly variant: SchematicVariant
  readonly caption: string
  readonly alt: string
  readonly src?: string
}) {
  return (
    <figure className="document-wide my-10">
      <div className="border-line relative aspect-16/10 overflow-hidden border">
        <ProjectFigure
          cover={src ? { src, alt } : undefined}
          fallback={variant}
          fallbackTitle={alt}
          sizes="(min-width: 1024px) 63rem, 100vw"
        />
      </div>
      {caption ? <figcaption className="type-small text-muted mt-3">{caption}</figcaption> : null}
    </figure>
  )
}

/** Three headline numbers. Used once, near the top of a case study. */
export function Metrics({ items }: { readonly items: readonly Metric[] }) {
  return (
    <div className="document-wide border-line my-10 border-y py-8">
      <MetricBlock metrics={items} />
    </div>
  )
}
