import type { AxiosInstance } from 'axios'

import { BaseRepository } from '@/repositories/BaseRepository'
import type { BasePaginationParam, BaseResponse } from '@/types/api'
import type { MenuDTO, MenuPaginationDTO, MenuParam } from '@/types/menu'

export class MenuRepository extends BaseRepository<
  MenuParam,
  MenuDTO,
  MenuPaginationDTO,
  BasePaginationParam
> {
  protected readonly moduleUrl = '/menu'

  constructor(client?: AxiosInstance) {
    super(client)
  }

  /**
   * The menus the signed-in user may see, with their permission flags — scoped
   * to the active role. This is what the sidebar and every permission check in
   * the panel read from.
   */
  async getUserAccessibleMenus(): Promise<BaseResponse<MenuDTO[]>> {
    const response = await this.client.get<BaseResponse<MenuDTO[]>>(`${this.moduleUrl}/user-access`)
    return response.data
  }
}

export const menuRepository = new MenuRepository()
