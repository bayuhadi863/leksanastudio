import { useEffect, useRef, useState } from 'react'

import { ErrorState } from '@/components/panel/ErrorState'
import { MediaGrid } from '@/components/panel/media/MediaGrid'
import { UploadZone } from '@/components/panel/media/UploadZone'
import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { useMediaLibrary } from '@/hooks/useMediaLibrary'
import type { MediaDTO } from '@/types/content'

type Props = {
  readonly open: boolean
  readonly onClose: () => void
  readonly onSelect: (media: MediaDTO) => void
  readonly selectedId?: string | null
  readonly title?: string
}

/**
 * Choosing an image, without leaving the form.
 *
 * A dialog rather than a separate page, because picking a cover is a step
 * inside writing a case study — sending someone to the library and back would
 * mean saving a half-finished draft first, or losing it.
 *
 * Upload lives inside the picker too. The file an editor wants is usually the
 * one still sitting on their desktop.
 */
export function MediaPicker({
  open,
  onClose,
  onSelect,
  selectedId,
  title = 'Pilih gambar',
}: Props) {
  const dialogRef = useRef<HTMLDialogElement>(null)
  const library = useMediaLibrary(open)
  const [picked, setPicked] = useState<MediaDTO | null>(null)

  useEffect(() => {
    const dialog = dialogRef.current
    if (!dialog) return

    if (open && !dialog.open) dialog.showModal()
    if (!open && dialog.open) dialog.close()
  }, [open])

  useEffect(() => {
    if (open) setPicked(null)
  }, [open])

  const confirm = (media: MediaDTO) => {
    onSelect(media)
    onClose()
  }

  return (
    <dialog
      ref={dialogRef}
      onCancel={(event) => {
        event.preventDefault()
        onClose()
      }}
      onClick={(event) => {
        if (event.target === dialogRef.current) onClose()
      }}
      // No display utility on the dialog itself: an author-level `display`
      // beats the user agent's `dialog:not([open]) { display: none }`, and the
      // panel would render its picker permanently open. The flex column lives
      // one level in.
      className="bg-bg border-line text-text m-auto w-[min(62rem,calc(100vw-2rem))] overflow-hidden rounded-[var(--radius-control)] border p-0 backdrop:bg-[color-mix(in_oklab,var(--color-text)_45%,transparent)]"
    >
      <div className="flex max-h-[min(44rem,calc(100vh-4rem))] flex-col">
        <header className="border-line flex items-baseline justify-between gap-6 border-b px-6 py-5">
          <div>
            <Label as="p">Berkas</Label>
            <h2 className="type-h3 mt-1.5">{title}</h2>
          </div>

          <button
            type="button"
            onClick={onClose}
            className="text-muted hover:text-accent type-label"
          >
            Tutup
          </button>
        </header>

        <div className="min-h-0 flex-1 overflow-y-auto px-6 py-5">
          <div className="flex flex-wrap items-center gap-4">
            <div className="min-w-56 flex-1">
              <label htmlFor="media-picker-search" className="sr-only">
                Cari berkas
              </label>
              <input
                id="media-picker-search"
                type="search"
                value={library.search}
                onChange={(event) => library.setSearch(event.target.value)}
                placeholder="Cari nama berkas atau keterangan…"
                className="border-muted bg-surface focus:border-accent focus:ring-accent/20 w-full rounded-[var(--radius-control)] border px-4 py-2.5 transition-colors duration-150 ease-out focus:ring-3 focus:outline-none"
              />
            </div>

            <UploadZone
              compact
              busy={library.uploading}
              className="min-w-64 flex-1"
              onFiles={(files) => {
                void library.upload(files).then((uploaded) => {
                  // The file someone just chose is almost certainly the one they
                  // came for: select it, and leave confirming to them.
                  const first = uploaded[0]
                  if (first) setPicked(first)
                })
              }}
            />
          </div>

          <div className="mt-6">
            {library.error ? (
              <ErrorState
                error={library.error}
                title="Gagal memuat berkas"
                onRetry={() => void library.reload()}
              />
            ) : library.isLoading ? (
              <ul className="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
                {Array.from({ length: 8 }, (_, index) => (
                  <li key={index} className="animate-pulse">
                    <span className="bg-surface border-line block aspect-[4/3] rounded-[var(--radius-control)] border" />
                    <span className="bg-surface mt-2 block h-4 w-2/3 rounded" />
                  </li>
                ))}
              </ul>
            ) : library.items.length === 0 ? (
              <div className="border-line border-t py-10 text-center">
                <p className="text-muted mx-auto max-w-md">
                  {library.search
                    ? 'Tidak ada berkas yang cocok dengan pencarian itu.'
                    : 'Belum ada berkas. Unggah tangkapan layar pertama lewat kotak di atas.'}
                </p>
              </div>
            ) : (
              <MediaGrid
                items={library.items}
                selectedId={picked?.id ?? selectedId ?? null}
                onPick={(item) => setPicked(item)}
              />
            )}
          </div>
        </div>

        <footer className="border-line flex flex-wrap items-center justify-between gap-4 border-t px-6 py-4">
          <p className="type-small text-muted min-w-0 truncate">
            {picked ? picked.label || picked.originalName : 'Belum ada yang dipilih'}
          </p>

          <div className="flex gap-3">
            <Button type="button" variant="ghost" onClick={onClose}>
              Batal
            </Button>
            <Button type="button" disabled={!picked} onClick={() => picked && confirm(picked)}>
              Gunakan gambar
            </Button>
          </div>
        </footer>
      </div>
    </dialog>
  )
}
