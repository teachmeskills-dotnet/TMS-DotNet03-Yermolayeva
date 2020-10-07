using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyProject.BLL.Configurations;
using MyProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.BLL.Context
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class MyProjectContext : IdentityDbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">DbContextOptions</param>
        public MyProjectContext(DbContextOptions<MyProjectContext> options):base(options){}

        /// <summary>
        /// Profiles.
        /// </summary>
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new ProfileConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
