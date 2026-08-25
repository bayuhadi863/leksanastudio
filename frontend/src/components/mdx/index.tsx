import type { MDXComponents } from 'mdx/types'
import type { ComponentPropsWithoutRef } from 'react'

import { Decision, Figure, Metrics } from '@/components/content/ContentBlocks'
import { Annotation } from '@/components/layout/Annotation'
import { AppLink } from '@/components/ui/AppLink'
import { Label } from '@/components/ui/Label'
import { Note } from '@/components/ui/Note'

/*
 * The composite blocks moved to `content/ContentBlocks`: the database renders
 * the same three pieces now, and one definition is what keeps the panel's
 * preview honest about what will be published.
 */

/* --------------------------------------------------------- element mapping */

const Anchor = ({ href = '', ...rest }: ComponentPropsWithoutRef<'a'>) => (
  <AppLink href={href} {...rest} />
)

/** Tables can be wider than the measure; they scroll inside their own box. */
const Table = (props: ComponentPropsWithoutRef<'table'>) => (
  <div className="scroll-x document-wide my-8">
    <table {...props} />
  </div>
)

export const mdxComponents: MDXComponents = {
  a: Anchor,
  table: Table,
  Note,
  Annotation,
  Decision,
  Figure,
  Metrics,
  Label,
}
