<script setup lang="ts">
import { ref } from 'vue';

const jokes = ref([
  "You shall not pass!",
  "Sorry, this show is VIP only.",
  "Houston, we have a permission problem"
]);

const randomJoke = jokes.value[Math.floor(Math.random() * jokes.value.length)];
</script>

<template>
  <div class="container unauthorized-container">
    <div class="card shadow-sm border-purple">
      <div class="card-body text-center p-5">
        <h2 class="mb-4 title">Unauthorized Access!</h2>
        
        <div class="image-container mb-4">
          <img src="https://media.giphy.com/media/njYrp176NQsHS/giphy.gif" alt="Access Denied" class="silly-image">
        </div>
        
        <p class="joke-text mb-4">{{ randomJoke }}</p>
        
        
        <router-link to="/" class="btn btn-lg btn-purple">
          Back home
        </router-link>
      </div>
    </div>
  </div>
</template>

<style scoped>
.unauthorized-container {
  display: flex;
  justify-content: center;
  align-items: start;
  min-height: 80vh;
  padding: 2rem 1rem;
}

.border-purple {
  border-color: #b39ddb;
  border-radius: 1rem;
  overflow: hidden;
}

.title {
  color: #673ab7;
  font-weight: 600;
}

.joke-text {
  color: #9575cd;
  font-style: italic;
}

.image-container {
  max-width: 300px;
  margin: 0 auto;
  border-radius: 1rem;
  overflow: hidden;
}

.silly-image {
  width: 100%;
  border-radius: 0.5rem;
  border: 3px solid #b39ddb;
  box-shadow: 0 4px 12px rgba(103, 58, 183, 0.2);
}

.alert-purple {
  background-color: #f3e5f5;
  color: #4a148c;
  border-color: #b39ddb;
}

.btn-purple {
  background-color: #9575cd;
  color: white;
}

.btn-purple:hover {
  background-color: #673ab7;
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(103, 58, 183, 0.2);
}
</style>