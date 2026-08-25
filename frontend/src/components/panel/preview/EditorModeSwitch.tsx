import { cn } from '@/lib/cn'
import type { EditorMode } from '@/hooks/useEditorMode'

const OPTIONS: readonly {
  readonly value: EditorMode
  readonly label: string
  readonly splitOnly?: boolean
}[] = [
  { value: 'tulis', label: 'Tulis' },
  { value: 'belah', label: 'Tulis + pratinjau', splitOnly: true },
  { value: 'pratinjau', label: 'Pratinjau' },
]

export function EditorModeSwitch({
  mode,
  onChange,
  canSplit,
}: {
  readonly mode: EditorMode
  readonly onChange: (mode: EditorMode) => void
  readonly canSplit: boolean
}) {
  const options = OPTIONS.filter((option) => canSplit || !option.splitOnly)

  return (
    <fieldset>
      <legend className="sr-only">Tampilan penyunting</legend>

      <div className="border-line flex rounded-[var(--radius-control)] border">
        {options.map((option) => (
          <label
            key={option.value}
            className={cn(
              'type-label flex min-h-11 cursor-pointer items-center px-3.5 whitespace-nowrap',
              'transition-colors duration-150 ease-out',
              'first:rounded-l-[3px] last:rounded-r-[3px]',
              'has-[:focus-visible]:outline has-[:focus-visible]:outline-2',
              'has-[:focus-visible]:outline-accent has-[:focus-visible]:outline-offset-2',
              mode === option.value ? 'bg-accent-soft text-accent' : 'text-muted hover:text-text',
            )}
          >
            <input
              type="radio"
              name="editor-mode"
              value={option.value}
              checked={mode === option.value}
              onChange={() => onChange(option.value)}
              className="sr-only"
            />
            {option.label}
          </label>
        ))}
      </div>
    </fieldset>
  )
}
