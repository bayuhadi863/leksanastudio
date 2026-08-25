import { useCallback, useState } from 'react'
import axios from 'axios'

import { removeActiveRoleId } from '@/lib/active-role'
import { menuRepository } from '@/repositories/MenuRepository'
import { useMenuStore } from '@/store/menu-store'

/**
 * Loads the current user's menus into the store.
 *
 * `error` is kept separate from "no menus" on purpose: a failed fetch means we
 * do not know what the user may see, and showing them an access-denied screen
 * for a backend outage would be a lie.
 */
export function useUserMenus() {
  const setMenus = useMenuStore((state) => state.setMenus)
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<unknown>(null)

  const fetchAndSync = useCallback(async (): Promise<void> => {
    setIsLoading(true)
    setError(null)
    try {
      const response = await menuRepository.getUserAccessibleMenus()
      if (response.success && response.data) {
        setMenus(response.data)
      } else {
        setError(new Error(response.message || 'Gagal memuat menu'))
      }
    } catch (caught) {
      // A 403 here means the stored active role is no longer one of the user's
      // roles (unassigned mid-session). Drop it, so a retry falls back to their
      // primary role instead of looping on the same rejected header.
      if (axios.isAxiosError(caught) && caught.response?.status === 403) {
        removeActiveRoleId()
      }
      setError(caught)
    } finally {
      setIsLoading(false)
    }
  }, [setMenus])

  return { fetchAndSync, isLoading, error }
}
