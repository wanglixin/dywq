using Dywq.Domain.CompanyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class CompanyEntityTypeConfiguration : BaseEntityTypeConfiguration<Company, int>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            base.Configure(builder);
            builder.ToTable("Company");
            builder.Property(p => p.Logo).HasMaxLength(150);
            builder.Property(p => p.CompanyTypeId);
            builder.Property(p => p.Name).HasMaxLength(150); ;
            builder.Property(p => p.Contact);
            builder.Property(p => p.CooperationContent);
            builder.Property(p => p.Introduce);
            builder.Property(p => p.IntroduceImage).HasMaxLength(200);
            builder.Property(p => p.MainBusiness);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Show);
            builder.Property(p => p.Status);
        }
    }

    class CompanyFieldEntityTypeConfiguration : BaseEntityTypeConfiguration<CompanyField, int>
    {
        public override void Configure(EntityTypeBuilder<CompanyField> builder)
        {
            base.Configure(builder);
            builder.ToTable("CompanyField");
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.GroupId);
            builder.Property(p => p.Sort);
            builder.Property(p => p.Type);
            builder.Property(p => p.Alias).HasMaxLength(50);
            builder.Property(p => p.Required);

        }
    }



    class CompanyFieldDefaultValueEntityTypeConfiguration : BaseEntityTypeConfiguration<CompanyFieldDefaultValue, int>
    {
        public override void Configure(EntityTypeBuilder<CompanyFieldDefaultValue> builder)
        {
            base.Configure(builder);
            builder.ToTable("CompanyFieldDefaultValue");
            builder.Property(p => p.CompanyFieldId);
            builder.Property(p => p.Value).HasMaxLength(50);
            builder.Property(p => p.Text).HasMaxLength(50);
            builder.Property(p => p.Sort);

        }
    }

    class CompanyFieldGroupEntityTypeConfiguration : BaseEntityTypeConfiguration<CompanyFieldGroup, int>
    {
        public override void Configure(EntityTypeBuilder<CompanyFieldGroup> builder)
        {
            base.Configure(builder);
            builder.ToTable("CompanyFieldGroup");
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.Sort);

        }
    }

    class CompanyFieldDataEntityTypeConfiguration : BaseEntityTypeConfiguration<CompanyFieldData, int>
    {
        public override void Configure(EntityTypeBuilder<CompanyFieldData> builder)
        {
            base.Configure(builder);
            builder.ToTable("CompanyFieldData");
            builder.Property(p => p.CompanyId);
            builder.Property(p => p.FieldId);
            builder.Property(p => p.Value);
            builder.Property(p => p.Type);
            builder.Property(p => p.Alias);

        }
    }

    class CompanyTypeEntityTypeConfiguration : BaseEntityTypeConfiguration<CompanyType, int>
    {
        public override void Configure(EntityTypeBuilder<CompanyType> builder)
        {
            base.Configure(builder);
            builder.ToTable("CompanyType");
            builder.Property(p => p.Name).HasMaxLength(50); 
            builder.Property(p => p.Sort);

        }
    }


}
