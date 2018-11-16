using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Spa.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetUsersWithTheirRoles()
        {
            return Ok("only admins can see this");
        }

        [HttpGet]
        [Authorize(Policy = "ModeratePhotoRole")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admins or Moderators can see this");
        }
    }
}