using Dywq.Domain.Purchase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class PurchaseEntityTypeConfiguration : BaseEntityTypeConfiguration<Purchase, int>
    {
        public override void Configure(EntityTypeBuilder<Purchase> builder)
        {
            base.Configure(builder);
            builder.ToTable("Purchase");
            builder.Property(p => p.CompanyId);
            builder.Property(p => p.Mobile).HasMaxLength(20);
            builder.Property(p => p.ProductName).HasMaxLength(50);
            builder.Property(p => p.Contacts).HasMaxLength(50);
            builder.Property(p => p.Type);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Status);
            builder.Property(p => p.Content);
            builder.Property(p => p.Show);
        }
    }
}
