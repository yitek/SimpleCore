import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Home from '../views/Home.vue'
import store from './../store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/login',
    name: '/Login',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/Login.vue')
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '/user-list',
    name: 'user-list',
    
    component: () => import('../views/Account/index.vue')
  }
]

const router = new VueRouter({
  routes
})

/*
NProgress.configure({ showSpinner: false }) // NProgress Configuration


router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()
  //const perms = await store.dispatch('permissions')
  //console.log(perms)
  //router.addRoutes(perms)
  next({ ...to, replace: true } as any)
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
console.log(router)
*/
export default router
