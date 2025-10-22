<template>
  <el-container style="min-height: 100vh">
    <el-header height="64px" class="header">
      <SiteHeader/>
    </el-header>

    <el-main class="main">
      <router-view />
    </el-main>

    <el-footer height="40px" class="footer">
      <small>Сделал Кучерук Николай</small>
    </el-footer>
  </el-container>
</template>

<script setup lang="ts">
  import SiteHeader from './components/SiteHeader.vue'
</script>

<style scoped>
  .header {
    background: #ffffff;
    border-bottom: 1px solid #ebeef5;
    display: flex;
    align-items: center;
    padding: 0 16px;
    box-sizing: border-box;
  }

  .header-inner {
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .brand {
    font-weight: 700;
    font-size: 1.25rem;
    text-decoration: none;
    color: #303133;
  }

  .nav el-button {
    margin-left: 8px;
  }

  .main {
    padding: 20px;
    background: #f5f7fa;
  }

  .footer {
    text-align: center;
    color: #909399;
  }
</style>
