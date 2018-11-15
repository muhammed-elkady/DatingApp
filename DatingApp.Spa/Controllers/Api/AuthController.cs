using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Core.Dtos.User;
using DatingApp.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DatingApp.Spa.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(IConfiguration config,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
        }


        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(userForLoginDto.UserName);
                var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);
                if (result.Succeeded)
                {

                    // TODO: include() the PHOTOS in the returning result
                    // EXEPTION: fires here becuase of PhotoUrl not mapped
                    var userToReturn = _mapper.Map<UserForListDto>(user);

                    return Ok(new { token = "token", user = userToReturn });
                }

            }
            return BadRequest(userForLoginDto);

        }


        public async Task<IActionResult> Register(UserForRegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var userToCreate = _mapper.Map<ApplicationUser>(registerDto);
                var result = await _userManager.CreateAsync(userToCreate, registerDto.Password);
                var userToReturn = _mapper.Map<UserForRegisterDto>(userToCreate);

                if (result.Succeeded)
                {
                    return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
                }
                return BadRequest(result.Errors);

            }
            return BadRequest(registerDto);
        }

    }
}