import { create } from 'zustand'

import {
  getActiveRoleId,
  removeActiveRoleId,
  setActiveRoleId,
  setLastRoleId,
} from '@/lib/active-role'
import type { UserRoleSummary } from '@/types/auth'

interface ActiveRoleState {
  /** Every role assigned to the signed-in user — the switcher's options. */
  roles: UserRoleSummary[]
  /** The role currently being acted as; drives the `X-Role-Active` header. */
  activeRoleId: string | null
  setRoles: (roles: UserRoleSummary[]) => void
  setActiveRole: (roleId: string) => void
  clear: () => void
}

export const useActiveRoleStore = create<ActiveRoleState>()((set) => ({
  roles: [],
  activeRoleId: getActiveRoleId(),
  setRoles: (roles) => set({ roles }),
  setActiveRole: (roleId) => {
    setActiveRoleId(roleId)
    // Remembered across logout so the next sign-in preselects it.
    setLastRoleId(roleId)
    set({ activeRoleId: roleId })
  },
  clear: () => {
    removeActiveRoleId()
    set({ roles: [], activeRoleId: null })
  },
}))
