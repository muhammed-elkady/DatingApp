using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Core.Dtos.User;
using DatingApp.Core.Helpers;
using DatingApp.Core.Identity;
using DatingApp.Infrastructure.Repositories.Interfaces;
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
        private readonly JwtFactory _jwtFactory;
        private readonly IUserRepository _userRepo;

        public AuthController(IConfiguration config,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, JwtFactory jwtFactory,
            IUserRepository userRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
            _jwtFactory = jwtFactory;
            _userRepo = userRepo;
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            if (ModelState.IsValid)
            {
                //ApplicationUser user = await _userManager.FindByNameAsync(userForLoginDto.UserName);
                var user = await _userRepo.GetUserById(userForLoginDto.UserName);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);
                    if (result.Succeeded)
                    {
                        var userToReturn = _mapper.Map<UserForListDto>(user);
                        var token = await _jwtFactory.GenerateJwtToken(user);
                        return Ok(new { token = token, user = userToReturn }); //user = userToReturn
                    }
                }
                return NotFound();

            }
            return BadRequest(userForLoginDto);
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var userToCreate = _mapper.Map<ApplicationUser>(registerDto);
                // TODO: handle when the username already exists
                var result = await _userManager.CreateAsync(userToCreate, registerDto.Password);
                var userToReturn = _mapper.Map<UserForRegisterDto>(userToCreate);

                if (result.Succeeded)
                {
                    // TODO: Create the users controller!
                    // TODO: Sign In the user on registeration! 
                    return Ok(userToReturn);
                    //return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(registerDto);
        }

        [HttpPost]
        public async Task<IActionResult> UserNameExists(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            if (result == null)
                return NotFound();

            return Ok($"{userName} exists!");
        }

        [HttpPost]
        public IActionResult UserSignedIn(string userName)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)User.Claims;
            var result = _signInManager.IsSignedIn(claimPrincipal);
            if (result)
                return Ok(true);

            return Ok(false);

        }



    }
}