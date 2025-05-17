using JucieAndFlower.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IProductDetailRepository, ProductDetailRepository>();
            service.AddScoped<IWorkshopRepository, WorkshopRepository>();
            service.AddScoped<IWorkshopTicketRepository, WorkshopTicketRepository>();
            service.AddScoped<IOrderRepository, OrderRepository>();
            return service;
        }
    }
}
