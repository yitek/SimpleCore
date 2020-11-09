import Vue from 'vue'
import Element from 'element-ui'
import './themes/default/app.scss'
import store from './store'
import router from './router'
import Layout from './components/Layout.vue'

Vue.use(Element)
Vue.config.productionTip = false

new Vue({
  router,
  store,
  // render: h => h(App)
  render: h => h(Layout)
}).$mount('#app')
