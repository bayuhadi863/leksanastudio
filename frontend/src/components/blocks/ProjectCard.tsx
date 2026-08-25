import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'

import { ProjectFigure } from '@/components/blocks/ProjectFigure'
import { type SchematicVariant } from '@/components/blocks/Schematic'
import { Label } from '@/components/ui/Label'
import { routes } from '@/config/routes'
import type { CaseStudy } from '@/lib/content'
import type { Metric } from '@/components/blocks/MetricBlock'

const LABEL_TEXT: Record<CaseStudy['frontmatter']['label'], string> = {
  klien: 'Klien',
  'produk-sendiri': 'Produk sendiri',
}

export type ProjectCardData = {
  readonly label: string
  readonly kind: string
  readonly title: string
  readonly problem: string
  readonly metrics: readonly Metric[]
  readonly cover?: { readonly src: string; readonly alt: string }
  readonly figure: SchematicVariant
}

/**
 * The most important component on the site.
 *
 * The card title is the problem that was solved, not the client's name — a
 * prospect scans for their own problem, not for an institution they have never
 * heard of. Honest labelling (`Klien` / `Produk sendiri`) is mandatory.
 *
 * Split into a view and a link wrapper so the panel can render exactly this
 * card as a preview without inventing a second version of it. A preview that
 * only resembles the real card is how a card ships broken.
 */
export function ProjectCardView({
  data,
  link,
}: {
  readonly data: ProjectCardData
  /** Wraps the card body — a router link on the site, nothing in a preview. */
  readonly link?: (children: ReactNode) => ReactNode
}) {
  const body = (
    <>
      <div className="border-line relative aspect-16/10 overflow-hidden border-b">
        <ProjectFigure
          cover={data.cover}
          fallback={data.figure}
          fallbackTitle={`Skema antarmuka ${data.title}`}
          sizes="(min-width: 1024px) 30rem, (min-width: 640px) 90vw, 100vw"
        />
      </div>

      <div className="p-6 lg:p-7">
        <Label as="p">
          {data.label}
          {data.kind ? ` · ${data.kind}` : ''}
        </Label>

        <h3 className="type-h3 group-hover/card:text-accent mt-4 transition-colors duration-150 ease-out">
          {data.title}
        </h3>

        <p className="text-muted mt-3">{data.problem}</p>

        <p className="type-label numeric text-muted mt-6">
          {data.metrics
            .filter((metric) => metric.value || metric.label)
            .map((metric) => `${metric.value} ${metric.label}`.trim())
            .join(' · ')}
        </p>

        <p className="text-accent mt-6 inline-flex items-center gap-2 font-semibold">
          Baca studi kasus
          <span
            aria-hidden="true"
            className="transition-transform duration-150 ease-out group-hover/card:translate-x-0.5"
          >
            &rarr;
          </span>
        </p>
      </div>
    </>
  )

  return (
    <article className="group/card border-line hover:border-accent border transition-colors duration-150 ease-out">
      {link ? link(body) : body}
    </article>
  )
}

export function ProjectCard({ caseStudy }: { readonly caseStudy: CaseStudy }) {
  const { frontmatter: meta } = caseStudy

  return (
    <ProjectCardView
      data={{
        label: LABEL_TEXT[meta.label],
        kind: meta.kind,
        title: meta.title,
        problem: meta.problem,
        metrics: meta.metrics,
        cover: meta.cover,
        figure: meta.figure,
      }}
      link={(children) => (
        <Link
          to={routes.caseStudy(caseStudy.slug)}
          className="block focus-visible:outline-offset-4"
        >
          {children}
        </Link>
      )}
    />
  )
}
