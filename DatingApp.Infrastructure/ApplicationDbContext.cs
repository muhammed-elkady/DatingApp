using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DatingApp.Core.Entities;
using DatingApp.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace DatingApp.Infrastructure
{
    // Adding Generic <ApplicationUser> isn't included in the tutorial
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        // The DbSet<Users> are in the inherited IdentityDbContext 

        public DbSet<Photo> Photos { get; set; }

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
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
                userRole.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
            });

            // Cascade delete photos when user is deleted
            builder.Entity<Photo>()
            .HasOne(b => b.ApplicationUser)
            .WithMany(a => a.Photos)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
