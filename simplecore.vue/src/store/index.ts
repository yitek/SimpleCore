import { clone } from '@/utils'
import { IResponse } from '@/utils/request'
import Vue from 'vue'
import Vuex from 'vuex'
import { Permissions, loadPermissions } from './../permission'
Vue.use(Vuex)

export let store = new Vuex.Store({
  state: {
    token:'',
    
    routes:[],
    menus:[]
  },
  mutations: {
    token(state,token:string){
      state.token = token
    },
    routes: (state, routes) => {
      state.routes = routes
    },
    
    menus: (state, menus) => {
      state.menus = menus
    }
  },
  actions: {
    permissions({ commit }, data) {
      loadPermissions().then((data:Permissions)=>{
        //let data:IPermissions = ret.data
        console.log(data)
        let routes = data.access["@routes"]["@roots"]
        let storeRoutes = clone(routes)
        commit('routes',storeRoutes)
        let menus = data.access["@pages"]["@menus"]["@items"]
        let storeMenus = clone(menus)
        commit('menus',storeMenus)
      })
    }
  },
  modules: {
  }
})
export default store
