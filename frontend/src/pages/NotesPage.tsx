import { Link } from 'react-router-dom'

import { CtaBlock } from '@/components/blocks/CtaBlock'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { Label } from '@/components/ui/Label'
import { Note } from '@/components/ui/Note'
import { routes } from '@/config/routes'
import { getNotes, PILLAR_LABEL } from '@/lib/content'
import { formatDate } from '@/lib/format'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'

export function NotesPage() {
  usePageMeta({
    title: 'Catatan teknis',
    description:
      'Keputusan teknis beserta alasannya, panduan untuk yang sedang mencari jasa, dan bedah masalah per industri.',
    path: routes.notes,
  })

  const notes = getNotes()

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="annotation">
          <div className="annotation__body">
            <h1 className="type-h1">Catatan teknis</h1>
            <p className="type-lead mt-6">
              Keputusan yang saya ambil, alternatif yang saya tolak, dan harga yang harus dibayar
              masing-masing. Ditulis untuk saya sendiri enam bulan lagi — kalau ternyata berguna
              buat Anda, lebih baik lagi.
            </p>
          </div>
          <Note>
            Saya menulis satu tulisan yang benar-benar selesai per dua minggu, bukan empat yang
            dangkal per bulan. Tulisan dangkal merusak hal yang justru sedang dibangun di sini.
          </Note>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <ul className="grid">
            {notes.map((note) => (
              <li key={note.slug} className="border-line border-b first:border-t">
                <Link
                  to={routes.note(note.slug)}
                  className="group/note grid gap-x-10 gap-y-3 py-8 lg:grid-cols-[10rem_1fr]"
                >
                  <div>
                    <Label as="p">{PILLAR_LABEL[note.frontmatter.pillar]}</Label>
                    <p className="type-small text-muted mt-2">
                      {formatDate(note.frontmatter.published)}
                    </p>
                  </div>

                  <div>
                    <h2 className="type-h3 group-hover/note:text-accent transition-colors duration-150 ease-out">
                      {note.frontmatter.title}
                    </h2>
                    <p className="text-muted mt-3 max-w-[var(--measure)]">
                      {note.frontmatter.summary}
                    </p>
                    <p className="type-label text-muted mt-4">{note.readingMinutes} menit baca</p>
                  </div>
                </Link>
              </li>
            ))}
          </ul>
        </div>
      </section>

      <CtaBlock whatsappMessage={whatsappMessages.default} />

      <WhatsAppBar message={whatsappMessages.default} />

      <JsonLd data={breadcrumbSchema([{ label: 'Catatan', path: routes.notes }])} />
    </>
  )
}
