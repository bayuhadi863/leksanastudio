import { Navigate, Route, Routes } from 'react-router-dom'
import { Toaster } from 'sonner'

import { authRoutes } from '@/config/panel'
import { LoginPage } from '@/pages/auth/LoginPage'
import { SelectRolePage } from '@/pages/auth/SelectRolePage'

/**
 * The sign-in subtree, loaded on demand.
 *
 * Kept out of the public bundle on purpose: a visitor reading a case study
 * should not download the login form, the HTTP client, or the form library
 * that only the panel needs.
 */
export default function AuthRoutes() {
  return (
    <>
      <Routes>
        <Route path="masuk" element={<LoginPage />} />
        <Route path="pilih-peran" element={<SelectRolePage />} />
        <Route path="*" element={<Navigate to={authRoutes.login} replace />} />
      </Routes>

      <Toaster
        position="bottom-right"
        closeButton
        toastOptions={{
          className:
            'border border-line bg-surface text-text rounded-[var(--radius-control)] shadow-none',
        }}
      />
    </>
  )
}
