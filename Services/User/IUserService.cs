using GenshinTheoryCrafting.Models;
using GenshinTheoryCrafting.Models.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace GenshinTheoryCrafting.Services.User
{
    public interface IUserService
    {
        Task<Users> Register(UserDto request);
        Task<string> Login(UserDto request, IConfiguration configuration);
        Task<string> RegAdmin(UserDto request, IConfiguration configuration);
    }
}
