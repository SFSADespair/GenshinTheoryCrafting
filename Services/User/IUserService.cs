using GenshinTheoryCrafting.Models;
using GenshinTheoryCrafting.Models.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace GenshinTheoryCrafting.Services.User
{
    public interface IUserService
    {
        Task<ServiceResponse<List<Users>>> Register(UserDto request);
        Task<ServiceResponse<string>> Login(LoginDto request, IConfiguration configuration);
        Task<ServiceResponse<string>> RegAdmin(UserDto request, IConfiguration configuration);
        //Task<ServiceResponse<string>> Delete(UserDto request);
    }
}
