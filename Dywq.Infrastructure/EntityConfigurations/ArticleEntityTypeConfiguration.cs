using Dywq.Domain.ArticleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class PartyBuildingArticleEntityTypeConfiguration : BaseEntityTypeConfiguration<PartyBuildingArticle, int>
    {
        public override void Configure(EntityTypeBuilder<PartyBuildingArticle> builder)
        {
            base.Configure(builder);
            builder.ToTable("PartyBuildingArticle");
            builder.Property(p => p.ThemeTitle).HasMaxLength(200);
            builder.Property(p => p.Subtitle).HasMaxLength(200);
            builder.Property(p => p.Source).HasMaxLength(100);
            builder.Property(p => p.Pic).HasMaxLength(200);
            builder.Property(p => p.Content);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Show);
        }
    }
   
}
