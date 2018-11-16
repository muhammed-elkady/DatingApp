using DatingApp.Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace DatingApp.Infrastructure.Data
{
    public class Seeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly string path = "../DatingApp.Infrastructure/data/userseeddata.json";


        public Seeder(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = File.ReadAllText(path);
                var users = JsonConvert.DeserializeObject<List<ApplicationUser>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name= "Member"},
                    new Role{Name= "Admin"},
                    new Role{Name= "Moderator"},
                    new Role{Name= "VIP"}
                };
                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }
                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "Password@123").Wait();
                    _userManager.AddToRoleAsync(user, "Member").Wait();
                }

                var adminUser = new ApplicationUser { UserName = "Admin", Email = "Admin@Admin.com" };

                IdentityResult result = _userManager.CreateAsync(adminUser, "Password@123").Result;
                if (result.Succeeded)
                {
                    ApplicationUser admin = _userManager.FindByNameAsync("admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
                }
            }
        }
    }
}
