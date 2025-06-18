using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IProductImageService
    {
        Task<string> UploadImageAsync(int productId, IFormFile file);
    }

}
