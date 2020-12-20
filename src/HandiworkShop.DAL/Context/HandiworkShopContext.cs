using HandiworkShop.DAL.Configurations;
using HandiworkShop.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public HandiworkShopContext(DbContextOptions<HandiworkShopContext> options) : base(options) { }

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
        /// OrderTags.
        /// </summary>
        public DbSet<OrderTag> OrderTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ApplyConfiguration(new ProfileConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new TaskConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new UserTagConfiguration());
            builder.ApplyConfiguration(new OrderTagConfiguration());

            base.OnModelCreating(builder);
        }
    }
}