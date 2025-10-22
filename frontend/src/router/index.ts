import { createRouter, createWebHistory } from 'vue-router'
import ApplicationsList from '../pages/ApplicationsList.vue'
import CreateApplication from '../pages/CreateApplication.vue'

const routes = [
  { path: '/', name: 'list', component: ApplicationsList },
  { path: '/create', name: 'create', component: CreateApplication }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
