using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Identity
{
    public class UserRole
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
