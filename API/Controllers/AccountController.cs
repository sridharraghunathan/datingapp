using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterUser(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username)) return BadRequest("UserName already taken!!!");
            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = registerDTO.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            _dataContext.Add(user);
            await _dataContext.SaveChangesAsync();

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {

            //First check the user exists
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.UserName == loginDTO.Username);
            if (user == null) return BadRequest("Invalid User");

            //Get the Password Salt for the verfication
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            //Check the user Password is correct 

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (user.PasswordHash[i] != computedHash[i]) return Unauthorized("Invalid Username or Password");
            }
            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _dataContext.Users.AnyAsync(u => u.UserName == username.ToLower());
        }

    }
}