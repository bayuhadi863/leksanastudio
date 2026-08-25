import { useRef, useState } from 'react'

import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'
import { ACCEPTED_IMAGE_TYPES, MAX_UPLOAD_BYTES, formatBytes } from '@/lib/media'

type Props = {
  readonly onFiles: (files: File[]) => void
  readonly busy?: boolean
  readonly compact?: boolean
  readonly className?: string
}

/**
 * Drop a file, or click to choose one.
 *
 * Both, always: dragging is faster for someone with a folder open, and
 * impossible for someone on a phone. The hidden input is a real input, so the
 * keyboard path is the same one everything else in the panel uses.
 */
export function UploadZone({ onFiles, busy, compact, className }: Props) {
  const inputRef = useRef<HTMLInputElement>(null)
  const [dragging, setDragging] = useState(false)

  const accept = (list: FileList | null) => {
    const files = Array.from(list ?? [])
    if (files.length > 0) onFiles(files)
  }

  return (
    <div
      onDragOver={(event) => {
        event.preventDefault()
        setDragging(true)
      }}
      onDragLeave={() => setDragging(false)}
      onDrop={(event) => {
        event.preventDefault()
        setDragging(false)
        if (!busy) accept(event.dataTransfer.files)
      }}
      className={cn(
        'rounded-[var(--radius-control)] border border-dashed text-center transition-colors duration-150 ease-out',
        compact ? 'px-4 py-5' : 'px-6 py-10',
        dragging ? 'border-accent bg-accent-soft' : 'border-muted',
        busy && 'opacity-60',
        className,
      )}
    >
      <input
        ref={inputRef}
        type="file"
        multiple
        accept={ACCEPTED_IMAGE_TYPES.join(',')}
        className="sr-only"
        disabled={busy}
        onChange={(event) => {
          accept(event.target.files)
          // Clearing lets the same file be chosen twice in a row — which is
          // exactly what happens after a failed upload.
          event.target.value = ''
        }}
      />

      <button
        type="button"
        disabled={busy}
        onClick={() => inputRef.current?.click()}
        className="text-accent font-semibold disabled:cursor-not-allowed"
      >
        {busy ? 'Mengunggah…' : 'Pilih berkas'}
      </button>

      <span className="text-muted"> atau seret ke sini</span>

      {compact ? null : (
        <Label as="p" className="text-muted mt-3">
          JPEG, PNG, GIF, atau WebP — maksimal {formatBytes(MAX_UPLOAD_BYTES)}
        </Label>
      )}
    </div>
  )
}
