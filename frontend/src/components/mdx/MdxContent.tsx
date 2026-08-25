import { mdxComponents } from '@/components/mdx'
import type { MdxComponent } from '@/lib/content'

type Props = {
  /** Compiled MDX, handed over by lib/content. */
  readonly component: MdxComponent
}

/**
 * Renders a compiled MDX document with the house component mapping.
 *
 * Compilation happens at build time, in Rollup — no MDX compiler and no source
 * text ever ships to the browser.
 */
export function MdxContent({ component: Content }: Props) {
  return <Content components={mdxComponents} />
}
