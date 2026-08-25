import { useEffect, useState } from 'react'
import { Navigate, useNavigate } from 'react-router-dom'
import { toast } from 'sonner'

import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { authRoutes, resolveLandingPath } from '@/config/panel'
import { useUserMenus } from '@/hooks/useUserMenus'
import { getLastRoleId } from '@/lib/active-role'
import { getErrorMessage } from '@/lib/api-error'
import { usePageMeta } from '@/lib/seo'
import { getAccessToken } from '@/lib/tokens'
import { AuthLayout } from '@/pages/auth/AuthLayout'
import { authRepository } from '@/repositories/AuthRepository'
import { useActiveRoleStore } from '@/store/active-role-store'
import { useMenuStore } from '@/store/menu-store'
import type { UserRoleSummary } from '@/types/auth'

/**
 * Shown only when an account holds more than one role.
 *
 * Picking is deliberate rather than automatic because the choice decides what
 * the whole session can see — silently defaulting would leave someone editing
 * under a role they did not realise they were wearing.
 */
export function SelectRolePage() {
  usePageMeta({
    title: 'Pilih peran',
    description: 'Pilih peran untuk sesi ini.',
    path: authRoutes.selectRole,
    noIndex: true,
  })

  const navigate = useNavigate()
  const { fetchAndSync: syncMenus } = useUserMenus()

  const roles = useActiveRoleStore((state) => state.roles)
  const setRoles = useActiveRoleStore((state) => state.setRoles)
  const setActiveRole = useActiveRoleStore((state) => state.setActiveRole)

  const [selected, setSelected] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  // A reload lands here with an empty store; fetch the roles back rather than
  // bouncing the user to login for a page they legitimately reached.
  useEffect(() => {
    if (roles.length > 0) return
    void authRepository
      .getMyRoles()
      .then((response) => {
        if (response.success && response.data) setRoles(response.data)
      })
      .catch(() => {
        // Left empty on purpose — the render below sends them back to login.
      })
  }, [roles.length, setRoles])

  // Preselect the role they used last, when it is still one of theirs.
  useEffect(() => {
    if (selected || roles.length === 0) return
    const last = getLastRoleId()
    const remembered = last && roles.some((role) => role.roleId === last) ? last : null
    setSelected(remembered ?? roles[0]!.roleId)
  }, [roles, selected])

  if (!getAccessToken()) {
    return <Navigate to={authRoutes.login} replace />
  }

  const choose = async (role: UserRoleSummary) => {
    setIsSubmitting(true)
    try {
      setActiveRole(role.roleId)
      await syncMenus()
      navigate(resolveLandingPath(role.defaultMenuCode, useMenuStore.getState().canAccess), {
        replace: true,
      })
    } catch (error) {
      toast.error(getErrorMessage(error, 'Gagal menerapkan peran. Coba lagi.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const active = roles.find((role) => role.roleId === selected) ?? null

  return (
    <AuthLayout
      eyebrow="Satu langkah lagi"
      title="Pilih peran"
      lead="Akun Anda punya lebih dari satu peran. Menu dan izin menyesuaikan peran yang dipilih — Anda bisa berganti kapan saja dari dalam panel."
    >
      <ul className="border-line grid border-t">
        {roles.map((role) => {
          const isSelected = role.roleId === selected
          return (
            <li key={role.roleId} className="border-line border-b">
              <label
                className={
                  'flex cursor-pointer items-start gap-4 py-5 transition-colors duration-150 ease-out ' +
                  (isSelected ? 'text-text' : 'text-muted hover:text-text')
                }
              >
                <input
                  type="radio"
                  name="role"
                  value={role.roleId}
                  checked={isSelected}
                  onChange={() => setSelected(role.roleId)}
                  className="sr-only"
                />
                <span
                  aria-hidden="true"
                  className={
                    'mt-[0.35em] h-3 w-3 shrink-0 rounded-full border transition-colors duration-150 ease-out ' +
                    (isSelected ? 'border-accent bg-accent' : 'border-muted')
                  }
                />
                <span className="flex-1">
                  <span className="block font-semibold">{role.roleName ?? role.roleCode}</span>
                  <Label as="span" className="mt-1 block">
                    {role.roleCode}
                  </Label>
                </span>
              </label>
            </li>
          )
        })}
      </ul>

      <Button
        type="button"
        size="large"
        className="mt-8 w-full"
        disabled={!active || isSubmitting}
        onClick={() => active && void choose(active)}
      >
        {isSubmitting ? 'Menerapkan…' : 'Lanjutkan'}
      </Button>
    </AuthLayout>
  )
}
