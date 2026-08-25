import { memo } from 'react'

import { ProjectCardView } from '@/components/blocks/ProjectCard'
import { type SchematicVariant as FigureVariant } from '@/components/blocks/Schematic'
import { BlockRenderer } from '@/components/content/BlockRenderer'
import { CaseStudyArticleView } from '@/components/content/CaseStudyArticle'
import { Label } from '@/components/ui/Label'
import { mediaUrl } from '@/lib/media'
import type { Block } from '@/types/blocks'
import type { CaseStudyLabel, SchematicVariant } from '@/types/content'

const LABEL_TEXT: Record<CaseStudyLabel, string> = {
  Client: 'Klien',
  OwnProduct: 'Produk sendiri',
}

const FIGURE_VARIANT: Record<SchematicVariant, FigureVariant> = {
  System: 'system',
  Website: 'website',
  Catalog: 'catalog',
}

export type CaseStudyPreviewData = {
  readonly label: CaseStudyLabel
  readonly figure: SchematicVariant
  readonly year: number
  readonly stack: readonly string[]
  readonly coverPath: string | null
  readonly coverAlt: string
  readonly title: string
  readonly summary: string
  readonly problem: string
  readonly client: string
  readonly kind: string
  readonly duration: string
  readonly role: string
  readonly metrics: readonly { readonly value: string; readonly label: string }[]
  readonly body: readonly Block[]
}

/**
 * What the reader will get, drawn from what is in the form right now.
 *
 * Two surfaces, because a case study is published in two places and half the
 * fields only ever appear in one of them. Editing "Masalah" changes nothing on
 * the article page — it is the line that carries the portfolio card, and an
 * editor who cannot see that will keep writing it for the wrong place.
 */
export const CaseStudyPreview = memo(function CaseStudyPreview({
  data,
}: {
  readonly data: CaseStudyPreviewData
}) {
  const cover = mediaUrl(data.coverPath)
  const figure = FIGURE_VARIANT[data.figure]

  return (
    <div>
      <section className="px-6 pt-6 pb-8 lg:px-8">
        <Label as="p" className="text-muted">
          Kartu di halaman portofolio
        </Label>

        <div className="mt-4 max-w-[30rem]">
          <ProjectCardView
            data={{
              label: LABEL_TEXT[data.label],
              kind: data.kind,
              title: data.title,
              problem: data.problem,
              metrics: data.metrics,
              cover: cover ? { src: cover, alt: data.coverAlt } : undefined,
              figure,
            }}
          />
        </div>
      </section>

      <div className="border-line border-t">
        <Label as="p" className="text-muted px-6 pt-6 lg:px-8">
          Halaman studi kasus
        </Label>

        <CaseStudyArticleView
          data={{
            label: LABEL_TEXT[data.label],
            kind: data.kind,
            year: data.year,
            title: data.title,
            summary: data.summary,
            metrics: data.metrics,
            role: data.role,
            duration: data.duration,
            client: data.client,
            stack: data.stack,
          }}
        >
          <BlockRenderer blocks={data.body} anchors />
        </CaseStudyArticleView>
      </div>
    </div>
  )
})
