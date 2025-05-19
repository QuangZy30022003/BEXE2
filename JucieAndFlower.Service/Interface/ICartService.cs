using JucieAndFlower.Data.Enities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface ICartService
    {
        Task<List<CartItemResponseDTO>> GetUserCartAsync(int userId);
        Task AddOrUpdateCartItemAsync(int userId, CartItemCreateDTO dto);
        Task RemoveCartItemAsync(int userId, int productId);
    }
}
