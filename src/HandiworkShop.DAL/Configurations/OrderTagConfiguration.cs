using HandiworkShop.Common.Constants;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandiworkShop.DAL.Configurations
{
    class OrderTagConfiguration : IEntityTypeConfiguration<OrderTag>
    {
        public void Configure(EntityTypeBuilder<OrderTag> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.OrderTagTable)
                .HasKey(o => o.Id);

            builder.Property(o => o.TagId)
                .IsRequired();

            builder.Property(o => o.OrderId)
                .IsRequired();

            builder.HasOne(o => o.Order)
                .WithMany(i => i.OrderTags)
                .HasForeignKey(o => o.OrderId);

            builder.HasOne(o => o.Tag)
                .WithMany(t => t.OrderTags)
                .HasForeignKey(u => u.TagId);
        }
    }
}
