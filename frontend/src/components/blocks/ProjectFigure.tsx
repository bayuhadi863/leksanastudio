import { Schematic, type SchematicVariant } from '@/components/blocks/Schematic'

type Props = {
  /** Real screenshot. When absent the schematic stands in. */
  readonly cover?: { readonly src: string; readonly alt: string }
  readonly fallback: SchematicVariant
  readonly fallbackTitle: string
  /**
   * Rendered widths, so the browser downloads one size instead of the largest.
   * Cards sit two-up inside the shell; figures run the full document width.
   */
  readonly sizes: string
  readonly priority?: boolean
}

/**
 * One decision point for every project illustration on the site.
 *
 * A real screenshot always wins — it is evidence, and the schematic only ever
 * existed because a client's admin panel cannot be published unredacted. Once
 * a redacted screenshot exists, this swaps to it everywhere the project
 * appears, without touching a single page.
 *
 * Next's <Image> did the fill positioning and the lazy loading; a plain <img>
 * does both too, with `loading` and `decoding` set explicitly. There is no
 * image optimiser in a static Vite build, so `sizes` is carried through for
 * the day the screenshots are exported at several widths.
 */
export function ProjectFigure({ cover, fallback, fallbackTitle, sizes, priority }: Props) {
  if (!cover) {
    return <Schematic variant={fallback} title={fallbackTitle} className="h-full" />
  }

  return (
    <img
      src={cover.src}
      alt={cover.alt}
      sizes={sizes}
      loading={priority ? 'eager' : 'lazy'}
      decoding={priority ? 'sync' : 'async'}
      fetchPriority={priority ? 'high' : 'auto'}
      // Screenshots are usually wider than 16:10. Anchoring left-top keeps the
      // navigation and the first columns — the part that carries the meaning —
      // and crops the far right, which is where tables run out of content.
      className="absolute inset-0 h-full w-full object-cover object-left-top"
    />
  )
}
