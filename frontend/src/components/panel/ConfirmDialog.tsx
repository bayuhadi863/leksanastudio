import { useEffect, useRef } from 'react'

import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'

type Props = {
  readonly open: boolean
  readonly title: string
  readonly body: string
  readonly confirmLabel?: string
  readonly onConfirm: () => void
  readonly onCancel: () => void
  readonly busy?: boolean
}

/**
 * The one moment the panel interrupts.
 *
 * Reserved for actions that destroy something. It says what will happen in
 * plain words rather than asking "are you sure?" — a question nobody has ever
 * answered thoughtfully.
 *
 * Built on the native dialog element: focus trapping, Escape, and inertness on
 * the rest of the page come from the platform rather than from code that has to
 * be maintained.
 */
export function ConfirmDialog({
  open,
  title,
  body,
  confirmLabel = 'Hapus',
  onConfirm,
  onCancel,
  busy,
}: Props) {
  const dialogRef = useRef<HTMLDialogElement>(null)

  useEffect(() => {
    const dialog = dialogRef.current
    if (!dialog) return

    if (open && !dialog.open) dialog.showModal()
    if (!open && dialog.open) dialog.close()
  }, [open])

  return (
    <dialog
      ref={dialogRef}
      onCancel={(event) => {
        event.preventDefault()
        onCancel()
      }}
      onClick={(event) => {
        // Clicking the backdrop lands on the dialog element itself.
        if (event.target === dialogRef.current) onCancel()
      }}
      className="bg-bg border-line text-text m-auto w-[min(28rem,calc(100vw-3rem))] rounded-[var(--radius-control)] border p-0 backdrop:bg-[color-mix(in_oklab,var(--color-text)_45%,transparent)]"
    >
      <div className="p-6">
        <Label as="p" className="text-danger">
          Konfirmasi
        </Label>
        <h2 className="type-h3 mt-3">{title}</h2>
        <p className="text-muted mt-3">{body}</p>

        <div className="mt-8 flex flex-wrap justify-end gap-3">
          <Button type="button" variant="ghost" onClick={onCancel} disabled={busy}>
            Batal
          </Button>
          <Button type="button" onClick={onConfirm} disabled={busy}>
            {busy ? 'Menghapus…' : confirmLabel}
          </Button>
        </div>
      </div>
    </dialog>
  )
}
