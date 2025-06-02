import { createRouter, createWebHistory } from 'vue-router'
import { useUserStore } from '../stores/userStore'
import HomeView from '../views/HomeView.vue'
import Login from '@/views/Login.vue'
import Register from '../views/Register.vue'
import CreateArtist from '@/views/CreateArtist.vue'
import ArtistList from '@/views/ArtistList.vue'
import EditArtist from '@/views/EditArtist.vue'
import Unauthorized from '@/views/Unauthorized.vue'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    },
    {
      path: '/register',
      name: 'register',
      component: Register
    },
    {
      path: '/artists',
      name: 'artists',
      component: ArtistList,
      meta: { requiresAuth: true }
    },
    {
      path: '/artist/create',
      name: 'createArtist',
      component: CreateArtist,
      meta: { requiresAuth: true }
    },
    {
      path: '/artist/edit/:id',
      name: 'editArtist',
      component: EditArtist,
      meta: { requiresAuth: true }
    },
    {
      path: '/unauthorized',
      name: 'unauthorized',
      component: Unauthorized
    },
    // a catch-all route for 404 errors
    {
      path: '/:pathMatch(.*)*',
      redirect: '/'
    }
  ]
})

router.beforeEach((to, from, next) => {
  const userStore = useUserStore()
  
  if (to.meta.requiresAuth && !userStore.isAuthenticated()) {
  //   next('/login')
  // } else if (to.meta.requiresRole && !userStore.hasRole(to.meta.requiresRole as string)) {
  //   
  next('/unauthorized')
  } else {
    next()
  }
})

export default router