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
        private readonly IUserRepository _userRepo;
        private readonly IPhotoRepository _photoRepo;
        private readonly Cloudinary _cloudinary;

        public PhotosController(
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryOptions,
            IUserRepository repo,
            IPhotoRepository photoRepo)
        {
            _userRepo = repo;
            _photoRepo = photoRepo;
            _mapper = mapper;
            _cloudinaryOptions = cloudinaryOptions;

            Account cloudinaryAcc = new Account(
                _cloudinaryOptions.Value.CloudName,
                _cloudinaryOptions.Value.ApiKey,
                _cloudinaryOptions.Value.ApiSecret);

            _cloudinary = new Cloudinary(cloudinaryAcc);
        }

        [HttpGet(Name = "GetPhoto")] //[HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _userRepo.GetPhoto(id);
            if (photoFromRepo != null)
            {
                var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
                return Ok(photo);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(string id, [FromForm] PhotoForCreationDto photoForCreationDto)
        {

            if (!CheckUserIdentity(id))
            {
                return Unauthorized();
            }
            var userFromRepo = await _userRepo.GetUserById(id);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file != null)
            {

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


                if (await _userRepo.SaveAll())
                {
                    var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                    return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
                }
            }
            return BadRequest("Could not add the photo");

        }

        //id = userId
        [HttpPost("{photoId}/setMain")]
        public async Task<IActionResult> SetMainPhoto(string id, int photoId)
        {
            if (!CheckUserIdentity(id))
                return Unauthorized();

            var user = await _userRepo.GetUserById(id);
            if (!user.Photos.Any(p => p.Id == photoId))
                return Unauthorized();

            var photoFromRepo = await _userRepo.GetPhoto(photoId);
            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo!");


            var currentMainPhoto = await _userRepo.GetMainPhotoForUser(id);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _userRepo.SaveAll())
                return NoContent();


            return BadRequest("Could not set main photo!");


        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId, string id)
        {
            if (!CheckUserIdentity(id))
                return Unauthorized();

            var user = await _userRepo.GetUserById(id);
            if (!user.Photos.Any(p => p.Id == photoId))
                return BadRequest("Photo doesn't exist");

            var photoFromRepo = await _userRepo.GetPhoto(photoId);
            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo!");

            if (photoFromRepo.PublicId != null)
            {
                // Delete from cloudinary
                var result = _cloudinary.Destroy(new DeletionParams(photoFromRepo.PublicId));

                if (result.Result == "ok")
                    _photoRepo.Delete(photoFromRepo);

            }
            if (photoFromRepo.PublicId == null)
            {
                _photoRepo.Delete(photoFromRepo);
            }

            // Delete from DB
            if (await _photoRepo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete photo");

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