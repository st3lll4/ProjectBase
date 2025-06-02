<script setup lang="ts">
import { ref } from 'vue';
import { IdentityService } from '../services/IdentityService';
import { useUserStore } from '../stores/userStore';
import router from '@/router';

const email = ref('stella_tukia@hotmail.com')
const password = ref('Foo.Bar.1')
const error = ref<string | null>(null)

const userService = new IdentityService();

const login = async () => {
  const userStore = useUserStore()
  const res = await userService.loginAsync(email.value, password.value);
  console.log(res);
    if (res.data) {
        userStore.jwt = res.data.jwt;
        userStore.refreshToken = res.data.refreshToken;
       
        router.push('/')
    } else {
        error.value = res.errors?.[0] || 'Login failed';
    }
}


</script>

<template>
  <div class="container py-2 h-100">
    <div class="row d-flex justify-content-center align-items-start h-100">
      <div class="col-12 col-md-8 col-lg-6 col-xl-5">
        <div class="card shadow-sm" style="border-radius: 1rem; border-color: #b39ddb;">
          <div class="card-body p-5 text-center">

            <h3 class="mb-3" style="color: #673ab7;">Welcome</h3>
            <p class="text-muted mb-4">Please sign in to your account</p>

            <div v-if="error" class="alert alert-danger" role="alert">
              {{ error }}
            </div>

            <form @submit.prevent="login">
              <div class="form-floating mb-3">
                <input 
                  v-model="email" 
                  type="email" 
                  class="form-control" 
                  id="email" 
                  placeholder="name@example.com" 
                  required
                >
              </div>

              <div class="form-floating mb-3">
                <input 
                  v-model="password" 
                  type="password" 
                  class="form-control" 
                  id="password" 
                  placeholder="Password" 
                  required
                >
              </div>

              <button 
                class="btn btn-lg w-100 mb-4" 
                type="submit" 
                style="background-color: #9575cd; color: white;"
              >
                Sign in
              </button>

              <div class="mt-3">
                <p class="mb-0">Don't have an account? 
                  <router-link to="/register" style="color: #673ab7;">Sign up</router-link>
                </p>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.h-100 {
  min-height: 100vh;
}

.btn:hover {
  background-color: #673ab7 !important;
}

.card {
  background-color: #fff;
}

.form-control:focus {
  border-color: #b39ddb;
  box-shadow: 0 0 0 0.25rem rgba(103, 58, 183, 0.25);
}
</style>