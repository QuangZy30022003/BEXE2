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

            body.AppendLine("<div style='font-family:Arial,sans-serif; background-color:#f2f2f2; padding:30px;'>");

            body.AppendLine("<div style='max-width:600px; margin:auto; background-color:#fff; border-radius:8px; padding:30px; box-shadow:0 0 10px rgba(0,0,0,0.1);'>");

            body.AppendLine("<h2 style='color:#D63384; text-align:center;'>Cảm ơn bạn đã đặt hàng tại <span style='color:#28A745;'>Jucie & Flower</span>!</h2>");

            body.AppendLine("<div style='margin-top:20px; font-size:15px;'>");
            body.AppendLine($"<p><strong>Mã đơn hàng:</strong> #{order.OrderId}</p>");
            body.AppendLine($"<p><strong>Ngày đặt:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</p>");
            body.AppendLine($"<p><strong>Tổng tiền:</strong> {string.Format(vietnamCulture, "{0:C0}", order.TotalAmount)}</p>");
            body.AppendLine($"<p><strong>Giảm giá:</strong> {string.Format(vietnamCulture, "{0:C0}", order.DiscountAmount)}</p>");
            body.AppendLine($"<p><strong>Thành Tiền:</strong> <span style='color:#28A745; font-weight:bold;'>{string.Format(vietnamCulture, "{0:C0}", order.FinalAmount)}</span></p>");
            body.AppendLine("</div>");

            body.AppendLine("<h4 style='margin-top:25px; border-bottom:1px solid #eee; padding-bottom:8px;'>Chi tiết sản phẩm:</h4>");
            body.AppendLine("<ul style='padding-left:20px; font-size:14px;'>");
            foreach (var item in order.OrderDetails)
            {
                body.AppendLine($"<li style='margin-bottom:6px;'>{item.Quantity} x {item.Product?.Name} - {item.UnitPrice?.ToString("C0", vietnamCulture)}</li>");
            }
            body.AppendLine("</ul>");

            body.AppendLine("<div style='margin-top:25px; font-size:14px;'>");
            body.AppendLine("<p>Chúng tôi sẽ sớm giao hàng đến bạn trong thời gian sớm nhất.</p>");
            body.AppendLine("<p style='color:#666;'>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email hoặc hotline.</p>");
            body.AppendLine("</div>");

            body.AppendLine("<div style='text-align:center; margin-top:40px; font-size:12px; color:#999;'>");
            body.AppendLine("<p>Jucie & Flower - Mang sắc màu đến cuộc sống của bạn.</p>");
            body.AppendLine("</div>");

            body.AppendLine("</div>"); // inner box
            body.AppendLine("</div>"); // outer wrapper


            var mailMessage = new MailMessage(fromEmail, toEmail, "Xác nhận đơn hàng từ Jucie & Flower", body.ToString())
            {
                IsBodyHtml = true
            };


            await smtpClient.SendMailAsync(mailMessage);
        }
    }
    }
