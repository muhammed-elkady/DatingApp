﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Identity;
using DatingApp.Infrastructure.Repositories.Interfaces;
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

        public IEnumerable<ApplicationUser> GetUsersWithTheirRoles()
        {
            var myUsersAndRoles = _context.Users.Include(u => u.UserRoles).ToList();
            return myUsersAndRoles;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            var user = await _context.Users.Include(c => c.Photos).FirstOrDefaultAsync(c => c.Id == id);
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            var users = await _context.Users.Include(c => c.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}