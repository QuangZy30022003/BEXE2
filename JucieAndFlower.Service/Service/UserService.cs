﻿using JucieAndFlower.Data.Enities.User;
using JucieAndFlower.Data.Enities.User.JucieAndFlower.Data.Enities.User;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IEmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<string> RegisterAsync(UserRegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return "Email already exists.";
            var emailToken = Guid.NewGuid().ToString();
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Address = dto.Address,
                Gender = dto.Gender,
                BirthDate = dto.BirthDate,
                RoleId = 1, // Default: 1 = customer
                CreatedAt = DateTime.Now,
                 IsEmailConfirmed = false,
                EmailConfirmationToken = emailToken
            };
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveAsync();

            var confirmLink = $"{_configuration["AppSettings:ApiBaseUrl"]}/api/auth/verify-email?email={user.Email}&token={emailToken}";
            await _emailService.SendEmailAsync(user.Email, "Xác thực Email", $"<p>Nhấp vào <a href='{confirmLink}'>đây</a> để xác thực email.</p>");

            return "Vui lòng kiểm tra email để xác nhận tài khoản.";
        }

        public async Task<object?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            if (!user.IsEmailConfirmed)
            {
                return new { error = "Vui lòng xác nhận email trước khi đăng nhập." };
            }

            // Tạo JWT Token
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())

        };

            var jwtKey = _configuration["JwtSettings:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentException("JWT key is not configured properly.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            // Tạo Refresh Token
            var refreshToken = GenerateRefreshToken();

            // Lưu Refresh Token vào cơ sở dữ liệu
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.SaveAsync();

            return new { Token = jwt, RefreshToken = refreshToken };  // Trả về cả JWT và Refresh Token
        }
        public async Task<bool> LogoutAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.SaveAsync();
            return true;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<object?> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
                return null;

            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return null;

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            // Tạo mới access token
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
    };

            var jwtKey = _configuration["JwtSettings:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.SaveAsync();

            return new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // ❗ Cho phép token hết hạn

                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateUserProfileAsync(string email, UserUpdateDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return (false, "User not found.");

            // Cập nhật thông tin cá nhân
            user.FullName = dto.FullName;
            user.Phone = dto.Phone;
            user.Address = dto.Address;
            user.Gender = dto.Gender;
            user.BirthDate = dto.BirthDate;

            // Nếu có ý định đổi mật khẩu
            if (!string.IsNullOrWhiteSpace(dto.OldPassword) ||
                !string.IsNullOrWhiteSpace(dto.NewPassword) ||
                !string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                if (string.IsNullOrWhiteSpace(dto.OldPassword) ||
                    string.IsNullOrWhiteSpace(dto.NewPassword) ||
                    string.IsNullOrWhiteSpace(dto.ConfirmPassword))
                {
                    return (false, "Please fill all password fields.");
                }

                if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                    return (false, "Old password is incorrect.");

                if (dto.NewPassword != dto.ConfirmPassword)
                    return (false, "New password and confirmation do not match.");

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            }

            await _userRepository.SaveAsync();
            return (true, "Update successful.");
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            return new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                RoleId = user.RoleId
            };
        }
        public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                // Debug log user not found
                return false;
            }
            if (string.IsNullOrEmpty(user.EmailConfirmationToken))
            {
                // Token đã được xác nhận hoặc chưa được cấp
                return false;
            }
            if (user.EmailConfirmationToken != token)
            {
                // Debug log token không khớp
                return false;
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            await _userRepository.SaveAsync();
            return true;
        }

    }
}
