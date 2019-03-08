using DatingApp.Core.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Entities
{
    public class Like
    {
        public string LikerId { get; set; }
        public string LikeeId { get; set; }

        // Navigational properties
        public ApplicationUser Liker { get; set; }
        public ApplicationUser Likee { get; set; }

    }
}
