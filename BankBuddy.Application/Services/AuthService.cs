using AutoMapper;
using BankBuddy.Application.DTOs.Auth;
using BankBuddy.Application.Interfaces.IRepositories;
using BankBuddy.Application.Interfaces.IServices;
using BankBuddy.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BankBuddy.Application.Commons;
using Microsoft.AspNetCore.Http;

namespace BankBuddy.Application.Services
{
    public class AuthService(
        IGenericRepository<User> _userRepository,
        IGenericRepository<Role> _roleRepository,
        IGenericRepository<RefreshToken> _refreshTokenRepository,
        IGenericRepository<VerificationToken> _verificationRepository,
        IEmailService _emailService,
        IConfiguration _config,
        IMapper _mapper
    ) : IAuthService
    {
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            User? existingUser = await _userRepository.FindAsync(u => u.Email == dto.Email);

            if (existingUser is not null) throw new AppException("Email already in use.", StatusCodes.Status409Conflict);

            Role? userRole = await _roleRepository.FindAsync(r => r.Name == "Customer") ?? throw new AppException("User role not found.", StatusCodes.Status404NotFound);

            var user = _mapper.Map<User>(dto);

            user.RoleId = userRole.RoleId;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            AuthData authData = new()
            {
                User = user,
                Role = userRole,
                AccessToken = GenerateToken(user, userRole),
                RefreshToken = await CreateRefreshToken(user)
            };

            return _mapper.Map<AuthResponseDTO>(authData);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            User? user = await _userRepository.FindAsync(
                u => u.Email == dto.Email,
                include: q => q.Include(u => u.Role)) ?? throw new AppException("Email or password incorrect.", StatusCodes.Status401Unauthorized);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid) throw new AppException("Email or password incorrect.", StatusCodes.Status401Unauthorized);

            AuthData authData= new()
            {
                User = user,
                Role = user.Role,
                AccessToken = GenerateToken(user, user.Role),
                RefreshToken = await CreateRefreshToken(user)
            };

            return _mapper.Map<AuthResponseDTO>(authData);
        }

        public async Task<string> LogoutAsync(string refreshToken)
        {
            RefreshToken token = await _refreshTokenRepository.FindAsync(r => r.Token == refreshToken) ?? throw new AppException("Refresh token not found.", StatusCodes.Status404NotFound);

            token.IsRevoked = true;
            
            _refreshTokenRepository.Update(token);
            await _userRepository.SaveChangesAsync();

            return "Logged out successfully.";
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            RefreshToken? tokenInfo = await _refreshTokenRepository.FindAsync(
                r => r.Token == token && !r.IsRevoked && r.ExpiresAt > DateTime.UtcNow,
                include: q => q.Include(r => r.User).ThenInclude(u => u.Role)) ?? throw new AppException("Invalid or expired refresh token.", StatusCodes.Status401Unauthorized);

            tokenInfo.IsRevoked = true;

            _refreshTokenRepository.Update(tokenInfo);
            await _refreshTokenRepository.SaveChangesAsync();

            User user = tokenInfo.User;
            Role role = tokenInfo.User.Role;
            
            AuthData authData = new()
            {
                User = user,
                Role = role,
                AccessToken = GenerateToken(user, role),
                RefreshToken = await CreateRefreshToken(user)
            };

            return _mapper.Map<AuthResponseDTO>(authData);
        }

        public async Task<string> ChangePasswordAsync(Guid userId, ChangePasswordDTO dto)
        {
            User? user = await _userRepository.GetByIdAsync(userId) ?? throw new AppException("User not found.", StatusCodes.Status404NotFound);

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash)) throw new AppException("Current password is incorrect.", StatusCodes.Status400BadRequest);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return "Password changed successfully.";
        }

        public async Task<UserInfoDTO> GetUserInfoAsync(Guid userId)
        {
            User user = await _userRepository.GetByIdAsync(userId) ?? throw new AppException("User not found.", StatusCodes.Status404NotFound);

            return _mapper.Map<UserInfoDTO>(user);
        }

        public async Task<string> SendVerificationEmailAsync(Guid userId, string email)
        {
            string token = Guid.NewGuid().ToString();

            VerificationToken verificationToken = new()
            {
                UserId = userId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
            };

            await _verificationRepository.AddAsync(verificationToken);
            await _userRepository.SaveChangesAsync();

            string subject = "Verify your email address";
            string body = $"Your verification token is: {token}";

            await _emailService.SendEmailAsync(email, subject, body);

            return "Verification email sent.";
        }

        public async Task<string> VerifyEmailAsync(string token)
        {
            VerificationToken verificationToken = await _verificationRepository.FindAsync(
                v => v.Token == token,
                include: q => q.Include(v => v.User)) ?? throw new AppException("Invalid or expired verification token.", StatusCodes.Status400BadRequest);

            User user = verificationToken.User;

            user.IsVerified = true;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            _verificationRepository.Remove(verificationToken);
            await _verificationRepository.SaveChangesAsync();

            return "Email verified successfully.";
        }

        private string GenerateToken(User user, Role role)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, role.Name),
                new Claim(ClaimTypes.Email, user.Email)
            ];

            int expiresInMinutes = int.Parse(_config["Jwt:ExpiresInMinutes"]!);

            JwtSecurityToken token = new(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> CreateRefreshToken(User user)
        {
            RefreshToken refreshToken = new()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.UserId
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return refreshToken.Token;
        }
    }
}
