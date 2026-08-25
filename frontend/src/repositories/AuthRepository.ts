import type { AxiosInstance } from 'axios'

import http from '@/lib/http'
import httpPublic from '@/lib/http-public'
import type { BaseRequest, BaseResponse } from '@/types/api'
import type {
  AuthTokens,
  AuthUser,
  LoginRequest,
  RefreshTokenRequest,
  UserRoleSummary,
} from '@/types/auth'

/**
 * The auth endpoints.
 *
 * Login and refresh go through the public client — sending a stale bearer
 * token with a login attempt is how you get a 401 on the one request that is
 * supposed to fix it.
 */
export class AuthRepository {
  private readonly client: AxiosInstance
  private readonly publicClient: AxiosInstance
  private readonly moduleUrl = '/auth'

  constructor(client?: AxiosInstance, publicClient?: AxiosInstance) {
    this.client = client ?? http
    this.publicClient = publicClient ?? httpPublic
  }

  async login(request: BaseRequest<LoginRequest>): Promise<BaseResponse<AuthTokens>> {
    const response = await this.publicClient.post<BaseResponse<AuthTokens>>(
      `${this.moduleUrl}/login`,
      request,
    )
    return response.data
  }

  async refreshToken(request: BaseRequest<RefreshTokenRequest>): Promise<BaseResponse<AuthTokens>> {
    const response = await this.publicClient.post<BaseResponse<AuthTokens>>(
      `${this.moduleUrl}/refresh`,
      request,
    )
    return response.data
  }

  async revokeToken(request: BaseRequest<RefreshTokenRequest>): Promise<BaseResponse<unknown>> {
    const response = await this.client.post<BaseResponse<unknown>>(
      `${this.moduleUrl}/revoke`,
      request,
    )
    return response.data
  }

  async logout(): Promise<BaseResponse<unknown>> {
    const response = await this.client.post<BaseResponse<unknown>>(`${this.moduleUrl}/logout`)
    return response.data
  }

  async getUserInfo(): Promise<BaseResponse<AuthUser>> {
    const response = await this.client.get<BaseResponse<AuthUser>>(`${this.moduleUrl}/user-info`)
    return response.data
  }

  async getMyRoles(): Promise<BaseResponse<UserRoleSummary[]>> {
    const response = await this.client.get<BaseResponse<UserRoleSummary[]>>(
      `${this.moduleUrl}/my-roles`,
    )
    return response.data
  }
}

export const authRepository = new AuthRepository()
