using System;
using System.Collections.Generic;
using System.Text;
using DatingApp.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using DatingApp.Core.Identity;

namespace DatingApp.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
           
        
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
