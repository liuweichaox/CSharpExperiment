using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreWebDemo.Filters;
using CoreWebDemo.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace CoreWebDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient(typeof(VierificationCodeServices));

            services.AddSession();//Session功能

            services.AddDirectoryBrowser();//目录浏览功能

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalActionFilter));//过滤器通过类型注入
                options.Filters.Add(new GlobalExceptionFilter());//过滤器通过实例注入               
            });


            //services.AddDbContextPool<BloggingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MySql")));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider spv)
        {
            Helper.HttpContext.ServiceProvider = spv;

            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

           
            app.UseDefaultFiles();

            app.UseStaticFiles();

            //提供 Web 根目录外的文件
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "Upload")),
            //    RequestPath = "/Upload"
            //});

            //启用目录浏览
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "Upload")),
            //    RequestPath = "/Upload"
            //});

            //设置 HTTP 响应标头
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    OnPrepareResponse = ctx =>
            //    {
            //        // Requires the following import:
            //        // using Microsoft.AspNetCore.Http;
            //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");//单位：秒
            //    }
            //});

            //UseFileServer 结合了 UseStaticFiles、UseDefaultFiles 和 UseDirectoryBrowser 的功能。
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Upload")),
                RequestPath = "/Upload",
                EnableDirectoryBrowsing = true
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
