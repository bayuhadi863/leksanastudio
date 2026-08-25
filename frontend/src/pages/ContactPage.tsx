import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { ButtonLink } from '@/components/ui/Button'
import { ContactForm } from '@/components/ui/ContactForm'
import { Label } from '@/components/ui/Label'
import { Note } from '@/components/ui/Note'
import { routes } from '@/config/routes'
import { site } from '@/config/site'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema } from '@/lib/structured-data'
import { whatsappLink, whatsappMessages } from '@/lib/whatsapp'

export function ContactPage() {
  usePageMeta({
    title: 'Kontak',
    description: `Ceritakan kebutuhan Anda lewat WhatsApp atau formulir. Dibalas dalam ${site.promises.replyWithinHours} jam pada jam kerja.`,
    path: routes.contact,
  })

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="annotation">
          <div className="annotation__body">
            <h1 className="type-h1">Ceritakan dulu masalahnya</h1>
            <p className="type-lead mt-6">
              Obrolan awal 30 menit, tanpa biaya dan tanpa kewajiban. Kalau ternyata kebutuhan Anda
              bukan bidang saya, saya katakan sejak awal dan merekomendasikan orang lain.
            </p>

            <div className="mt-9 flex flex-col gap-3 sm:flex-row">
              <ButtonLink href={whatsappLink(whatsappMessages.default)} size="large">
                Diskusi lewat WhatsApp
              </ButtonLink>
              <ButtonLink href={`mailto:${site.email}`} variant="secondary" size="large">
                Kirim surel
              </ButtonLink>
            </div>

            <p className="type-small text-muted mt-5">
              Dibalas dalam {site.promises.replyWithinHours} jam pada jam kerja.
            </p>
          </div>

          <Note>
            WhatsApp hampir selalu lebih cepat daripada formulir. Formulir ada untuk Anda yang ingin
            menulis panjang lebih dulu, atau sedang tidak ingin menampilkan nomor.
          </Note>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <div className="grid gap-12 lg:grid-cols-[1.5fr_1fr] lg:gap-16">
            <div>
              <h2 className="type-h2">Atau tulis di sini</h2>
              <p className="text-muted mt-4 max-w-[var(--measure)]">
                Tiga isian saja. Sisanya kita bicarakan langsung — pertanyaan yang tepat tergantung
                jawaban Anda.
              </p>

              <div className="mt-10">
                <ContactForm />
              </div>
            </div>

            <aside className="lg:pt-4">
              <div className="border-line border-t pt-5">
                <Label as="p">Yang berguna disebut</Label>
                <ul className="mt-4 grid gap-2.5">
                  {[
                    'Jenis usaha atau unit Anda',
                    'Apa yang paling merepotkan sekarang',
                    'Target kapan harus jalan',
                    'Rentang anggaran yang disiapkan',
                  ].map((item) => (
                    <li key={item} className="type-small text-muted relative pl-5">
                      <span
                        aria-hidden="true"
                        className="bg-accent absolute top-[0.7em] left-0 h-px w-2.5"
                      />
                      {item}
                    </li>
                  ))}
                </ul>
              </div>

              <div className="border-line mt-10 border-t pt-5">
                <Label as="p">Kontak langsung</Label>
                <p className="type-small mt-4">
                  <a href={`mailto:${site.email}`} className="hover:text-accent">
                    {site.email}
                  </a>
                  <br />
                  <a href={`tel:+${site.whatsapp.number}`} className="numeric hover:text-accent">
                    {site.whatsapp.display}
                  </a>
                  <br />
                  <span className="text-muted">
                    {site.city}, {site.region}
                  </span>
                </p>
              </div>
            </aside>
          </div>
        </div>
      </section>

      <WhatsAppBar message={whatsappMessages.default} />

      <JsonLd data={breadcrumbSchema([{ label: 'Kontak', path: routes.contact }])} />
    </>
  )
}
