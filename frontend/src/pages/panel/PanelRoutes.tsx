import { Navigate, Route, Routes } from 'react-router-dom'
import { Toaster } from 'sonner'

import { MenuRoute } from '@/components/route/MenuRoute'
import { ProtectedRoute } from '@/components/route/ProtectedRoute'
import { menuCodes, panelRoutes } from '@/config/panel'
import { PanelDashboardPage } from '@/pages/panel/PanelDashboardPage'
import { PanelLayout } from '@/pages/panel/PanelLayout'
import { CaseStudyFormPage } from '@/pages/panel/case-study/CaseStudyFormPage'
import { CaseStudyListPage } from '@/pages/panel/case-study/CaseStudyListPage'
import { MediaLibraryPage } from '@/pages/panel/media/MediaLibraryPage'

/**
 * The management panel subtree, loaded on demand.
 *
 * Two gates, in order: `ProtectedRoute` settles the session and loads the
 * menus, then `MenuRoute` checks the one code that owns each page. The server
 * enforces both again on every request.
 *
 * Paths here are written as segments relative to `/panel`; the full paths live
 * in `panelRoutes` and are what everything else links to.
 */
export default function PanelRoutes() {
  return (
    <>
      <Routes>
        <Route element={<ProtectedRoute />}>
          <Route element={<PanelLayout />}>
            <Route index element={<Navigate to={panelRoutes.dashboard} replace />} />

            <Route
              path="dasbor"
              element={
                <MenuRoute menuCode={menuCodes.dashboard}>
                  <PanelDashboardPage />
                </MenuRoute>
              }
            />

            <Route
              path="portofolio"
              element={
                <MenuRoute menuCode={menuCodes.caseStudy}>
                  <CaseStudyListPage />
                </MenuRoute>
              }
            />
            <Route
              path="portofolio/baru"
              element={
                <MenuRoute menuCode={menuCodes.caseStudy}>
                  <CaseStudyFormPage />
                </MenuRoute>
              }
            />
            <Route
              path="portofolio/:id"
              element={
                <MenuRoute menuCode={menuCodes.caseStudy}>
                  <CaseStudyFormPage />
                </MenuRoute>
              }
            />

            <Route
              path="berkas"
              element={
                <MenuRoute menuCode={menuCodes.media}>
                  <MediaLibraryPage />
                </MenuRoute>
              }
            />

            <Route path="*" element={<Navigate to={panelRoutes.dashboard} replace />} />
          </Route>
        </Route>
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
