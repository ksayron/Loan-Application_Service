import axios from 'axios'
import type { ApplicationDto, CreateApplicationDto } from '@/types/application'

const api = axios.create({ baseURL: '/api' })

export const getApplications = async (params = {}) => {
  const res = await api.get('/applications', { params })
  const data = res.data

  if (Array.isArray(data)) return data
  // if backend wraps the array into { items: [...] }:
  if (data && Array.isArray(data.items)) return data.items
  // if backend returns { data: [...] }:
  if (data && Array.isArray(data.data)) return data.data

  // Otherwise fallback to empty array and log
  console.warn('getApplications: unexpected response shape', data)
  return []

}

export const createApplication = async (dto: CreateApplicationDto) => {
  const res = await api.post<ApplicationDto>('/applications', dto)
  return res.data
}

export const toggleStatus = async (number: string) => {
  const res = await api.patch<ApplicationDto>(`/applications/${encodeURIComponent(number)}/status`)
  return res.data
}
