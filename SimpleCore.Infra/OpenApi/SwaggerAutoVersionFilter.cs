using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.OpenApi
{
    public class SwaggerAutoVersionFilter : IOperationFilter
    {
        /// <summary>
        /// 应用过滤器
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            #region Swagger版本描述处理
            foreach (var parameter in operation.Parameters)
            {
                if (parameter.Name != "api-version") continue;
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                if (parameter.Description == null)
                {
                    parameter.Description = "填写版本号如:1.0";
                    parameter.Example = new OpenApiString(context.ApiDescription.GroupName.Replace("v", ""));
                }
            }
            #endregion
        }
    }
}
