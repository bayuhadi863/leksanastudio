import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { getErrorMessage } from '@/lib/api-error'

type Props = {
  readonly error: unknown
  readonly title?: string
  readonly onRetry?: () => void
}

/** Says what failed and offers the one action that can fix it. Nothing else. */
export function ErrorState({ error, title = 'Terjadi kesalahan', onRetry }: Props) {
  return (
    <div className="border-line max-w-[var(--measure)] border-t pt-6">
      <Label as="p" className="text-danger">
        Gagal
      </Label>
      <h2 className="type-h3 mt-3">{title}</h2>
      <p className="text-muted mt-3">{getErrorMessage(error)}</p>

      {onRetry ? (
        <div className="mt-6">
          <Button variant="secondary" onClick={onRetry}>
            Coba lagi
          </Button>
        </div>
      ) : null}
    </div>
  )
}
