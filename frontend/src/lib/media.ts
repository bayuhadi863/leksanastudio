import { API_BASE_URL } from '@/config/api'

/**
 * Viewable URL for a stored object.
 *
 * Files stream through the API rather than straight from object storage, so the
 * bucket stays private and the URL survives a change of storage provider. The
 * endpoint is anonymous by design — these are images that end up on a public
 * page anyway.
 */
export const mediaUrl = (objectPath: string | null | undefined): string | null =>
  objectPath ? `${API_BASE_URL}/file/download?path=${encodeURIComponent(objectPath)}` : null

const UNITS = ['B', 'KB', 'MB'] as const

/** File size in the shortest honest form. */
export const formatBytes = (bytes: number): string => {
  let value = bytes
  let unit = 0
  while (value >= 1024 && unit < UNITS.length - 1) {
    value /= 1024
    unit += 1
  }
  return `${value >= 10 || unit === 0 ? Math.round(value) : value.toFixed(1)} ${UNITS[unit]}`
}

/**
 * Reads an image's own dimensions in the browser.
 *
 * Sent along with the upload so the server never needs an image codec to learn
 * two integers it will only ever echo back.
 */
export const readImageSize = (file: File): Promise<{ width: number; height: number } | null> =>
  new Promise((resolve) => {
    if (!file.type.startsWith('image/')) {
      resolve(null)
      return
    }

    const url = URL.createObjectURL(file)
    const image = new Image()

    image.onload = () => {
      URL.revokeObjectURL(url)
      resolve({ width: image.naturalWidth, height: image.naturalHeight })
    }
    image.onerror = () => {
      URL.revokeObjectURL(url)
      resolve(null)
    }
    image.src = url
  })

/**
 * What the upload endpoint accepts — mirrors `FileUploadOptions` on the server.
 *
 * Checked here only so a doomed upload fails instantly instead of after a
 * megabyte of transfer. The server checks again, and its answer is the one that
 * counts.
 */
export const ACCEPTED_IMAGE_TYPES = ['image/jpeg', 'image/png', 'image/gif', 'image/webp']
export const MAX_UPLOAD_BYTES = 5 * 1024 * 1024

/** Why this file cannot be uploaded, or null when it can. */
export const rejectUpload = (file: File): string | null => {
  if (!ACCEPTED_IMAGE_TYPES.includes(file.type)) {
    return 'Format tidak didukung. Gunakan JPEG, PNG, GIF, atau WebP.'
  }
  if (file.size > MAX_UPLOAD_BYTES) {
    return `Ukuran maksimal ${formatBytes(MAX_UPLOAD_BYTES)}. Berkas ini ${formatBytes(file.size)}.`
  }
  return null
}
