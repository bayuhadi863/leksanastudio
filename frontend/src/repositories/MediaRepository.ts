import type { AxiosInstance } from 'axios'

import { BaseRepository } from '@/repositories/BaseRepository'
import type { BaseResponse } from '@/types/api'
import type {
  MediaDTO,
  MediaPaginationDTO,
  MediaPaginationParam,
  MediaParam,
  MediaUploadParam,
} from '@/types/content'

export class MediaRepository extends BaseRepository<
  MediaParam,
  MediaDTO,
  MediaPaginationDTO,
  MediaPaginationParam
> {
  protected readonly moduleUrl = '/media'

  constructor(client?: AxiosInstance) {
    super(client)
  }

  /**
   * Uploads one image and records it in the library.
   *
   * The content type is set explicitly: this client defaults to JSON, and axios
   * would otherwise serialise the FormData into a JSON object. The browser
   * replaces the header with its own boundary before the request leaves.
   */
  async upload(file: File, param: MediaUploadParam = {}): Promise<MediaDTO> {
    const form = new FormData()
    form.append('file', file)
    if (param.width != null) form.append('width', String(param.width))
    if (param.height != null) form.append('height', String(param.height))
    if (param.label) form.append('label', param.label)

    const response = await this.client.post<BaseResponse<MediaDTO>>(
      `${this.moduleUrl}/upload`,
      form,
      { headers: { 'Content-Type': 'multipart/form-data' } },
    )

    return response.data.data as MediaDTO
  }
}

export const mediaRepository = new MediaRepository()
