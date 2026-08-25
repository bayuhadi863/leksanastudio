/**
 * The small slice of the panel API the import script needs.
 *
 * The importer writes through the same endpoints an editor does — no direct
 * database access. That is the point: whatever the server refuses from a person
 * it also refuses from this script, so an import can never plant content the
 * panel would have rejected.
 */

import { readFile } from 'node:fs/promises'
import path from 'node:path'

const DEFAULT_BASE = 'http://localhost:5180/api/v1'

export const apiBase = () => process.env.LEKSANA_API_URL ?? DEFAULT_BASE

/**
 * Credentials, in order of preference: the environment, then the development
 * seeder in appsettings. Nothing is hard-coded here — a password living in a
 * script is a password that eventually lives in a repository.
 */
export async function resolveCredentials(repoRoot) {
  const email = process.env.LEKSANA_ADMIN_EMAIL
  const password = process.env.LEKSANA_ADMIN_PASSWORD
  if (email && password) return { email, password, source: 'environment' }

  const settingsPath = path.join(repoRoot, 'backend', 'src', 'appsettings.Development.json')

  try {
    const settings = JSON.parse(await readFile(settingsPath, 'utf8'))
    const user = settings?.Seeder?.Users?.[0]
    if (user?.Email && user?.Password) {
      return { email: user.Email, password: user.Password, source: 'appsettings.Development.json' }
    }
  } catch {
    // Falls through to the error below, which says what to do about it.
  }

  throw new Error(
    'Kredensial tidak ditemukan. Set LEKSANA_ADMIN_EMAIL dan LEKSANA_ADMIN_PASSWORD, ' +
      'atau jalankan dari repo yang punya backend/src/appsettings.Development.json.',
  )
}

export async function connect(credentials) {
  const base = apiBase()

  const login = await request(`${base}/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ data: { email: credentials.email, password: credentials.password } }),
  })

  const token = login?.data?.accessToken
  if (!token) throw new Error('Login gagal — periksa kredensial.')

  const roles = await request(`${base}/auth/my-roles`, {
    headers: { Authorization: `Bearer ${token}` },
  })

  const roleId = roles?.data?.[0]?.roleId
  if (!roleId) throw new Error('Akun ini tidak punya peran aktif.')

  const headers = {
    Authorization: `Bearer ${token}`,
    'X-Role-Active': roleId,
  }

  const json = (url, options = {}) =>
    request(`${base}${url}`, {
      ...options,
      headers: { ...headers, 'Content-Type': 'application/json', ...(options.headers ?? {}) },
    })

  return {
    base,
    headers,
    json,

    /* -------------------------------------------------------------- media */

    listMedia: async () => {
      const response = await json('/media/get/pagination?page=1&pageSize=100')
      return response?.data?.items ?? []
    },

    uploadMedia: async (filePath, { label, width, height } = {}) => {
      const bytes = await readFile(filePath)
      const name = path.basename(filePath)

      const form = new FormData()
      form.append('file', new Blob([bytes], { type: mimeFor(name) }), name)
      if (width) form.append('width', String(width))
      if (height) form.append('height', String(height))
      if (label) form.append('label', label)

      const response = await request(`${base}/media/upload`, {
        method: 'POST',
        headers,
        body: form,
      })

      return response?.data
    },

    /* --------------------------------------------------------- case study */

    listCaseStudies: async () => {
      const response = await json('/case-study/get/pagination?page=1&pageSize=100')
      return response?.data?.items ?? []
    },

    getCaseStudy: async (id) => {
      const response = await json(`/case-study/get/${id}`)
      return response?.data
    },

    createCaseStudy: async (param) => {
      const response = await json('/case-study/create', {
        method: 'POST',
        body: JSON.stringify({ data: param }),
      })
      return response?.data
    },

    updateCaseStudy: async (id, param) => {
      const response = await json(`/case-study/update/${id}`, {
        method: 'PUT',
        body: JSON.stringify({ data: param }),
      })
      return response?.data
    },
  }
}

/**
 * One request, with the server's own message surfaced on failure.
 *
 * The validation messages are written for editors, so repeating them verbatim
 * is more useful than any wrapper this script could invent.
 */
async function request(url, options) {
  const response = await fetch(url, options)
  const text = await response.text()

  let payload = null
  try {
    payload = text ? JSON.parse(text) : null
  } catch {
    payload = null
  }

  if (!response.ok) {
    const detail = payload?.errors
      ? Object.values(payload.errors).flat().join(' · ')
      : (payload?.message ?? text.slice(0, 200))
    throw new Error(`${response.status} ${url.replace(apiBase(), '')} — ${detail}`)
  }

  return payload
}

const MIME = {
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.jpeg': 'image/jpeg',
  '.gif': 'image/gif',
  '.webp': 'image/webp',
}

const mimeFor = (name) => MIME[path.extname(name).toLowerCase()] ?? 'application/octet-stream'

/**
 * Pixel size, read from the file's own header.
 *
 * Only PNG is decoded, because that is what this site's screenshots are. An
 * unknown format simply reports no dimensions rather than guessing.
 */
export function imageSize(bytes) {
  const isPng =
    bytes.length > 24 &&
    bytes[0] === 0x89 &&
    bytes[1] === 0x50 &&
    bytes[2] === 0x4e &&
    bytes[3] === 0x47

  if (!isPng) return null

  return { width: bytes.readUInt32BE(16), height: bytes.readUInt32BE(20) }
}
