using JucieAndFlower.Data.Models;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _imageRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductImageService(IProductImageRepository imageRepo, IProductRepository productRepo, ICloudinaryService cloudinaryService)
        {
            _imageRepo = imageRepo;
            _productRepo = productRepo;
            _cloudinaryService = cloudinaryService;
        }

        //public async Task<string> UploadImageAsync(int productId, IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        throw new ArgumentException("File is empty");

        //    var product = await _productRepo.GetByIdAsync(productId);
        //    if (product == null)
        //        throw new Exception("Product not found");

        //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        //    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

        //    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //    var filePath = Path.Combine(uploadsFolder, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    var imageUrl = $"/images/{fileName}";

        //    var image = new ProductImage
        //    {
        //        ProductId = productId,
        //        ImageUrl = imageUrl
        //    };

        //    await _imageRepo.AddAsync(image);
        //    await _imageRepo.SaveChangesAsync();

        //    return imageUrl;
        //}

        public async Task<string> UploadImageAsync(int productId, IFormFile file)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            var imageUrl = await _cloudinaryService.UploadImageAsync(file);

            var image = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl
            };

            await _imageRepo.AddAsync(image);
            await _imageRepo.SaveChangesAsync();

            return imageUrl;
        }
    }

}
