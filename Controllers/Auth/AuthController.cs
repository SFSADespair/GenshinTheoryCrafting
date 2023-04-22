using GenshinTheoryCrafting.Models.Dto.User;
using GenshinTheoryCrafting.Models;
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
using Microsoft.AspNetCore.Http.HttpResults;

namespace GenshinTheoryCrafting.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Users user = new Users();
        private readonly IConfiguration _configuration;

        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<Users>>> Register(UserDto request)
        {
            if (request is null)
                return BadRequest(request);

            return Ok(await _userService.Register(request));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<Users>>> Login(LoginDto request)
        {
            var response = await _userService.Login(request, _configuration);
            if (response.Data is null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("RegisterAdmin"), Authorize]
        public async Task<ActionResult<ServiceResponse<Users>>> PutAdmin(UserDto request)
        {
            var response = await _userService.RegAdmin(request, _configuration);

            if (response.Data is null)
            {
                Console.WriteLine(response.Data);
                return NotFound(response.Message);
            }

            return Ok(response);
        }
    }
}
