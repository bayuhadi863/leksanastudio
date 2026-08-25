export interface AuthTokens {
  accessToken: string
  refreshToken: string
  tokenType: string
  expiresIn: number
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RefreshTokenRequest {
  refreshToken: string
}

export interface AuthUser {
  id: string
  name?: string | null
  email?: string | null
  createdDate: string
  updatedDate?: string | null
  createdBy: string
  updatedBy?: string | null
}

export interface UserRoleSummary {
  roleId: string
  roleCode: string | null
  roleName: string | null
  /** Menu code the role lands on after login / switch (null = unset). */
  defaultMenuCode: string | null
}
