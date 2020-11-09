const viewFiles = require.context('@/views', true,/.vue$/)
// https://webpack.js.org/guides/dependency-management/#requirecontext
export let views: {[index: string]: any} = viewFiles.keys().reduce((modules: any, modulePath: string) => {
  // set './app.js' => 'app'
  if (modulePath.indexOf('.component.vue') >= 0) return modules
  const value = viewFiles(modulePath)
  modulePath = modulePath.substr(1)
  modules[modulePath] = value.default
  return modules
}, {})

const configFiles = require.context('@/configs', true,/(.ts)|(.json)|(.js)$/)
export let configs:{[index:string]:any}  = configFiles.keys().reduce((modules: any, modulePath: string) => {
  // set './app.js' => 'app'
  const value = configFiles(modulePath)
  modulePath = modulePath.substr(1)
  modules[modulePath] = value.default
  return modules
}, {})
export default {
  views,
  configs
}
