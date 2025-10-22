<template>
  <div class="application-card border-div">
    <h3 class="card-title">Создание нового предложения займа</h3>

    <form @submit.prevent="submit" novalidate class="application-form">
      <div class="form-grid">
        <div class="form-field">
          <label for="number">Номер заявки</label>
          <input id="number" v-model.trim="form.number" class="my_input" type="text" placeholder="45321" />
          <div v-if="errors.number" class="error-text">{{ errors.number }}</div>
        </div>

        <div class="form-field">
          <label for="amount">Сумма займа (BYN)</label>
          <input id="amount" v-model.number="form.amount" class="my_input" type="number" min="0" step="10" placeholder="1000" />
          <div v-if="errors.amount" class="error-text">{{ errors.amount }}</div>
        </div>

        <div class="form-field">
          <label for="termValue">Срок займа (дни)</label>
          <input id="termValue" v-model.number="form.termValue" class="my_input" type="number" min="1" step="1" placeholder="30" />
          <div v-if="errors.termValue" class="error-text">{{ errors.termValue }}</div>
        </div>

        <div class="form-field">
          <label for="interestValue">Процентная ставка (%)</label>
          <input id="interestValue" v-model.number="form.interestValue" class="my_input" type="number" min="0.01" step="0.1" placeholder="2.3" />
          <div v-if="errors.interestValue" class="error-text">{{ errors.interestValue }}</div>
        </div>
      </div>

      <div class="form-actions">
        <el-button type="primary" :loading="submitting" native-type="submit" style="background-color:var(--bs-primary)">Создать</el-button>
        <el-button type="text" @click="onCancel" style="color:var(--bs-primary)">Отмена</el-button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
  import { reactive, ref } from 'vue'
  import { useRouter } from 'vue-router'
  import { ElMessage } from 'element-plus'
  import { useApplicationsStore } from '../stores/applications'
  import type { CreateApplicationDto } from '../types/application'

  const store = useApplicationsStore()
  const router = useRouter()

  const submitting = ref(false)

  const form = reactive<CreateApplicationDto>({
    number: '',
    amount: 0,
    termValue: 0,
    interestValue: 0
  })

  const errors = reactive({
    number: '' as string | null,
    amount: '' as string | null,
    termValue: '' as string | null,
    interestValue: '' as string | null
  })

  function validate(): boolean {
    errors.number = null
    errors.amount = null
    errors.termValue = null
    errors.interestValue = null

    let ok = true

    if (!form.number || form.number.trim() === '') {
      errors.number = 'Номер заявки обязателен к заполнению'
      ok = false
    }

    if (!form.amount || Number(form.amount) <= 0) {
      errors.amount = 'Сумма займа должна превышать 0'
      ok = false
    }

    if (!form.termValue || Number(form.termValue) <= 0) {
      errors.termValue = 'Срок займа должна превышать 0'
      ok = false
    }

    if (!form.interestValue || Number(form.interestValue) <= 0) {
      errors.interestValue = 'Процентная ставка займа должна превышать 0'
      ok = false
    }

    return ok
  }

  async function submit() {
    if (!validate()) return

    submitting.value = true
    try {
      const dto: CreateApplicationDto = {
        number: form.number,
        amount: Number(form.amount),
        termValue: Number(form.termValue),
        interestValue: Number(form.interestValue)
      }

      await store.createApplication(dto)
      ElMessage.success('Application created')
      router.push('/')
    } catch (err: any) {
      console.error('create error', err)
      ElMessage.error(err?.response?.data?.message ?? err?.message ?? 'Failed to create application')
    } finally {
      submitting.value = false
    }
  }

  function onCancel() {
    router.back()
  }
</script>

<style scoped>
  .application-card {
    max-width: 400px;
    margin: 0 auto;
    padding: 22px;
    box-sizing: border-box;
  }

  .card-title {
    margin: 0 0 12px 0;
    font-size: 18px;
    font-weight: 700;
    color: var(--bs-body-color);
  }

  .form-grid {
    display: grid;
    
    gap: 14px;
    margin-bottom: 16px;
  }

  .form-field {
    display: flex;
    flex-direction: column;
    gap: 8px;
  }

    .form-field label {
      font-weight: 600;
      color: var(--bs-body-color);
    }

  .my_input {
    width: 100%;
    padding: 10px 12px;
    border-radius: 6px;
    border: 1px solid var(--bs-border-color, #dee2e6);
    background: transparent;
    font-size: 14px;
  }

  .form-actions {
    display: flex;
    gap: 12px;
    justify-content: flex-start;
    align-items: center;
  }

  .error-text {
    color: var(--bs-danger, #dc3545);
    font-size: 13px;
    min-height: 18px;
  }

</style>
