using AutoMapper;
using DatingApp.Core.Dtos.User;
using DatingApp.Core.Extensions;
using DatingApp.Core.Identity;
using System;
using System.Collections.Generic;
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
                 });
            //.ForMember(dst => dst.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(c => c.IsMain).Url) });


            CreateMap<ApplicationUser, UserForDetailsDto>()
              .ForMember(dest => dest.Age, opt =>
              {
                  opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
              });
            //.ForMember(dest => dest.PhotoUrl, opt => {
            //    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            //})

            CreateMap<ApplicationUser, UserForLoginDto>();
            CreateMap<UserForRegisterDto, ApplicationUser>();

        }
    }
}
