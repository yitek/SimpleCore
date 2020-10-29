
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SimpleCore.Domains;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace SimpleCore.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup:BasStartup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration):base(configuration)
        {
            
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        // 
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            services.AddDbContext<MyDbContext>(options => {
                options.UseMySql(Configuration.GetConnectionString("MySqlContext"));
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app,env);
            //启用中间件服务生成Swagger作为JSON终结点
            
        }
    }
}
