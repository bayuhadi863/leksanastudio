/**
 * Minimal class-name joiner. No dependency needed for the amount of
 * conditional styling this site has.
 */
export type ClassValue = string | false | null | undefined

export const cn = (...values: ClassValue[]): string => values.filter(Boolean).join(' ')
