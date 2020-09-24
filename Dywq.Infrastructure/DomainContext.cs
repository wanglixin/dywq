using DotNetCore.CAP;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Dywq.Domain.SiteAggregate;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;
using Microsoft.Extensions.Logging;

namespace Dywq.Infrastructure
{
    public class DomainContext : EFContext
    {
        //创建日志工厂
      

        public DomainContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options, mediator, capBus)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 注册领域模型与数据库的映射关系
            modelBuilder.ApplyConfiguration(new SiteInfoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyFieldEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyFieldDefaultValueEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyFieldGroupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyFieldDataEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PartyBuildingArticleEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PolicyTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PolicyArticleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GuestbookEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new CooperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CooperationInfoEntityConfiguration());

            modelBuilder.ApplyConfiguration(new FinancingEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ExpertEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExpertTypeEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PurchaseEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new NoticeNewsEntityTypeConfiguration());

            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
