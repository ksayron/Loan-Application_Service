<template>
  <div>
    <application-form />
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import ApplicationForm from '../components/ApplicationForm.vue'

const router = useRouter()

function goBack() {
  router.push('/')
}
</script>

<style scoped>
</style>
