<template>
  <div>
    <el-row justify="space-between" align="middle" style="margin-bottom: 12px">
      <el-col>
        <h2 style="margin:5;color:var(--bs-body-color);">Для вас было найдено {{ store.total }} предложений</h2>
      </el-col>
    </el-row>

    <filters-bar />

    <div style="margin-top: 8px">
      <application-table :items="store.applications"
                         :loading="store.loading"
                         @toggle="onToggle" />
    </div>

  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import FiltersBar from '../components/FiltersBar.vue'
import ApplicationTable from '../components/ApplicationTable.vue'
import { useApplicationsStore } from '../stores/applications'

const store = useApplicationsStore()
const router = useRouter()

onMounted(() => {
  store.fetchApplications()
})

function goCreate() {
  router.push('/create')
}

async function refresh() {
  await store.fetchApplications()
  if (!store.error) {
    ElMessage.success('List refreshed')
  }
}

async function onToggle(number: string) {
  try {
    await store.toggleStatus(number)
    ElMessage.success('Status updated')
  } catch (err: any) {
    ElMessage.error(store.error ?? err?.message ?? 'Failed to update status')
  }
}
</script>

<style scoped>
  h2 {
    font-weight: 600;
  }
</style>
