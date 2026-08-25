import { useParams } from 'react-router-dom'

import { CtaBlock } from '@/components/blocks/CtaBlock'
import { DeliverableList } from '@/components/blocks/DeliverableList'
import { FaqList } from '@/components/blocks/FaqList'
import { PhaseList } from '@/components/blocks/PhaseList'
import { PricingTable } from '@/components/blocks/PricingTable'
import { ProblemList } from '@/components/blocks/ProblemList'
import { ProjectCard } from '@/components/blocks/ProjectCard'
import { SectionHeading } from '@/components/blocks/SectionHeading'
import { Annotation } from '@/components/layout/Annotation'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { ArrowLink } from '@/components/ui/ArrowLink'
import { ButtonLink } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { businessPackages, corporatePackages, systemPhases } from '@/config/packages'
import { routes } from '@/config/routes'
import { getServiceBySlug } from '@/config/services'
import { getVerticalBySlug, type Vertical } from '@/config/verticals'
import { getCaseStudy } from '@/lib/content'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema, faqSchema, serviceSchema } from '@/lib/structured-data'
import { whatsappLink } from '@/lib/whatsapp'
import { NotFoundPage } from '@/pages/NotFoundPage'

/**
 * Vertical landing pages — the SEO engine.
 *
 * One template, many data files. A generalist cannot win the head of the
 * search curve, so this is how the long tail gets covered: a prospect from one
 * industry lands on a page written entirely in their language and never feels
 * like they are talking to a generalist.
 *
 * A slug not listed in the config 404s cleanly instead of rendering an empty
 * shell — the contract `dynamicParams = false` used to give us.
 */
export function VerticalPage() {
  const { vertical: slug = '' } = useParams<{ vertical: string }>()
  const vertical = getVerticalBySlug(slug)

  if (!vertical) return <NotFoundPage />

  return <VerticalLanding vertical={vertical} slug={slug} />
}

function VerticalLanding({
  vertical,
  slug,
}: {
  readonly vertical: Vertical
  readonly slug: string
}) {
  usePageMeta({
    title: vertical.headline,
    description: vertical.intro.slice(0, 200),
    path: routes.vertical(slug),
  })

  const service = getServiceBySlug(vertical.serviceSlug)
  const caseStudy = service?.caseStudySlug ? getCaseStudy(service.caseStudySlug) : undefined

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="document">
          <Label as="p">{vertical.industry}</Label>
          <h1 className="type-h1 mt-5">{vertical.headline}</h1>
          <p className="type-lead mt-6">{vertical.intro}</p>

          <div className="mt-9 flex flex-col gap-3 sm:flex-row">
            <ButtonLink href={whatsappLink(vertical.whatsappIntro)} size="large">
              Diskusi lewat WhatsApp
            </ButtonLink>
            <ButtonLink href="#harga" variant="secondary" size="large">
              Lihat harga
            </ButtonLink>
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Masalah"
            title={`Yang biasanya terjadi di ${vertical.industry.toLowerCase()}`}
          />
          <div className="mt-10">
            <ProblemList problems={vertical.problems} />
          </div>
        </div>
      </section>

      <section className="section--tight">
        <div className="shell">
          <Annotation note={vertical.note}>
            <SectionHeading eyebrow="Yang dikerjakan" title="Khusus untuk kebutuhan ini" />
            <div className="mt-8">
              <DeliverableList items={vertical.deliverables} />
            </div>
          </Annotation>
        </div>
      </section>

      {caseStudy ? (
        <section className="section--tight border-line border-t">
          <div className="shell">
            <SectionHeading eyebrow="Contoh nyata" title="Pekerjaan yang bisa diperiksa" />
            <div className="mt-10 max-w-2xl">
              <ProjectCard caseStudy={caseStudy} />
            </div>
          </div>
        </section>
      ) : null}

      <section id="harga" className="section border-line scroll-mt-28 border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Harga"
            title={
              vertical.pricingShape === 'phases'
                ? 'Dijual per tahap, bukan per paket'
                : 'Tiga paket, harga tertulis'
            }
          />

          <div className="mt-12">
            {vertical.pricingShape === 'phases' ? (
              <PhaseList phases={systemPhases} />
            ) : (
              <PricingTable
                packages={
                  vertical.pricingShape === 'business-packages'
                    ? businessPackages
                    : corporatePackages
                }
                whatsappIntro={vertical.whatsappIntro}
              />
            )}
          </div>

          {service ? (
            <ArrowLink href={routes.service(service.slug)} className="mt-10">
              Lihat layanan {service.shortName} selengkapnya
            </ArrowLink>
          ) : null}
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Pertanyaan"
            title={`Yang sering ditanyakan ${vertical.industry.toLowerCase()}`}
          />
          <div className="mt-10">
            <FaqList items={vertical.faq} />
          </div>
        </div>
      </section>

      <CtaBlock whatsappMessage={vertical.whatsappIntro} />

      <WhatsAppBar message={vertical.whatsappIntro} />

      <JsonLd
        data={[
          serviceSchema({
            name: vertical.headline,
            description: vertical.intro,
            path: routes.vertical(slug),
            startingPrice: service?.startingPrice ?? 3_500_000,
          }),
          faqSchema(vertical.faq),
          breadcrumbSchema([{ label: vertical.industry, path: routes.vertical(slug) }]),
        ]}
      />
    </>
  )
}
