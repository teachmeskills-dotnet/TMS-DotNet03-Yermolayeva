using HandiworkShop.Common.Constants;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HandiworkShop.DAL.Configurations
{
    /// <summary>
    /// EF Configuration for Profile entity.
    /// </summary>
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.ProfileTable)
                .HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.StandartLenghtForStringField);

            builder.Property(p => p.Avatar)
                .HasColumnType(ConfigurationConstants.AvatarFormat);

            builder.Property(p => p.Created)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.DateFormat);

            builder.Property(p => p.UserId)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithOne(i => i.Profile)
                .HasForeignKey<Profile>(p => p.UserId);
        }
    }
}