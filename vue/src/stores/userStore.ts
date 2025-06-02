import { defineStore } from 'pinia';
import { ref, watch } from 'vue';
import axios from 'axios';
import { parseJwt } from '@/helpers/parseJwt';

export const useUserStore = defineStore('user', () => {
  const jwt = ref<string | undefined>(localStorage.getItem('_jwt') || undefined);
  const refreshToken = ref<string | undefined>(localStorage.getItem('_refreshToken') || undefined);
  const roles = ref<string[] | undefined>([]);
  const userName = ref<string | undefined>(undefined);
  const email = ref<string | undefined>(undefined);
  const isHydrated = ref(false);

  function initialize() {
    if (jwt.value) {
      axios.defaults.headers.common.Authorization = `Bearer ${jwt.value}`;
      const parsed = parseJwt(jwt.value);
      roles.value = parsed.roles;
      userName.value = parsed.name;
    }
    isHydrated.value = true;
  }

  watch(jwt, (newJwt) => {
    if (newJwt) {
      localStorage.setItem('_jwt', newJwt);
      axios.defaults.headers.common.Authorization = `Bearer ${newJwt}`;
      
      const parsed = parseJwt(newJwt);
      roles.value = parsed.roles;
      userName.value = parsed.name;
    } else {
      localStorage.removeItem('_jwt');
      delete axios.defaults.headers.common.Authorization;
      roles.value = undefined;
      userName.value = undefined;
    }
  });

  watch(refreshToken, (newRefreshToken) => {
    if (newRefreshToken) {
      localStorage.setItem('_refreshToken', newRefreshToken);
    } else {
      localStorage.removeItem('_refreshToken');
    }
  });

  function setAuthInfo(authData: { jwt?: string; refreshToken?: string }) {
    jwt.value = authData.jwt;
    refreshToken.value = authData.refreshToken;
  }

  function logout() {
    jwt.value = undefined;
    refreshToken.value = undefined;
    email.value = undefined;
    userName.value = undefined;
    roles.value = undefined;
  }

  const isAuthenticated = () => !!jwt.value;

  const hasRole = (role: string) => roles.value?.includes(role) || false;

  initialize();

  return {
    jwt,
    refreshToken,
    roles,
    userName,
    email,
    isHydrated,
    setAuthInfo,
    logout,
    isAuthenticated,
    hasRole
  };
});