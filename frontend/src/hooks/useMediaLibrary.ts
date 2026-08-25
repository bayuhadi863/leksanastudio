import { useCallback, useEffect, useState } from 'react'
import { toast } from 'sonner'

import { notifyApiError } from '@/lib/api-error'
import { readImageSize, rejectUpload } from '@/lib/media'
import { mediaRepository } from '@/repositories/MediaRepository'
import type { MediaDTO, MediaPaginationDTO } from '@/types/content'

/** One page is deliberately large: a studio's library is dozens of files, not thousands. */
const PAGE_SIZE = 60

/**
 * The media library as both the picker and the library page need it.
 *
 * One hook rather than two copies, because the two views differ only in
 * chrome — and a picker whose list behaves differently from the library it
 * picks from is a bug waiting for a Monday.
 */
export function useMediaLibrary(enabled = true) {
  const [items, setItems] = useState<MediaPaginationDTO[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<unknown>(null)
  const [search, setSearch] = useState('')
  const [uploading, setUploading] = useState(false)

  const load = useCallback(async () => {
    setIsLoading(true)
    setError(null)
    try {
      const response = await mediaRepository.getPagination({
        page: 1,
        pageSize: PAGE_SIZE,
        search: search || undefined,
        sortBy: 'createdDate',
        sortOrder: 'desc',
      })
      setItems(response.data?.items ?? [])
    } catch (caught) {
      setError(caught)
    } finally {
      setIsLoading(false)
    }
  }, [search])

  useEffect(() => {
    if (!enabled) return
    const timer = window.setTimeout(() => void load(), search ? 250 : 0)
    return () => window.clearTimeout(timer)
  }, [enabled, load, search])

  /**
   * Uploads a batch, one request per file.
   *
   * Every file is judged before it is sent — the browser knows the size and the
   * type already, and failing after a megabyte of transfer teaches nothing the
   * client could not have been told immediately.
   */
  const upload = useCallback(async (files: readonly File[]): Promise<MediaDTO[]> => {
    if (files.length === 0) return []

    setUploading(true)
    const uploaded: MediaDTO[] = []

    try {
      for (const file of files) {
        const rejection = rejectUpload(file)
        if (rejection) {
          toast.error(`${file.name}: ${rejection}`)
          continue
        }

        try {
          const size = await readImageSize(file)
          const media = await mediaRepository.upload(file, {
            width: size?.width,
            height: size?.height,
          })
          uploaded.push(media)
        } catch (caught) {
          notifyApiError(caught, `Gagal mengunggah ${file.name}.`)
        }
      }
    } finally {
      setUploading(false)
    }

    if (uploaded.length > 0) {
      // Prepend rather than refetch: the newest file is what the person is
      // looking for, and it should be there the instant the request returns.
      setItems((current) => [
        ...uploaded.map((media, index) => ({ ...media, number: index + 1 })),
        ...current,
      ])
      toast.success(
        uploaded.length === 1 ? 'Berkas terunggah' : `${uploaded.length} berkas terunggah`,
      )
    }

    return uploaded
  }, [])

  const removeLocally = useCallback((id: string) => {
    setItems((current) => current.filter((item) => item.id !== id))
  }, [])

  const patchLocally = useCallback((id: string, patch: Partial<MediaPaginationDTO>) => {
    setItems((current) => current.map((item) => (item.id === id ? { ...item, ...patch } : item)))
  }, [])

  return {
    items,
    isLoading,
    error,
    search,
    setSearch,
    reload: load,
    upload,
    uploading,
    removeLocally,
    patchLocally,
  }
}
