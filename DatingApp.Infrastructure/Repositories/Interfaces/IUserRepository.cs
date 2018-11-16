using DatingApp.Core.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> GetUsersWithTheirRoles();
    }
}
