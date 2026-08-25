import { Link } from 'react-router-dom'

import { CtaBlock } from '@/components/blocks/CtaBlock'
import { FaqList } from '@/components/blocks/FaqList'
import { Hero } from '@/components/blocks/Hero'
import { ProcessSteps } from '@/components/blocks/ProcessSteps'
import { ProjectCard } from '@/components/blocks/ProjectCard'
import { ProofPillars } from '@/components/blocks/ProofPillars'
import { SectionHeading } from '@/components/blocks/SectionHeading'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { ArrowLink } from '@/components/ui/ArrowLink'
import { ButtonLink } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { home, homeFaq, proofPillars } from '@/config/copy'
import { processSteps } from '@/config/process'
import { routes } from '@/config/routes'
import { services } from '@/config/services'
import { formatIDRShort, site } from '@/config/site'
import { getCaseStudies } from '@/lib/content'
import { usePageMeta } from '@/lib/seo'
import { faqSchema } from '@/lib/structured-data'
import { whatsappLink, whatsappMessages } from '@/lib/whatsapp'

/**
 * Section order is not taste. Each block answers the objection created by the
 * one above it: what can you do → what do I need → why you → is it messy →
 * how much → still unsure → let's talk.
 */
export function HomePage() {
  usePageMeta({
    title: `${site.legalName} — Website & sistem web`,
    exactTitle: true,
    description: site.description,
    path: routes.home,
  })

  const caseStudies = getCaseStudies()
  const featured = caseStudies.slice(0, 2)

  return (
    <>
      <Hero
        headline={home.headline}
        lead={home.subheadline}
        note={home.heroNote}
        footnoteLabel={home.latestLabel}
        footnote={home.latestBody}
        actions={
          <>
            <ButtonLink href={whatsappLink(whatsappMessages.default)} size="large">
              Diskusi lewat WhatsApp
            </ButtonLink>
            <ButtonLink href={routes.work} variant="secondary" size="large">
              Lihat portofolio
            </ButtonLink>
          </>
        }
      />

      {/* Proof before offer. A prospect who does not trust you yet will not
          read a list of services. */}
      <section className="section border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Pekerjaan"
            title="Dua hal yang bisa Anda periksa sendiri"
            lead="Bukan galeri tangkapan layar. Tiap proyek ditulis lengkap dengan batasan yang dihadapi dan alasan tiap keputusan diambil."
          />

          <div className="mt-12 grid gap-8 lg:grid-cols-2 lg:gap-10">
            {featured.map((caseStudy) => (
              <ProjectCard key={caseStudy.slug} caseStudy={caseStudy} />
            ))}
          </div>

          {caseStudies.length > featured.length ? (
            <ArrowLink href={routes.work} className="mt-10">
              Lihat semua pekerjaan
            </ArrowLink>
          ) : null}
        </div>
      </section>

      <section className="section border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Layanan" title="Tiga jenis pekerjaan, tiga cara menghitungnya" />

          <ul className="mt-12 grid gap-x-10 gap-y-12 lg:grid-cols-3">
            {services.map((service) => (
              <li key={service.slug} className="border-line border-t pt-6">
                <h3 className="type-h3">{service.name}</h3>
                <Label as="p" className="mt-2">
                  {service.audience}
                </Label>
                <p className="text-muted mt-4">{service.summary}</p>
                <p className="numeric mt-5 font-semibold">{service.startingPriceLabel}</p>
                <ArrowLink href={routes.service(service.slug)} className="mt-5">
                  Lihat detailnya
                </ArrowLink>
              </li>
            ))}
          </ul>
        </div>
      </section>

      <section className="section border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Kenapa saya"
            title="Tiga hal yang bisa dibuktikan, bukan diklaim"
          />
          <div className="mt-12">
            <ProofPillars pillars={proofPillars} />
          </div>
        </div>
      </section>

      <section className="section border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Proses"
            title="Empat langkah, dan Anda tahu posisinya setiap saat"
            lead="Ketakutan yang paling jarang diucapkan bukan soal harga — melainkan soal prosesnya berantakan dan tidak jelas harus berbuat apa."
          />
          <div className="mt-12 max-w-[calc(var(--measure)+8rem)]">
            <ProcessSteps steps={processSteps} />
          </div>
          <ArrowLink href={routes.process} className="mt-10">
            Baca prosesnya lebih detail
          </ArrowLink>
        </div>
      </section>

      <section className="section border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Harga" title="Harga ditampilkan, bukan disembunyikan" />

          <dl className="mt-12 grid gap-x-10 gap-y-10 sm:grid-cols-3">
            {services.map((service) => (
              <div key={service.slug} className="border-line border-t pt-5">
                <dt className="type-small text-muted">{service.name}</dt>
                <dd className="numeric font-display mt-2 text-[clamp(1.5rem,1.2rem+1.2vw,2rem)] leading-none font-semibold">
                  {formatIDRShort(service.startingPrice)}
                </dd>
              </div>
            ))}
          </dl>

          <p className="text-muted mt-10 max-w-[var(--measure)]">{home.pricingNote}</p>

          <ArrowLink href={routes.pricing} className="mt-6">
            Lihat rincian paket dan termin
          </ArrowLink>
        </div>
      </section>

      <section className="section border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Pertanyaan" title="Yang biasanya ditanyakan lebih dulu" />
          <div className="mt-12">
            <FaqList items={homeFaq} />
          </div>
          <p className="type-small text-muted mt-8">
            Pertanyaan yang lebih spesifik ada di{' '}
            <Link to={routes.services} className="text-accent underline underline-offset-[0.22em]">
              tiap halaman layanan
            </Link>
            .
          </p>
        </div>
      </section>

      <CtaBlock whatsappMessage={whatsappMessages.default} />

      <WhatsAppBar message={whatsappMessages.default} />

      <JsonLd data={faqSchema(homeFaq)} />
    </>
  )
}
