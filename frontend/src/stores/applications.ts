import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { type ApplicationDto, type CreateApplicationDto, EApplicationStatus } from '../types/application'
import * as api from '../services/api'

type Filters = {
  status: EApplicationStatus | string | null
  minAmount: number | null
  maxAmount: number | null
  minTerm: number | null
  maxTerm: number | null
}

export const useApplicationsStore = defineStore('applications', () => {
  const applications = ref<ApplicationDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const filters = ref<Filters>({
    status: null,
    minAmount: null,
    maxAmount: null,
    minTerm: null,
    maxTerm: null
  })

  const total = computed(() => applications.value.length)

  function buildQueryParams(f: Filters) {
    const p: Record<string, unknown> = {}
    if (f.status != null && f.status !== '') p.status = f.status
    if (f.minAmount != null) p.minAmount = f.minAmount
    if (f.maxAmount != null) p.maxAmount = f.maxAmount
    if (f.minTerm != null) p.minTerm = f.minTerm
    if (f.maxTerm != null) p.maxTerm = f.maxTerm
    return p
  }

  async function fetchApplications() {
    loading.value = true
    error.value = null
    try {
      const params = buildQueryParams(filters.value)
      const list = await api.getApplications(params)

      applications.value = list
    } catch (e: any) {
      error.value = e?.response?.data?.message ?? e?.message ?? 'Failed to fetch applications'
      console.error('fetchApplications error', e)
    } finally {
      loading.value = false
    }
  }

  async function createApplication(dto: CreateApplicationDto) {
    loading.value = true
    error.value = null
    try {
      const created = await api.createApplication(dto)
      applications.value.unshift(created)
      return created
    } catch (e: any) {
      error.value = e?.response?.data?.message ?? e?.message ?? 'Failed to create application'
      console.error('createApplication error', e)
      throw e
    } finally {
      loading.value = false
    }
  }

 
  async function toggleStatus(number: string) {
    error.value = null

    const idx = applications.value.findIndex(a => a.number === number)
    if (idx === -1) {
      error.value = 'Application not found locally'
      return
    }

    const current = applications.value[idx]
    if (!current) {
      error.value = 'Application not found'
      return
    }

    const original: ApplicationDto = { ...current }

    let newStatus: ApplicationDto['status']
    if (current.status === EApplicationStatus.Published || current.status === 'Published') {
      newStatus = EApplicationStatus.Unpublished
    } else {
      newStatus = EApplicationStatus.Published
    }
    applications.value[idx] = {
      ...current,
      status: newStatus,
      modifiedAt: new Date().toISOString()
    }

    try {
      const updated = await api.toggleStatus(number)
      applications.value[idx] = updated
      return updated
    } catch (e: any) {
      
      applications.value[idx] = original
      error.value = e?.response?.data?.message ?? e?.message ?? 'Failed to toggle status'
      throw e
    }
  }

  function setFilters(next: Partial<Filters>) {
    filters.value = { ...filters.value, ...next }
  }

  function clearFilters() {
    filters.value = {
      status: null,
      minAmount: null,
      maxAmount: null,
      minTerm: null,
      maxTerm: null
    }
  }

  return {
    applications,
    loading,
    error,
    filters,
    total,
    fetchApplications,
    createApplication,
    toggleStatus,
    setFilters,
    clearFilters
  }
})
