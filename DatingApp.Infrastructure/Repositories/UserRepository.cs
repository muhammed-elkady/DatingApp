using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Core.Identity;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ApplicationUser entity)
        {

            _context.Add(entity);
        }

        public void Delete(ApplicationUser entity)
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<ApplicationUser> GetUsersWithTheirRoles()
        {
            var myUsersAndRoles = _context.Users.Include(u => u.UserRoles).ToList();
            return myUsersAndRoles;
        }

    }
}