using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using MyProject.Common.Constants;
using MyProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.BLL.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.ProfileTable)
                .HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.StandartLenghtForStringField);

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
