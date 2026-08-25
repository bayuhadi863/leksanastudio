import { Suspense, lazy } from 'react'
import { Route, Routes } from 'react-router-dom'

import { ScrollManager } from '@/components/layout/ScrollManager'
import { SiteLayout } from '@/components/layout/SiteLayout'
import { PageLoader } from '@/components/panel/PageLoader'
import { accessDeniedRoute, authRoutes, panelRoutes } from '@/config/panel'
import { AboutPage } from '@/pages/AboutPage'
import { CaseStudyPage } from '@/pages/CaseStudyPage'
import { ContactPage } from '@/pages/ContactPage'
import { ContactThanksPage } from '@/pages/ContactThanksPage'
import { HomePage } from '@/pages/HomePage'
import { NotFoundPage } from '@/pages/NotFoundPage'
import { NoteDetailPage } from '@/pages/NoteDetailPage'
import { NotesPage } from '@/pages/NotesPage'
import { PricingPage } from '@/pages/PricingPage'
import { PrivacyPage } from '@/pages/PrivacyPage'
import { ProcessPage } from '@/pages/ProcessPage'
import { ServiceDetailPage } from '@/pages/ServiceDetailPage'
import { ServicesPage } from '@/pages/ServicesPage'
import { VerticalPage } from '@/pages/VerticalPage'
import { WorkPage } from '@/pages/WorkPage'

// Everything behind sign-in is split out: a visitor reading the site never
// downloads the panel, and the public bundle stays the size it was.
//
// `panelEnabled` is a build-time constant, so a build without the panel does
// not merely hide these routes — the imports fold away and the chunks are never
// emitted. See `config/features.ts`.
const AuthRoutes = __PANEL_ENABLED__ ? lazy(() => import('@/pages/auth/AuthRoutes')) : null
const PanelRoutes = __PANEL_ENABLED__ ? lazy(() => import('@/pages/panel/PanelRoutes')) : null
const AccessDeniedPage = __PANEL_ENABLED__
  ? lazy(() =>
      import('@/pages/AccessDeniedPage').then((module) => ({ default: module.AccessDeniedPage })),
    )
  : null

/**
 * Three worlds, one router.
 *
 * The public site renders inside `SiteLayout`; the sign-in screens and the
 * management panel bring their own shells. Static paths are listed before the
 * vertical catch-all only for readability — the router ranks by specificity,
 * so `/harga` and `/panel` can never be swallowed by `/:vertical`.
 */
export function App() {
  return (
    <>
      <ScrollManager />

      <Routes>
        {AuthRoutes && PanelRoutes && AccessDeniedPage ? (
          <>
            {/* Sign-in */}
            <Route
              path={`${authRoutes.base}/*`}
              element={
                <Suspense fallback={<PageLoader />}>
                  <AuthRoutes />
                </Suspense>
              }
            />

            {/* Management panel */}
            <Route
              path={`${panelRoutes.base}/*`}
              element={
                <Suspense fallback={<PageLoader message="Menyiapkan panel…" />}>
                  <PanelRoutes />
                </Suspense>
              }
            />

            <Route
              path={accessDeniedRoute}
              element={
                <Suspense fallback={<PageLoader />}>
                  <AccessDeniedPage />
                </Suspense>
              }
            />
          </>
        ) : null}

        {/* Public site */}
        <Route element={<SiteLayout />}>
          <Route path="/" element={<HomePage />} />

          <Route path="/layanan" element={<ServicesPage />} />
          <Route path="/layanan/:slug" element={<ServiceDetailPage />} />

          <Route path="/portofolio" element={<WorkPage />} />
          <Route path="/portofolio/:slug" element={<CaseStudyPage />} />

          <Route path="/proses" element={<ProcessPage />} />
          <Route path="/harga" element={<PricingPage />} />
          <Route path="/tentang" element={<AboutPage />} />

          <Route path="/kontak" element={<ContactPage />} />
          <Route path="/kontak/terima-kasih" element={<ContactThanksPage />} />

          <Route path="/catatan" element={<NotesPage />} />
          <Route path="/catatan/:slug" element={<NoteDetailPage />} />

          <Route path="/kebijakan-privasi" element={<PrivacyPage />} />

          {/* Vertical landing pages — the SEO engine. One template, many data files. */}
          <Route path="/:vertical" element={<VerticalPage />} />

          <Route path="*" element={<NotFoundPage />} />
        </Route>
      </Routes>
    </>
  )
}
