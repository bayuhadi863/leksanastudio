import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'

import { Button } from '@/components/ui/Button'
import { TextField } from '@/components/ui/TextField'
import { authRoutes, resolveLandingPath } from '@/config/panel'
import { routes } from '@/config/routes'
import { useApi } from '@/hooks/useApi'
import { useUserMenus } from '@/hooks/useUserMenus'
import { getErrorMessage } from '@/lib/api-error'
import { setAuthTokens } from '@/lib/tokens'
import { AuthLayout } from '@/pages/auth/AuthLayout'
import { authRepository } from '@/repositories/AuthRepository'
import { useActiveRoleStore } from '@/store/active-role-store'
import { useMenuStore } from '@/store/menu-store'
import { usePageMeta } from '@/lib/seo'
import type { BaseRequest } from '@/types/api'
import type { LoginRequest } from '@/types/auth'

const loginSchema = z.object({
  email: z.string().trim().min(1, 'Email wajib diisi.').email('Format email tidak valid.'),
  password: z.string().min(1, 'Kata sandi wajib diisi.'),
})

type LoginValues = z.infer<typeof loginSchema>

export function LoginPage() {
  usePageMeta({
    title: 'Masuk',
    description: 'Masuk ke panel pengelolaan Leksana Studio.',
    path: authRoutes.login,
    noIndex: true,
  })

  const navigate = useNavigate()
  const [showPassword, setShowPassword] = useState(false)
  const { fetchAndSync: syncMenus } = useUserMenus()
  const setRoles = useActiveRoleStore((state) => state.setRoles)
  const setActiveRole = useActiveRoleStore((state) => state.setActiveRole)
  const clearActiveRole = useActiveRoleStore((state) => state.clear)

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: { email: '', password: '' },
  })

  const { execute: submitLogin, isLoading } = useApi(
    (request: BaseRequest<LoginRequest>) => authRepository.login(request),
    {
      onSuccess: async (response) => {
        if (!response.success || !response.data) {
          toast.error(response.message || 'Gagal masuk.')
          return
        }

        setAuthTokens(response.data)

        // Roles first: with more than one, the active role is a decision the
        // user makes — and menus are scoped to whichever they pick.
        let defaultMenuCode: string | null = null
        try {
          const rolesResponse = await authRepository.getMyRoles()
          const roles = rolesResponse.success && rolesResponse.data ? rolesResponse.data : []
          setRoles(roles)

          if (roles.length > 1) {
            navigate(authRoutes.selectRole, { replace: true })
            return
          }

          if (roles.length === 1) {
            setActiveRole(roles[0]!.roleId)
            defaultMenuCode = roles[0]!.defaultMenuCode
          } else {
            clearActiveRole()
          }
        } catch {
          // Roles unavailable — carry on; the server falls back to the user's
          // primary role, which is exactly what an absent header means.
        }

        // Menus settle before navigating, so the first page renders against a
        // real permission set instead of an empty one.
        await syncMenus()

        navigate(resolveLandingPath(defaultMenuCode, useMenuStore.getState().canAccess), {
          replace: true,
        })
      },
      onError: (error: unknown) => {
        toast.error(getErrorMessage(error, 'Tidak bisa masuk. Coba lagi.'))
      },
    },
  )

  const onSubmit = (values: LoginValues) => {
    void submitLogin({ data: values })
  }

  return (
    <AuthLayout
      eyebrow="Panel pengelolaan"
      title="Masuk"
      lead="Halaman ini untuk pengelola situs. Kalau Anda sampai di sini karena tersesat, tautan di bawah membawa Anda kembali."
    >
      <form onSubmit={handleSubmit(onSubmit)} noValidate>
        <div className="grid gap-4">
          <TextField
            id="email"
            label="Email"
            type="email"
            inputMode="email"
            autoComplete="username"
            autoCapitalize="none"
            autoCorrect="off"
            placeholder="nama@leksana.id"
            error={errors.email?.message}
            {...register('email')}
          />

          <TextField
            id="password"
            label="Kata sandi"
            type={showPassword ? 'text' : 'password'}
            autoComplete="current-password"
            className="pr-24"
            error={errors.password?.message}
            adornment={
              <button
                type="button"
                onClick={() => setShowPassword((shown) => !shown)}
                className="type-label text-muted hover:text-accent absolute top-1/2 right-3 -translate-y-1/2 px-1 transition-colors duration-150 ease-out"
              >
                {showPassword ? 'Sembunyikan' : 'Tampilkan'}
                <span className="sr-only"> kata sandi</span>
              </button>
            }
            {...register('password')}
          />
        </div>

        <Button type="submit" size="large" disabled={isLoading} className="mt-2 w-full">
          {isLoading ? 'Memeriksa…' : 'Masuk'}
        </Button>
      </form>

      <div className="border-line mt-10 border-t pt-6">
        <Link to={routes.home} className="text-accent inline-flex items-center gap-2 font-semibold">
          <span aria-hidden="true">&larr;</span>
          Kembali ke situs
        </Link>
      </div>
    </AuthLayout>
  )
}
