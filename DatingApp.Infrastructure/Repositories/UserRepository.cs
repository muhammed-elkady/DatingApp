using DatingApp.Core.Identity;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatingApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetUsersWithTheirRoles()
        {
            var myUsersAndRoles = _context.Users.Include(u => u.UserRoles).ToList();
            return myUsersAndRoles;
        }


    }
}
