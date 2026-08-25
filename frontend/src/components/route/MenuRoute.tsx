import type { ReactNode } from 'react'
import { Navigate } from 'react-router-dom'

import { accessDeniedRoute, authRoutes } from '@/config/panel'
import { getAccessToken } from '@/lib/tokens'
import { useMenuStore } from '@/store/menu-store'

type Props = {
  readonly menuCode: string
  readonly children: ReactNode
}

/**
 * Guards one page behind one menu code. The server enforces the same rule on
 * every request this page makes; this only spares the reader a 403.
 */
export function MenuRoute({ menuCode, children }: Props) {
  const canAccess = useMenuStore((state) => state.canAccess)

  // No token (just logged out, session lost) — defer to login rather than
  // accusing them of lacking access based on a cleared menu list.
  if (!getAccessToken()) {
    return <Navigate to={authRoutes.login} replace />
  }

  if (!canAccess(menuCode)) {
    return <Navigate to={accessDeniedRoute} replace />
  }

  return <>{children}</>
}
