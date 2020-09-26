
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dywq.Domain.SiteAggregate;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class SiteInfoEntityTypeConfiguration : BaseEntityTypeConfiguration<SiteInfo, int>
    {
        public override void Configure(EntityTypeBuilder<SiteInfo> builder)
        {
            base.Configure(builder);
            builder.ToTable("SiteInfo");
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.Domain).HasMaxLength(200);
            builder.Property(p => p.Desc).HasMaxLength(1000);

        }
    }


    class AboutUsEntityTypeConfiguration : BaseEntityTypeConfiguration<AboutUs, int>
    {
        public override void Configure(EntityTypeBuilder<AboutUs> builder)
        {
            base.Configure(builder);
            builder.ToTable("AboutUs");
            builder.Property(p => p.Title).HasMaxLength(50);
            builder.Property(p => p.Content);
            builder.Property(p => p.Pic).HasMaxLength(200);
            builder.Property(p => p.Show);
            builder.Property(p => p.Sort);

        }
    }
}
