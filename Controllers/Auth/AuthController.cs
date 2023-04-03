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

        [HttpGet, Authorize]
        public ActionResult<string> GetMyName()
        {
            return Ok(_userService.GetMyName());
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Users>> Register(UserDto request)
        {
            return Ok(_userService.Register(request));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Users>> Login(UserDto request)
        {
            var result = _userService.Login(request, _configuration);
            try
            {
                switch (result)
                {
                    case "Not Found":
                        return NotFound();
                        break;
                    case "Bad Request":
                        return BadRequest();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        [HttpPut("RegisterAdmin"), Authorize]
        public async Task<ActionResult<Users>> PutAdmin(UserDto request)
        {
            var result = _userService.RegAdmin(request, _configuration);
            try
            {
                switch (result)
                {
                    case "Not Found":
                        return NotFound();
                        break;
                    case "Bad Request":
                        return BadRequest();
                        break;
                    default:
                        break;
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }
    }
}
