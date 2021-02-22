using Dywq.Domain.CooperationAggregate;
using Dywq.Domain.InvestmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class CooperationTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<CooperationType, int>
    {
        public override void Configure(EntityTypeBuilder<CooperationType> builder)
        {
            base.Configure(builder);
            builder.ToTable("CooperationType");
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.Sort);
        }
    }


    class CooperationInfoEntityConfiguration : BaseEntityTypeConfiguration<CooperationInfo, int>
    {
        public override void Configure(EntityTypeBuilder<CooperationInfo> builder)
        {
            base.Configure(builder);
            builder.ToTable("CooperationInfo");
            builder.Property(p => p.CooperationTypeId);
            builder.Property(p => p.Title).HasMaxLength(50);
            builder.Property(p => p.Content);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Show);
            builder.Property(p => p.CompanyId);
            builder.Property(p => p.UserId);
        }

    }




    class InvestmentTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<InvestmentType, int>
    {
        public override void Configure(EntityTypeBuilder<InvestmentType> builder)
        {
            base.Configure(builder);
            builder.ToTable("InvestmentType");
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.Sort);
        }
    }


    class InvestmentInfoEntityConfiguration : BaseEntityTypeConfiguration<InvestmentInfo, int>
    {
        public override void Configure(EntityTypeBuilder<InvestmentInfo> builder)
        {
            base.Configure(builder);
            builder.ToTable("InvestmentInfo");
            builder.Property(p => p.InvestmentTypeId);
            builder.Property(p => p.Title).HasMaxLength(50);
            builder.Property(p => p.Content);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Show);
            builder.Property(p => p.CompanyId);
            builder.Property(p => p.UserId);
        }
    }

}
