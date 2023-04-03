using GenshinTheoryCrafting.Models;
using GenshinTheoryCrafting.Models.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace GenshinTheoryCrafting.Services.User
{
    public interface IUserService
    {
        string GetMyName();
        Users Register(UserDto request);
        string Login(UserDto request, IConfiguration configuration);
        string RegAdmin(UserDto request, IConfiguration configuration);
    }
}
