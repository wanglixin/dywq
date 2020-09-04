using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure;
using Dywq.Web.Extensions;
using Dywq.Web.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dywq.Web
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
            services.AddMvc(mvcOptions =>
            {
                mvcOptions.Filters.Add<CustomerExceptionFilter>();
                mvcOptions.Filters.Add<ActionFilter>();
            }).AddJsonOptions(jsonoptions =>
            {
                jsonoptions.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //关闭参数自动校验,我们需要返回自定义的格式
            services.Configure<ApiBehaviorOptions>((o) =>
            {
                o.SuppressModelStateInvalidFilter = true;
            });

            services.AddMediatRServices();
            services.AddSqlServerDomainContext(Configuration.GetValue<string>("SqlServerSql"));
            services.AddRepositories();
            services.AddEventBus(Configuration);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dc = scope.ServiceProvider.GetService<DomainContext>();
                //dc.Database.EnsureCreated(); //首次创建数据 可以写一个判断 如果不存在就不创建


                /* 迁移过程：
                 1.Enable-Migrations，启动迁徙
                 2.后面每次数据库变化  Add-Migration 备注，如： Add-Migration InitialCreate
                 
                 */
                if (dc.Database.GetPendingMigrations().Any())
                {
                    try
                    {
                        dc.Database.Migrate();//执行迁移，自动更新数据库
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("【创建或修改数据库失败】：" + ex.Message);
                    }
                }
            }


            app.UseDeveloperExceptionPage();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
