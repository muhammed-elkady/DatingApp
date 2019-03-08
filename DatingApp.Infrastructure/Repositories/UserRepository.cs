using System;
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
            var users = _context.Users.Include(c => c.Photos)
                .Include(c => c.UserRoles)
                .OrderByDescending(U => U.LastActive)
                .AsQueryable();

            users = users
            // exclude the admin user from returning to caller
            .Where(c => c.UserRoles.All(x => x.Role.Name != "Admin"))
            // exclude the user himself from returning to caller
            .Where(u => u.Id != userParams.UserId)
            // include the opposite gender to the caller
            .Where(u => u.Gender == userParams.Gender);


            // Retrieve the likers or likees
            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }


            // checks if the user specified an age range
            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                // calculate a DateTime based on Min date of birth
                var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);

                var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                // TODO: switch on a Enum of createria
                switch (userParams.OrderBy)
                {

                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;

                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<ApplicationUser>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
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

        public async Task<Like> GetLike(string userId, string recipientId)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(l => l.LikerId == userId && l.LikeeId == recipientId);
        }


        private async Task<IEnumerable<string>> GetUserLikes(string id, bool likers)
        {
            var user = await _context.Users.Include(x => x.Likers).Include(x => x.Likees)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                // return all the users that have like the currently logged in user
                return user.Likers.Where(u => u.LikeeId == id).Select(c => c.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(c => c.LikeeId);
            }
        }

    }
}