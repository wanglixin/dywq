using Dywq.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User, int>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.ToTable("User");
            builder.Property(p => p.UserName).HasMaxLength(50);
            builder.Property(p => p.Password).HasMaxLength(150);
            builder.Property(p => p.Type);

            builder.Property(p => p.RealName).HasMaxLength(50);
            builder.Property(p => p.IDCard).HasMaxLength(50);
            builder.Property(p => p.Mobile).HasMaxLength(20);
            builder.Property(p => p.LoginCount);

        }
    }

    class CompanyUserEntityTypeConfiguration : BaseEntityTypeConfiguration<CompanyUser, int>
    {
        public override void Configure(EntityTypeBuilder<CompanyUser> builder)
        {
            base.Configure(builder);
            builder.ToTable("CompanyUser");
            builder.Property(p => p.UserId);
            builder.Property(p => p.CompanyId);

        }
    }
}
