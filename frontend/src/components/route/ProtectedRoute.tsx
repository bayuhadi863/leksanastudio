import { useEffect } from 'react'
import { Navigate, Outlet, useLocation } from 'react-router-dom'

import { ErrorState } from '@/components/panel/ErrorState'
import { PageLoader } from '@/components/panel/PageLoader'
import { authRoutes } from '@/config/panel'
import { useUserMenus } from '@/hooks/useUserMenus'
import { getAccessToken } from '@/lib/tokens'

/**
 * The gate in front of everything authenticated.
 *
 * It also loads the user's menus before rendering a single child, so no page
 * ever renders against an empty permission set and flashes a denial it would
 * then take back.
 */
export function ProtectedRoute() {
  const token = getAccessToken()
  const location = useLocation()
  const { fetchAndSync, isLoading, error } = useUserMenus()

  useEffect(() => {
    if (!token) return
    void fetchAndSync()
  }, [token, fetchAndSync])

  if (!token) {
    // Remember where they were headed so login can return them to it.
    return <Navigate to={authRoutes.login} replace state={{ from: location.pathname }} />
  }

  if (isLoading) {
    return <PageLoader message="Menyiapkan panel…" />
  }

  // The menu fetch failed — the backend is down, or unreachable. That is not
  // the same as "no access", and must not be shown as one.
  if (error) {
    return (
      <div className="shell flex min-h-dvh items-center py-20">
        <ErrorState error={error} title="Gagal memuat menu" onRetry={() => void fetchAndSync()} />
      </div>
    )
  }

  return <Outlet />
}
