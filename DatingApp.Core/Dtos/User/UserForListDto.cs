using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Dtos.User
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        // PhotoUrl is mapped from the main photo in the Application User 
        public string PhotoUrl { get; set; }
    }
}
