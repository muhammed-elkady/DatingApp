using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Core.Dtos.User;
using DatingApp.Core.Entities;
using DatingApp.Core.Extensions;
using DatingApp.Core.Helpers;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Spa.Controllers.Api
{
    //Authorized Controller
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivityActionFilter))]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ILikeRepository _likeRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, ILikeRepository likeRepo, IMapper mapper)
        {
            _repo = repo;
            _likeRepo = likeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _repo.GetUserById(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender.ToLower() == "male" ? "female" : "male";



            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);


            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _repo.GetUserById(id);
            var userToReturn = _mapper.Map<UserForDetailsDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserForUpdateDto user)
        {

            if (!CheckUserIdentity(id))
            {
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUserById(id);
            _mapper.Map(user, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user with {id} failed on save");
        }



        [HttpPost("{id}/like/{recepientId}")]
        public async Task<IActionResult> LikeUser(string id, string recepientId)
        {
            if (!CheckUserIdentity(id))
                return Unauthorized();

            var like = await _repo.GetLike(id, recepientId);

            if (like != null)
                return BadRequest("You already like this user");


            if (await _repo.GetUserById(recepientId) == null)
                return NotFound();

            like = new Like { LikerId = id, LikeeId = recepientId };



            _likeRepo.Add(like);

            if (await _likeRepo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user");


        }


        #region Helpers
        private bool CheckUserIdentity(string id)
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