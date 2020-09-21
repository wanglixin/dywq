using Dywq.Domain.CooperationAggregate;
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
        }
    }
}
