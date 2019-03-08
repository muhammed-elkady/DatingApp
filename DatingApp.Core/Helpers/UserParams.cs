﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }


        public int PageNumber { get; set; } = 1;


        public string UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;




    }
}
