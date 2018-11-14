using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using DatingApp.Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Infrastructure.Data
{
    public class Seed
    {
        private readonly UserManager<ApplicationUser> _manager;

        public Seed(UserManager<ApplicationUser> manager)
        {
            _manager = manager;
        }


        public void SeedUsers()
        {
            if (!_manager.Users.Any())
            {
                var userData = File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<ApplicationUser>>(userData);
                foreach (var user in users)
                {
                    _manager.CreateAsync(user, "Password@123").Wait();
                }
            }
        }

    }
}
