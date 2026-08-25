import { useParams } from 'react-router-dom'

import { CtaBlock } from '@/components/blocks/CtaBlock'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { MdxContent } from '@/components/mdx/MdxContent'
import { ArrowLink } from '@/components/ui/ArrowLink'
import { Label } from '@/components/ui/Label'
import { routes } from '@/config/routes'
import { getNote, getNotes, PILLAR_LABEL, type Note } from '@/lib/content'
import { formatDate } from '@/lib/format'
import { usePageMeta } from '@/lib/seo'
import { articleSchema, breadcrumbSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'
import { NotFoundPage } from '@/pages/NotFoundPage'

export function NoteDetailPage() {
  const { slug = '' } = useParams<{ slug: string }>()
  const note = getNote(slug)

  if (!note) return <NotFoundPage />

  return <NoteArticle note={note} slug={slug} />
}

function NoteArticle({ note, slug }: { readonly note: Note; readonly slug: string }) {
  usePageMeta({
    title: note.frontmatter.title,
    description: note.frontmatter.summary,
    path: routes.note(slug),
    type: 'article',
    publishedTime: note.frontmatter.published,
    modifiedTime: note.frontmatter.updated ?? note.frontmatter.published,
  })

  const notes = getNotes()
  const index = notes.findIndex((entry) => entry.slug === slug)
  const next = notes[index + 1] ?? notes[0]
  const hasNext = next && next.slug !== slug

  return (
    <>
      <article>
        <header className="shell pt-14 pb-12 lg:pt-24 lg:pb-14">
          <div className="document">
            <Label as="p">
              {PILLAR_LABEL[note.frontmatter.pillar]} · {formatDate(note.frontmatter.published)} ·{' '}
              {note.readingMinutes} menit baca
            </Label>

            <h1 className="type-h1 mt-5">{note.frontmatter.title}</h1>

            <p className="type-lead mt-6">{note.frontmatter.summary}</p>
          </div>
        </header>

        <div className="shell pb-20 lg:pb-28">
          <div className="document copy border-line border-t pt-12">
            <MdxContent component={note.Component} />
          </div>

          {note.frontmatter.updated ? (
            <p className="type-small border-line text-muted mt-16 border-t pt-5">
              Diperbarui {formatDate(note.frontmatter.updated)}
            </p>
          ) : null}
        </div>
      </article>

      {hasNext ? (
        <section className="section--tight border-line border-t">
          <div className="shell">
            <Label as="p">Catatan lain</Label>
            <h2 className="type-h2 mt-3 max-w-[var(--measure)]">{next.frontmatter.title}</h2>
            <ArrowLink href={routes.note(next.slug)} className="mt-6">
              Baca catatan
            </ArrowLink>
          </div>
        </section>
      ) : null}

      <CtaBlock
        title="Ada masalah teknis yang mirip?"
        body="Kalau Anda sedang menghadapi sesuatu yang mirip, ceritakan saja. Kalaupun tidak berujung proyek, biasanya saya bisa menunjukkan arahnya."
        whatsappMessage={whatsappMessages.default}
      />

      <WhatsAppBar message={whatsappMessages.default} />

      <JsonLd
        data={[
          articleSchema({
            title: note.frontmatter.title,
            description: note.frontmatter.summary,
            path: routes.note(slug),
            published: note.frontmatter.published,
            updated: note.frontmatter.updated,
          }),
          breadcrumbSchema([
            { label: 'Catatan', path: routes.notes },
            { label: note.frontmatter.title, path: routes.note(slug) },
          ]),
        ]}
      />
    </>
  )
}
