export interface MenuDTO {
  id: string
  code: string | null
  name: string | null
  /** Merged CRUD permissions for the current user (from /menu/user-access). */
  canView: boolean
  canCreate: boolean
  canUpdate: boolean
  canDelete: boolean
  canVerify: boolean
  /** Granted custom-event codes beyond CRUD/verify. */
  customEvents: string[]
  createdDate: string
  updatedDate: string | null
  createdBy: string
  updatedBy: string | null
}

export interface MenuParam {
  code?: string | null
  name?: string | null
}

export interface MenuPaginationDTO {
  number: number
  id: string
  code: string | null
  name: string | null
  createdDate: string
  updatedDate: string | null
  createdBy: string
  updatedBy: string | null
}
