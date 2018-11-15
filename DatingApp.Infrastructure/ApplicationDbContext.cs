using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DatingApp.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace DatingApp.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(IConfiguration Configuration)
        {

        }
        public IConfiguration Configuration { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                var configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // To configure 'UserRole' Many-to-many relationship
            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.ApplicationUserId, ur.RoleId });
                userRole.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
                userRole.HasOne(ur => ur.ApplicationUser).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.ApplicationUserId).IsRequired();
            });
        }
    }
}
