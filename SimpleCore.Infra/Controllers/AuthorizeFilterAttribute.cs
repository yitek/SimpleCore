using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using SimpleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SimpleCore.Controllers
{
    public class AuthorizeFilterAttribute : IActionFilter
    {
        IConfiguration _configuration;
        public AuthorizeFilterAttribute(IConfiguration configuration) {
            this._configuration = configuration;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as ControllerBase;
            if (controller == null || controller.User == null || controller.User.Anonymous) {
                var loginPage = this._configuration.GetValue<string>("loginPage");
                context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.Headers.Add("Location: " , UrlEncoder.Default.Encode(loginPage));
                var jsonView = JsonViewModel.Error(loginPage,"该操作需要先登录");
                context.HttpContext.Response.WriteAsync(jsonView.ToJson());
                context.HttpContext.Response.CompleteAsync().Wait();
            }
        }
    }
}
