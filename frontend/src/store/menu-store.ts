import { create } from 'zustand'

import type { MenuDTO } from '@/types/menu'

interface MenuState {
  menus: MenuDTO[]
  isFetched: boolean
  setMenus: (menus: MenuDTO[]) => void
  clearMenus: () => void
  canAccess: (menuCode: string) => boolean
}

/**
 * The menus the current user can reach, as the server reported them.
 *
 * Held in memory only, and re-fetched on every entry into the panel: a
 * permission revoked while someone was signed in must take effect on their
 * next visit, not whenever a cache decides to expire.
 */
export const useMenuStore = create<MenuState>()((set, get) => ({
  menus: [],
  isFetched: false,
  setMenus: (menus) => set({ menus, isFetched: true }),
  clearMenus: () => set({ menus: [], isFetched: false }),
  canAccess: (menuCode) =>
    get().menus.some((menu) => menu.code?.toLowerCase() === menuCode.toLowerCase()),
}))
