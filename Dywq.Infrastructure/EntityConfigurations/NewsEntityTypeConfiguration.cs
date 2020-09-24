using Dywq.Domain.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class NoticeNewsEntityTypeConfiguration : BaseEntityTypeConfiguration<NoticeNews, int>
    {
        public override void Configure(EntityTypeBuilder<NoticeNews> builder)
        {
            base.Configure(builder);
            builder.ToTable("NoticeNews");
            builder.Property(p => p.Title).HasMaxLength(50);
            builder.Property(p => p.Source).HasMaxLength(50);
            builder.Property(p => p.Content);
            builder.Property(p => p.Pic).HasMaxLength(200);
            builder.Property(p => p.Show);
            builder.Property(p => p.Sort);
        }
    }
}
