using JucieAndFlower.Service.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JucieAndFlower.Data.Models;
using System.Globalization;

namespace JucieAndFlower.Service.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            using var smtpClient = new SmtpClient(_config["Email:SmtpServer"])
            {
                Port = int.Parse(_config["Email:Port"]),
                Credentials = new NetworkCredential(_config["Email:Username"], _config["Email:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Email:From"]),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendOrderInvoiceEmailAsync(string toEmail, Order order)
        {
            var fromEmail = _config["Email:Username"];
            var password = _config["Email:Password"];

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var body = new StringBuilder();
            var vietnamCulture = new CultureInfo("vi-VN");

            body.AppendLine("<div style='font-family:Arial,sans-serif; padding:20px; background-color:#f9f9f9; color:#333;'>");

            body.AppendLine("<h2 style='color:#D63384;'>Cảm ơn bạn đã đặt hàng tại <span style='color:#28A745;'>Jucie & Flower</span>!</h2>");

            body.AppendLine("<div style='margin-top:10px;'>");
            body.AppendLine($"<p><strong>Mã đơn hàng:</strong> #{order.OrderId}</p>");
            body.AppendLine($"<p><strong>Ngày đặt:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</p>");
            body.AppendLine($"<p><strong>Tổng tiền:</strong> {string.Format(vietnamCulture, "{0:C0}", order.TotalAmount)}</p>");
            body.AppendLine($"<p><strong>Tổng tiền:</strong> {string.Format(vietnamCulture, "{0:C0}", order.DiscountAmount)}</p>");
            body.AppendLine($"<p><strong>Tổng tiền:</strong> {string.Format(vietnamCulture, "{0:C0}", order.FinalAmount)}</p>");
            body.AppendLine("</div>");

            body.AppendLine("<h4 style='margin-top:20px;'>Chi tiết sản phẩm:</h4>");
            body.AppendLine("<ul style='padding-left:20px;'>");
            foreach (var item in order.OrderDetails)
            {
                body.AppendLine($"<li style='margin-bottom:5px;'>{item.Quantity} x {item.Product?.Name} - {item.UnitPrice?.ToString("C0", vietnamCulture)}</li>");
            }
            body.AppendLine("</ul>");

            body.AppendLine("<p style='margin-top:20px;'>Chúng tôi sẽ sớm liên hệ để xác nhận đơn hàng và giao hàng đến bạn trong thời gian sớm nhất.</p>");
            body.AppendLine("<p style='color:#888;'>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email hoặc hotline.</p>");

            body.AppendLine("<p style='margin-top:30px; font-size:12px; color:#aaa;'>Jucie & Flower - Mang sắc màu đến cuộc sống của bạn.</p>");
            body.AppendLine("</div>");

            var mailMessage = new MailMessage(fromEmail, toEmail, "Xác nhận đơn hàng từ Jucie & Flower", body.ToString())
            {
                IsBodyHtml = true
            };


            await smtpClient.SendMailAsync(mailMessage);
        }
    }
    }
