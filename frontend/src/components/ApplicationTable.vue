<template>
  <div class="loan-table">
    <div class="header_table offer">
      <div v-for="h in headers"
           :key="h.field"
           class="header_arrow_block"
           :class="h.class"
           :data-sort-field="h.field"
           @click="onHeaderClick(h.field)"
           role="button"
           :aria-pressed="sortField === h.field ? 'true' : 'false'"
           tabindex="0"
           @keyup.enter="onHeaderClick(h.field)">
        <div class="header-label" v-html="h.label"></div>
        <div class="img-arrow" :class="{ asc: isSortedAsc(h.field), desc: isSortedDesc(h.field) }"></div>
      </div>
    </div>

    <div class="table_req__items">
      <div v-for="app in sortedItems" :key="app.number" class="items offer">
        <div v-for="h in headers"
             :key="`${app.number}-${h.field}`"
             class="item"
             :class="h.class">
          <div class="value">
            {{ cellText(app, h.field) }}
            <span v-if="h.field === 'interestValue'" > в день</span>
          </div>
        </div>

        <div class="details-table">
          <div class="buttons">
            <el-button :loading="loadingSet.has(app.number)"
                       @click="onToggle(app.number)"
                       size="small"
                       :class="buttonClass(app)">
              {{ actionButtonText(app) }}
            </el-button>
          </div>
        </div>
      </div>

      <div v-if="items.length === 0" class="empty">
        <el-empty description="No applications" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed, ref, watch } from 'vue'
  import { useApplicationsStore } from '../stores/applications'
  import type { ApplicationDto } from '../types/application'
  import { EApplicationStatus } from '../types/application'
  import { formatCurrency } from '../utils/format'


 

  const props = defineProps<{ items?: ApplicationDto[] }>()
  const store = useApplicationsStore()


  const items = computed<ApplicationDto[]>(() => {
    if (props.items && Array.isArray(props.items)) {
      return props.items
    }
    return store.applications
  })

  const sortField = ref<string | null>(null)
  const sortDir = ref<'asc' | 'desc'>('asc')

  const loadingSet = ref(new Set<string>())

  const headers = [
    { field: 'number', label: 'Номер\nпредложения', class: 'docNum' },
    { field: 'amount', label: 'Сумма\nзайма, BYN', class: 'amount' },
    { field: 'amountToRepay', label: 'Сумма\nк возврату, BYN', class: 'amountToRepay' },
    { field: 'interestAmount', label: 'Сумма\nпроцентов, BYN', class: 'InterestAmount' },
    { field: 'termValue', label: 'Срок', class: 'termValue' },
    { field: 'interestValue', label: 'Процентная\nставка', class: 'interest' },
  ]
  function cellText(app: ApplicationDto, field: string): string {
    if (field === 'number') return String(app.number || '—')
    if (field === 'amount') return formatCurrency(Number((app as any).amount) || 0)
    if (field === 'amountToRepay') return formatCurrency(amountToRepay(app))
    if (field === 'interestAmount') return formatCurrency(interestAmount(app))
    if (field === 'termValue') return `${app.termValue ?? '—'} дн.`
    if (field === 'interestValue') {
      const r = Number((app as any).interestValue)
      return Number.isFinite(r) ? `${r} %` : '—'
    }
    if (field === 'creditScore') return (app as any).creditScore ? `от ${(app as any).creditScore}` : '—'
    return '—'
  }

  
  function interestAmount(app: ApplicationDto): number {
    const amt = Number(app.amount) || 0
    const rate = Number(app.interestValue) || 0
    const term = Number(app.termValue) || 0
    const interest = amt * (rate / 100) * term
    return Math.round((interest + Number.EPSILON) * 100) / 100
  }

  function amountToRepay(app: ApplicationDto): number {
    const principal = Number(app.amount) || 0
    const interest = interestAmount(app)
    const total = principal + interest
    return Math.round((total + Number.EPSILON) * 100) / 100
  }

  function formatCurrencyNumber(value: number | string | undefined): string {
    const num = Number(value) || 0
    return formatCurrency(num)
  }

  function formatInterest(rate: number | string | undefined): string {
    const r = Number(rate)
    if (Number.isNaN(r)) {
      return '—'
    }
    const fixed = r.toString()
    return fixed + ' %'
  }


  function isPublished(app: ApplicationDto): boolean {
    const s = (app as any).status
    if (s === EApplicationStatus.Published) return true
    if (s === 'Published') return true
    if (typeof s === 'number' && s === +EApplicationStatus.Published) return true
    return false
  }


  function actionButtonText(app: ApplicationDto): string {
    return isPublished(app) ? 'Снять с публикации' : 'Опубликовать'
  }
  function buttonClass(app: ApplicationDto) {
    return isPublished(app) ? 'publish-btn' : 'unpublish-btn'
  }

  async function onToggle(number: string) {
    try {
      loadingSet.value.add(number)
      await store.toggleStatus(number)
    } catch (e) {
      console.error('toggle error', e)
    } finally {
      loadingSet.value.delete(number)
    }
  }


  function onHeaderClick(field: string) {
    if (sortField.value === field) {
      if (sortDir.value === 'asc') {
        sortDir.value = 'desc'
      } else {
        sortDir.value = 'asc'
      }
    } else {
      sortField.value = field
      sortDir.value = 'asc'
    }
  }

  function isSortedAsc(field: string): boolean {
    return sortField.value === field && sortDir.value === 'asc'
  }
  function isSortedDesc(field: string): boolean {
    return sortField.value === field && sortDir.value === 'desc'
  }

  const sortedItems = computed<ApplicationDto[]>(() => {
    const arr = items.value.slice() // copy

    if (!sortField.value) {
      return arr
    }

    const field = sortField.value
    const dir = sortDir.value

    arr.sort((a: ApplicationDto, b: ApplicationDto) => {
      let va: any = null
      let vb: any = null

      if (field === 'number') {
        va = a.number ?? ''
        vb = b.number ?? ''
      } else if (field === 'amount') {
        va = Number(a.amount) || 0
        vb = Number(b.amount) || 0
      } else if (field === 'amountToRepay') {
        va = amountToRepay(a)
        vb = amountToRepay(b)
      } else if (field === 'interestAmount') {
        va = interestAmount(a)
        vb = interestAmount(b)
      } else if (field === 'termValue') {
        va = Number(a.termValue) || 0
        vb = Number(b.termValue) || 0
      } else if (field === 'interestValue') {
        va = Number(a.interestValue) || 0
        vb = Number(b.interestValue) || 0
      } else {
        va = a.number ?? ''
        vb = b.number ?? ''
      }

      let cmp = 0
      if (typeof va === 'string' && typeof vb === 'string') {
        if (va < vb) cmp = -1
        else if (va > vb) cmp = 1
        else cmp = 0
      } else {
        if (va < vb) cmp = -1
        else if (va > vb) cmp = 1
        else cmp = 0
      }

      if (dir === 'asc') return cmp
      return -cmp
    })

    return arr
  })
