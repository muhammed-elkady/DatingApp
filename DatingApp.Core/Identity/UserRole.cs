using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Identity
{
    public class UserRole : IdentityUserRole<string>
    {
        // The RoleId, UserId are in the inherited IdentityUserRole<string>

        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }
}
