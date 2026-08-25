/** The envelope every endpoint answers with. Mirrors `BaseResponse<T>` in the API. */
export interface BaseResponse<T = unknown> {
  success: boolean
  message: string
  data?: T
  errors?: unknown
  code: string
}

/** The envelope every write endpoint expects. Mirrors `BaseRequest<T>` in the API. */
export interface BaseRequest<T> {
  data: T
}

export interface BasePaginationParam {
  page: number
  pageSize: number
  sortBy?: string
  sortOrder?: 'asc' | 'desc'
  search?: string
}

export interface PaginationResponse<T> {
  items: T[]
  totalCount: number
  totalPages: number
  page: number
  pageSize: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}
