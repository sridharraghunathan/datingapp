using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

        // //  [AllowAnonymous]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        // {
        //     var users = await _userRepository.GetUsersAsync();
        //     return Ok(users);
        // }

        // // [Authorize]
        // [HttpGet("{username}")]
        // public async Task<ActionResult<AppUser>> GetUser(string username)
        // {
        //     return await _userRepository.GetUserByUsernameAsync(username);
        // }
    }
}