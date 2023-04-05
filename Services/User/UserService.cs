using GenshinTheoryCrafting.Controllers.Auth;
using GenshinTheoryCrafting.Models;
using GenshinTheoryCrafting.Models.Dto.User;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace GenshinTheoryCrafting.Services.User
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static Users user = new Users();
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string>> Login(UserDto request, IConfiguration configuration)
        {
            var serviceResponse = new ServiceResponse<string>();

            if (request.Username == null)
                throw new ArgumentNullException(nameof(request.Username));

            if (user.Username != request.Username)
            {
                serviceResponse.Data = "Not Found";
                return serviceResponse;
            }

            if (user.Email != request.Email)
            {
                serviceResponse.Data = "Not Found";
                return serviceResponse;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                serviceResponse.Data = "Not Found";
                return serviceResponse;
            }

            CrtToken crtToken = new CrtToken(configuration);
            var tServiceResponse = new ServiceResponse<string>();
            string token = crtToken.CreateToken(user);

            tServiceResponse.Data = token;

            return tServiceResponse;
        }

        public async Task<ServiceResponse<Users>> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var serviceResponse = new ServiceResponse<Users>();

            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.Admin = false;
            serviceResponse.Data = user;

            return serviceResponse; 
        }

        public async Task<ServiceResponse<string>> RegAdmin(UserDto request, IConfiguration configuration)
        {
            var serviceResponse = new ServiceResponse<string>();

            if (request.Username == null)
                throw new ArgumentNullException(nameof(request.Username));

            if (user.Username != request.Username)
            {
                serviceResponse.Message = "Not Found";
                return serviceResponse;
            }

            if (user.Email != request.Email)
            {
                serviceResponse.Message = "Not Found";
                return serviceResponse;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                serviceResponse.Message = "Not Found";
                return serviceResponse;
            }

            user.Admin = request.Admin;

            CrtToken crtToken = new CrtToken(configuration);
            var tServiceResponse = new ServiceResponse<string>();
            string token = crtToken.CreateToken(user);

            tServiceResponse.Data = token;

            return tServiceResponse;
        }
    }

}
