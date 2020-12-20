using HandiworkShop.Common.Constants;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HandiworkShop.DAL.Configurations
{
    /// <summary>
    /// EF Configuration for UserTag entity.
    /// </summary>
    public class UserTagConfiguration : IEntityTypeConfiguration<UserTag>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserTag> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.UserTagTable)
                .HasKey(u => u.Id);

            builder.Property(u => u.TagId)
                .IsRequired();

            builder.Property(u => u.UserId)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(i => i.UserTags)
                .HasForeignKey(u => u.UserId);

            builder.HasOne(u => u.Tag)
                .WithMany(t => t.UserTags)
                .HasForeignKey(u => u.TagId);
        }
    }
}