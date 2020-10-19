using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HandiworkShop.Common.Constants;
using HandiworkShop.DAL.Entities;
using System;

namespace HandiworkShop.DAL.Configurations
{
    public class UserTagsConfiguration : IEntityTypeConfiguration<UserTags>
    {
        public void Configure(EntityTypeBuilder<UserTags> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.UserTagsTable)
                .HasKey(u => u.Id);

            builder.Property(u => u.TagId)
                .IsRequired();

            builder.Property(u => u.UserId)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(i => i.UserTags)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Tag)
                .WithMany(t => t.UserTags)
                .HasForeignKey(u => u.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
