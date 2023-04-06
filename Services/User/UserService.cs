
using GenshinTheoryCrafting.Controllers.Auth;
using GenshinTheoryCrafting.Models;
using GenshinTheoryCrafting.Models.Dto.User;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace GenshinTheoryCrafting.Services.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        public static Users user = new Users();

        public UserService( IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(UserDto request, IConfiguration configuration)
        {
            var tServiceResponse = new ServiceResponse<string>();
            try
            {
                if (request.Username is null)
                    throw new Exception($"User with username '{request.Username}' not found.");

                if (user.Username != request.Username)
                    throw new Exception($"User with username '{request.Username}' not found.");

                if (user.Email != request.Email)
                    throw new Exception("Email or password is incorrect.");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                    throw new Exception("Email or password is incorrect.");

                CrtToken crtToken = new CrtToken(configuration);
                string token = crtToken.CreateToken(user);

                tServiceResponse.Data = _mapper.Map<string>(token);
            } catch (Exception ex)
            {
                tServiceResponse.Success = false;
                tServiceResponse.Message = ex.Message;
            }

            return tServiceResponse;
        }

        public async Task<ServiceResponse<Users>> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var serviceResponse = new ServiceResponse<Users>();

            try
            {
                if (request is null)
                    throw new Exception("There are some empty paramaters that need data.");

                user.Username = request.Username;
                user.Email = request.Email;
                user.PasswordHash = passwordHash;
                user.Admin = false;

                serviceResponse.Data = _mapper.Map<Users>(user);
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse; 
        }

        public async Task<ServiceResponse<string>> RegAdmin(UserDto request, IConfiguration configuration)
        {
            var tServiceResponse = new ServiceResponse<string>();
            string token = "";
            try
            {
                if (request.Username is null)
                    throw new Exception($"User with username '{request.Username}' not found.");

                if (user.Username != request.Username)
                    throw new Exception($"User with username '{request.Username}' not found.");

                if (user.Email != request.Email)
                    throw new Exception("Email or password is incorrect.");

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                    throw new Exception("Email or password is incorrect.");

                user.Admin = request.Admin;

                CrtToken crtToken = new CrtToken(configuration);

                token = crtToken.CreateToken(user);
            } catch (Exception ex)
            {
                tServiceResponse.Message = ex.Message;
                tServiceResponse.Success = false;
            }

            tServiceResponse.Data = _mapper.Map<string>(token);

            return tServiceResponse;
        }
    }

}
