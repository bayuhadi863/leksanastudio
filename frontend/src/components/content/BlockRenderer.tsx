import { Decision, Figure, Metrics } from '@/components/content/ContentBlocks'
import { Note } from '@/components/ui/Note'
import { mediaUrl } from '@/lib/media'
import type { Block, RichTextBlock } from '@/types/blocks'

/**
 * Blocks, drawn the way the site draws them.
 *
 * The same components the published page uses — not a stand-in that looks
 * roughly similar. A preview whose margin note sits somewhere else, or whose
 * decision block is styled by hand, teaches an editor to distrust it, and a
 * preview nobody trusts is worse than none: they publish and check anyway.
 *
 * Rich text arrives as HTML that the server sanitises against a closed
 * allow-list on every write, and that the editor itself produced in the
 * browser. It is inserted directly for that reason — there is no path here
 * that carries markup from anywhere else.
 */
export function BlockRenderer({
  blocks,
  /** Marks each rendered block so the panel can scroll to the one being edited. */
  anchors,
}: {
  readonly blocks: readonly Block[]
  readonly anchors?: boolean
}) {
  return (
    <>
      {blocks.map((block) => (
        <div key={block.id} data-preview-block={anchors ? block.id : undefined}>
          <BlockView block={block} />
        </div>
      ))}
    </>
  )
}

function BlockView({ block }: { readonly block: Block }) {
  switch (block.type) {
    case 'richText':
      return <RichText block={block} />

    case 'heading':
      return block.level === 2 ? (
        <h2 className="type-h2 mt-12 first:mt-0">{block.text}</h2>
      ) : (
        <h3 className="type-h3 mt-10 first:mt-0">{block.text}</h3>
      )

    case 'decision':
      return (
        <Decision
          step={block.step}
          title={block.title}
          chose={block.chose}
          because={block.because}
          despite={block.despite}
        >
          {(block.body ?? []).map((child) => (
            <RichText key={child.id} block={child} />
          ))}
        </Decision>
      )

    case 'figure':
      return (
        <Figure
          variant={block.variant}
          alt={block.alt}
          caption={block.caption}
          src={mediaUrl(block.src) ?? undefined}
        />
      )

    case 'metrics':
      return <Metrics items={block.items} />

    case 'note':
      return <Note>{<div dangerouslySetInnerHTML={{ __html: block.html }} />}</Note>

    case 'codeBlock':
      return (
        <pre>
          <code>{block.code}</code>
        </pre>
      )

    case 'table':
      return (
        <div className="scroll-x document-wide my-8">
          <table>
            <thead>
              <tr>
                {block.head.map((cell, index) => (
                  <th key={index} scope="col">
                    {cell}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {block.rows.map((row, rowIndex) => (
                <tr key={rowIndex}>
                  {row.map((cell, cellIndex) => (
                    <td key={cellIndex}>{cell}</td>
                  ))}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )
  }
}

function RichText({ block }: { readonly block: RichTextBlock }) {
  return <div dangerouslySetInnerHTML={{ __html: block.html }} />
}
