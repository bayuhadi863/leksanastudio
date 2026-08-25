import { CtaBlock } from '@/components/blocks/CtaBlock'
import { ProjectCard } from '@/components/blocks/ProjectCard'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { Note } from '@/components/ui/Note'
import { routes } from '@/config/routes'
import { getCaseStudies } from '@/lib/content'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'

export function WorkPage() {
  usePageMeta({
    title: 'Portofolio',
    description:
      'Pekerjaan yang bisa diperiksa: batasan yang dihadapi, keputusan yang diambil, dan alasan tiap alternatif ditolak.',
    path: routes.work,
  })

  const caseStudies = getCaseStudies()

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="annotation">
          <div className="annotation__body">
            <h1 className="type-h1">Pekerjaan yang bisa Anda periksa sendiri</h1>
            <p className="type-lead mt-6">
              Bukan galeri tangkapan layar. Tiap proyek ditulis dengan format yang sama: apa
              masalahnya, batasan apa yang membuatnya tidak biasa, keputusan apa yang diambil, dan
              apa yang akan saya lakukan berbeda kalau mengulang.
            </p>
          </div>
          <Note>
            Tiap kartu berlabel jujur. <strong>Klien</strong> berarti dikerjakan untuk pihak lain
            dan dipakai sungguhan. <strong>Produk sendiri</strong> berarti saya yang membangunnya
            untuk diri sendiri. Menyamarkan yang kedua sebagai yang pertama adalah cara tercepat
            kehilangan kepercayaan.
          </Note>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <div className="grid gap-8 lg:grid-cols-2 lg:gap-10">
            {caseStudies.map((caseStudy) => (
              <ProjectCard key={caseStudy.slug} caseStudy={caseStudy} />
            ))}
          </div>
        </div>
      </section>

      <CtaBlock
        title="Masalah Anda mirip salah satu di atas?"
        whatsappMessage={whatsappMessages.default}
      />

      <WhatsAppBar message={whatsappMessages.default} />

      <JsonLd data={breadcrumbSchema([{ label: 'Portofolio', path: routes.work }])} />
    </>
  )
}
