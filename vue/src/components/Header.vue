<script setup lang="ts">
import { useUserStore } from '../stores/userStore';
import { computed } from 'vue';
import router from '@/router';

const userStore = useUserStore();
const isLoggedIn = computed(() => !!userStore.jwt);

const logout = () => {
  userStore.jwt = '';
  userStore.refreshToken = '';
  router.push('/login');
};
</script>

<template>
  <header class="header">
    <div class="header-container">
      <router-link to="/" class="brand">Brandname</router-link>
      
      <nav class="nav-links">
        <router-link to="/" class="nav-item">Home</router-link>
        <router-link v-if="isLoggedIn" to="/" class="nav-item">View xxxxxxx</router-link>
        <router-link v-if="isLoggedIn" to="/" class="nav-item">Create a new xxxxxxxx</router-link>
      </nav>
      
      <div class="auth-buttons">
        <template v-if="isLoggedIn">
          <button @click="logout" class="auth-btn logout-btn">Logout</button>
        </template>
        <template v-else>
          <router-link to="/login" class="auth-btn login-btn">Login</router-link>
          <router-link to="/register" class="auth-btn register-btn">Register</router-link>
        </template>
      </div>
    </div>
  </header>
</template>

<style scoped>
.header {
  background-color: #ede5f5;;
  box-shadow: 0 2px 8px rgba(92, 45, 174, 0.15);
  padding: 12px 0;
}

.header-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 24px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.brand {
  font-size: 1.6rem;
  font-weight: 600;
  color: #421989;
  text-decoration: none;
  letter-spacing: 1px;
}

.nav-links {
  display: flex;
  gap: 32px;
}

.nav-item {
  color: #9575cd;
  text-decoration: none;
  font-weight: 300;
  position: relative;
  transition: color 0.2s ease;
}

.nav-item:hover, .nav-item.router-link-active {
  color: #c8b0f1;
}

.nav-item::after {
  content: '';
  position: absolute;
  width: 0;
  height: 2px;
  bottom: -4px;
  left: 0;
  background-color: #c8b0f1;
  transition: width 0.2s ease;
}

.nav-item:hover::after, .nav-item.router-link-active::after {
  width: 100%;
}

.auth-buttons {
  display: flex;
  gap: 12px;
}

.auth-btn {
  padding: 8px 20px;
  border-radius: 6px;
  font-weight: 300;
  border: none;
  cursor: pointer;
  text-decoration: none;
  transition: all 0.2s ease;
  font-size: 0.95rem;
}

.auth-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(103, 58, 183, 0.2);
}

.login-btn {

  background-color: #e2d2ff;
  color: #4a148c;
}

.register-btn, .logout-btn {
  background-color: #66459e;
  color: white;
}
</style>