import { useParams } from 'react-router-dom'

import { CtaBlock } from '@/components/blocks/CtaBlock'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { CaseStudyArticleView } from '@/components/content/CaseStudyArticle'
import { MdxContent } from '@/components/mdx/MdxContent'
import { ArrowLink } from '@/components/ui/ArrowLink'
import { Label } from '@/components/ui/Label'
import { routes } from '@/config/routes'
import { getCaseStudies, getCaseStudy, type CaseStudy } from '@/lib/content'
import { formatDate } from '@/lib/format'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'
import { NotFoundPage } from '@/pages/NotFoundPage'

const LABEL_TEXT = {
  klien: 'Klien',
  'produk-sendiri': 'Produk sendiri',
} as const

export function CaseStudyPage() {
  const { slug = '' } = useParams<{ slug: string }>()
  const caseStudy = getCaseStudy(slug)

  if (!caseStudy) return <NotFoundPage />

  return <CaseStudyArticle caseStudy={caseStudy} slug={slug} />
}

function CaseStudyArticle({
  caseStudy,
  slug,
}: {
  readonly caseStudy: CaseStudy
  readonly slug: string
}) {
  const { frontmatter: meta } = caseStudy

  usePageMeta({
    title: meta.title,
    description: meta.summary,
    path: routes.caseStudy(slug),
    type: 'article',
    modifiedTime: meta.updated,
  })

  const next = getCaseStudies().find((entry) => entry.slug !== slug)

  return (
    <>
      <CaseStudyArticleView
        data={{
          label: LABEL_TEXT[meta.label],
          kind: meta.kind,
          year: meta.year,
          title: meta.title,
          summary: meta.summary,
          metrics: meta.metrics,
          role: meta.role,
          duration: meta.duration,
          client: meta.client,
          stack: meta.stack,
        }}
        footer={
          <p className="type-small border-line text-muted mt-16 border-t pt-5">
            Diperbarui {formatDate(meta.updated)}
          </p>
        }
      >
        <MdxContent component={caseStudy.Component} />
      </CaseStudyArticleView>

      {next ? (
        <section className="section--tight border-line border-t">
          <div className="shell">
            <Label as="p">Selanjutnya</Label>
            <h2 className="type-h2 mt-3 max-w-[var(--measure)]">{next.frontmatter.title}</h2>
            <ArrowLink href={routes.caseStudy(next.slug)} className="mt-6">
              Baca studi kasus
            </ArrowLink>
          </div>
        </section>
      ) : null}

      <CtaBlock
        title="Masalah Anda mirip yang di atas?"
        whatsappMessage={whatsappMessages.caseStudy(meta.title)}
      />

      <WhatsAppBar message={whatsappMessages.caseStudy(meta.title)} />

      <JsonLd
        data={breadcrumbSchema([
          { label: 'Portofolio', path: routes.work },
          { label: meta.title, path: routes.caseStudy(slug) },
        ])}
      />
    </>
  )
}
