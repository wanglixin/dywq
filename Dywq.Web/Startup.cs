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

            //�رղ����Զ�У��,������Ҫ�����Զ���ĸ�ʽ
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
