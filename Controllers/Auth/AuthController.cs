using GenshinTheoryCrafting.Models.Dto.User;
using GenshinTheoryCrafting.Models.User;
using GenshinTheoryCrafting.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GenshinTheoryCrafting.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;

        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        [HttpGet, Authorize]
        public ActionResult<string> GetMyName()
        {
            return Ok(_userService.GetMyName());
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.Admin = false;

            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserDto request)
        {
            if (request.Username == null)
                throw new ArgumentNullException(nameof(request.Username));

            if (user.Username != request.Username)
                return NotFound();

            if (user.Email != request.Email)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return BadRequest();

            CrtToken crtToken = new CrtToken(_configuration);

            string token = crtToken.CreateToken(user);
            
            return Ok(token);
        }

        [HttpPut("RegisterAdmin"), Authorize]
        public async Task<ActionResult<User>> PutAdmin(UserDto request)
        { 
            if (request.Username == null)
                throw new ArgumentNullException(nameof(request.Username));

            if (user.Username != request.Username)
                return NotFound();

            if (user.Email != request.Email)
                return NotFound();

            if (user.Admin)
                return BadRequest();

            user.Admin = true;

            CrtToken crtToken = new CrtToken(_configuration);
            string token = crtToken.CreateToken(user);

            return Ok(token);
        }
    }
}
