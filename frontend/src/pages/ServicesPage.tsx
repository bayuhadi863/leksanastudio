import { CtaBlock } from '@/components/blocks/CtaBlock'
import { OutOfScopeList } from '@/components/blocks/OutOfScopeList'
import { SectionHeading } from '@/components/blocks/SectionHeading'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { ArrowLink } from '@/components/ui/ArrowLink'
import { Label } from '@/components/ui/Label'
import { Note } from '@/components/ui/Note'
import { outOfScope } from '@/config/copy'
import { routes } from '@/config/routes'
import { services } from '@/config/services'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'

export function ServicesPage() {
  usePageMeta({
    title: 'Layanan',
    description:
      'Tiga jenis pekerjaan: website bisnis, company profile, dan sistem web. Lengkap dengan daftar pekerjaan yang tidak saya kerjakan.',
    path: routes.services,
  })

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="annotation">
          <div className="annotation__body">
            <h1 className="type-h1">Tiga jenis pekerjaan, tiga cara menghitungnya</h1>
            <p className="type-lead mt-6">
              Perbedaannya bukan besar-kecilnya, melainkan dari mana desainnya dimulai dan seberapa
              dalam alurnya. Itu yang menentukan harga dan waktu pengerjaan.
            </p>
          </div>
          <Note>
            Kalau kebutuhan Anda sebenarnya sudah tercakup paket yang lebih murah, saya akan
            mengatakannya. Menjual paket yang terlalu besar menghasilkan satu proyek dan
            menghabiskan satu rujukan.
          </Note>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <ul className="grid gap-16">
            {services.map((service) => (
              <li key={service.slug} className="grid gap-8 lg:grid-cols-[1fr_1.4fr] lg:gap-12">
                <div>
                  <Label as="p">{service.audience}</Label>
                  <h2 className="type-h2 mt-3">{service.name}</h2>
                  <p className="numeric mt-4 font-semibold">{service.startingPriceLabel}</p>
                </div>

                <div>
                  <p className="type-lead">{service.summary}</p>

                  <ul className="mt-6 grid gap-2.5">
                    {service.deliverables.slice(0, 3).map((item) => (
                      <li key={item} className="type-small text-muted relative pl-6">
                        <span
                          aria-hidden="true"
                          className="bg-accent absolute top-[0.72em] left-0 h-px w-3"
                        />
                        {item}
                      </li>
                    ))}
                  </ul>

                  <ArrowLink href={routes.service(service.slug)} className="mt-7">
                    Lihat detail {service.shortName}
                  </ArrowLink>
                </div>
              </li>
            ))}
          </ul>
        </div>
      </section>

      <section className="section border-line border-t">
        <div className="shell">
          <div className="annotation">
            <div className="annotation__body">
              <SectionHeading
                eyebrow="Batas"
                title="Yang tidak saya kerjakan"
                lead="Ditulis supaya jelas sejak awal, dan supaya kita sama-sama tidak membuang waktu."
              />
            </div>
            <Note>
              Daftar ini terasa seperti menolak uang. Fungsinya menyaring — dan di beberapa hal di
              bawah, menolak justru melindungi Anda, bukan saya.
            </Note>
          </div>

          <div className="mt-10">
            <OutOfScopeList items={outOfScope} />
          </div>

          <p className="type-small text-muted mt-8 max-w-[var(--measure)]">
            Kalau kebutuhan Anda ada di daftar itu, saya senang merekomendasikan orang lain yang
            memang mengerjakannya.
          </p>
        </div>
      </section>

      <CtaBlock whatsappMessage={whatsappMessages.default} />

      <WhatsAppBar message={whatsappMessages.default} />

      <JsonLd data={breadcrumbSchema([{ label: 'Layanan', path: routes.services }])} />
    </>
  )
}
