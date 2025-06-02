<script setup lang="ts">
import { RouterView } from 'vue-router'
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'
</script>

<template>
  <Header />

  <main class="container mt-5">
    <RouterView />
  </main>

  <Footer />
</template>

<style>
body {
  min-height: 100vh;
  background-color: #fffeff;
}
</style>
