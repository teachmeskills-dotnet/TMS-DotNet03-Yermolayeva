using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HandiworkShop.Common.Constants;
using HandiworkShop.DAL.Entities;
using System;

namespace HandiworkShop.DAL.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.TagTable)
                .HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.ShortLenghtForStringField);
        }
    }
}
