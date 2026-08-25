import type { ReactNode } from 'react'

import { MetricBlock, type Metric } from '@/components/blocks/MetricBlock'
import { Label } from '@/components/ui/Label'

export type CaseStudyArticleData = {
  readonly label: string
  readonly kind: string
  readonly year: number
  readonly title: string
  readonly summary: string
  readonly metrics: readonly Metric[]
  readonly role: string
  readonly duration: string
  readonly client: string
  readonly stack: readonly string[]
}

/**
 * The shape of a case study page, with the body left to the caller.
 *
 * Two callers today: the published page, which still hands it MDX, and the
 * panel preview, which hands it blocks. One definition of the header, the
 * metric band, and the fact table means the preview cannot quietly disagree
 * with what a visitor will see — the moment it does, the preview is worthless.
 */
export function CaseStudyArticleView({
  data,
  children,
  footer,
}: {
  readonly data: CaseStudyArticleData
  readonly children: ReactNode
  readonly footer?: ReactNode
}) {
  const facts = [
    { label: 'Peran', value: data.role },
    { label: 'Durasi', value: data.duration },
    { label: 'Klien', value: data.client },
    { label: 'Stack', value: data.stack.join(' · ') },
  ]

  return (
    <article>
      <header className="shell pt-14 pb-14 lg:pt-24 lg:pb-16">
        <div className="document">
          <Label as="p">
            {[data.label, data.kind, data.year].filter(Boolean).join(' · ')}
          </Label>

          <h1 className="type-h1 mt-5">{data.title}</h1>

          <p className="type-lead mt-6">{data.summary}</p>
        </div>

        <div className="border-line mt-12 border-y py-10">
          <MetricBlock metrics={data.metrics} />
        </div>

        <dl className="mt-10 grid gap-x-10 gap-y-6 sm:grid-cols-2 lg:grid-cols-4">
          {facts.map((fact) => (
            <div key={fact.label} className="border-line border-t pt-4">
              <Label as="dt">{fact.label}</Label>
              <dd className="type-small mt-2">{fact.value}</dd>
            </div>
          ))}
        </dl>
      </header>

      <div className="shell pb-20 lg:pb-28">
        <div className="document copy">{children}</div>
        {footer}
      </div>
    </article>
  )
}
