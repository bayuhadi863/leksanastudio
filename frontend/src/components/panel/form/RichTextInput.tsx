import { useEffect } from 'react'
import Link from '@tiptap/extension-link'
import { EditorContent, useEditor, type Editor } from '@tiptap/react'
import StarterKit from '@tiptap/starter-kit'

import { CharacterCount } from '@/components/panel/form/Field'
import { cn } from '@/lib/cn'

/**
 * Rich text, cut down to exactly what the server accepts.
 *
 * Everything StarterKit offers beyond the allow-list is switched off rather than
 * merely hidden: an editor that can produce a heading the sanitiser will strip
 * teaches people that their work disappears at random. What is offered is what
 * survives.
 *
 * Markdown shortcuts stay on, so writing a paragraph here is as fast as writing
 * it in a text file — the objection that usually sinks block editors.
 */
const buildExtensions = () => [
  StarterKit.configure({
    heading: false,
    blockquote: false,
    codeBlock: false,
    horizontalRule: false,
    strike: false,
    // Blocks are the document's structure; a rich-text block is one passage
    // inside it, so it has no business declaring its own sections.
    link: false,
  }),
  Link.configure({
    openOnClick: false,
    autolink: true,
    protocols: ['http', 'https', 'mailto'],
    HTMLAttributes: { rel: 'noopener noreferrer' },
  }),
]

type Props = {
  readonly value: string
  readonly onChange: (html: string) => void
  readonly maxLength: number
  readonly placeholder?: string
  readonly compact?: boolean
  readonly error?: string
  readonly label?: string
  readonly hint?: string
}

export function RichTextInput({
  value,
  onChange,
  maxLength,
  compact,
  error,
  label,
  hint,
}: Props) {
  const editor = useEditor({
    extensions: buildExtensions(),
    content: value,
    editorProps: {
      attributes: {
        class: cn(
          'copy focus:outline-none',
          compact ? 'min-h-20' : 'min-h-40',
        ),
      },
    },
    onUpdate: ({ editor: instance }) => {
      const html = instance.getHTML()
      // Tiptap reports an empty document as an empty paragraph; storing that
      // would make "no content" indistinguishable from "a blank line".
      onChange(html === '<p></p>' ? '' : html)
    },
  })

  // Content set from outside — switching language, loading a draft — has to
  // reach the editor without wiping what someone is currently typing.
  useEffect(() => {
    if (!editor) return
    const current = editor.getHTML()
    const incoming = value || '<p></p>'
    if (current !== incoming) {
      editor.commands.setContent(incoming, { emitUpdate: false })
    }
  }, [editor, value])

  const length = editor?.getText().length ?? 0

  return (
    <div>
      {label ? (
        <div className="flex items-baseline justify-between gap-4">
          <span className="font-semibold">{label}</span>
          <CharacterCount value={length} max={maxLength} />
        </div>
      ) : null}

      {hint ? <p className="type-small text-muted mt-1">{hint}</p> : null}

      <div
        className={cn(
          'bg-surface overflow-hidden rounded-[var(--radius-control)] border transition-colors duration-150 ease-out',
          'focus-within:border-accent focus-within:ring-accent/20 focus-within:ring-3',
          error || length > maxLength ? 'border-danger' : 'border-muted',
          label ? 'mt-2' : '',
        )}
      >
        {editor ? <Toolbar editor={editor} /> : null}

        <div className="px-4 py-3">
          <EditorContent editor={editor} />
        </div>
      </div>

      {!label ? (
        <div className="mt-1 flex justify-end">
          <CharacterCount value={length} max={maxLength} />
        </div>
      ) : null}

      <p className="type-small text-danger min-h-6 pt-1" aria-live="polite">
        {error}
      </p>
    </div>
  )
}

/**
 * Six controls, and no more.
 *
 * Every button here maps to a tag the server keeps. There is no font size, no
 * colour, no alignment — those are the design's decisions, and a toolbar that
 * offered them would be promising something the page will not honour.
 */
function Toolbar({ editor }: { readonly editor: Editor }) {
  const setLink = () => {
    const previous = editor.getAttributes('link').href as string | undefined
    const href = window.prompt('Alamat tautan', previous ?? 'https://')

    if (href === null) return
    if (href === '') {
      editor.chain().focus().extendMarkRange('link').unsetLink().run()
      return
    }

    editor.chain().focus().extendMarkRange('link').setLink({ href }).run()
  }

  return (
    <div className="border-line bg-bg flex flex-wrap items-center gap-0.5 border-b px-2 py-1.5">
      <ToolbarButton
        label="Tebal"
        active={editor.isActive('bold')}
        onClick={() => editor.chain().focus().toggleBold().run()}
      >
        <span className="font-bold">B</span>
      </ToolbarButton>

      <ToolbarButton
        label="Miring"
        active={editor.isActive('italic')}
        onClick={() => editor.chain().focus().toggleItalic().run()}
      >
        <span className="italic">I</span>
      </ToolbarButton>

      <ToolbarButton label="Tautan" active={editor.isActive('link')} onClick={setLink}>
        <span aria-hidden="true">🔗</span>
      </ToolbarButton>

      <span aria-hidden="true" className="bg-line mx-1 h-5 w-px" />

      <ToolbarButton
        label="Daftar bertitik"
        active={editor.isActive('bulletList')}
        onClick={() => editor.chain().focus().toggleBulletList().run()}
      >
        •
      </ToolbarButton>

      <ToolbarButton
        label="Daftar bernomor"
        active={editor.isActive('orderedList')}
        onClick={() => editor.chain().focus().toggleOrderedList().run()}
      >
        1.
      </ToolbarButton>

      <ToolbarButton
        label="Kode sebaris"
        active={editor.isActive('code')}
        onClick={() => editor.chain().focus().toggleCode().run()}
      >
        <span className="font-mono text-xs">{'</>'}</span>
      </ToolbarButton>

      <span className="type-label text-muted ml-auto hidden pr-1 sm:inline">
        **tebal** · - daftar
      </span>
    </div>
  )
}

function ToolbarButton({
  children,
  label,
  active,
  onClick,
}: {
  readonly children: React.ReactNode
  readonly label: string
  readonly active: boolean
  readonly onClick: () => void
}) {
  return (
    <button
      type="button"
      title={label}
      aria-pressed={active}
      onClick={onClick}
      className={cn(
        'flex h-8 min-w-8 items-center justify-center rounded-[3px] px-2 text-[0.9375rem]',
        'transition-colors duration-150 ease-out',
        'focus-visible:outline-accent focus-visible:outline focus-visible:outline-2',
        active ? 'bg-accent-soft text-accent' : 'text-muted hover:text-text hover:bg-surface',
      )}
    >
      {children}
      <span className="sr-only">{label}</span>
    </button>
  )
}