</script>

<style scoped>
  .loan-table {
    font-family: var(--bs-font-sans-serif, Arial, sans-serif);
    color: var(--bs-body-color, #212529);
    border: 1px solid var(--bs-border-color);
  }

  .header_table {
    display: flex;
    gap: 70px;
    align-items: center;
    padding: 6px 12px;
    background-color: white;
    border-radius: 8px;
    font-weight: 600;
    margin-bottom: 12px;
  }

  .header_arrow_block,
  .items.offer .item {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 12px;
    box-sizing: border-box;
  }

    .header_arrow_block.docNum,
    .items.offer .item.docNum {
      flex: 0 0 130px;
      min-width: 110px;
    }

    .header_arrow_block.amount,
    .items.offer .item.amount {
      flex: 0 0 140px;
      min-width: 120px;
      text-align: right;
    }

    .header_arrow_block.amountToRepay,
    .items.offer .item.amountToRepay {
      flex: 0 0 160px;
      min-width: 140px;
      text-align: right;
    }

    .header_arrow_block.InterestAmount,
    .items.offer .item.InterestAmount {
      flex: 0 0 160px;
      min-width: 140px;
      text-align: right;
    }

    .header_arrow_block.termValue,
    .items.offer .item.termValue {
      flex: 0 0 80px;
      min-width: 60px;
      text-align: center;
    }

    .header_arrow_block.interest,
    .items.offer .item.interest {
      flex: 0 0 120px;
      min-width: 100px;
      text-align: right;
    }

    .header_arrow_block.creditScore,
    .items.offer .item.creditScore {
      flex: 0 0 100px;
      min-width: 80px;
      text-align: center;
    }
    .header_arrow_block .img-arrow {
      width: 12px;
      height: 12px;
      border-left: 6px solid transparent;
      border-right: 6px solid transparent;
      border-top: 8px solid var(--bs-secondary, #6c757d);
      transition: transform .18s ease, opacity .12s ease;
      opacity: .8;
    }

      .header_arrow_block .img-arrow.asc {
        transform: rotate(180deg);
      }

      .header_arrow_block .img-arrow.desc {
        transform: rotate(0deg);
      }

  .table_req__items {
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  .items.offer {
    display: flex;
    gap: 70px;
    align-items: center;
    padding: 12px;
    background: white;
    border-radius: 8px;
    border: 1px solid var(--bs-border-color, #dee2e6);
  }

  .item {
    display: flex;
    flex-direction: column;
    gap: 6px;
  }

    .item .label {
      font-size: 12px;
      color: var(--bs-secondary);
      display: none;
    }

    .item .value {
      font-size: 14px;
      font-weight: 600;
      color: var(--bs-body-color);
    }


  .details-table {
    margin-left: auto;
    display: flex;
    flex-direction: column;
    gap: 8px;
    align-items: flex-end;
  }

    .details-table .buttons {
      display: flex;
      gap: 8px;
    }

  .publish-btn {
    background: transparent;
    color: var(--bs-primary);
    font-size: 16px;
    font-weight: 600;
    border: 1px solid var(--bs-primary);
    border-radius: 8px;
    padding: 4px 12px;
  }

  .unpublish-btn {
    
    background: var(--bs-primary);
    color: white;
    font-size:16px;
    font-weight:600;
    border: 1px solid var(--bs-primary);
    border-radius: 8px;
    padding: 4px 12px;
  }
</style>
