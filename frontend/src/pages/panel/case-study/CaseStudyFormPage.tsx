import { useCallback, useDeferredValue, useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { toast } from 'sonner'

import { ConfirmDialog } from '@/components/panel/ConfirmDialog'
import { ErrorState } from '@/components/panel/ErrorState'
import { LanguageTabs } from '@/components/panel/LanguageTabs'
import { PageLoader } from '@/components/panel/PageLoader'
import { PanelPage } from '@/components/panel/PanelPage'
import { SaveBar } from '@/components/panel/SaveBar'
import { CaseStudyPreview } from '@/components/panel/preview/CaseStudyPreview'
import { EditorModeSwitch } from '@/components/panel/preview/EditorModeSwitch'
import { PreviewPane } from '@/components/panel/preview/PreviewPane'
import { useEditorMode } from '@/hooks/useEditorMode'
import { useFillHeight } from '@/hooks/useFillHeight'
import { StatusChip } from '@/components/panel/StatusChip'
import { BlockEditor } from '@/components/panel/blocks/BlockEditor'
import { FieldRow } from '@/components/panel/form/FieldRow'
import { FormSection } from '@/components/panel/form/FormSection'
import {
  NumberInput,
  ReadOnlyField,
  SegmentedInput,
  TextAreaInput,
  TextInput,
} from '@/components/panel/form/Inputs'
import { Repeater, StringRepeater } from '@/components/panel/form/Repeater'
import { SlugInput } from '@/components/panel/form/SlugInput'
import { MediaField } from '@/components/panel/media/MediaField'
import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { menuCodes, panelRoutes } from '@/config/panel'
import { usePermission } from '@/hooks/usePermission'
import { getValidationMessages, notifyApiError } from '@/lib/api-error'
import { cn } from '@/lib/cn'
import { formatDate } from '@/lib/format'
import { deriveSlug, slugify } from '@/lib/slug'
import { usePageMeta } from '@/lib/seo'
import {
  caseStudyRepository,
  fetchActiveLocales,
  fetchBlockSchema,
} from '@/repositories/CaseStudyRepository'
import {
  FALLBACK_LIMITS,
  withUniqueBlockIds,
  type Block,
  type BlockKind,
  type BlockLimits,
} from '@/types/blocks'
import type {
  CaseStudyDTO,
  CaseStudyLabel,
  CaseStudyParam,
  ContentStatus,
  LocaleDTO,
  SchematicVariant,
} from '@/types/content'

/* -------------------------------------------------------------------- state */

type EntryDraft = {
  contentKey: string
  label: CaseStudyLabel
  figure: SchematicVariant
  coverMediaId: string | null
  coverMediaPath: string | null
  year: number
  stack: string[]
  order: number
}

type TranslationDraft = {
  /** False until this language is given a translation of its own. */
  enabled: boolean
  slug: string
  slugTouched: boolean
  /** The address the public has already been served, if any. */
  publishedSlug: string | null
  status: ContentStatus
  publishedAt: string | null
  title: string
  summary: string
  problem: string
  client: string
  kind: string
  duration: string
  role: string
  coverAlt: string
  metrics: { value: string; label: string }[]
  body: Block[]
}

const emptyMetrics = (count: number) =>
  Array.from({ length: count }, () => ({ value: '', label: '' }))

const emptyTranslation = (metricsCount: number): TranslationDraft => ({
  enabled: false,
  slug: '',
  slugTouched: false,
  publishedSlug: null,
  status: 'Draft',
  publishedAt: null,
  title: '',
  summary: '',
  problem: '',
  client: '',
  kind: '',
  duration: '',
  role: '',
  coverAlt: '',
  metrics: emptyMetrics(metricsCount),
  body: [],
})

const emptyEntry = (): EntryDraft => ({
  contentKey: '',
  label: 'Client',
  figure: 'System',
  coverMediaId: null,
  coverMediaPath: null,
  year: new Date().getFullYear(),
  stack: [],
  order: 0,
})

const toDrafts = (
  dto: CaseStudyDTO,
  locales: readonly LocaleDTO[],
  metricsCount: number,
): Record<string, TranslationDraft> => {
  const drafts: Record<string, TranslationDraft> = {}

  for (const locale of locales) {
    const translation = dto.translations.find((item) => item.localeCode === locale.code)

    if (!translation) {
      drafts[locale.code] = emptyTranslation(metricsCount)
      continue
    }

    const metrics = translation.metrics ?? []

    drafts[locale.code] = {
      enabled: true,
      slug: translation.slug ?? '',
      // An existing entry never re-derives its address from the title: that
      // address may already be printed in someone's proposal.
      slugTouched: true,
      publishedSlug: translation.status === 'Published' ? translation.slug : null,
      status: translation.status,
      publishedAt: translation.publishedAt,
      title: translation.title ?? '',
      summary: translation.summary ?? '',
      problem: translation.problem ?? '',
      client: translation.client ?? '',
      kind: translation.kind ?? '',
      duration: translation.duration ?? '',
      role: translation.role ?? '',
      coverAlt: translation.coverAlt ?? '',
      metrics: Array.from({ length: metricsCount }, (_, index) => ({
        value: metrics[index]?.value ?? '',
        label: metrics[index]?.label ?? '',
      })),
      // Repaired on the way in, so a document stored before the server
      // guaranteed unique ids still opens one card at a time.
      body: withUniqueBlockIds(translation.body ?? []),
    }
  }

  return drafts
}

/* --------------------------------------------------------------------- page */

/**
 * Writing one case study, in every language it exists in.
 *
 * The screen is split the way the data is: what belongs to the project itself —
 * the year, the client type, the cover — is edited once, and everything that is
 * language-specific sits under the tabs. Getting that division wrong is how
 * multilingual panels end up with three different years for the same project.
 */
export function CaseStudyFormPage() {
  const { id } = useParams<{ id: string }>()
  const isCreate = !id
  const navigate = useNavigate()
  const permission = usePermission(menuCodes.caseStudy)

  const { mode, setMode, canSplit } = useEditorMode()

  // Which block the cursor is in, so the preview can follow it.
  const [focusBlockId, setFocusBlockId] = useState<string | null>(null)

  const [locales, setLocales] = useState<LocaleDTO[]>([])
  const [limits, setLimits] = useState<BlockLimits>(FALLBACK_LIMITS)
  const [kinds, setKinds] = useState<readonly BlockKind[]>([
    'richText',
    'heading',
    'decision',
    'figure',
    'metrics',
    'note',
    'codeBlock',
    'table',
  ])

  const [entry, setEntry] = useState<EntryDraft>(emptyEntry)
  const [drafts, setDrafts] = useState<Record<string, TranslationDraft>>({})
  const [active, setActive] = useState('')

  const [loading, setLoading] = useState(true)
  const [loadError, setLoadError] = useState<unknown>(null)
  const [saving, setSaving] = useState(false)
  const [deleting, setDeleting] = useState(false)
  const [confirmingDelete, setConfirmingDelete] = useState(false)
  const [serverErrors, setServerErrors] = useState<string[]>([])
  const [showErrors, setShowErrors] = useState(false)
  const [baseline, setBaseline] = useState('')
  const [meta, setMeta] = useState<Pick<
    CaseStudyDTO,
    'createdDate' | 'updatedDate' | 'createdBy' | 'updatedBy'
  > | null>(null)

  /*
   * With a preview open the editor stops being a page that scrolls and becomes
   * two columns that scroll: the writing on the left, the result on the right,
   * both anchored to the window. Otherwise the preview's own height depends on
   * how far the page happens to be scrolled, which is how it ended up cut off.
   */
  const columnsRef = useRef<HTMLDivElement>(null)
  const columnsHeight = useFillHeight(columnsRef, mode !== 'tulis', `${mode}|${loading}|${id ?? 'baru'}`)

  const load = useCallback(async () => {
    setLoading(true)
    setLoadError(null)

    try {
      const [localeList, schema, dto] = await Promise.all([
        fetchActiveLocales(),
        fetchBlockSchema(),
        id ? caseStudyRepository.get(id).then((response) => response.data ?? null) : null,
      ])

      const metricsCount = schema.limits.metricsCount
      setLocales(localeList)
      setLimits(schema.limits)
      setKinds(schema.kinds)

      const defaultLocale = localeList.find((locale) => locale.isDefault) ?? localeList[0]

      if (dto) {
        const nextEntry: EntryDraft = {
          contentKey: dto.contentKey ?? '',
          label: dto.label,
          figure: dto.figure,
          coverMediaId: dto.coverMediaId,
          coverMediaPath: dto.coverMediaPath,
          year: dto.year,
          stack: dto.stack ?? [],
          order: dto.order,
        }
        const nextDrafts = toDrafts(dto, localeList, metricsCount)

        setEntry(nextEntry)
        setDrafts(nextDrafts)
        setMeta({
          createdDate: dto.createdDate,
          updatedDate: dto.updatedDate,
          createdBy: dto.createdBy,
          updatedBy: dto.updatedBy,
        })
        setBaseline(snapshot(nextEntry, nextDrafts, localeList))

        const firstFilled =
          localeList.find((locale) => nextDrafts[locale.code]?.enabled) ?? defaultLocale
        setActive(firstFilled?.code ?? '')
      } else {
        const nextEntry = emptyEntry()
        const nextDrafts: Record<string, TranslationDraft> = {}
        for (const locale of localeList) {
          nextDrafts[locale.code] = emptyTranslation(metricsCount)
        }
        // The default language starts open: a new entry with every tab empty is
        // a form asking a question nobody needed asked.
        if (defaultLocale) nextDrafts[defaultLocale.code]!.enabled = true

        setEntry(nextEntry)
        setDrafts(nextDrafts)
        setBaseline(snapshot(nextEntry, nextDrafts, localeList))
        setActive(defaultLocale?.code ?? '')
      }
    } catch (caught) {
      setLoadError(caught)
    } finally {
      setLoading(false)
    }
  }, [id])

  useEffect(() => {
    void load()
  }, [load])

  const current = active ? drafts[active] : undefined
  const activeLocale = locales.find((locale) => locale.code === active)

  const patch = (localeCode: string, changes: Partial<TranslationDraft>) =>
    setDrafts((previous) => {
      const existing = previous[localeCode]
      if (!existing) return previous
      return { ...previous, [localeCode]: { ...existing, ...changes } }
    })

  const dirty = useMemo(
    () => baseline !== '' && snapshot(entry, drafts, locales) !== baseline,
    [baseline, entry, drafts, locales],
  )

  const preview = useMemo(
    () => ({
      label: entry.label,
      figure: entry.figure,
      year: entry.year,
      stack: entry.stack.filter(Boolean),
      coverPath: entry.coverMediaPath,
      coverAlt: current?.coverAlt ?? '',
      title: current?.title ?? '',
      summary: current?.summary ?? '',
      problem: current?.problem ?? '',
      client: current?.client ?? '',
      kind: current?.kind ?? '',
      duration: current?.duration ?? '',
      role: current?.role ?? '',
      metrics: current?.metrics ?? [],
      body: current?.body ?? [],
    }),
    [entry, current],
  )

  /*
   * The preview renders behind the keystroke, never in front of it. Thirty-six
   * blocks re-render on every character otherwise, and the first thing an
   * editor would notice about a live preview is that typing got heavier.
   */
  const deferredPreview = useDeferredValue(preview)

  const errors = useMemo(() => validate(drafts, locales), [drafts, locales])
  const errorsFor = (localeCode: string) => errors[localeCode] ?? {}
  const currentErrors = showErrors ? errorsFor(active) : {}

  const headingTitle = useMemo(() => {
    if (isCreate) return 'Studi kasus baru'
    const withTitle = locales
      .map((locale) => drafts[locale.code])
      .find((draft) => draft?.enabled && draft.title)
    return withTitle?.title || 'Tanpa judul'
  }, [drafts, isCreate, locales])

  usePageMeta({
    title: isCreate ? 'Studi kasus baru' : headingTitle,
    description: 'Sunting studi kasus.',
    path: isCreate ? panelRoutes.caseStudyCreate : panelRoutes.caseStudyEdit(id ?? ''),
    noIndex: true,
  })

  const save = async () => {
    const invalidLocale = locales.find(
      (locale) => Object.keys(errorsFor(locale.code)).length > 0,
    )

    if (invalidLocale) {
      setShowErrors(true)
      setActive(invalidLocale.code)
      toast.error('Ada isian yang perlu diperbaiki sebelum disimpan.')
      return
    }

    setSaving(true)
    setServerErrors([])

    try {
      const param = buildParam(entry, drafts, locales)

      if (isCreate) {
        const response = await caseStudyRepository.create({ data: param })
        toast.success('Studi kasus dibuat')
        const newId = response.data
        if (newId) {
          // Replace, so the browser's back button still leads to the list rather
          // than to a create form that would now make a second copy.
          navigate(panelRoutes.caseStudyEdit(newId), { replace: true })
        } else {
          navigate(panelRoutes.caseStudies)
        }
        return
      }

      await caseStudyRepository.update(id!, { data: param })
      toast.success('Perubahan tersimpan')
      setShowErrors(false)
      // Reload rather than trust the local copy: the server normalises slugs,
      // stamps publish dates, and may have renumbered a decision block.
      await load()
    } catch (caught) {
      const messages = getValidationMessages(caught)
      if (messages.length > 0) {
        setServerErrors(messages)
        toast.error('Ditolak server. Lihat daftar di bawah formulir.')
      } else {
        notifyApiError(caught, 'Gagal menyimpan studi kasus.')
      }
    } finally {
      setSaving(false)
    }
  }

  const remove = async () => {
    if (!id) return
    setDeleting(true)
    try {
      await caseStudyRepository.remove(id)
      toast.success('Studi kasus dihapus')
      navigate(panelRoutes.caseStudies, { replace: true })
    } catch (caught) {
      notifyApiError(caught, 'Gagal menghapus studi kasus.')
    } finally {
      setDeleting(false)
      setConfirmingDelete(false)
    }
  }

  if (loading) return <PageLoader />

  if (loadError) {
    return (
      <PanelPage
        eyebrow="Konten"
        title="Studi kasus"
        backTo={{ href: panelRoutes.caseStudies, label: 'Portofolio' }}
      >
        <ErrorState
          error={loadError}
          title="Gagal memuat studi kasus"
          onRetry={() => void load()}
        />
      </PanelPage>
    )
  }

  const readOnly = isCreate ? !permission.canCreate : !permission.canUpdate

  return (
    <PanelPage
      eyebrow={isCreate ? 'Konten · baru' : 'Konten'}
      title={headingTitle}
      lead={
        isCreate
          ? 'Satu proyek, satu halaman. Yang dinilai calon klien adalah keputusannya — bukan daftar fiturnya.'
          : undefined
      }
      backTo={{ href: panelRoutes.caseStudies, label: 'Portofolio' }}
      actions={<EditorModeSwitch mode={mode} onChange={setMode} canSplit={canSplit} />}
      wide={mode !== 'belah'}
      fluid={mode === 'belah'}
    >
      <div
        ref={columnsRef}
        style={columnsHeight ? { height: `${columnsHeight}px` } : undefined}
        className={cn(
          'min-h-0',
          // 75rem, the same width `useEditorMode` unlocks the split at. Using
          // Tailwind's `xl` here would leave a five-rem band where split is
          // offered but the columns have not started — one column stacked on
          // the other inside a fixed height, which is a worse view than either.
          mode === 'belah' &&
            'grid items-stretch gap-7 min-[75rem]:grid-rows-[minmax(0,1fr)] min-[75rem]:grid-cols-[minmax(24rem,0.85fr)_minmax(28rem,1.15fr)]',
        )}
      >
        {/*
          Writing and preview are one screen, not two: the whole point is to see
          the effect of a sentence while it is still being written. The form is
          the thing that scrolls; the preview keeps its own scroll and follows
          the block being edited.
        */}
        {mode === 'pratinjau' ? null : (
          <div
            data-compact={mode === 'belah' ? 'true' : undefined}
            onFocusCapture={(event) => {
              const card = (event.target as HTMLElement).closest('[data-block-id]')
              setFocusBlockId(card?.getAttribute('data-block-id') ?? null)
            }}
            className={cn(
              'form-column grid min-h-0',
              mode === 'belah' ? 'gap-7 overflow-y-auto pr-3' : 'gap-9',
            )}
          >
        <FormSection
          eyebrow="Semua bahasa"
          title="Proyek"
          lead="Fakta yang tidak berubah walau bahasanya berubah."
          note={
            <>
              <p>
                <strong>Label</strong> menentukan apa yang tertulis di kartu portofolio. Produk
                sendiri ditandai jujur sebagai produk sendiri; menyamarkannya sebagai pekerjaan
                klien adalah cara tercepat kehilangan kepercayaan yang sedang dibangun halaman ini.
              </p>
              <p className="mt-3">
                <strong>Urutan</strong> menentukan siapa yang dibaca lebih dulu. Angka kecil tampil
                di atas.
              </p>
            </>
          }
        >
          <SegmentedInput<CaseStudyLabel>
            label="Label"
            hint="Pekerjaan untuk klien, atau produk yang dibuat sendiri."
            value={entry.label}
            onChange={(label) => setEntry((previous) => ({ ...previous, label }))}
            options={[
              { value: 'Client', label: 'Klien' },
              { value: 'OwnProduct', label: 'Produk sendiri' },
            ]}
          />

          <FieldRow>
            <NumberInput
              label="Tahun"
              hint="Tahun proyek dikerjakan."
              value={entry.year}
              min={2000}
              max={2100}
              onChange={(year) => setEntry((previous) => ({ ...previous, year }))}
              required
            />

            <NumberInput
              label="Urutan"
              hint="Posisi di halaman portofolio. Angka kecil lebih dulu."
              value={entry.order}
              min={0}
              onChange={(order) => setEntry((previous) => ({ ...previous, order }))}
            />
          </FieldRow>

          <SegmentedInput<SchematicVariant>
            label="Skema kartu"
            hint="Gambar pengganti pada kartu selama belum ada sampul asli."
            value={entry.figure}
            onChange={(figure) => setEntry((previous) => ({ ...previous, figure }))}
            options={[
              { value: 'System', label: 'Sistem' },
              { value: 'Website', label: 'Website' },
              { value: 'Catalog', label: 'Katalog' },
            ]}
          />

          <StringRepeater
            label="Teknologi"
            hint="Yang benar-benar dipakai. Daftar panjang tidak menambah kepercayaan."
            items={entry.stack}
            onChange={(stack) => setEntry((previous) => ({ ...previous, stack }))}
            placeholder="ASP.NET Core"
            addLabel="Tambah teknologi"
            max={12}
          />

          {isCreate ? (
            <TextInput
              label="Pengenal tetap"
              hint="Dipakai untuk menautkan konten ini antar bahasa. Boleh dikosongkan — akan dibuat dari judul."
              value={entry.contentKey}
              onChange={(contentKey) =>
                setEntry((previous) => ({ ...previous, contentKey: slugify(contentKey) }))
              }
              mono
              maxLength={120}
            />
          ) : (
            <ReadOnlyField label="Pengenal tetap">
              <span className="font-mono">{entry.contentKey || '—'}</span>
              <span className="text-muted"> · tidak diubah setelah entri dibuat</span>
            </ReadOnlyField>
          )}
        </FormSection>

        <FormSection
          eyebrow="Semua bahasa"
          title="Sampul"
          lead="Gambar yang tampil di kartu portofolio dan di atas halaman."
          note={
            <p>
              Gambar dipakai bersama semua bahasa, tetapi <strong>deskripsinya tidak</strong> —
              deskripsi gambar adalah kalimat yang dibaca orang yang tidak bisa melihatnya, jadi ia
              ikut bahasa pembacanya. Isinya ada di tab bahasa di bawah.
            </p>
          }
        >
          <MediaField
            label="Gambar sampul"
            hint="Tangkapan layar yang paling mewakili. Kosongkan untuk memakai skema."
            mediaId={entry.coverMediaId}
            objectPath={entry.coverMediaPath}
            onChange={(media) =>
              setEntry((previous) => ({
                ...previous,
                coverMediaId: media?.id ?? null,
                coverMediaPath: media?.objectPath ?? null,
              }))
            }
          />
        </FormSection>

        <div>
          <LanguageTabs
            locales={locales}
            active={active}
            onChange={setActive}
            state={(code) => ({
              enabled: drafts[code]?.enabled ?? false,
              status: drafts[code]?.status ?? 'Draft',
              hasError: showErrors && Object.keys(errorsFor(code)).length > 0,
            })}
          />

          <div className="pt-10">
            {!current || !activeLocale ? null : !current.enabled ? (
              <EmptyLanguage
                locale={activeLocale}
                onEnable={() => patch(activeLocale.code, { enabled: true })}
                disabled={readOnly}
              />
            ) : (
              <div className="grid gap-14">
                <FormSection
                  eyebrow={`Bahasa ${activeLocale.code.toUpperCase()}`}
                  title="Judul & ringkasan"
                  lead="Yang dibaca calon klien sebelum memutuskan membuka halamannya."
                  note={
                    <>
                      <p>
                        <strong>Masalah</strong> adalah satu kalimat yang harus bisa dikenali calon
                        klien sebagai masalahnya sendiri. Bukan daftar fitur, bukan nama teknologi.
                      </p>
                      <p className="mt-3">
                        <strong>Alamat halaman</strong> ikut bahasa. Setelah terbit, mengubahnya
                        berarti alamat lama harus dialihkan — server melakukannya otomatis, tetapi
                        tautan yang sudah beredar tetap sebaiknya jangan dipatahkan tanpa alasan.
                      </p>
                    </>
                  }
                >
                  <TextInput
                    label="Judul"
                    value={current.title}
                    maxLength={160}
                    required
                    error={currentErrors.title}
                    placeholder="Sistem manajemen penelitian dan pengabdian"
                    onChange={(title) =>
                      patch(activeLocale.code, {
                        title,
                        slug: deriveSlug(title, current.slug, current.slugTouched),
                      })
                    }
                  />

                  <SlugInput
                    value={current.slug}
                    prefix="/portofolio/"
                    published={Boolean(current.publishedSlug)}
                    error={currentErrors.slug}
                    onChange={(slug) =>
                      patch(activeLocale.code, { slug, slugTouched: true })
                    }
                  />

                  <TextAreaInput
                    label="Ringkasan"
                    hint="Dua sampai tiga kalimat. Apa yang dikerjakan dan untuk siapa."
                    value={current.summary}
                    rows={3}
                    maxLength={420}
                    required
                    error={currentErrors.summary}
                    onChange={(summary) => patch(activeLocale.code, { summary })}
                  />

                  <TextAreaInput
                    label="Masalah"
                    hint="Satu kalimat. Ini yang muncul di kartu portofolio."
                    value={current.problem}
                    rows={2}
                    maxLength={280}
                    required
                    error={currentErrors.problem}
                    onChange={(problem) => patch(activeLocale.code, { problem })}
                  />

                  <TextAreaInput
                    label="Deskripsi gambar sampul"
                    hint="Menjelaskan isi gambar, bukan menamainya. Boleh kosong bila belum ada sampul."
                    value={current.coverAlt}
                    rows={2}
                    maxLength={220}
                    error={currentErrors.coverAlt}
                    onChange={(coverAlt) => patch(activeLocale.code, { coverAlt })}
                  />
                </FormSection>

                <FormSection
                  eyebrow={`Bahasa ${activeLocale.code.toUpperCase()}`}
                  title="Fakta proyek"
                  lead="Kolom kecil di samping tulisan. Pendek, spesifik, bisa diperiksa."
                  note={
                    <p>
                      <strong>Metrik</strong> harus tepat tiga angka, dan hanya wajib saat halaman
                      diterbitkan. Tiga adalah jumlah yang masih terbaca sekaligus; lebih dari itu
                      berhenti jadi bukti dan berubah jadi hiasan.
                    </p>
                  }
                >
                  <FieldRow>
                    <TextInput
                      label="Klien"
                      hint="Nama klien, atau “Produk sendiri”."
                      value={current.client}
                      maxLength={160}
                      onChange={(client) => patch(activeLocale.code, { client })}
                    />

                    <TextInput
                      label="Jenis"
                      hint="Misalnya: sistem internal, situs profil."
                      value={current.kind}
                      maxLength={80}
                      onChange={(kind) => patch(activeLocale.code, { kind })}
                    />

                    <TextInput
                      label="Durasi"
                      hint="Misalnya: 3 bulan."
                      value={current.duration}
                      maxLength={80}
                      onChange={(duration) => patch(activeLocale.code, { duration })}
                    />

                    <TextInput
                      label="Peran"
                      hint="Apa yang Anda kerjakan sendiri di proyek ini."
                      value={current.role}
                      maxLength={400}
                      onChange={(role) => patch(activeLocale.code, { role })}
                    />
                  </FieldRow>

                  <Repeater
                    label="Metrik"
                    hint={`Tepat ${limits.metricsCount} angka. Wajib diisi sebelum diterbitkan.`}
                    items={current.metrics}
                    fixedLength
                    error={currentErrors.metrics}
                    create={() => ({ value: '', label: '' })}
                    onChange={(metrics) => patch(activeLocale.code, { metrics })}
                    itemLabel={(_, index) => `Angka ${index + 1}`}
                    renderItem={(item, update) => (
                      <FieldRow columns="10rem minmax(0, 1fr)">
                        <TextInput
                          label="Angka"
                          value={item.value}
                          maxLength={limits.metricValueMaxChars}
                          placeholder="70%"
                          onChange={(value) => update({ ...item, value })}
                        />
                        <TextInput
                          label="Keterangan"
                          value={item.label}
                          maxLength={limits.metricLabelMaxChars}
                          placeholder="waktu rekap berkurang"
                          onChange={(label) => update({ ...item, label })}
                        />
                      </FieldRow>
                    )}
                  />
                </FormSection>

                <FormSection
                  eyebrow={`Bahasa ${activeLocale.code.toUpperCase()}`}
                  title="Tulisan"
                  lead="Isi halaman studi kasus, disusun dari blok."
                  note={
                    <p>
                      Blok <strong>Keputusan</strong> adalah bentuk rumah situs ini: saya memilih X
                      karena Y, walaupun Z. Bagian “walaupun” wajib — keputusan tanpa alternatif
                      yang ditolak hanyalah selera.
                    </p>
                  }
                >
                  <BlockEditor
                    value={current.body}
                    kinds={kinds}
                    limits={limits}
                    onChange={(body) => patch(activeLocale.code, { body })}
                  />
                </FormSection>

                <FormSection
                  eyebrow={`Bahasa ${activeLocale.code.toUpperCase()}`}
                  title="Status"
                  lead="Terbit berarti halaman ini bisa dibuka siapa pun yang tahu alamatnya."
                  note={
                    <p>
                      Status berlaku per bahasa. Versi Indonesia bisa terbit sementara versi Inggris
                      masih draf — itu memang yang diinginkan, bukan kelalaian.
                    </p>
                  }
                >
                  <SegmentedInput<ContentStatus>
                    label="Status halaman"
                    value={current.status}
                    onChange={(status) => patch(activeLocale.code, { status })}
                    options={[
                      { value: 'Draft', label: 'Draf' },
                      { value: 'Published', label: 'Terbit' },
                    ]}
                  />

                  {current.publishedAt ? (
                    <ReadOnlyField label="Pertama terbit">
                      {formatDate(current.publishedAt)}
                    </ReadOnlyField>
                  ) : null}
                </FormSection>
              </div>
            )}
          </div>
        </div>

        {meta ? (
          <div className="border-line text-muted type-small grid gap-1.5 border-t pt-6">
            <p>
              Dibuat {formatDate(meta.createdDate)} oleh {meta.createdBy}
            </p>
            {meta.updatedDate ? (
              <p>
                Diperbarui {formatDate(meta.updatedDate)}
                {meta.updatedBy ? ` oleh ${meta.updatedBy}` : ''}
              </p>
            ) : null}
          </div>
        ) : null}

        {serverErrors.length > 0 ? (
          <div className="border-danger max-w-[var(--measure)] border-l-2 pl-4">
            <Label as="p" className="text-danger">
              Ditolak server
            </Label>
            <ul className="text-muted mt-2 grid gap-1.5">
              {serverErrors.map((message) => (
                <li key={message}>{message}</li>
              ))}
            </ul>
          </div>
        ) : null}

        {!isCreate && permission.canDelete ? (
          <div className="border-line border-t pt-6">
            <button
              type="button"
              onClick={() => setConfirmingDelete(true)}
              className="text-muted hover:text-danger"
            >
              Hapus studi kasus ini
            </button>
          </div>
        ) : null}
          </div>
        )}

        {mode === 'tulis' ? null : (
          <PreviewPane
            focusBlockId={focusBlockId}
            dirty={dirty}
            className="h-full min-h-0"
          >
            <CaseStudyPreview data={deferredPreview} />
          </PreviewPane>
        )}
      </div>

      {readOnly ? null : (
        <SaveBar
          dirty={dirty}
          saving={saving}
          onSave={() => void save()}
          saveLabel={isCreate ? 'Simpan studi kasus' : 'Simpan perubahan'}
        >
          {current?.enabled && activeLocale ? (
            <StatusChip
              status={current.status}
              label={`${activeLocale.code.toUpperCase()} · ${current.status === 'Published' ? 'terbit' : 'draf'}`}
            />
          ) : null}
        </SaveBar>
      )}

      <ConfirmDialog
        open={confirmingDelete}
        title="Hapus studi kasus ini?"
        body="Halamannya akan hilang dari situs dalam semua bahasa. Alamat lamanya akan menjawab 404."
        busy={deleting}
        onCancel={() => setConfirmingDelete(false)}
        onConfirm={() => void remove()}
      />
    </PanelPage>
  )
}

/* ---------------------------------------------------------------- helpers */

function EmptyLanguage({
  locale,
  onEnable,
  disabled,
}: {
  readonly locale: LocaleDTO
  readonly onEnable: () => void
  readonly disabled?: boolean
}) {
  return (
    <div className="max-w-[var(--measure)]">
      <h2 className="type-h3">Belum ada versi {locale.nativeName}</h2>
      <p className="text-muted mt-3">
        Menambahkan bahasa berarti menulis ulang isinya, bukan menyalinnya. Judul, alamat halaman,
        dan tulisannya berdiri sendiri — termasuk alamatnya, supaya pembaca bahasa ini mendapat
        tautan dalam bahasanya sendiri.
      </p>

      <div className="mt-6">
        <Button type="button" variant="secondary" onClick={onEnable} disabled={disabled}>
          Tambahkan versi {locale.code.toUpperCase()}
        </Button>
      </div>
    </div>
  )
}

/** Everything that would be sent, serialised — the cheapest honest dirty check. */
function snapshot(
  entry: EntryDraft,
  drafts: Record<string, TranslationDraft>,
  locales: readonly LocaleDTO[],
): string {
  return JSON.stringify(buildParam(entry, drafts, locales))
}

function buildParam(
  entry: EntryDraft,
  drafts: Record<string, TranslationDraft>,
  locales: readonly LocaleDTO[],
): CaseStudyParam {
  const clean = (value: string): string | null => {
    const trimmed = value.trim()
    return trimmed.length > 0 ? trimmed : null
  }

  return {
    contentKey: clean(entry.contentKey),
    label: entry.label,
    figure: entry.figure,
    coverMediaId: entry.coverMediaId,
    year: entry.year,
    stack: entry.stack.map((item) => item.trim()).filter(Boolean),
    order: entry.order,
    translations: locales
      .filter((locale) => drafts[locale.code]?.enabled)
      .map((locale) => {
        const draft = drafts[locale.code]!

        return {
          localeCode: locale.code,
          slug: clean(draft.slug),
          status: draft.status,
          title: clean(draft.title),
          summary: clean(draft.summary),
          problem: clean(draft.problem),
          client: clean(draft.client),
          kind: clean(draft.kind),
          duration: clean(draft.duration),
          role: clean(draft.role),
          coverAlt: clean(draft.coverAlt),
          metrics: draft.metrics.map((metric) => ({
            value: metric.value.trim(),
            label: metric.label.trim(),
          })),
          body: draft.body,
        }
      }),
  }
}

type FieldErrors = Partial<
  Record<'title' | 'slug' | 'summary' | 'problem' | 'coverAlt' | 'metrics', string>
>

/**
 * The same rules the server enforces, checked before the request leaves.
 *
 * Not a substitute for the server's copy — that one is the authority and runs
 * regardless. This one exists so the answer arrives while the editor is still
 * looking at the field.
 */
function validate(
  drafts: Record<string, TranslationDraft>,
  locales: readonly LocaleDTO[],
): Record<string, FieldErrors> {
  const result: Record<string, FieldErrors> = {}

  for (const locale of locales) {
    const draft = drafts[locale.code]
    if (!draft?.enabled) continue

    const errors: FieldErrors = {}

    if (!draft.title.trim()) errors.title = 'Judul wajib diisi.'
    if (!draft.summary.trim()) errors.summary = 'Ringkasan wajib diisi.'
    if (!draft.problem.trim()) {
      errors.problem = 'Masalah wajib diisi — ini kalimat yang muncul di kartu portofolio.'
    }

    if (draft.slug.trim() && slugify(draft.slug) !== draft.slug.trim()) {
      errors.slug = 'Hanya huruf kecil, angka, dan tanda hubung.'
    }

    const alt = draft.coverAlt.trim()
    if (alt.length > 0 && alt.length < 10) {
      errors.coverAlt = 'Minimal 10 karakter — jelaskan isinya, jangan menamainya.'
    }

    if (draft.status === 'Published') {
      const incomplete = draft.metrics.some(
        (metric) => !metric.value.trim() || !metric.label.trim(),
      )
      if (incomplete) errors.metrics = 'Semua metrik harus terisi sebelum halaman diterbitkan.'
    }

    if (Object.keys(errors).length > 0) result[locale.code] = errors
  }

  return result
}
