using AutoMapper;
using DatingApp.Core.Dtos.Photo;
using DatingApp.Core.Dtos.User;
using DatingApp.Core.Entities;
using DatingApp.Core.Extensions;
using DatingApp.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatingApp.Core.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationUser, UserForListDto>()
            .ForMember(dst => dst.Age, opt =>
            {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            })
            .ForMember(dest => dest.PhotoUrl, c =>
            {
                c.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url);
            });

            CreateMap<ApplicationUser, UserForDetailsDto>()
              .ForMember(dest => dest.Age, opt =>
              {
                  opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
              })
              .ForMember(dest => dest.PhotoUrl, c =>
              {
                  c.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url);
              });

            CreateMap<ApplicationUser, UserForLoginDto>();
            CreateMap<UserForRegisterDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserForRegisterDto>();
            CreateMap<Photo, PhotosForDetailsDto>();

        }
    }
}
