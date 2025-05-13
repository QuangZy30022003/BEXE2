using JucieAndFlower.Data.Enities;
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
    }
}
