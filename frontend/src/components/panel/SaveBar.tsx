import type { ReactNode } from 'react'
import { useEffect } from 'react'

import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

type Props = {
  readonly dirty: boolean
  readonly saving: boolean
  readonly onSave: () => void
  readonly onCancel?: () => void
  readonly saveLabel?: string
  /** Extra controls — publish, preview — shown to the left of save. */
  readonly children?: ReactNode
}

/**
 * The bar that never scrolls away.
 *
 * A long content form has its save button a page and a half below the field
 * someone just edited; pinning it removes the single most common source of lost
 * work. It also states plainly whether there is anything to save, so "did that
 * go through?" stops being a question.
 */
export function SaveBar({ dirty, saving, onSave, onCancel, saveLabel = 'Simpan', children }: Props) {
  // The browser's own guard is the only one that survives a tab close, so it is
  // worth wiring even though the router guard covers in-app navigation.
  useEffect(() => {
    if (!dirty) return

    const onBeforeUnload = (event: BeforeUnloadEvent) => {
      event.preventDefault()
      event.returnValue = ''
    }

    window.addEventListener('beforeunload', onBeforeUnload)
    return () => window.removeEventListener('beforeunload', onBeforeUnload)
  }, [dirty])

  return (
    <div
      data-save-bar
      className="border-line bg-bg/95 sticky bottom-0 z-30 -mx-5 mt-8 border-t px-5 py-2.5 backdrop-blur lg:-mx-7 lg:px-7"
    >
      <div className="mx-auto flex max-w-6xl flex-wrap items-center justify-between gap-4">
        <Label as="p" className={cn(dirty ? 'text-accent' : 'text-muted')}>
          {saving ? 'Menyimpan…' : dirty ? 'Ada perubahan belum disimpan' : 'Tersimpan'}
        </Label>

        <div className="flex flex-wrap items-center gap-3">
          {children}

          {onCancel ? (
            <Button type="button" variant="ghost" onClick={onCancel} disabled={saving}>
              Batal
            </Button>
          ) : null}

          <Button type="button" onClick={onSave} disabled={saving || !dirty}>
            {saving ? 'Menyimpan…' : saveLabel}
          </Button>
        </div>
      </div>
    </div>
  )
}
