<template>
  <form class="border-div form filter" @submit.prevent="apply">
    <div class="filter-collaps">
      <div class="filter-row"
           :aria-expanded="isCollapsed ? 'false' : 'true'"
           @click="toggleCollapse">
        <div class="filter-title">
          <svg width="28" height="28" viewBox="0 0 28 28" fill="none" xmlns="http://www.w3.org/2000/svg" style="stroke: currentcolor">
            <path stroke="currentColor" d="M2.79844 1.40039H25.1984C25.5697 1.40039 25.9258 1.54789 26.1884 1.81044C26.4509 2.07299 26.5984 2.42909 26.5984 2.80039V5.02079C26.5984 5.39206 26.4508 5.7481 26.1882 6.01059L17.2072 14.9902C16.9452 15.2529 16.7981 15.6089 16.7984 15.98V24.807C16.7984 25.0198 16.7499 25.2298 16.6566 25.421C16.5632 25.6123 16.4275 25.7797 16.2597 25.9107C16.092 26.0416 15.8966 26.1325 15.6884 26.1766C15.4802 26.2207 15.2647 26.2167 15.0582 26.165L12.2582 25.465C11.9555 25.3891 11.6868 25.2143 11.4948 24.9683C11.3027 24.7222 11.1984 24.4191 11.1984 24.107V15.98C11.1984 15.6087 11.0508 15.2527 10.7882 14.9902L1.80724 6.01059C1.54518 5.74787 1.39815 5.39186 1.39844 5.02079V2.80039C1.39844 2.42909 1.54594 2.07299 1.80849 1.81044C2.07104 1.54789 2.42713 1.40039 2.79844 1.40039Z" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
          </svg>
          Подобрать займ
        </div>

        <div class="filter-arrow" :class="{ rotated: !isCollapsed }">
          <svg width="18" height="18" viewBox="0 0 24 24">
            <path d="M6 9l6 6 6-6" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
          </svg>
        </div>
      </div>
    </div>

    <div class="collapse" :class="{ show: !isCollapsed }" id="collapseFilter">
      <div class="sub-form min-filter" id="filter-content">

        <div v-if="!fullOpen" class="filer-firs-block min-filter">
          <div class="filter-block">
            <label>Сумма, BYN</label>
            <div class="filter-block-in min-filter">
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.amountMin"
                         id="amountMinFilterFrom"
                         class="my_input"
                         placeholder="От 50 до 3000"
                         type="number"
                         min="0" />
                </div>
                <div class="error-text" v-if="errors.amount">{{ errors.amount }}</div>
              </div>
            </div>
          </div>

          <div class="filter-block">
            <label>Срок займа, день</label>
            <div class="filter-block-in min-filter">
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.termMin"
                         id="termMinFilterFrom"
                         class="my_input"
                         placeholder="От 3 до 60"
                         type="number"
                         min="0" />
                </div>
                <div class="error-text" v-if="errors.term">{{ errors.term }}</div>
              </div>
            </div>
          </div>

          <div class="filter-block buttons">
            <button type="button" class="btn apply-link" id="btnApplyMinFilter" @click="apply">Подобрать</button>
            <button type="button" class="btn btn-secondary apply-link" id="btnClearMinFilter" @click="clear">Сбросить</button>
          </div>
        </div>

        <div class="filter-open collapsed" @click="toggleFull">
          <span v-if="!fullOpen">Ещё фильтры</span>
          <span v-else>Скрыть</span>
        </div>
      </div>

      <div class="sub-form" v-show="fullOpen" id="collapseFullFilter">
        <div class="filer-firs-block">
          <div class="filter-block">
            <label>Сумма, BYN</label>
            <div class="filter-block-in">
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.amountFrom" id="amountFrom" class="my_input" placeholder="От 50" type="number" />
                </div>
              </div>
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.amountTo" id="amountTo" class="my_input" placeholder="До 3000" type="number" />
                </div>
              </div>
            </div>
          </div>

          <div class="filter-block">
            <label>Ставка, % в день</label>
            <div class="filter-block-in">
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.interestFrom" id="interestFrom" class="my_input" placeholder="От" type="number" step="0.01" />
                </div>
              </div>
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.interestTo" id="interestTo" class="my_input" placeholder="До" type="number" step="0.01" />
                </div>
              </div>
            </div>
          </div>

          <div class="filter-block term">
            <label>Срок займа, день</label>
            <div class="filter-block-in">
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.termFrom" id="termFrom" class="my_input" placeholder="От 3" type="number" />
                </div>
              </div>
              <div class="m-input-block">
                <div class="m-input">
                  <input v-model.number="local.termTo" id="termTo" class="my_input" placeholder="До 60" type="number" />
                </div>
              </div>
            </div>
          </div>

        </div>

        <div class="filer-firs-block" style="margin-top: 40px">
          <div class="filter-block">
            <div class="filter-block-in">
              <button id="btnApplyFilter" data-is-filter="true" class="btn apply-link" @click="apply">Подобрать</button>
              <button id="btnClearFilter" class="btn btn-secondary apply-link" @click="clear">Сбросить</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </form>
</template>
<script setup lang="ts">
import { reactive, ref, watch } from 'vue'
import { useApplicationsStore } from '../stores/applications'

const store = useApplicationsStore()

const local = reactive({
  amountMin: null as number | null,
  termMin: null as number | null,
  amountFrom: null as number | null,
  amountTo: null as number | null,
  interestFrom: null as number | null,
  interestTo: null as number | null,
  termFrom: null as number | null,
  termTo: null as number | null,
  creditFrom: null as number | null,
  creditTo: null as number | null
})

