using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.OpenApi
{
    public class LayoutMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 管道执行到该中间件时候下一个中间件的RequestDelegate请求委托，如果有其它参数，也同样通过注入的方式获得
        /// </summary>
        /// <param name="next"></param>
        public LayoutMiddleware(RequestDelegate next)
        {
            //通过注入方式获得对象
            _next = next;
        }

        /// <summary>
        /// 自定义中间件要执行的逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;
            var ass = this.GetType().Assembly;

            path = "SimpleCore.Asserts." + path.TrimStart('/').Replace('/','.');

            string content = null;
            using (var stream = ass.GetManifestResourceStream(path))
            {
                if (stream == null) { await _next(context); return; }
                using (StreamReader sr = new StreamReader(stream))
                {
                    content = sr.ReadToEnd();
                }

            }
            if (content != null)
            {
                //await context.Response.sta
                await context.Response.WriteAsync(content, System.Text.Encoding.UTF8);

                return;
            }

            await _next(context);//把context传进去执行下一个中间件
            
        }
    }
}
