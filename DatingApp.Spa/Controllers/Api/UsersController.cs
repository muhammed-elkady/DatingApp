﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Core.Dtos.User;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Spa.Controllers.Api
{
    //Authorized Controller
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetAllUsers();
            var userToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(userToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailsDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserForUpdateDto user)
        {

            if (!checkUserIdentity(id))
            {
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(user, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user with {id} failed on save");
        }

        #region Helpers
        private bool checkUserIdentity(string id)
        {
            if (id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}