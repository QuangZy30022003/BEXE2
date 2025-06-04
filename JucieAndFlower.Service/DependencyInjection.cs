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
            service.AddScoped<IWorkshopTicketService, WorkshopTicketService>();
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<IVNPayService, VNPayService>();
            service.AddScoped<ICartService, CartService>();
            service.AddScoped<IPaymentService, PaymentService>();
            service.AddScoped<IPromotionService, PromotionService>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IFeedbackService, FeedbackService>();
            service.AddScoped<IRevenueService, RevenueService>();
            return service;
        }
    }
}
