import {getCookie, setCookie} from './cookie'
import store from '../store'
import * as qs from 'qs'
// var baseUrl = process.env.NODE_ENV === 'production' ? window.g.ApiUrl : '/api'
function loadToken(): string{
    let token = store?.state?.token
    if(token) return token
    let q = qs.parse(window.location.search)
    token = q['x-token'] as string
    if(!token) token = localStorage.getItem("Authorization") as string
    if(!token) token = getCookie('Authorization') as string
    if(token)storeToken(token)
    return token
}
function storeToken(token:string):void{
    store.commit('token',token)
    localStorage.setItem('Authorization',token)
    setCookie('Authorization',token,1)
}

export function token(token?:string):any{
    if(token===undefined) return loadToken()
    storeToken(token)
}

