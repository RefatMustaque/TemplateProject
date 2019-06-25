using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TemplateProject.EntityModels;
using TemplateProject.Models;

namespace TemplateProject.Data
{
    public class TemplateProjectDbContext : IdentityDbContext<ApplicationUser>
    {
        public TemplateProjectDbContext(DbContextOptions<TemplateProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<LoginHistory> LoginHistories { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserTypeId)
                .IsUnique();
                
            base.OnModelCreating(builder);
        }
    }
}
