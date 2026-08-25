import { cn } from '@/lib/cn'

/** Hairline divider. Structure here is made of rules and space, not shadows. */
export function Rule({ className }: { readonly className?: string }) {
  return <hr className={cn('rule', className)} />
}
