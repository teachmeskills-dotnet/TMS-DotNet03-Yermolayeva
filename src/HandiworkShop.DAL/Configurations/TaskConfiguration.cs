using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HandiworkShop.Common.Constants;
using HandiworkShop.DAL.Entities;
using System;

namespace HandiworkShop.DAL.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.TaskTable)
                .HasKey(t => t.Id);

            builder.Property(t => t.OrderId)
                .IsRequired();

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.LongLenghtForStringField);

            builder.Property(t => t.Start)
                .IsRequired()
                .HasColumnType(ConfigurationConstants.DateFormat);

            builder.Property(t => t.End)
                //.IsRequired()
                .HasColumnType(ConfigurationConstants.DateFormat);

            builder.HasOne(t => t.Order)
               .WithMany(o => o.Tasks)
               .HasForeignKey(t => t.OrderId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
