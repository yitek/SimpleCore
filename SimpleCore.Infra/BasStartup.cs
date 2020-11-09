using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using SimpleCore.OpenApi;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace SimpleCore
{
    public class BasStartup
    {
        public BasStartup(IConfiguration configuration)
        {
            Configuration = GlobalObject.Configuration = configuration;
            this.ApiDocumentInfo = this.Configuration.GetSection("OpenDocument").Get<ApiDocumentInfo>();
            if (this.ApiDocumentInfo == null) {
                this.ApiDocumentInfo = new ApiDocumentInfo()
                {
                    Title = "WEB API 接口说明",
                    Description =null,
                    ConstractName = "YIY",
                    ContractEmail = "y-tec@qq.com",
                    ContractUrl = "http://localhost"
                };
            }

            if (this.ApiDocumentInfo.Versions == null) this.ApiDocumentInfo.Versions = new Dictionary<string, ApiDocumentInfo>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        protected ApiDocumentInfo ApiDocumentInfo { get; private set; }

        protected IReadOnlyList<ApiVersionDescription> ApiVersionDescriptors { get;private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            
            // 跨域支持
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("any", builder =>
            //    {
            //        var urls = new string[] { "https://localhost:8080", "http://0.0.0.0:3201" };
            //        //builder.WithOrigins("https://localhost:8080", "http://0.0.0.0:3201").AllowAnyHeader();
            //        builder.WithOrigins(urls) // 允许部分站点跨域请求
            //                                  //.AllowAnyOrigin() // 允许所有站点跨域请求（net core2.2版本后将不适用）
            //        .AllowAnyMethod() // 允许所有请求方法
            //        .AllowAnyHeader() // 允许所有请求头
            //        .AllowCredentials(); // 允许Cookie信息
            //    });
            //});

            
            
            services.AddControllers();

            this.AddApiVersioning(services);
            services.AddVersionedApiExplorer();

            this.ApiVersionDescriptors = services.BuildServiceProvider()
                     .GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions;
            services.AddMvcCore();

            this.ConfigureApiDocumentServices(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            


            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        protected virtual void ConfigureApiDocumentServices(IServiceCollection services)
        {
            //swagger依赖该操作做api发现
            //services.AddMvcCore().AddApiExplorer();
            IReadOnlyList<ApiVersionDescription> versionDescriptors = this.ApiVersionDescriptors;
            services.AddSwaggerGen(options =>
            {
                if (versionDescriptors != null && versionDescriptors.Count > 0)
                {
                    foreach (var versionDescriptor in versionDescriptors)
                    {
                        this.ApiDocumentInfo.Versions.TryGetValue(versionDescriptor.GroupName, out var docForVersion);
                        var contractUrl = docForVersion?.ContractUrl ?? this.ApiDocumentInfo.ContractUrl;
                        Uri uri = null;
                        if (!string.IsNullOrWhiteSpace(contractUrl)) uri = new Uri(contractUrl);
                        options.SwaggerDoc(versionDescriptor.GroupName, new OpenApiInfo
                        {
                            Version = versionDescriptor.GroupName,
                            Title = docForVersion?.Title ?? this.ApiDocumentInfo.Title,
                            Description = docForVersion?.Description ?? this.ApiDocumentInfo.Description ?? "版本:" + versionDescriptor.GroupName,
                            Contact = new OpenApiContact { Name = docForVersion?.ConstractName ?? this.ApiDocumentInfo.ConstractName, Email = docForVersion?.ConstractName ?? this.ApiDocumentInfo.ConstractName, Url = uri },
                            License = new OpenApiLicense { Name = docForVersion?.ConstractName ?? this.ApiDocumentInfo.ConstractName, Url = uri }
                        });
                    }
                    options.OperationFilter<SwaggerAutoVersionFilter>();
                    options.DocumentFilter<ControllerVersionFilter>();
                    options.OperationFilter<RemoveVersionParameterOperationFilter>();
                    options.DocumentFilter<SetVersionInPathDocumentFilter>();
                }
                else {
                    var contractUrl = this.ApiDocumentInfo.ContractUrl;
                    Uri uri = null;
                    if (!string.IsNullOrWhiteSpace(contractUrl)) uri = new Uri(contractUrl);
                    options.SwaggerDoc("v0", new OpenApiInfo
                    {
                        Version = "0",
                        Title = this.ApiDocumentInfo.Title,
                        Description = this.ApiDocumentInfo.Description ?? "版本:缺省",
                        Contact = new OpenApiContact { Name =  this.ApiDocumentInfo.ConstractName, Email =  this.ApiDocumentInfo.ConstractName, Url = uri },
                        License = new OpenApiLicense { Name =  this.ApiDocumentInfo.ConstractName, Url = uri }
                    });
                }
                
                

                //添加注释服务
                var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{this.GetType().Assembly.GetName().Name}.xml");
                options.IncludeXmlComments(xmlDocFile, true);
                

                // 添加授权
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                    { new OpenApiSecurityScheme{
                        Reference = new OpenApiReference(){
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
                });



                //options.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    var versions = apiDesc.CustomAttributes()
                //        .OfType<ApiVersionAttribute>()
                //        .SelectMany(attr => attr.Versions);

                //    return versions.Any(v => $"v{v.ToString()}" == docName);
                //});
                
                options.SchemaFilter<SwaggerIgnoreFilter>();
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //app.UseMvc(options=>options.map);

            

            this.ConfigureApiDocument(app, env);

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<CorsMiddleware>();
            //app.UseCors((options) => {
            //    // options.AllowAnyOrigin()
            //    var urls = new string[] { "https://localhost:8080", "http://0.0.0.0:3201" };
            //    //builder.WithOrigins("https://localhost:8080", "http://0.0.0.0:3201").AllowAnyHeader();
            //    options.WithOrigins(urls) // 允许部分站点跨域请求
            //                              //.AllowAnyOrigin() // 允许所有站点跨域请求（net core2.2版本后将不适用）
            //    .AllowAnyMethod() // 允许所有请求方法
            //    .AllowAnyHeader(); // 允许所有请求头
            //});





            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
            });
            if(this.ApiVersionDescriptors!=null && this.ApiVersionDescriptors.Count>0)
                app.UseApiVersioning();

            app.UseMiddleware<LayoutMiddleware>();
        }

        void AddApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(apiVersioningOptions =>
            {
                apiVersioningOptions.ApiVersionReader =
                ApiVersionReader.Combine(new QueryStringApiVersionReader() { ParameterNames = { "api-version"} }, new HeaderApiVersionReader() { HeaderNames = { "api-version" } });
                apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;//是否启用未指明版本API，指向默认版本
                var apiVersion = new Version(Convert.ToString(this.Configuration["DefaultApiVersion"]));
                apiVersioningOptions.DefaultApiVersion = new ApiVersion(apiVersion.Major, apiVersion.Minor);
                apiVersioningOptions.ReportApiVersions = true;
                apiVersioningOptions.UseApiBehavior = false; // It means include only api controller not mvc controller.
                //apiVersioningOptions.Conventions.Controller<AppController>().HasApiVersion(apiVersioningOptions.DefaultApiVersion);
                //apiVersioningOptions.Conventions.Controller<UserController>().HasApiVersion(apiVersioningOptions.DefaultApiVersion);
                apiVersioningOptions.ApiVersionSelector = new CurrentImplementationApiVersionSelector(apiVersioningOptions);
            }).AddVersionedApiExplorer(option =>
            {
                var apiVersion = new Version(Convert.ToString(this.Configuration["DefaultApiVersion"]));
                option.DefaultApiVersion = new ApiVersion(apiVersion.Major, apiVersion.Minor);

                option.GroupNameFormat = "'v'VVV";
                option.AssumeDefaultVersionWhenUnspecified = true;
            }); // It will be used to explorer api versioning and add custom text box in swagger to take version number.
        }

        protected virtual void ConfigureApiDocument(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseSwagger(c =>
            {
                //c.PreSerializeFilters.Add((swagger, httpReq) =>
                //{
                //    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                //});
            });
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            if (this.ApiVersionDescriptors != null && this.ApiVersionDescriptors.Count > 0) {
                app.UseSwaggerUI(c =>
                {
                    foreach (var versionDescriptor in this.ApiVersionDescriptors)
                    {
                        this.ApiDocumentInfo.Versions.TryGetValue(versionDescriptor.GroupName, out var docForVersion);

                        c.SwaggerEndpoint("/swagger/" + versionDescriptor.GroupName + "/swagger.json", docForVersion?.Title?? this.ApiDocumentInfo.Title + " v" + versionDescriptor.GroupName);
                    }

                });
            }
            
        }

        

    }
}
