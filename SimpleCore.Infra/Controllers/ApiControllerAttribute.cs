using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using SimpleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Controllers
{
    public class ApiControllerAttribute : Microsoft.AspNetCore.Mvc.ApiControllerAttribute, IApiVersionProvider,IExceptionFilter, IResultFilter, IRouteTemplateProvider
    {
        public ApiControllerAttribute() {
            this.Template = "[controller]";
        }

        public ApiControllerAttribute(string version) {
            this.Template = "[controller]";
            int major = 0;
            int minor = 0;
            if (version != null) {
                var vers = version.Split('.');
                int.TryParse(vers[0], out major);
                if (vers.Length > 1) int.TryParse(vers[1], out minor);
            }
           
            this.Versions = new List<ApiVersion>() { 
                new ApiVersion(major,minor)
            }; 
        }

        public ApiVersionProviderOptions Options { get; private set; }

        public IReadOnlyList<ApiVersion> Versions  {get;private set;}

        public string Name => null;

        public int? Order =>null;

        public string Template { get; set; }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            var jsonView = JsonViewModel.Error( ex.Message,ex);
            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.WriteAsync(jsonView.ToString()).Wait();
            context.HttpContext.Response.CompleteAsync().Wait();
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
           
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //var rs = context.Result;
        }
    }
}
