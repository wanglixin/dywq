using DotNetCore.CAP.Messages;
using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.SiteAggregate;
using Dywq.Domain.UserAggregate;
using Dywq.Infrastructure;
using Dywq.Infrastructure.Core.Utilitiy;
using Dywq.Infrastructure.Repositories;
using Dywq.Web.Application.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Dywq.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DomainContextTransactionBehavior<,>));
            return services.AddMediatR(typeof(SiteInfo).Assembly, typeof(Program).Assembly);
        }


        public static IServiceCollection AddDomainContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            return services.AddDbContext<DomainContext>(optionsAction);
        }

        //public static IServiceCollection AddInMemoryDomainContext(this IServiceCollection services)
        //{
        //    return services.AddDomainContext(builder => builder.UseInMemoryDatabase("domanContextDatabase"));
        //}

        public static IServiceCollection AddSqlServerDomainContext(this IServiceCollection services, string connectionString)
        {

            return services.AddDomainContext(builder =>
            {

                builder.UseSqlServer(connectionString);
            });
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IBaseRepository<Company>, BaseRepository<Company>>();
            services.AddScoped<IBaseRepository<CompanyField>, BaseRepository<CompanyField>>();
            services.AddScoped<IBaseRepository<CompanyFieldData>, BaseRepository<CompanyFieldData>>();
            services.AddScoped<IBaseRepository<CompanyFieldDefaultValue>, BaseRepository<CompanyFieldDefaultValue>>();
            services.AddScoped<IBaseRepository<CompanyFieldGroup>, BaseRepository<CompanyFieldGroup>>();
            services.AddScoped<IBaseRepository<SiteInfo>, BaseRepository<SiteInfo>>();
            services.AddScoped<IBaseRepository<CompanyUser>, BaseRepository<CompanyUser>>();
            services.AddScoped<IBaseRepository<CompanyType>, BaseRepository<CompanyType>>();
            services.AddScoped<IBaseRepository<PartyBuildingArticle>, BaseRepository<PartyBuildingArticle>>();
            return services;
        }



        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddCap(options =>
            {
                //options.UseEntityFramework<DomainContext>();

                options.UseSqlServer(configuration.GetValue<string>("SqlServerSql"));

                options.UseRabbitMQ(options =>
                {
                    configuration.GetSection("RabbitMQ").Bind(options);
                    //options.Port
                });
                options.FailedRetryCount = 20; //重试20次结束
                options.FailedThresholdCallback = failed =>
                {
                    var logger = failed.ServiceProvider.GetService<ILogger>();
                    logger.LogInformation($@"A message of type {failed.MessageType} failed after executing {options.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                };
                options.UseDashboard();
            });

            return services;
        }

        public static IServiceCollection AddMd5(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<Md5Options>().Configure(options =>
            {
                configuration.Bind(options);
            }).Services.AddSingleton(new Md5Options());

            services.AddScoped<IMd5, MyMd5>();
            return services;
        }

    }
}
