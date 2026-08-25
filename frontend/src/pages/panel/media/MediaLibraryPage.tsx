import { useEffect, useRef, useState } from 'react'
import { toast } from 'sonner'

import { ConfirmDialog } from '@/components/panel/ConfirmDialog'
import { ErrorState } from '@/components/panel/ErrorState'
import { PanelPage } from '@/components/panel/PanelPage'
import { MediaGrid } from '@/components/panel/media/MediaGrid'
import { UploadZone } from '@/components/panel/media/UploadZone'
import { TextInput } from '@/components/panel/form/Inputs'
import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { menuCodes, panelRoutes } from '@/config/panel'
import { useMediaLibrary } from '@/hooks/useMediaLibrary'
import { usePermission } from '@/hooks/usePermission'
import { notifyApiError } from '@/lib/api-error'
import { formatDate } from '@/lib/format'
import { formatBytes, mediaUrl } from '@/lib/media'
import { usePageMeta } from '@/lib/seo'
import { mediaRepository } from '@/repositories/MediaRepository'
import type { MediaPaginationDTO } from '@/types/content'

/**
 * Every file the site uses, in one place.
 *
 * Uploads live here and in the picker both — an editor who thinks "I should put
 * that screenshot somewhere" and an editor who thinks "this case study needs a
 * cover" are the same person on different days, and neither should have to
 * learn the other's route.
 */
export function MediaLibraryPage() {
  usePageMeta({
    title: 'Berkas & Gambar',
    description: 'Kelola berkas yang dipakai di situs.',
    path: panelRoutes.media,
    noIndex: true,
  })

  const permission = usePermission(menuCodes.media)
  const library = useMediaLibrary(true)
  const [detail, setDetail] = useState<MediaPaginationDTO | null>(null)

  return (
    <PanelPage
      eyebrow="Konten"
      title="Berkas & Gambar"
      lead="Tangkapan layar, sampul studi kasus, dan gambar lain. Satu berkas bisa dipakai di banyak halaman."
      wide
    >
      {permission.canCreate ? (
        <UploadZone busy={library.uploading} onFiles={(files) => void library.upload(files)} />
      ) : null}

      <div className="mt-8 flex flex-wrap items-center justify-between gap-4">
        <div className="min-w-56 flex-1 sm:max-w-sm">
          <label htmlFor="media-search" className="sr-only">
            Cari berkas
          </label>
          <input
            id="media-search"
            type="search"
            value={library.search}
            onChange={(event) => library.setSearch(event.target.value)}
            placeholder="Cari nama berkas atau keterangan…"
            className="border-muted bg-surface focus:border-accent focus:ring-accent/20 w-full rounded-[var(--radius-control)] border px-4 py-2.5 transition-colors duration-150 ease-out focus:ring-3 focus:outline-none"
          />
        </div>

        <Label as="p" className="text-muted">
          {library.isLoading ? 'Memuat…' : `${library.items.length} berkas`}
        </Label>
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
          <div className="border-line border-t py-12 text-center">
            <h2 className="type-h3">
              {library.search ? 'Tidak ada yang cocok' : 'Belum ada berkas'}
            </h2>
            <p className="text-muted mx-auto mt-3 max-w-md">
              {library.search
                ? 'Coba kata kunci lain, atau kosongkan pencariannya.'
                : 'Unggah tangkapan layar proyek di sini, lalu pakai berkasnya saat menulis studi kasus.'}
            </p>
          </div>
        ) : (
          <MediaGrid items={library.items} onPick={(item) => setDetail(item)} />
        )}
      </div>

      <MediaDetailDialog
        item={detail}
        canUpdate={permission.canUpdate}
        canDelete={permission.canDelete}
        onClose={() => setDetail(null)}
        onSaved={(id, label) => library.patchLocally(id, { label })}
        onDeleted={(id) => {
          library.removeLocally(id)
          setDetail(null)
        }}
      />
    </PanelPage>
  )
}

/**
 * One file, up close.
 *
 * The label is the only editable thing — replacing a file means uploading a new
 * one, so that the pages already pointing at this path keep showing what their
 * editor approved.
 */
