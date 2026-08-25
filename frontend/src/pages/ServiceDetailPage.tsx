import { useParams } from 'react-router-dom'

import { CtaBlock } from '@/components/blocks/CtaBlock'
import { DeliverableList } from '@/components/blocks/DeliverableList'
import { FaqList } from '@/components/blocks/FaqList'
import { PhaseList } from '@/components/blocks/PhaseList'
import { PricingTable } from '@/components/blocks/PricingTable'
import { ProblemList } from '@/components/blocks/ProblemList'
import { ProjectCard } from '@/components/blocks/ProjectCard'
import { SectionHeading } from '@/components/blocks/SectionHeading'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { ButtonLink } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { Note } from '@/components/ui/Note'
import { businessPackages, corporatePackages, systemPhases } from '@/config/packages'
import { routes } from '@/config/routes'
import { getServiceBySlug, type Service } from '@/config/services'
import { getCaseStudy } from '@/lib/content'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema, faqSchema, serviceSchema } from '@/lib/structured-data'
import { whatsappLink, whatsappMessages } from '@/lib/whatsapp'
import { NotFoundPage } from '@/pages/NotFoundPage'

/**
 * A slug that is not in the config 404s cleanly instead of rendering an empty
 * shell — the same contract `dynamicParams = false` gave us before.
 */
export function ServiceDetailPage() {
  const { slug = '' } = useParams<{ slug: string }>()
  const service = getServiceBySlug(slug)

  if (!service) return <NotFoundPage />

  return <ServiceDetail service={service} slug={slug} />
}

function ServiceDetail({ service, slug }: { readonly service: Service; readonly slug: string }) {
  usePageMeta({
    title: service.name,
    description: service.summary,
    path: routes.service(slug),
  })

  const caseStudy = service.caseStudySlug ? getCaseStudy(service.caseStudySlug) : undefined
  const whatsappMessage = whatsappMessages.service(service.name)

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="document">
          <Label as="p">{service.audience}</Label>
          <h1 className="type-h1 mt-5">{service.headline}</h1>
          <p className="type-lead mt-6">{service.summary}</p>

          <div className="mt-9 flex flex-col gap-3 sm:flex-row">
            <ButtonLink href={whatsappLink(whatsappMessage)} size="large">
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
          <SectionHeading eyebrow="Masalah" title="Yang biasanya sedang terjadi" />
          <div className="mt-10">
            <ProblemList problems={service.problems} />
          </div>
        </div>
      </section>

      <section className="section--tight">
        <div className="shell">
          <div className="annotation">
            <div className="annotation__body">
              <SectionHeading eyebrow="Yang didapat" title="Konkret, bukan sifat" />
              <div className="mt-8">
                <DeliverableList items={service.deliverables} />
              </div>
            </div>
            <Note>
              Daftar ini sengaja tidak memuat nama teknologi. Yang menentukan hasil bukan framework
              yang saya pakai, melainkan apakah tiap halaman punya pekerjaan yang jelas.
            </Note>
          </div>
        </div>
      </section>

      {caseStudy ? (
        <section className="section--tight border-line border-t">
          <div className="shell">
            <SectionHeading
              eyebrow="Contoh nyata"
              title="Pekerjaan yang paling mirip kebutuhan ini"
            />
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
              service.pricingShape === 'phases'
                ? 'Dijual per tahap, bukan per paket'
                : 'Tiga paket, harga tertulis'
            }
            lead={
              service.pricingShape === 'phases'
                ? 'Sistem tidak bisa diberi harga pasti sebelum ruang lingkupnya jelas. Karena itu tahap pertama adalah lokakarya berbayar yang keluarannya milik Anda.'
                : undefined
            }
          />

          <div className="mt-12">
            {service.pricingShape === 'phases' ? (
              <PhaseList phases={systemPhases} />
            ) : (
              <PricingTable
                packages={
                  service.pricingShape === 'business-packages'
                    ? businessPackages
                    : corporatePackages
                }
                whatsappIntro={whatsappMessage}
              />
            )}
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Batas" title="Yang tidak termasuk" />
          <ul className="mt-8 grid max-w-[var(--measure)] gap-3.5">
            {service.exclusions.map((item) => (
              <li key={item} className="text-muted relative pl-7">
                <span aria-hidden="true" className="absolute left-0 opacity-55">
                  ✕
                </span>
                {item}
              </li>
            ))}
          </ul>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Pertanyaan" title={`Soal ${service.shortName}`} />
          <div className="mt-10">
            <FaqList items={service.faq} />
          </div>
        </div>
      </section>

      <CtaBlock whatsappMessage={whatsappMessage} />

      <WhatsAppBar message={whatsappMessage} />

      <JsonLd
        data={[
          serviceSchema({
            name: service.name,
            description: service.summary,
            path: routes.service(slug),
            startingPrice: service.startingPrice,
          }),
          faqSchema(service.faq),
          breadcrumbSchema([
            { label: 'Layanan', path: routes.services },
            { label: service.name, path: routes.service(slug) },
          ]),
        ]}
      />
    </>
  )
}
