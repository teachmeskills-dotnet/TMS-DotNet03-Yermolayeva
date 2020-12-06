﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HandiworkShop.Common.Constants;
using HandiworkShop.DAL.Entities;
using System;

namespace HandiworkShop.DAL.Configurations
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.OrderTable)
                .HasKey(o => o.Id);

            builder.Property(o => o.Price)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.DecimalFormat);

            builder.Property(o => o.ClientId)
                .IsRequired();

            builder.Property(o => o.Title)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.StandartLenghtForStringField);

            builder.Property(o => o.Start)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.DateFormat);

            builder.Property(o => o.End)
                .HasColumnType(ConfigurationConstants.DateFormat);

            builder.Property(o => o.CommentCreated)
                .HasColumnType(ConfigurationConstants.DateFormat);

            builder.HasOne(o => o.Vendor)
               .WithMany(i => i.VendorOrders)
               .HasForeignKey(o => o.VendorId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Client)
               .WithMany(i => i.ClientOrders)
               .HasForeignKey(o => o.ClientId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