function MediaDetailDialog({
  item,
  canUpdate,
  canDelete,
  onClose,
  onSaved,
  onDeleted,
}: {
  readonly item: MediaPaginationDTO | null
  readonly canUpdate: boolean
  readonly canDelete: boolean
  readonly onClose: () => void
  readonly onSaved: (id: string, label: string | null) => void
  readonly onDeleted: (id: string) => void
}) {
  const dialogRef = useRef<HTMLDialogElement>(null)
  const [label, setLabel] = useState('')
  const [saving, setSaving] = useState(false)
  const [confirming, setConfirming] = useState(false)
  const [deleting, setDeleting] = useState(false)

  useEffect(() => {
    const dialog = dialogRef.current
    if (!dialog) return

    if (item && !dialog.open) dialog.showModal()
    if (!item && dialog.open) dialog.close()
    if (item) setLabel(item.label ?? '')
  }, [item])

  const save = async () => {
    if (!item) return
    setSaving(true)
    try {
      const next = label.trim() || null
      await mediaRepository.update(item.id, { data: { label: next } })
      onSaved(item.id, next)
      toast.success('Keterangan tersimpan')
      onClose()
    } catch (error) {
      notifyApiError(error, 'Gagal menyimpan keterangan.')
    } finally {
      setSaving(false)
    }
  }

  const remove = async () => {
    if (!item) return
    setDeleting(true)
    try {
      await mediaRepository.remove(item.id)
      toast.success('Berkas dihapus')
      onDeleted(item.id)
    } catch (error) {
      notifyApiError(error, 'Gagal menghapus berkas.')
    } finally {
      setDeleting(false)
      setConfirming(false)
    }
  }

  const url = item ? mediaUrl(item.objectPath) : null

  return (
    <>
      <dialog
        ref={dialogRef}
        onCancel={(event) => {
          event.preventDefault()
          onClose()
        }}
        onClick={(event) => {
          if (event.target === dialogRef.current) onClose()
        }}
        className="bg-bg border-line text-text m-auto w-[min(48rem,calc(100vw-2rem))] rounded-[var(--radius-control)] border p-0 backdrop:bg-[color-mix(in_oklab,var(--color-text)_45%,transparent)]"
      >
        {item ? (
          <div className="grid gap-0 sm:grid-cols-[1fr_1.1fr]">
            <div className="bg-surface border-line border-b sm:border-r sm:border-b-0">
              <img src={url ?? ''} alt="" className="aspect-[4/3] w-full object-contain" />
            </div>

            <div className="p-6">
              <Label as="p">Berkas</Label>
              <h2 className="type-h3 mt-1.5 break-all">{item.originalName}</h2>

              <dl className="type-small text-muted mt-4 grid grid-cols-2 gap-x-4 gap-y-1.5">
                <dt>Ukuran</dt>
                <dd className="numeric text-text">{formatBytes(item.sizeBytes)}</dd>
                <dt>Dimensi</dt>
                <dd className="numeric text-text">
                  {item.width && item.height ? `${item.width}×${item.height}` : '—'}
                </dd>
                <dt>Jenis</dt>
                <dd className="text-text">{item.mime}</dd>
                <dt>Diunggah</dt>
                <dd className="text-text">{formatDate(item.createdDate)}</dd>
              </dl>

              <div className="mt-6">
                <TextInput
                  label="Keterangan"
                  hint="Nama yang mudah dikenali saat memilih berkas. Bukan deskripsi gambar."
                  value={label}
                  onChange={setLabel}
                  disabled={!canUpdate}
                  maxLength={120}
                  placeholder="Dasbor panel P3M"
                />
              </div>

              <div className="mt-6 flex flex-wrap items-center justify-between gap-3">
                {canDelete ? (
                  <button
                    type="button"
                    onClick={() => setConfirming(true)}
                    className="text-muted hover:text-danger"
                  >
                    Hapus berkas
                  </button>
                ) : (
                  <span />
                )}

                <div className="flex gap-3">
                  <Button type="button" variant="ghost" onClick={onClose}>
                    Tutup
                  </Button>
                  {canUpdate ? (
                    <Button
                      type="button"
                      onClick={() => void save()}
                      disabled={saving || (item.label ?? '') === label.trim()}
                    >
                      {saving ? 'Menyimpan…' : 'Simpan'}
                    </Button>
                  ) : null}
                </div>
              </div>
            </div>
          </div>
        ) : null}
      </dialog>

      <ConfirmDialog
        open={confirming}
        title="Hapus berkas ini?"
        body="Halaman yang memakai berkas ini akan kehilangan gambarnya. Tindakan ini tidak bisa dibatalkan."
        busy={deleting}
        onCancel={() => setConfirming(false)}
        onConfirm={() => void remove()}
      />
    </>
  )
}
