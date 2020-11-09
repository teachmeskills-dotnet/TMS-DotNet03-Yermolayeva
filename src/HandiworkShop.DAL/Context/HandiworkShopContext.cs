using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HandiworkShop.DAL.Configurations;
using HandiworkShop.DAL.Entities;
using System;

namespace HandiworkShop.DAL.Context
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class HandiworkShopContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">DbContextOptions</param>
        public HandiworkShopContext(DbContextOptions<HandiworkShopContext> options):base(options){}

        /// <summary>
        /// Profiles.
        /// </summary>
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Orders.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Tasks.
        /// </summary>
        public DbSet<Task> Tasks { get; set; }

        /// <summary>
        /// Tags.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// UserTags.
        /// </summary>
        public DbSet<UserTag> UserTags { get; set; }

        /// <summary>
        /// Comments.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new UserTagsConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
