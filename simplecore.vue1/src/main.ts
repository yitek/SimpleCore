import {createApp} from 'vue'
import Element from 'element-ui';
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'



createApp(App).use(store).use(store).use(router).mount('#app')
