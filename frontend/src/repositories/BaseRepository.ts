import type { AxiosInstance } from 'axios'

import http from '@/lib/http'
import type {
  BasePaginationParam,
  BaseRequest,
  BaseResponse,
  PaginationResponse,
} from '@/types/api'

/**
 * The five endpoints every CRUD module on the backend exposes, in one place.
 *
 * A module repository declares its URL prefix and its four types; it inherits
 * the rest. That is the same bargain `BaseCrudController` makes on the server,
 * which is why the two stay in step.
 */
export abstract class BaseRepository<
  TParam,
  TDTO,
  TPaginationDTO,
  TPaginationParam extends BasePaginationParam,
> {
  protected readonly client: AxiosInstance
  protected abstract readonly moduleUrl: string

  constructor(client?: AxiosInstance) {
    this.client = client ?? http
  }

  async create(request: BaseRequest<TParam>): Promise<BaseResponse<string>> {
    const response = await this.client.post<BaseResponse<string>>(
      `${this.moduleUrl}/create`,
      request,
    )
    return response.data
  }

  async update(id: string, request: BaseRequest<TParam>): Promise<BaseResponse<string>> {
    const response = await this.client.put<BaseResponse<string>>(
      `${this.moduleUrl}/update/${id}`,
      request,
    )
    return response.data
  }

  async remove(id: string): Promise<BaseResponse<string>> {
    const response = await this.client.delete<BaseResponse<string>>(
      `${this.moduleUrl}/delete/${id}`,
    )
    return response.data
  }

  async get(id: string): Promise<BaseResponse<TDTO>> {
    const response = await this.client.get<BaseResponse<TDTO>>(`${this.moduleUrl}/get/${id}`)
    return response.data
  }

  async getList(): Promise<BaseResponse<TPaginationDTO[]>> {
    const response = await this.client.get<BaseResponse<TPaginationDTO[]>>(
      `${this.moduleUrl}/get/list`,
    )
    return response.data
  }

  async getPagination(
    params: TPaginationParam,
  ): Promise<BaseResponse<PaginationResponse<TPaginationDTO>>> {
    const response = await this.client.get<BaseResponse<PaginationResponse<TPaginationDTO>>>(
      `${this.moduleUrl}/get/pagination`,
      { params },
    )
    return response.data
  }
}
