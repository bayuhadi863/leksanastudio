import type { AxiosInstance } from 'axios'

import http from '@/lib/http'
import httpPublic from '@/lib/http-public'
import { BaseRepository } from '@/repositories/BaseRepository'
import type { BaseResponse } from '@/types/api'
import type { BlockSchema } from '@/types/blocks'
import type {
  CaseStudyDTO,
  CaseStudyPaginationDTO,
  CaseStudyPaginationParam,
  CaseStudyParam,
  LocaleDTO,
} from '@/types/content'

export class CaseStudyRepository extends BaseRepository<
  CaseStudyParam,
  CaseStudyDTO,
  CaseStudyPaginationDTO,
  CaseStudyPaginationParam
> {
  protected readonly moduleUrl = '/case-study'

  constructor(client?: AxiosInstance) {
    super(client)
  }

  /** Applies a new display order in one write, so the list never shows two entries sharing a position. */
  async reorder(ids: string[]): Promise<BaseResponse<unknown>> {
    const response = await this.client.put<BaseResponse<unknown>>(`${this.moduleUrl}/reorder`, {
      data: { ids },
    })
    return response.data
  }
}

export const caseStudyRepository = new CaseStudyRepository()

/** The languages the panel offers when editing content. */
export const fetchActiveLocales = async (): Promise<LocaleDTO[]> => {
  const response = await http.get<BaseResponse<LocaleDTO[]>>('/locale/active')
  return response.data.data ?? []
}

/**
 * The block contract, straight from the server.
 *
 * Fetched rather than hard-coded so the editor's limits and the server's cannot
 * drift — the one failure a second copy of these numbers guarantees.
 */
export const fetchBlockSchema = async (): Promise<BlockSchema> => {
  const response = await httpPublic.get<BaseResponse<BlockSchema>>('/public/block-schema')
  return response.data.data as BlockSchema
}
