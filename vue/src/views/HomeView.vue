<script setup lang="ts">
import { useUserStore } from '../stores/userStore';

const userStore = useUserStore();
const isLoggedIn = !!userStore.jwt;
</script>

<template>
  <div class="home-container">
    <div class="welcome-card">
      <h1 class="welcome-title">Welcome to <span class="app-name">xxxxxxxxxx</span></h1>
      
      <div class="cat-container">
        <img src="https://media.giphy.com/media/vFKqnCdLPNOKc/giphy.gif" alt="Cat waving hi" class="cat-gif">
        <div class="speech-bubble">Hi bablabla!</div>
      </div>
      
      <p class="welcome-text">
        Some title
      </p>
      <p class="crud-text">
        Some hyperlinks
      </p>
      
      <div class="action-buttons">
        <router-link v-if="isLoggedIn" to="/artists" class="btn action-btn primary">
          View 
        </router-link>
        <router-link v-if="isLoggedIn" to="/artist/create" class="btn action-btn secondary">
          Add
        </router-link>
        <router-link v-if="!isLoggedIn" to="/login" class="btn action-btn primary">
          Log in
        </router-link>
        <router-link v-if="!isLoggedIn" to="/register" class="btn action-btn secondary">
          Register
        </router-link>
      </div>

      <div class="features-section">
        <div class="feature-card">
          <div class="feature-icon">🎤</div>
          <h3>DO thing 1</h3>
          <p>thing 1 description</p>
        </div>
        <div class="feature-card">
          <div class="feature-icon">🌟</div>
          <h3>DO thing 2</h3>
          <p>thing 1 description</p>
        </div>
        <div class="feature-card">
          <div class="feature-icon">💖</div>
          <h3>DO thing 3</h3>
          <p>thing 1 description</p>
        </div>
      </div> 
    </div>
  </div>
</template>

<style scoped>
.home-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 80vh;
  padding: 20px;
}

.welcome-card {
  background-color: white;
  border-radius: 1.5rem;
  box-shadow: 0 6px 18px rgba(103, 58, 183, 0.15);
  padding: 2.5rem;
  max-width: 900px;
  width: 100%;
  text-align: center;
  border: 2px solid #b39ddb;
}

.welcome-title {
  color: #673ab7;
  font-weight: 600;
  margin-bottom: 1.5rem;
  font-size: 2.5rem;
}

.app-name {
  font-weight: 800;
  background: linear-gradient(180deg, #9575cd, #4a148c);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
}

.welcome-text {
  font-size: 1.2rem;
  color: #9575cd;
  margin-bottom: 1rem;
}

.crud-text {
  font-size: 1rem;
  color: #b39ddb;
  margin-bottom: 1rem;
}

.cat-container {
  position: relative;
  margin: 2rem auto;
  width: 250px;
  height: 200px;
}

.cat-gif {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 1rem;
  box-shadow: 0 4px 8px rgba(103, 58, 183, 0.15);
  border: 3px solid #b39ddb;
}

.speech-bubble {
  position: absolute;
  top: -30px;
  right: -30px;
  background-color: #f3e5f5;
  color: #673ab7;
  padding: 10px 20px;
  border-radius: 20px;
  font-weight: bold;
  box-shadow: 0 2px 5px rgba(103, 58, 183, 0.1);
  animation: bounce 2s infinite;
}

.speech-bubble::after {
  content: '';
  position: absolute;
  bottom: -10px;
  left: 20px;
  border-width: 10px 10px 0;
  border-style: solid;
  border-color: #f3e5f5 transparent;
}

@keyframes bounce {
  0%, 100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-10px);
  }
}

.action-buttons {
  display: flex;
  justify-content: center;
  gap: 1rem;
  margin-bottom: 2.5rem;
  flex-wrap: wrap;
}

.action-btn {
  padding: 0.8rem 1.5rem;
  font-weight: 600;
  border-radius: 2rem;
  transition: all 0.3s ease;
  font-size: 1rem;
  text-decoration: none;
}

.primary {
  background-color: #9575cd;
  color: white;
  border: none;
}

.primary:hover {
  background-color: #673ab7;
  transform: translateY(-3px);
  box-shadow: 0 4px 12px rgba(103, 58, 183, 0.3);
}

.secondary {
  background-color: #b39ddb;
  color: #4a148c;
  border: none;
}

.secondary:hover {
  background-color: #c8b0f1;
  transform: translateY(-3px);
  box-shadow: 0 4px 12px rgba(103, 58, 183, 0.3);
}

.features-section {
  display: flex;
  justify-content: space-between;
  margin-top: 2rem;
  gap: 1.5rem;
  flex-wrap: wrap;
}

.feature-card {
  flex: 1;
  min-width: 200px;
  background-color: #f9f0fb;
  border-radius: 1rem;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(103, 58, 183, 0.08);
  transition: transform 0.3s ease;
  border: 1px solid #b39ddb;
}

.feature-card:hover {
  transform: translateY(-5px);
}

.feature-icon {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.feature-card h3 {
  color: #673ab7;
  margin-bottom: 0.5rem;
}

.feature-card p {
  color: #9575cd;
  font-size: 0.9rem;
}

@media (max-width: 768px) {
  .features-section {
    flex-direction: column;
  }
  
  .feature-card {
    margin-bottom: 1rem;
  }
  
  .welcome-title {
    font-size: 2rem;
  }
}
</style>