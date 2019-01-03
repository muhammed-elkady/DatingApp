using DatingApp.Core.Entities;
using DatingApp.Core.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetUsersWithTheirRoles();

        Task<ApplicationUser> GetUser(string username);

        Task<ApplicationUser> GetUserById(string id);

        Task<IEnumerable<ApplicationUser>> GetAllUsers();

        Task<Photo> GetPhoto(int id);

        Task<Photo> GetMainPhotoForUser(string userId);
    }
}
