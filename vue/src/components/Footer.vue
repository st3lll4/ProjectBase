<script setup lang="ts">
const currentYear = new Date().getFullYear();
</script>

<template>
  <footer class="footer">
    <div class="footer-container">
      <div class="footer-section">
        <h4 class="footer-title">Some project</h4>
        <p class="footer-text">Your go-to platform for doing something.</p>
      </div>
      
      <div class="footer-section">
        <h4 class="footer-title">Go</h4>
        <ul class="footer-links">
          <li><router-link to="/" class="footer-link">Home</router-link></li>
          <li><router-link to="/" class="footer-link">About us</router-link></li>
          <li><router-link to="/" class="footer-link">Contact</router-link></li>
        </ul>
      </div>
      
      <div class="footer-section">
        <h4 class="footer-title">Connect</h4>
        <div class="social-links">
          <a href="#" class="social-link">Twitter</a>
          <a href="#" class="social-link">Instagram</a>
          <a href="#" class="social-link">Facebook</a>
        </div>
      </div>
    </div>
    
    <div class="footer-bottom">
      <p>© {{ currentYear }}. All rights reserved.</p>
      <p>Made with 💜 by Stella's last braincell</p>
    </div>
  </footer>
</template>

<style scoped>
.footer {
  background-color: #f3e5f5;
  padding-top: 3rem;
  margin-top: 3rem;
  border-top: 1px solid #b39ddb;
}

.footer-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 24px;
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  gap: 2rem;
}

.footer-section {
  flex: 1;
  min-width: 200px;
}

.footer-title {
  color: #673ab7;
  margin-bottom: 1rem;
  font-weight: 600;
  font-size: 1.2rem;
}

.footer-text {
  color: #9575cd;
  line-height: 1.6;
  font-size: 0.95rem;
}

.footer-links {
  list-style: none;
  padding: 0;
  margin: 0;
}

.footer-links li {
  margin-bottom: 0.5rem;
}

.footer-link {
  color: #9575cd;
  text-decoration: none;
  transition: color 0.2s ease;
  font-size: 0.95rem;
}

.footer-link:hover {
  color: #673ab7;
  text-decoration: underline;
}

.social-links {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.social-link {
  color: #9575cd;
  text-decoration: none;
  transition: color 0.2s ease;
  font-size: 0.95rem;
}

.social-link:hover {
  color: #673ab7;
  text-decoration: underline;
}

.footer-bottom {
  max-width: 1200px;
  margin: 2rem auto 0;
  padding: 1.5rem 24px;
  border-top: 1px solid #d1c4e9;
  text-align: center;
  color: #9575cd;
  font-size: 0.9rem;
}

.footer-bottom p {
  margin: 0.5rem 0;
}
</style>