import { useMemo } from 'react'

import { useMenuStore } from '@/store/menu-store'

export interface MenuPermission {
  canView: boolean
  canCreate: boolean
  canUpdate: boolean
  canDelete: boolean
  canVerify: boolean
  /** Granted custom-event codes on this menu. */
  customEvents: string[]
  hasEvent: (code: string) => boolean
}

const NONE: MenuPermission = {
  canView: false,
  canCreate: false,
  canUpdate: false,
  canDelete: false,
  canVerify: false,
  customEvents: [],
  hasEvent: () => false,
}

/**
 * The current user's permissions for one menu code.
 *
 * Use it to hide controls the user cannot use. The backend enforces the same
 * flags on every request, so this is presentation only — hiding a button is
 * not a security measure and is not treated as one.
 */
export function usePermission(menuCode: string): MenuPermission {
  const menus = useMenuStore((state) => state.menus)

  return useMemo(() => {
    const menu = menus.find((item) => item.code?.toLowerCase() === menuCode.toLowerCase())
    if (!menu) return NONE

    const customEvents = menu.customEvents ?? []

    return {
      canView: menu.canView,
      canCreate: menu.canCreate,
      canUpdate: menu.canUpdate,
      canDelete: menu.canDelete,
      canVerify: menu.canVerify,
      customEvents,
      hasEvent: (code: string) =>
        customEvents.some((event) => event.toLowerCase() === code.toLowerCase()),
    }
  }, [menus, menuCode])
}
