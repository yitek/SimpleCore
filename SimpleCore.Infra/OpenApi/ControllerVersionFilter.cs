using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleCore.OpenApi
{
    internal class ControllerVersionFilter : IDocumentFilter
    {

        static readonly Regex r = new Regex("V\\d+$", RegexOptions.IgnoreCase);
        /// <summary>
        /// //swagger版本控制过滤
        /// </summary>
        /// <param name="swaggerDoc">文档</param>
        /// <param name="schemaRegistry">schema注册</param>
        /// <param name="apiExplorer">api概览</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //缓存目标路由api
            OpenApiPaths paths = new OpenApiPaths();
            //取版本
            var version = swaggerDoc.Info.Version;
            foreach (var path in swaggerDoc.Paths)
            {
                string newKey = r.Replace(path.Key, "");
                
                //保存修正的path
                paths.Add(newKey, path.Value);
            }
            //当前版本的swagger document
            swaggerDoc.Paths = paths;
        }
    }
}
