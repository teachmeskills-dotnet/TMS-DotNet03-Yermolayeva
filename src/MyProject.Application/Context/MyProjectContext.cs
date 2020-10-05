using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.Application.Context
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
    }
}
