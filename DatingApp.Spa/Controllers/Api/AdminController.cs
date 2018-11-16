using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Infrastructure;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Spa.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public AdminController(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
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

        [HttpGet]
        [Authorize(Policy = "ModeratePhotoRole")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admins or Moderators can see this");
        }
    }
}