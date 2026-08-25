import { CtaBlock } from '@/components/blocks/CtaBlock'
import { SectionHeading } from '@/components/blocks/SectionHeading'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { Annotation } from '@/components/layout/Annotation'
import { Label } from '@/components/ui/Label'
import { about } from '@/config/copy'
import { routes } from '@/config/routes'
import { site } from '@/config/site'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'

export function AboutPage() {
  usePageMeta({
    title: 'Tentang',
    description: `${site.ownerName} — pengembang web dan sistem web di ${site.city}. Apa yang sudah dikerjakan, cara kerjanya, dan apa yang bukan bidangnya.`,
    path: routes.about,
  })

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <Annotation note={about.note}>
          <Label as="p">{site.city}, Indonesia</Label>
          <h1 className="type-h1 mt-5">{site.ownerName}</h1>
          <p className="type-lead mt-6">{about.intro}</p>
        </Annotation>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Kenapa bisa dipercaya"
            title="Bukti, bukan jumlah tahun pengalaman"
          />

          <ul className="mt-10 grid gap-x-10 gap-y-10 lg:grid-cols-3">
            {about.credibility.map((item) => (
              <li key={item.title} className="border-accent border-t-2 pt-6">
                <h3 className="type-h3">{item.title}</h3>
                <p className="text-muted mt-4">{item.body}</p>
              </li>
            ))}
          </ul>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Cara kerja" title="Tiga hal yang punya konsekuensi nyata" />

          <ul className="mt-10 grid gap-8">
            {about.principles.map((item) => (
              <li
                key={item.title}
                className="border-line grid gap-x-10 gap-y-3 border-t pt-6 lg:grid-cols-[1fr_1.6fr]"
              >
                <h3 className="type-h3">{item.title}</h3>
                <p className="text-muted max-w-[var(--measure)]">{item.body}</p>
              </li>
            ))}
          </ul>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Batas"
            title="Yang bukan bidang saya"
            lead="Merekomendasikan orang lain untuk hal-hal ini jauh lebih berguna bagi Anda daripada saya mencobanya."
          />

          <ul className="mt-8 grid max-w-[var(--measure)] gap-3">
            {about.notMyField.map((item) => (
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
          <SectionHeading eyebrow="Kontak" title="Cara menghubungi" />
          <dl className="mt-8 grid gap-6 sm:grid-cols-3">
            <div className="border-line border-t pt-4">
              <Label as="dt">WhatsApp</Label>
              <dd className="numeric type-small mt-2">
                <a href={`tel:+${site.whatsapp.number}`} className="hover:text-accent">
                  {site.whatsapp.display}
                </a>
              </dd>
            </div>
            <div className="border-line border-t pt-4">
              <Label as="dt">Surel</Label>
              <dd className="type-small mt-2">
                <a href={`mailto:${site.email}`} className="hover:text-accent">
                  {site.email}
                </a>
              </dd>
            </div>
            <div className="border-line border-t pt-4">
              <Label as="dt">LinkedIn</Label>
              <dd className="type-small mt-2">
                <a
                  href={site.social.linkedin}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="hover:text-accent"
                >
                  {site.ownerName}
                </a>
              </dd>
            </div>
          </dl>
        </div>
      </section>

      <CtaBlock whatsappMessage={whatsappMessages.default} />

      <WhatsAppBar message={whatsappMessages.default} />

      <JsonLd data={breadcrumbSchema([{ label: 'Tentang', path: routes.about }])} />
    </>
  )
}
