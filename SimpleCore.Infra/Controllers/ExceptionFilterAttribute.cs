using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Controllers
{
    public class ExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            var jsonView = JsonViewModel.Error("系统异常，请联系管理员",ex);
            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.WriteAsync(jsonView.ToString());
            context.HttpContext.Response.CompleteAsync().Wait();
        }
    }
}
