<script setup lang="ts">
import { ref } from 'vue'
import { IdentityService } from '../services/IdentityService'
import { useUserStore } from '../stores/userStore'
import router from '@/router'

const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const firstName = ref('')
const lastName = ref('')

const error = ref<string | null>(null)

const userService = new IdentityService()

const validateForm = () => {
  if (password.value !== confirmPassword.value) {
    error.value = 'Passwords do not match'
    return false
  }

  if (password.value.length < 6) {
    error.value = 'Password must be at least 6 characters'
    return false
  }

  return true
}

const register = async () => {
  error.value = null

  if (!validateForm()) {
    return
  }

  const userStore = useUserStore()

  try {
    const res = await userService.registerAsync(email.value, password.value, firstName.value, lastName.value)

    if (res.data) {
      userStore.jwt = res.data.jwt
      userStore.refreshToken = res.data.refreshToken
      userStore.email = email.value

      router.push('/')
    } else {
      error.value = res.errors?.[0] || 'Registration failed'
    }
  } catch (err) {
    error.value = 'An unexpected error occurred during registration'
    console.error(err)
  }
}
</script>

<template>
  <div class="container py-2 h-100">
    <div class="row d-flex justify-content-center align-items h-100">
      <div class="col-12 col-md-8 col-lg-6 col-xl-5">
        <div class="card shadow-sm" style="border-radius: 1rem; border-color: #b39ddb">
          <div class="card-body p-5 text-center">
            <h3 class="mb-3" style="color: #673ab7">Create an account</h3>
            <p class="text-muted mb-4">Please fill in your information</p>

            <div v-if="error" class="alert alert-danger" role="alert">
              {{ error }}
            </div>

            <form @submit.prevent="register">
              <div class="form-floating mb-3">
                <input
                  v-model="email"
                  type="email"
                  class="form-control"
                  id="email"
                  placeholder="name@email.com"
                  required
                />
              </div>

              <div class="form-floating mb-3">
                <input
                  v-model="firstName"
                  type="text"
                  class="form-control"
                  id="firstName"
                  placeholder="Susie"
                  required
                />
              </div>

              <div class="form-floating mb-3">
                <input
                  v-model="lastName"
                  type="text"
                  class="form-control"
                  id="lastName"
                  placeholder="Toot"
                  required
                />
              </div>

              <div class="form-floating mb-3">
                <input
                  v-model="password"
                  type="password"
                  class="form-control"
                  id="password"
                  placeholder="Password"
                  required
                />
              </div>

              <div class="form-floating mb-4">
                <input
                  v-model="confirmPassword"
                  type="password"
                  class="form-control"
                  id="confirmPassword"
                  placeholder="Confirm password"
                  required
                />
              </div>

              <button
                class="btn btn-lg w-100 mb-4"
                type="submit"
                style="background-color: #9575cd; color: white"
              >
                Register
              </button>

              <div class="mt-3">
                <p class="mb-0">
                  Already have an account?
                  <router-link to="/login" style="color: #673ab7">Sign in</router-link>
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
  background-color: #673ab7;
}

.card {
  background-color: #fff;
}

.form-control:focus,
.form-select:focus {
  border-color: #b39ddb;
  box-shadow: 0 0 0 0.25rem rgba(103, 58, 183, 0.25);
}

.form-select {
  padding-top: 1.625rem;
  padding-bottom: 0.625rem;
  border-color: #b39ddb;
}
</style>
