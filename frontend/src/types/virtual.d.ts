declare module 'virtual:leksana-reading-time' {
  /** Reading estimates keyed by `folder/slug`. Built by server/reading-time.ts. */
  export const readingTimes: Record<string, number>
}
