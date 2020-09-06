using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dywq.Infrastructure;
using Dywq.Infrastructure.Core;
using Dywq.Web.Extensions;
using Dywq.Web.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

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
            services.AddSession();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
               {
                   options.LoginPath = new PathString("/Home/Index");
                   options.AccessDeniedPath = new PathString("/Home/Privacy");
                   options.Events = new CookieAuthenticationEvents()
                   {
                       OnRedirectToAccessDenied = context =>
                       {
                           Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                                     "OnRedirectToAccessDenied", context.HttpContext.User.Identity.Name);
                           context.Response.ContentType = "application/json";
                           context.Response.StatusCode = StatusCodes.Status200OK;
                           context.Response.WriteAsync(JsonConvert.SerializeObject(Result.Failure("û��Ȩ��")));
                           return Task.CompletedTask;
                       },
                       OnRedirectToLogin = context =>
                       {
                           Console.WriteLine("{0} - {1}: {2}", DateTime.Now,
                                    "OnRedirectToLogin", context.HttpContext.User.Identity.Name);
                           if ((context.Request.Headers.ContainsKey("x-requested-with") && context.Request.Headers["x-requested-with"] == "XMLHttpRequest")||
                           (context.Request.Headers.ContainsKey("content-type") && context.Request.Headers["content-type"] == "application/json"))
                           {
                               context.Response.ContentType = "application/json";
                               context.Response.StatusCode = StatusCodes.Status200OK;
                               context.Response.WriteAsync(JsonConvert.SerializeObject(Result.Failure("û��Ȩ��")));
                               return Task.CompletedTask;
                           }
                           context.Response.Redirect($"{context.RedirectUri}");
                           context.Response.StatusCode = StatusCodes.Status302Found;
                           return Task.CompletedTask;
                       }
                   };

               });//��cookie�ķ�ʽ��֤��˳���ʼ����¼��ַ




        



            //�رղ����Զ�У��,������Ҫ�����Զ���ĸ�ʽ
            services.Configure<ApiBehaviorOptions>((o) =>
            {
                o.SuppressModelStateInvalidFilter = true;
            });

            services.AddMediatRServices();
            services.AddSqlServerDomainContext(Configuration.GetValue<string>("SqlServerSql"));
            services.AddRepositories();
            services.AddEventBus(Configuration);
            services.AddMd5(Configuration.GetSection("Md5Options"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

           

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dc = scope.ServiceProvider.GetService<DomainContext>();
                //dc.Database.EnsureCreated(); //�״δ������� ����дһ���ж� ��������ھͲ�����


                /* Ǩ�ƹ��̣�
                 1.Enable-Migrations������Ǩ��
                 2.����ÿ�����ݿ�仯  Add-Migration ��ע���磺 Add-Migration InitialCreate
                 
                 */
                if (dc.Database.GetPendingMigrations().Any())
                {
                    try
                    {
                        dc.Database.Migrate();//ִ��Ǩ�ƣ��Զ��������ݿ�
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("���������޸����ݿ�ʧ�ܡ���" + ex.Message);
                    }
                }
            }


            app.UseDeveloperExceptionPage();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();//��Ȩ�������û�е�¼����¼����˭����ֵ��User
            app.UseAuthorization();//������Ȩ�����Ȩ��

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");




            });
        }
    }
}
