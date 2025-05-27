using JucieAndFlower.Data.Enities.User;
using JucieAndFlower.Data.Enities.User.JucieAndFlower.Data.Enities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IUserService
    {
        Task<string> RegisterAsync(UserRegisterDto dto);

        Task<object?> LoginAsync(string email, string password);

        Task<bool> LogoutAsync(string email);

        Task<object?> RefreshTokenAsync(string accessToken, string refreshToken);

        Task<(bool IsSuccess, string Message)> UpdateUserProfileAsync(string email, UserUpdateDto dto);

        Task<UserProfileDto?> GetUserProfileAsync(string email);

        Task<bool> VerifyEmailAsync(string email, string token);

    }
}
