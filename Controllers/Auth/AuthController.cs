﻿using GenshinTheoryCrafting.Models.Dto.User;
using GenshinTheoryCrafting.Models.User;
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

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;

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
    }
}