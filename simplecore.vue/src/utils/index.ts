export interface ITreeNode{
    id:string|number;
    parentId?:string|number;
    children?:ITreeNode[];
    parent?:ITreeNode;
}
export function makeTreeNodes<T extends ITreeNode>(nodes:any[],eachCallback?:{(node:T,raw:any):T}):T[]{
    let root :T[]= []
    let map :{[id:string]:T} = {}
    let parentNotFounds:T[] = [];
    for(let i = 0,j=nodes.length;i<j;i++){
        let rawNode = nodes[i]
        let node = clone(rawNode)
        Object.defineProperty(node,"__raw__",{enumerable:false,configurable:false,writable:false,value:rawNode})
        let id = rawNode.id
        map[id] = node
        let pid = rawNode.parentId
        if(pid){
            let p = map[pid]
            if (p) {
                node.parent = p
                if(eachCallback){
                    node = eachCallback(node,rawNode)|| node
                    map[id] = node
                }
                let children = p.children || (p.children=[])
                children.push(node)
                
            }else parentNotFounds.push(node)
        }else{
            root.push(node)
        }
        for(let i = 0,j=parentNotFounds.length;i<j;i++){
            let node = parentNotFounds[i]
            let p = map[node.parentId as string]
            if(!p){
                console.warn('指定了parentId，但未能找到该节点',node)
                continue
            }
            node.parent = p
            if(eachCallback){
                node = eachCallback(node, (node as any).__raw__)|| node
                map[id] = node
            }
            let children = p.children || (p.children=[])
            children.push(node)
        }
    }
    return root
}

export function clone(target:any, clones?:any):any {
  if (!target) return target
  const t = typeof target
  if (t !== 'object') return target
  if (!clones) clones = []
  for (const cloned of clones) {
    if (cloned.origin === target) return cloned.clone
  }
  let isArr:boolean = false
  if (target.length !== undefined) {
    if (target.push && target.pop) isArr = true
  }
  let ret:any
  if (isArr) {
    ret = []
    clones.push({ origin: target, clone: ret })
    for (var i = 0, j = target.length; i < j; i++) {
      ret.push(clone(target[i], clones))
    }
  } else {
    ret = {}
    clones.push({ origin: target, clone: ret })
    for (var n in target) ret[n] = clone(target[n], clones)
  }
  clones
  return ret
}

export function array_index<T=any>(arr:T[],item:T,at=0):number{
  if(!arr)return -1
  for(let i = at,j=arr.length;i<j;i++) if(arr[i]===item)return i
  return -1
}
export function array_contains<T=any>(arr:T[],item:T):boolean {
  return array_index(arr,item)>=0
}
export function array_seek<T=any>(arr:T[],predicate:(item:T,index:number)=>boolean):T{
  if(!arr) return undefined as any as T
  let item :T
  for(let i = 0,j=arr.length;i<j;i++) {
    item = arr[i]
    if(predicate(item,i)) return item
  }
  return undefined as any as T
}