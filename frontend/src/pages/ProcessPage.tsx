import { CtaBlock } from '@/components/blocks/CtaBlock'
import { ProcessSteps } from '@/components/blocks/ProcessSteps'
import { SectionHeading } from '@/components/blocks/SectionHeading'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { Label } from '@/components/ui/Label'
import { Note } from '@/components/ui/Note'
import { commonFears, processSteps } from '@/config/process'
import { paymentTerms } from '@/config/packages'
import { routes } from '@/config/routes'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'

export function ProcessPage() {
  usePageMeta({
    title: 'Proses',
    description:
      'Empat langkah dari obrolan awal sampai serah terima, lengkap dengan apa yang saya butuhkan dari Anda di tiap langkah.',
    path: routes.process,
  })

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="annotation">
          <div className="annotation__body">
            <h1 className="type-h1">Empat langkah, dan Anda tahu posisinya setiap saat</h1>
            <p className="type-lead mt-6">
              Ketakutan yang paling jarang diucapkan bukan soal harga. Melainkan soal prosesnya
              berantakan, kabarnya hilang, dan tidak jelas harus berbuat apa. Halaman ini menjawab
              itu sebelum ditanyakan.
            </p>
          </div>
          <Note>
            Tiap langkah memuat baris “yang saya butuhkan dari Anda”. Proyek yang molor hampir
            selalu molor di sana, bukan di pengerjaannya.
          </Note>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <div className="max-w-[calc(var(--measure)+10rem)]">
            <ProcessSteps steps={processSteps} detailed />
          </div>
        </div>
      </section>

      <section className="section border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Yang biasanya ditakutkan"
            title="Tiga hal yang jarang ditanyakan langsung"
          />

          <ul className="mt-10 grid gap-x-10 gap-y-10 lg:grid-cols-3">
            {commonFears.map((fear) => (
              <li key={fear.question} className="border-accent border-t-2 pt-6">
                <h3 className="type-h3">{fear.question}</h3>
                <p className="text-muted mt-4">{fear.answer}</p>
              </li>
            ))}
          </ul>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Pembayaran" title="Termin per jenis pekerjaan" />

          <div className="scroll-x mt-10">
            <table className="w-full min-w-lg text-left">
              <caption className="sr-only">Termin pembayaran per jenis pekerjaan</caption>
              <thead>
                <tr className="border-line border-b">
                  <th scope="col" className="pb-3">
                    <Label as="span">Pekerjaan</Label>
                  </th>
                  <th scope="col" className="pb-3">
                    <Label as="span">Termin</Label>
                  </th>
                </tr>
              </thead>
              <tbody>
                {paymentTerms.map((term) => (
                  <tr key={term.scope} className="border-line border-b">
                    <th scope="row" className="type-small py-4 pr-8 font-semibold">
                      {term.scope}
                    </th>
                    <td className="type-small text-muted py-4">{term.schedule}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <p className="type-small text-muted mt-8 max-w-[var(--measure)]">
            Domain dan hosting dibayar langsung oleh Anda atas nama Anda sendiri. Saya tidak
            menalanginya — itu sumber sengketa nomor satu di jasa web, dan mencegahnya jauh lebih
            mudah daripada menyelesaikannya.
          </p>
        </div>
      </section>

      <CtaBlock whatsappMessage={whatsappMessages.process} />

      <WhatsAppBar message={whatsappMessages.process} />

      <JsonLd data={breadcrumbSchema([{ label: 'Proses', path: routes.process }])} />
    </>
  )
}
