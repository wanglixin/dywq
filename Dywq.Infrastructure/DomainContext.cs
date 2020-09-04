using DotNetCore.CAP;
using Dywq.Infrastructure.Core;
using Dywq.Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Dywq.Domain.SiteAggregate;
using Dywq.Domain.CompanyAggregate;
using Dywq.Domain.UserAggregate;

namespace Dywq.Infrastructure
{
    public class DomainContext : EFContext
    {
        public DomainContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options, mediator, capBus)
        {
        }

        public DbSet<SiteInfo> SiteInfos { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<CompanyField> CompanyFields { get; set; }
        public DbSet<CompanyFieldDefaultValue> CompanyFieldDefaultValues { get; set; }
        public DbSet<CompanyFieldGroup> CompanyFieldGroups { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CompanyFieldData> CompanyFieldDatas { get; set; }

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
            
            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
