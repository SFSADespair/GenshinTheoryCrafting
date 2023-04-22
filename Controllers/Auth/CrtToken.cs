using GenshinTheoryCrafting.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GenshinTheoryCrafting.Controllers.Auth
{
    public class CrtToken
    {
        private readonly IConfiguration _configuration;

        public CrtToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(Users user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        _configuration.GetSection("AppSettings:Token").Value!
                    ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            if (user.Admin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            Console.WriteLine(user.Admin);

            return jwt;
        }
    }
}
