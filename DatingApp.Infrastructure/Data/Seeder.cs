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
        private readonly string path = "../DatingApp.Infrastructure/data/userseeddata.json";
        

        public Seeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = File.ReadAllText(path);

                var users = JsonConvert.DeserializeObject<List<ApplicationUser>>(userData);
                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "Password@123").Wait();
                }
            }
        }
    }
}
