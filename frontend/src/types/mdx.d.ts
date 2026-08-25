declare module '*.mdx' {
  import type { MDXProps } from 'mdx/types'
  import type { JSX } from 'react'

  /** Compiled by @mdx-js/rollup. Frontmatter is exported by remark-mdx-frontmatter. */
  export const frontmatter: Record<string, unknown>

  const MDXContent: (props: MDXProps) => JSX.Element
  export default MDXContent
}
