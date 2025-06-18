using JucieAndFlower.Data.Enities.CusFlower;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface ICustomFlowerItemService
    {
        Task<CustomFlowerItem> CreateCustomItemAsync(int userId, CustomFlowerItemCreateDTO dto);
        Task<List<CustomFlowerItem>> GetUserCustomItemsAsync(int userId);
    }
}
