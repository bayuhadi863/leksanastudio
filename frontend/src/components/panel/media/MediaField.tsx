import { useState } from 'react'

import { MediaPicker } from '@/components/panel/media/MediaPicker'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'
import { mediaUrl } from '@/lib/media'
import type { MediaDTO } from '@/types/content'

type Props = {
  readonly label: string
  readonly hint?: string
  readonly error?: string
  readonly mediaId: string | null
  /** Stored path of the current image, so the preview works before anything is picked. */
  readonly objectPath: string | null
  readonly onChange: (media: MediaDTO | null) => void
  readonly aspect?: string
}

/**
 * One image, chosen from the library.
 *
 * The preview is the control: it shows what will actually appear on the site,
 * at the shape it will appear in. A field that says "cover.jpg" and nothing else
 * is how the wrong picture ends up published.
 */
export function MediaField({
  label,
  hint,
  error,
  mediaId,
  objectPath,
  onChange,
  aspect = 'aspect-[16/10]',
}: Props) {
  const [picking, setPicking] = useState(false)
  const url = mediaUrl(objectPath)

  return (
    <div>
      <p className="font-semibold">{label}</p>
      {hint ? <p className="type-small text-muted mt-1">{hint}</p> : null}

      <div className="mt-2 flex flex-wrap items-start gap-5">
        <button
          type="button"
          onClick={() => setPicking(true)}
          className={cn(
            'bg-surface w-56 overflow-hidden rounded-[var(--radius-control)] border transition-colors duration-150 ease-out',
            'focus-visible:outline-accent focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2',
            error ? 'border-danger' : 'border-line hover:border-accent',
          )}
        >
          {url ? (
            <img src={url} alt="" className={cn('w-full object-cover', aspect)} />
          ) : (
            <span
              className={cn('text-muted flex w-full items-center justify-center', aspect)}
            >
              <span className="type-label">Belum ada gambar</span>
            </span>
          )}
        </button>

        <div className="flex flex-col gap-2">
          <button
            type="button"
            onClick={() => setPicking(true)}
            className="text-accent w-fit font-semibold"
          >
            {mediaId ? 'Ganti gambar' : 'Pilih gambar'}
          </button>

          {mediaId ? (
            <button
              type="button"
              onClick={() => onChange(null)}
              className="text-muted hover:text-danger w-fit"
            >
              Hapus gambar
            </button>
          ) : null}
        </div>
      </div>

      <p className="type-small text-danger mt-1.5 min-h-5" role={error ? 'alert' : undefined}>
        {error ?? ''}
      </p>

      <MediaPicker
        open={picking}
        selectedId={mediaId}
        onClose={() => setPicking(false)}
        onSelect={onChange}
      />
    </div>
  )
}

/** The same control, minus the field furniture — for use inside a block card. */
export function MediaPickerButton({
  mediaId,
  objectPath,
  onChange,
}: {
  readonly mediaId: string | null
  readonly objectPath: string | null
  readonly onChange: (media: MediaDTO | null) => void
}) {
  const [picking, setPicking] = useState(false)
  const url = mediaUrl(objectPath)

  return (
    <div className="flex flex-wrap items-center gap-4">
      <button
        type="button"
        onClick={() => setPicking(true)}
        className="border-line bg-surface hover:border-accent focus-visible:outline-accent w-32 overflow-hidden rounded-[var(--radius-control)] border transition-colors duration-150 ease-out focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2"
      >
        {url ? (
          <img src={url} alt="" className="aspect-[4/3] w-full object-cover" />
        ) : (
          <span className="text-muted flex aspect-[4/3] w-full items-center justify-center">
            <Label as="span">Skema</Label>
          </span>
        )}
      </button>

      <div className="flex flex-col gap-1.5">
        <button
          type="button"
          onClick={() => setPicking(true)}
          className="text-accent w-fit font-semibold"
        >
          {mediaId ? 'Ganti tangkapan layar' : 'Pakai tangkapan layar'}
        </button>

        {mediaId ? (
          <button
            type="button"
            onClick={() => onChange(null)}
            className="text-muted hover:text-danger type-small w-fit"
          >
            Kembali ke skema
          </button>
        ) : (
          <p className="type-small text-muted max-w-xs">
            Selama belum ada gambar asli, skema di atas yang tampil.
          </p>
        )}
      </div>

      <MediaPicker
        open={picking}
        selectedId={mediaId}
        onClose={() => setPicking(false)}
        onSelect={onChange}
        title="Pilih tangkapan layar"
      />
    </div>
  )
}
