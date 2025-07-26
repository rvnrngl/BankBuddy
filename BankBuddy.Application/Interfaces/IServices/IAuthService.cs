using BankBuddy.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> ChangePasswordAsync(Guid userId, ChangePasswordDTO dto);
        Task<UserInfoDTO> GetUserInfoAsync(Guid userId);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
        Task<string> LogoutAsync(string refreshToken);
        Task<AuthResponseDTO> RefreshTokenAsync(string token);
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<string> SendVerificationEmailAsync(Guid userId, string email);
        Task<string> VerifyEmailAsync(string token);
    }
}
