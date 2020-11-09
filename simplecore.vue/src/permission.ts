import { RouteConfigSingleView } from 'vue-router/types/router'
import {clone,makeTreeNodes,ITreeNode} from './utils'
import {IResponse, request} from './utils/request'

import {views} from './utils/pack'

export interface Func extends ITreeNode{
  id: string;
  name: string;
  description: string;
  type: number;
  url: string;
  path: string;
  icon: string;
  image: string;
  rawUrl: string;
  pages: Func[];
  menus: Func[];
  routeInfo?: RouteConfigSingleView;
}
export interface MenuItem extends ITreeNode{
  name: string;
  description: string;
  url: string;
  icon: string;
  image: string;
}
export type AccessFuncCollection = {
  [index in number|string]: Func
} & {
  length: number
};
export type AccessMenus = AccessFuncCollection & {
  '@roots':AccessFuncCollection
  '@items':MenuItem[]
};

export type AccessPages = AccessFuncCollection & {
  '@menus':AccessMenus
};

export type AccessRouteCollection = {
  [index in number | string]: RouteConfigSingleView
} & {
  length: number
};
export type AccessRoutes = AccessRouteCollection &{
  '@roots':AccessRouteCollection;
};

export interface AccessPermissions{
  [index:number]:Func;
  '@apis':AccessFuncCollection;
  '@pages':AccessPages;
  '@routes':AccessRoutes;
}
export interface Permissions{
  access:AccessPermissions;
}
let permissions:Permissions = {} as any;

export function getPermissions(){
  return permissions;
}

export function loadPermissions():Promise<Permissions>{
  return new Promise((resolve,reject)=>{
    request({
      url:'/permissions',
      method:'get'
    }).then((ret:IResponse<Permissions>)=>{
      console.log(ret)
      permissions = initPermissions(ret.data)
      resolve(permissions)
    },reject)
  })
}

export function initPermissions(data:any):Permissions{
  let ret:Permissions ={} as any;
  ret.access = initAccessPermissions(data.funcs)
  return  ret; 
}

function initAccessPermissions(rawFuncs:Func[]):any{
  let routes:RouteConfigSingleView[] = []
  let funcs:Func[]=[]
  let apis:Func[] = []
  let pages:Func[] = []
  let menus:Func[] = []
  let rootMenus:Func[] = []
  let rootRoutes:RouteConfigSingleView[] = []
  //let routeMaps:{[index:string]:RouteConfigSingleView} ={}
  let rootFuncs = makeTreeNodes(rawFuncs,(func:Func,raw:any):any=>{
    let url = func.rawUrl = func.url || func.path;
    if(func.type===0){
      Object.defineProperty(funcs,url,{enumerable:false,configurable:false,writable:false,value:func})
      funcs.push(func)
      Object.defineProperty(apis,url,{enumerable:false,configurable:false,writable:false,value:func})
      apis.push(func)
      return
    }
    // 0 = api , 1= page,2= menu
    func.path = resolveRoutePath(func)
    Object.defineProperty(funcs,url,{enumerable:false,configurable:false,writable:false,value:func})
    funcs.push(func)
    Object.defineProperty(pages,url,{enumerable:false,configurable:false,writable:false,value:func})
    pages.push(func)


    let viewname = func.path
    let at = viewname.indexOf('/:id')
    if(at>=0) viewname = viewname.substr(0,at)
    let component = views[viewname]
    let routeInfo:RouteConfigSingleView = func.routeInfo || (func.routeInfo={} as any);
    routeInfo.path  = func.path
    routeInfo.name = func.name
    routeInfo.component = component
    if((routes as any)[routeInfo.path]){
      console.error(`route已经存在${routeInfo.path}`,(funcs as any)[routeInfo.path],func)
      throw new Error(`route已经存在${routeInfo.path}`)
    }
    (routes as any)[routeInfo.path] = routeInfo
    let pNode = func.parent as Func
    let appendToChildren = false
    while(pNode){
      if(pNode.type===0) {
        console.warn('funcs表配置不恰当，page不应该是api的子功能',func,pNode)
        pNode = pNode.parent as Func
        continue
      }

      let pRouteInfo = pNode.routeInfo || (pNode.routeInfo={} as any)
      let parentChildren = pRouteInfo.children || (pRouteInfo.children=[])
      parentChildren.push(routeInfo)
      let subPages = pNode.pages || (pNode.pages=[])
      subPages.push(func)
      appendToChildren = true
      break
    }
    if(!appendToChildren){
      if(func.parent){
        console.error('未能将Func.page加入到路由中,其数据库中的结构不正确',func)
        throw new Error('未能将Func.page加入到路由中,其数据库中的结构不正确')
      }
      rootRoutes.push(routeInfo)
      if(func.type>=2){
        Object.defineProperty(menus,func.url,{enumerable:false,configurable:false,writable:false,value:func})
        menus.push(func)
        Object.defineProperty(rootMenus,func.url,{enumerable:false,configurable:false,writable:false,value:func})
        rootMenus.push(func)
      }
    }else{
      if(func.type>=2){
        while(pNode){
          if(pNode.type>=2){
            let submenus = pNode.menus || (pNode.menus=[])
            Object.defineProperty(submenus,func.url,{enumerable:false,configurable:false,writable:false,value:func})
            submenus.push(func)
          }
        }
      }
      
    }
  });
  Object.defineProperty(funcs,'@apis',{enumerable:false,configurable:false,writable:false,value:apis})
  Object.defineProperty(funcs,'@routes',{enumerable:false,configurable:false,writable:false,value:routes})
  Object.defineProperty(routes,'@roots',{enumerable:false,configurable:false,writable:false,value:rootRoutes})
  Object.defineProperty(funcs,'@pages',{enumerable:false,configurable:false,writable:false,value:pages})
  Object.defineProperty(pages,'@menus',{enumerable:false,configurable:false,writable:false,value:menus})
  Object.defineProperty(menus,'@roots',{enumerable:false,configurable:false,writable:false,value:rootMenus})
  let menuItems = []
  for(let i =0,j=rootMenus.length;i<j;i++){
    menuItems.push(makeMenuData(rootMenus[i]))
  }
  Object.defineProperty(menus,'@items',{enumerable:false,configurable:false,writable:false,value:menuItems})
}
function resolveRoutePath(func:Func):string{
  let url = func.path
  if(url[0]==='/') return url
  let pNode = func.parent as Func
  while(pNode){
    if(pNode.type===0){
      console.warn('funcs表配置不恰当，page不应该是api的子功能',func,pNode)
      pNode = pNode.parent as Func
      continue
    }
    let purl = pNode.url
    if(purl[0]==='/') return purl + url
    url = purl + '/' + url
    pNode = pNode.parent as Func
  }
  if(url[0]!=='/') return '/' + url
  return url

}

function makeMenuData(func:Func):MenuItem{
  let menuItem:MenuItem = {
    name:func.name,
    description:func.description,
    id:func.id,
    icon:func.icon,
    image:func.image,
    url:func.url
  }
  if(func.children && func.children.length){
    let children:MenuItem[] = menuItem.children = [];
    for(let i=0,j=func.children.length;i<j;i++){
      children.push(makeMenuData(func.children[i] as Func))
    }
  }
  return menuItem
}

