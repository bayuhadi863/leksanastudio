import type { BasePaginationParam } from '@/types/api'
import type { Block } from '@/types/blocks'

export type ContentStatus = 'Draft' | 'Published'

export type CaseStudyLabel = 'Client' | 'OwnProduct'
export type SchematicVariant = 'System' | 'Website' | 'Catalog'

export interface LocaleDTO {
  id: string
  code: string
  name: string
  nativeName: string
  isDefault: boolean
  isActive: boolean
  order: number
}

/** How far along one language is for one entry. Drives the status chips in every list. */
export interface TranslationSummary {
  localeCode: string
  title: string | null
  slug: string | null
  status: ContentStatus
  publishedAt: string | null
}

export interface CaseStudyTranslationDTO {
  id: string
  localeCode: string
  slug: string | null
  status: ContentStatus
  publishedAt: string | null
  updatedDate: string | null
  updatedBy: string | null
  title: string | null
  summary: string | null
  problem: string | null
  client: string | null
  kind: string | null
  duration: string | null
  role: string | null
  coverAlt: string | null
  metrics: { value: string; label: string }[] | null
  body: Block[] | null
}

export interface CaseStudyDTO {
  id: string
  contentKey: string | null
  label: CaseStudyLabel
  figure: SchematicVariant
  coverMediaId: string | null
  coverMediaPath: string | null
  year: number
  stack: string[] | null
  order: number
  createdDate: string
  updatedDate: string | null
  createdBy: string
  updatedBy: string | null
  translations: CaseStudyTranslationDTO[]
}

export interface CaseStudyTranslationParam {
  localeCode: string
  slug?: string | null
  status: ContentStatus
  title?: string | null
  summary?: string | null
  problem?: string | null
  client?: string | null
  kind?: string | null
  duration?: string | null
  role?: string | null
  coverAlt?: string | null
  metrics?: { value: string; label: string }[] | null
  body?: Block[] | null
}

export interface CaseStudyParam {
  contentKey?: string | null
  label: CaseStudyLabel
  figure: SchematicVariant
  coverMediaId?: string | null
  year: number
  stack?: string[] | null
  order: number
  translations: CaseStudyTranslationParam[]
}

export interface CaseStudyPaginationDTO {
  number: number
  id: string
  order: number
  title: string | null
  slug: string | null
  label: CaseStudyLabel
  year: number
  coverMediaId: string | null
  coverMediaPath: string | null
  translations: TranslationSummary[]
  createdDate: string
  updatedDate: string | null
  updatedBy: string | null
}

export interface ContentPaginationParam {
  page: number
  pageSize: number
  sortBy?: string
  sortOrder?: 'asc' | 'desc'
  search?: string
  localeCode?: string
  status?: ContentStatus
}

export interface CaseStudyPaginationParam extends ContentPaginationParam {
  label?: CaseStudyLabel
  year?: number
}

export interface MediaDTO {
  id: string
  objectPath: string
  mime: string
  sizeBytes: number
  width: number | null
  height: number | null
  originalName: string
  label: string | null
  createdDate: string
}

export type MediaPaginationDTO = MediaDTO & { number: number }

export type MediaPaginationParam = BasePaginationParam

/** Only the label is editable — a file is replaced by uploading a new one. */
export interface MediaParam {
  label?: string | null
}

export interface MediaUploadParam {
  width?: number | null
  height?: number | null
  label?: string | null
}
