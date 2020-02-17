using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Core.Dtos.Message;
using DatingApp.Core.Entities;
using DatingApp.Core.Helpers;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Spa.Controllers.Api
{
    //Authorized Controller
    [ApiController]
    [ServiceFilter(typeof(LogUserActivityActionFilter))]
    [Route("api/users/{userId}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMessageRepository _msgRepo;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepo,
            IMessageRepository msgRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _msgRepo = msgRepo;
            _mapper = mapper;
        }




        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(string userId, int id)
        {
            if (!CheckUserIdentity(userId))
                return Unauthorized();

            var msgFromRepo = await _msgRepo.GetMessage(id);
            if (msgFromRepo == null)
                return NotFound();

            return Ok(msgFromRepo);


        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(string userId, MessageForCreationDto msgDto)
        {
            if (!CheckUserIdentity(userId))
                return Unauthorized();

            msgDto.SenderId = userId;
            var recipient = await _userRepo.GetUserById(msgDto.RecipientId);

            if (recipient == null)
                return BadRequest("Couldn't find recipient");

            var message = _mapper.Map<Message>(msgDto);

            _msgRepo.Add(message);

            var msgToReturn = _mapper.Map<Message>(msgDto);

            if (await _msgRepo.SaveAll())
                return CreatedAtRoute(nameof(GetMessage), new { id = message.Id }, msgToReturn);

            return BadRequest("Creating Message failed on save");





        }


        #region Helpers
        private bool CheckUserIdentity(string id)
        {
            if (id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return true;

            return false;
        }
        #endregion


    }
}