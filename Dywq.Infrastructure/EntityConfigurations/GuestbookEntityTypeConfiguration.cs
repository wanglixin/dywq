using Dywq.Domain.GuestbookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dywq.Infrastructure.EntityConfigurations
{
    class GuestbookEntityTypeConfiguration : BaseEntityTypeConfiguration<Guestbook, int>
    {
        public override void Configure(EntityTypeBuilder<Guestbook> builder)
        {
            base.Configure(builder);
            builder.ToTable("Guestbook");
            builder.Property(p => p.UserId).HasMaxLength(200);
            builder.Property(p => p.Content).HasMaxLength(1000);
            builder.Property(p => p.Type);
            builder.Property(p => p.ReplyId);
            builder.Property(p => p.Status);
        }
    }
}
