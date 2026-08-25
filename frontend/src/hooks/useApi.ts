import { useCallback, useEffect, useRef, useState } from 'react'

export interface UseApiOptions<TData, TArgs extends unknown[], TError = unknown> {
  onSuccess?: (data: TData, ...args: TArgs) => void
  onError?: (error: TError, ...args: TArgs) => void
  onMutate?: (...args: TArgs) => void
}

/**
 * Loading / error / data around one async call.
 *
 * The callbacks live in a ref so an inline `onSuccess` — the normal way to
 * write one — does not change `execute`'s identity and re-trigger every effect
 * that depends on it.
 */
export function useApi<TData, TArgs extends unknown[], TError = unknown>(
  apiFunction: (...args: TArgs) => Promise<TData>,
  options?: UseApiOptions<TData, TArgs, TError>,
) {
  const [data, setData] = useState<TData | null>(null)
  const [error, setError] = useState<TError | null>(null)
  const [isLoading, setIsLoading] = useState(false)

  const optionsRef = useRef(options)

  useEffect(() => {
    optionsRef.current = options
  }, [options])

  const execute = useCallback(
    async (...args: TArgs): Promise<TData | undefined> => {
      try {
        setIsLoading(true)
        setError(null)
        optionsRef.current?.onMutate?.(...args)

        const response = await apiFunction(...args)

        setData(response)
        optionsRef.current?.onSuccess?.(response, ...args)
        return response
      } catch (caught) {
        setError(caught as TError)
        optionsRef.current?.onError?.(caught as TError, ...args)
        return undefined
      } finally {
        setIsLoading(false)
      }
    },
    [apiFunction],
  )

  return { execute, data, error, isLoading, setData, setError }
}
