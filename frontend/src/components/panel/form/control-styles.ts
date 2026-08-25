import { cn } from '@/lib/cn'

/** Shared look for every text-like control in the panel. */
export const controlClasses = (hasError?: boolean, extra?: string) =>
  cn(
    'bg-surface w-full rounded-[var(--radius-control)] border px-3 py-2',
    'transition-colors duration-150 ease-out',
    'focus:border-accent focus:ring-accent/20 focus:ring-3 focus:outline-none',
    hasError ? 'border-danger' : 'border-muted',
    extra,
  )
