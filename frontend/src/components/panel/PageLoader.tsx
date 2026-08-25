import { Label } from '@/components/ui/Label'

/**
 * Full-page wait state. A rule that fills, not a spinner — the same vocabulary
 * the rest of the site uses to show structure.
 */
export function PageLoader({ message = 'Memuat…' }: { readonly message?: string }) {
  return (
    <div className="flex min-h-dvh items-center justify-center p-6">
      <div className="w-full max-w-xs text-center">
        <div className="bg-line relative mx-auto h-px w-full overflow-hidden">
          <span className="bg-accent absolute inset-y-0 left-0 w-1/3 animate-[loader-sweep_1.1s_ease-in-out_infinite]" />
        </div>
        <Label as="p" className="mt-4">
          {message}
        </Label>
      </div>
    </div>
  )
}
