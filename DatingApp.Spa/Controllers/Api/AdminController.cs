using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Admin;
using DatingApp.Core.Identity;
using DatingApp.Infrastructure;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Spa.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IUserRepository userRepository,
         UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetUsersWithTheirRoles()
        {
            var usersFromRepo = _userRepository.GetUsersWithTheirRoles();
            var userWithRoleNames = usersFromRepo.Select(user => new
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = (
                          user.UserRoles
                          .Where(ur => ur.RoleId == _context.Roles.FirstOrDefault(c => c.Id == ur.RoleId).Id)
                          .Select(c => c.Role.Name)
                          ).ToList()
            }).ToList();

            return Ok(userWithRoleNames);
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.RoleNames;

            selectedRoles = selectedRoles ?? new string[] { };
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove the roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }


        [HttpGet]
        [Authorize(Policy = "ModeratePhotoRole")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admins or Moderators can see this");
        }
    }
}