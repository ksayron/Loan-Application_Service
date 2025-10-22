import { EApplicationStatus } from '../types/application'

export function formatDate(isoOrDate: string | Date | undefined | null): string {
  if (!isoOrDate) return ''
  const d = typeof isoOrDate === 'string' ? new Date(isoOrDate) : isoOrDate
  try {
    return new Intl.DateTimeFormat(undefined, {
      year: 'numeric',
      month: 'short',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    }).format(d)
  } catch {
    return d.toLocaleString()
  }
}

export function formatCurrency(value: number | null | undefined): string {
  if (value == null) return ''
  try {
    return new Intl.NumberFormat(undefined, { style: 'currency', currency: 'BYN', maximumFractionDigits: 2 }).format(value)
  } catch {
    return String(value)
  }
}

export function formatStatus(status: EApplicationStatus | string | number): string {
  if (typeof status === 'string') {
    return status
  }

  if ((status as number) === (EApplicationStatus as any).Published) return 'Published'
  if ((status as number) === (EApplicationStatus as any).Unpublished) return 'Unpublished'
  return String(status)
}
