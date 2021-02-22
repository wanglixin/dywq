using Dywq.Domain.ArticleAggregate;
using Dywq.Domain.Expert;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class ExpertTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<ExpertType, int>
    {
        public override void Configure(EntityTypeBuilder<ExpertType> builder)
        {
            base.Configure(builder);
            builder.ToTable("ExpertType");
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.Sort);
        }
    }



    class ExpertEntityTypeConfiguration : BaseEntityTypeConfiguration<Expert, int>
    {
        public override void Configure(EntityTypeBuilder<Expert> builder)
        {
            base.Configure(builder);
            builder.ToTable("Expert");
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Pic).HasMaxLength(200);
            builder.Property(p => p.Title).HasMaxLength(50);
            builder.Property(p => p.Introduction);
            builder.Property(p => p.Show);
            builder.Property(p => p.ExpertTypeId);
            builder.Property(p => p.UserId);

        }
    }


}
