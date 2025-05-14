using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using JucieAndFlower.Service.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IProductDetailService, ProductDetailService>();
            service.AddScoped<IWorkshopService, WorkshopService>();
            return service;
        }
    }
}
