using GenshinTheoryCrafting.Controllers.Auth;
using GenshinTheoryCrafting.Models;
using GenshinTheoryCrafting.Models.Dto.User;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GenshinTheoryCrafting.Services.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public static Users user = new Users();

        public UserService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<string>> Login(LoginDto request, IConfiguration configuration)
        {
            var tServiceResponse = new ServiceResponse<string>();

            try
            {
                var dbUser = await _context.Users.Where(x => x.Username == request.Username).ToListAsync();

                if (BCrypt.Net.BCrypt.Verify(request.Password, dbUser[0].PasswordHash))
                {
                    CrtToken crtToken = new CrtToken(configuration);
                    string token = crtToken.CreateToken(dbUser[0]);

                    tServiceResponse.Data = _mapper.Map<string>(token);
                }
                else
                    throw new Exception("Email or password is incorrect.");
            }
            catch (Exception ex)
            {
                tServiceResponse.Success = false;
                tServiceResponse.Message = ex.Message;
            }

            return tServiceResponse;
        }

        public async Task<ServiceResponse<List<Users>>> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var serviceResponse = new ServiceResponse<List<Users>>();

            try
            {
                if (request is null)
                    throw new Exception("There are some empty paramaters that need data.");

                List<Users> req = new List<Users>
                {
                    new Users
                    {
                        Username = request.Username,
                        Email = request.Email,
                        PasswordHash = passwordHash,
                        Admin = false
                    }
                };

                _context.Users.Add(req[0]);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<List<Users>>(req);

                var dbUsers = await _context.Users.ToListAsync();
                foreach (var dbUser in dbUsers)
                {
                    Console.WriteLine(dbUser.Username);
                }
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse; 
        }

        public async Task<ServiceResponse<string>> RegAdmin(LoginDto request, IConfiguration configuration)
        {
            var tServiceResponse = new ServiceResponse<string>();
            try
            {
                var dbUser = await _context.Users.Where(x => x.Username == request.Username).ToListAsync();

                if (BCrypt.Net.BCrypt.Verify(request.Password, dbUser[0].PasswordHash))
                {
                    dbUser[0].Admin = true;
                    Console.WriteLine(dbUser[0].Admin);
                    CrtToken crtToken = new CrtToken(configuration);
                    string token = crtToken.CreateToken(dbUser[0]);

                    tServiceResponse.Data = _mapper.Map<string>(token);
                }
                else
                    throw new Exception("Email or password is incorrect.");
            }
            catch (Exception ex)
            {
                tServiceResponse.Success = false;
                tServiceResponse.Message = ex.Message;
            }

            return tServiceResponse;
        }

        //public async Task<ServiceResponse<string>> Delete(UserDto request)
        //{
        //    var serviceResponse = new ServiceResponse<string>();
        //    try
        //    {
        //        if (request.Username is null)
        //            throw new Exception($"User with username '{request.Username}' not found.");

        //        if (user.Username != request.Username)
        //            throw new Exception($"User with username '{request.Username}' not found.");

        //        if (user.Email != request.Email)
        //            throw new Exception("Email or password is incorrect.");

        //        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        //            throw new Exception("Email or password is incorrect.");

        //        serviceResponse.Message = "Deleted User";
        //        serviceResponse.Success = true;
        //    } catch (Exception ex)
        //    {
        //        serviceResponse.Message = ex.Message;
        //        serviceResponse.Success = false;
        //    }

        //    return serviceResponse;
        //}
    }

}
