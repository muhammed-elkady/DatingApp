using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Entities;
using DatingApp.Core.Helpers;
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

        public async Task<ApplicationUser> GetUser(string username)
        {
            var user = await _context.Users.Include(c => c.Photos).FirstOrDefaultAsync(c => c.UserName == username);
            return user;
        }

        public async Task<PagedList<ApplicationUser>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(c => c.Photos).Include(c => c.UserRoles);
            var usersWithoutAdmin = users.Where(c => c.UserRoles.All(x => x.Role.Name != "Admin"));
            return await PagedList<ApplicationUser>.CreateAsync(usersWithoutAdmin, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(string userId)
        {
            return await _context.Photos
                .Where(u => u.ApplicationUserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _context.Users.Include(c => c.Photos).FirstOrDefaultAsync(c => c.Id == id);
            return user;
        }
    }
}