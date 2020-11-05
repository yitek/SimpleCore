import Vue from 'vue'
import Element from 'element-ui'
// import App from './App.vue'
import Layout from './components/Layout.vue'
import router from './router'
import store from './store'
import './themes/default/app.scss'

Vue.use(Element)
Vue.config.productionTip = false

new Vue({
  router,
  store,
  // render: h => h(App)
  render: h => h(Layout)
}).$mount('#app')

