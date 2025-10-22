<template>
  <header class="site-header">
    <div class="container">
      <router-link to="/" class="brand" aria-label="Home">
      </router-link>
      <img src="./icons/logo.svg" width="50" height="50"/>
      <h2 class="site-name">BONGO.MY</h2>
      <nav class="actions">
        <el-button type="text" class="action-btn" @click="goList" style="color:var(--bs-primary)">Предложения</el-button>
        <el-button type="primary" class="action-btn" @click="goCreate" style="background-color:var(--bs-primary)">Создать заявку</el-button>
      </nav>

     
    </div>
  </header>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()
const logoSrc = '../components/icons/logo.svg' 

function goList() {
  router.push('/')
}
function goCreate() {
  router.push('/create')
}


</script>

<style scoped>
  .site-header {
    background: transparent;
    position: sticky;
    top: 0;
    z-index: 50;
    display: block;
    border-bottom: 1px solid var(--bs-light-border-subtle, #e9ecef);
    backdrop-filter: blur(6px);
    --header-height: 64px;
  }

  .site-name{
    color : var(--bs-body-color);
  }

  .container {
    max-width: 1200px;
    margin: 0 auto;
    height: var(--header-height);
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 16px;
    gap: 12px;
    box-sizing: border-box;
  }

  .brand {
    display: inline-flex;
    align-items: center;
    text-decoration: none;
  }

  .brand__img {
    height: 40px;
    width: auto;
    display: block;
  }

  .actions {
    display: flex;
    align-items: center;
    gap: 12px;
  }

  .action-btn {
    font-weight: 600;
    padding: 6px 12px;
    border-radius: 8px;
  }

</style>
