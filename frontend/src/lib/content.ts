import type { ComponentType } from 'react'
import type { MDXComponents } from 'mdx/types'
import { readingTimes } from 'virtual:leksana-reading-time'
import { z } from 'zod'

/* ------------------------------------------------------------------ schemas */

const metricSchema = z.object({
  value: z.string(),
  label: z.string(),
})

const caseStudyFrontmatterSchema = z.object({
  title: z.string(),
  /** Honest labelling is mandatory — see blueprint 05 §5.6. */
  label: z.enum(['klien', 'produk-sendiri']),
  client: z.string(),
  kind: z.string(),
  /**
   * Fallback illustration when no real screenshot exists yet.
   * See components/blocks/Schematic.
   */
  figure: z.enum(['system', 'website', 'catalog']),
  /**
   * Real screenshot. When present it replaces the schematic everywhere the
   * project is shown. Path is relative to /public.
   */
  cover: z
    .object({
      src: z.string().startsWith('/'),
      alt: z.string().min(10, 'Alt perlu menjelaskan isi tangkapan layar, bukan menamainya.'),
    })
    .optional(),
  summary: z.string(),
  problem: z.string(),
  year: z.number().int(),
  duration: z.string(),
  role: z.string(),
  stack: z.array(z.string()).min(1),
  metrics: z.array(metricSchema).length(3),
  updated: z.string(),
  order: z.number().int(),
  draft: z.boolean().default(false),
})

const noteFrontmatterSchema = z.object({
  title: z.string(),
  summary: z.string(),
  published: z.string(),
  updated: z.string().optional(),
  pillar: z.enum(['keputusan', 'panduan', 'industri']),
  draft: z.boolean().default(false),
})

export type CaseStudyFrontmatter = z.infer<typeof caseStudyFrontmatterSchema>
export type NoteFrontmatter = z.infer<typeof noteFrontmatterSchema>

export type MdxComponent = ComponentType<{ readonly components?: MDXComponents }>

export type ContentEntry<T> = {
  readonly slug: string
  readonly frontmatter: T
  /** Compiled MDX. Rendered through components/mdx/MdxContent. */
  readonly Component: MdxComponent
  readonly readingMinutes: number
}

export type CaseStudy = ContentEntry<CaseStudyFrontmatter>
export type Note = ContentEntry<NoteFrontmatter>

/* ------------------------------------------------------------------- loader */

type MdxModule = {
  readonly default: MdxComponent
  readonly frontmatter?: unknown
}

/*
 * Compiled at build time by @mdx-js/rollup; the `frontmatter` export comes from
 * remark-mdx-frontmatter. No MDX compiler and no article source reaches the
 * browser — the reading estimates arrive precomputed through a virtual module
 * (see server/reading-time.ts).
 */
const caseStudyModules = import.meta.glob<MdxModule>('../content/studi-kasus/*.mdx', {
  eager: true,
})

const noteModules = import.meta.glob<MdxModule>('../content/catatan/*.mdx', { eager: true })

const slugOf = (filePath: string): string =>
  filePath
    .split('/')
    .pop()
    ?.replace(/\.mdx$/, '') ?? filePath

const readCollection = <S extends z.ZodTypeAny>(
  folder: string,
  modules: Record<string, MdxModule>,
  schema: S,
): readonly ContentEntry<z.infer<S>>[] =>
  Object.entries(modules).map(([filePath, module]) => {
    const slug = slugOf(filePath)
    const parsed = schema.safeParse(module.frontmatter)

    if (!parsed.success) {
      // Failing loudly is correct here: a malformed content file must never
      // reach production silently.
      throw new Error(
        `Frontmatter tidak valid pada content/${folder}/${slug}.mdx\n${parsed.error.toString()}`,
      )
    }

    return {
      slug,
      frontmatter: parsed.data,
      Component: module.default,
      readingMinutes: readingTimes[`${folder}/${slug}`] ?? 1,
    } satisfies ContentEntry<z.infer<S>>
  })

const isPublished = (draft: boolean): boolean => import.meta.env.DEV || !draft

/* -------------------------------------------------------------- case studies */

const caseStudies: readonly CaseStudy[] = readCollection(
  'studi-kasus',
  caseStudyModules,
  caseStudyFrontmatterSchema,
)
  .filter((entry) => isPublished(entry.frontmatter.draft))
  .sort((a, b) => a.frontmatter.order - b.frontmatter.order)

export const getCaseStudies = (): readonly CaseStudy[] => caseStudies

export const getCaseStudy = (slug: string): CaseStudy | undefined =>
  caseStudies.find((entry) => entry.slug === slug)

/* --------------------------------------------------------------------- notes */

const notes: readonly Note[] = readCollection('catatan', noteModules, noteFrontmatterSchema)
  .filter((entry) => isPublished(entry.frontmatter.draft))
  .sort(
    (a, b) =>
      new Date(b.frontmatter.published).getTime() - new Date(a.frontmatter.published).getTime(),
  )

export const getNotes = (): readonly Note[] => notes

export const getNote = (slug: string): Note | undefined =>
  notes.find((entry) => entry.slug === slug)

export const PILLAR_LABEL: Record<NoteFrontmatter['pillar'], string> = {
  keputusan: 'Keputusan teknis',
  panduan: 'Panduan pembeli',
  industri: 'Bedah industri',
}
