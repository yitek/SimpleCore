import axios, { AxiosRequestConfig } from 'axios'
import {token} from './token'

export interface Response<T=any>{
    success:boolean;
    fail:boolean;
    message:string;
    data:T;
}
//var baseUrl = process.env.NODE_ENV === 'production' ? window.g.ApiUrl : '/api'

axios.defaults.headers.common["token"] = token()
axios.defaults.headers.post["Content-type"] = "application/json"
axios.defaults.baseURL = 'https://localhost:5000/' //设置统一路径前缀

axios.interceptors.request.use(function (req:AxiosRequestConfig):any {
    if(req.headers && req.headers['Content-type']==='application/json'){
        if(typeof req.data!=='string') req.data = JSON.stringify(req.data)
    }
    return req
})

export function request(opts:AxiosRequestConfig):Promise<any>{
    return axios(opts);
}

