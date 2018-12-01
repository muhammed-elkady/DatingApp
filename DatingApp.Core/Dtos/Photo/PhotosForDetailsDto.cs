﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Dtos.Photo
{
    public class PhotosForDetailsDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }

    }
}