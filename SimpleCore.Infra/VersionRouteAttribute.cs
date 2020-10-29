
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore
{
    /// <summary>
    /// 自定义路由 /api/{version}/[controler]/[action]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class VersionRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {

        /// <summary>
        /// 分组名称,是来实现接口 IApiDescriptionGroupNameProvider
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 自定义路由构造函数，继承基类路由
        /// </summary>
        /// <param name="actionName"></param>
        //public VersionRouteAttribute(string template = "[controller]") : base("/api/{version}/" + template)
        //{
        //}
        /// <summary>
        /// 自定义版本+路由构造函数，继承基类路由
        /// </summary>
        /// <param name="template"></param>
        /// <param name="version"></param>
        public VersionRouteAttribute(string version, string template = "[controller]") : base($"/api/{version.ToString()}/" + template)
        {
            GroupName = version.ToString();
        }
    }
}
