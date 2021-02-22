using Dywq.Domain.FinancingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class FinancingEntityTypeConfiguration : BaseEntityTypeConfiguration<Financing, int>
    {
        public override void Configure(EntityTypeBuilder<Financing> builder)
        {
            base.Configure(builder);
            builder.ToTable("Financing");
            builder.Property(p => p.CompanyId);
            builder.Property(p => p.Bank).HasMaxLength(50);
            builder.Property(p => p.Title).HasMaxLength(50);
            builder.Property(p => p.Content);
            builder.Property(p => p.Pic).HasMaxLength(200);
            builder.Property(p => p.Show);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Status);
            builder.Property(p => p.UserId);
            builder.Property(p => p.Describe);
        }
    }
}
