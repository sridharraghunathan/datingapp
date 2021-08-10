using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            // Getting the UserId By AuthorizationCode
            var sourceUserId = User.GetUserId();
            //Getting the Liked User Profile
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            if (likedUser == null) return NotFound();
            //Getting the Source User Profile to Update the Like
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);
            //Checking whether given username he like not the same his name
            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");
            //Checking the Profile he liked has been Liked by before
            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            if (userLike != null) return BadRequest("You already like this user");
            //Creating the instance to UserLike Table
            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };
            //Adding the Information to the Like table
            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {

            // This is fetching the users who Liked other User with complete Object
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
            users.TotalCount, users.TotalPages);
            return Ok(users);
        }
    }
}