const errors = reactive<{ [k: string]: string | null }>({
  amount: null,
  term: null
})

const isCollapsed = ref(false) // top collapse
const fullOpen = ref(false) // full filter expanded

watch(
  () => store.filters,
  (next) => {
    local.amountFrom = (next.minAmount as number) ?? null
    local.amountTo = (next.maxAmount as number) ?? null
    local.termFrom = (next.minTerm as number) ?? null
    local.termTo = (next.maxTerm as number) ?? null
  },
  { deep: true, immediate: true }
)

  function toggleCollapse() {
    isCollapsed.value = !isCollapsed.value;

    if (!isCollapsed.value && fullOpen.value) {
      if (local.amountFrom != null) {
        local.amountMin = local.amountFrom
      } else if (local.amountMin != null) {
        local.amountFrom = local.amountMin
      }

      if (local.termFrom != null) {
        local.termMin = local.termFrom
      } else if (local.termMin != null) {
        local.termFrom = local.termMin
      }
    }
  }

  function toggleFull() {
    fullOpen.value = !fullOpen.value

    if (fullOpen.value) {
      if (local.amountFrom == null && local.amountMin != null) {
        local.amountFrom = local.amountMin
      }
      if (local.termFrom == null && local.termMin != null) {
        local.termFrom = local.termMin
      }
    } else {
      if (local.amountFrom != null) {
        local.amountMin = local.amountFrom
      }
      if (local.termFrom != null) {
        local.termMin = local.termFrom
      }
    }
  }

function validate(): boolean {
  errors.amount = null
  errors.term = null

  if (local.amountFrom != null && local.amountTo != null && local.amountFrom > local.amountTo) {
    errors.amount = 'Минимальная сумма не может быть больше максимальной'
    return false
  }
  if (local.termFrom != null && local.termTo != null && local.termFrom > local.termTo) {
    errors.term = 'Минимальный срок не может быть больше максимального'
    return false
  }
  return true
}

function apply() {
  if (!validate()) return
  store.setFilters({
    minAmount: local.amountFrom ?? local.amountMin ?? null,
    maxAmount: local.amountTo ?? null,
    minTerm: local.termFrom ?? local.termMin ?? null,
    maxTerm: local.termTo ?? null
  })
  store.fetchApplications()
}

function clear() {

  for (const k in local) {
    // @ts-ignore
    local[k] = null
  }
  store.clearFilters()
  store.fetchApplications()
}

</script>

<style scoped>

.border-div {
  font-family: var(--bs-font-family);
  font-size: 14px;
  color: var(--bs-body-color);
  margin-bottom: 40px;
  padding: var(--filter-padding);
  border-radius: var(--bs-border-radius);
  background-color: var(--filter-bg);
  border: 1px solid var(--bs-border-color);
}

.filter-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  cursor: pointer;
  user-select: none;
}
.filter-title {
  display: flex;
  align-items: center;
  gap: 12px;
  font-weight: 600;
  color: var(--bs-body-color);
}
.filter-arrow {
  transition: transform .25s ease;
}
.filter-arrow.rotated {
  transform: rotate(180deg);
}
  .filter-open span {
    border-bottom: 3px solid var(--bs-primary);
    font-weight: 600;
    color: var(--bs-body-color);
  }

.collapse {
  overflow: hidden;
  transition: max-height .28s ease, opacity .18s ease;
}
.collapse.show {
  max-height: 2000px;
  opacity: 1;
}
.collapse:not(.show) {
  max-height: 0;
  opacity: 0;
}

.sub-form { margin-top: 12px; }
.filer-firs-block { display:flex; gap: 20px; align-items:flex-start; flex-wrap:wrap; }
.filter-block { min-width: 220px; flex: 1 1 220px; display:flex; flex-direction:column; gap: 8px; }
.filter-block label { font-weight: 500; color: var(--bs-secondary); }

.m-input-block { display:flex; flex-direction:column; gap:20px; padding-bottom:10px }
.m-input { position:relative; }
.my_input {
  width:100%;
  padding: 10px 12px;
  border-radius: 6px;
  border: 1px solid var(--bs-border-color);
  outline: none;
  background: transparent;
  font-size: 14px;
}
.my_input:focus {
  box-shadow: 0 0 0 4px rgba(var(--bs-primary-rgb,13,110,253),0.06);
  border-color: var(--bs-primary);
}

.error-text { color: var(--bs-danger); font-size: 12px; min-height: 18px; }

.btn {
  display: inline-flex;
  align-items:center;
  justify-content:center;
  padding: 8px 14px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  background: var(--bs-primary);
  color: white;
  font-weight: 600;
}
.btn.btn-secondary { background: transparent; color: var(--bs-secondary); border: 1px solid var(--bs-border-color); }
.btn.apply-link { margin-right: 8px; }

.m_input_with_arrow .input_with_arrow { display:flex; align-items:center; gap:8px; }
.arrow_input { display:flex; flex-direction:column; gap:2px; margin-left: 6px; }
.arrow_up, .arrow_down {
  width:12px; height:8px; background: var(--bs-secondary); opacity:0.7; cursor:pointer;
}
.arrow_up { transform: rotate(0deg); clip-path: polygon(50% 0, 100% 100%, 0 100%); }
.arrow_down { transform: rotate(0deg); clip-path: polygon(0 0, 100% 0, 50% 100%); }

@media (max-width: 900px) {
  .filer-firs-block { flex-direction: column; }
  .filter-block { min-width: 100%; }
}
</style>
