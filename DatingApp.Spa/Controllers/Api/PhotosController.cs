using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Core.Dtos.Photo;
using DatingApp.Core.Entities;
using DatingApp.Core.Helpers;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.Spa.Controllers.Api
{
    [Route("api/users/{id}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryOptions;
        private readonly IUserRepository _repo;
        private readonly Cloudinary _cloudinary;

        public PhotosController(
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryOptions,
            IUserRepository repo)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryOptions = cloudinaryOptions;

            Account cloudinaryAcc = new Account(
                _cloudinaryOptions.Value.CloudName,
                _cloudinaryOptions.Value.ApiKey,
                _cloudinaryOptions.Value.ApiSecret);

            _cloudinary = new Cloudinary(cloudinaryAcc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(string id, PhotoForCreationDto photoForCreationDto)
        {
            if (!CheckUserIdentity(id))
            {
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUser(id);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                // Load the image into memory
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                        .Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);


            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }
            return BadRequest("Could not add the photo");

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