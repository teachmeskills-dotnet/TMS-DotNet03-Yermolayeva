using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Common.Constants;
using MyProject.DAL.Entities;
using System;

namespace MyProject.DAL.Configurations
{
    class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.CommentTable)
                .HasKey(o => o.Id);

            builder.HasOne(c => c.Author)
               .WithMany(i => i.AuthorComments)
               .HasForeignKey(c => c.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Profile)
               .WithMany(i => i.ProfileComments)
               .HasForeignKey(c => c.ProfileId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
