using Api1.DTO;
using Api1.Migrations;
using Api1.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Services
{
    public interface IAccountServices
    {
        Task<GeneralResponse> RegisterUserAsync(RegisterModel registerModel);
        Task<GeneralResponse> LoginUserAsync(LoginModel loginModel);

        Task<GeneralResponse> LogoutUserAsync(string userId);

        Task<GeneralResponse> ConfirmEmailAsync(string userId, string token);

        Task<GeneralResponse> ResendEmailAsync(string email);

        Task<GeneralResponse> ForgetPasswordAsync(string email);

        Task<GeneralResponse> ResetPasswordAsync([FromQuery]string token ,[FromBody] ResetPasswordModel resetModel);

        RefreshTokens GenerateRefreshToken();
        Task<GeneralResponse> RefreshTokenAsync(string token);

        Task<GeneralResponse> GoogleSignInAsync(GoogleSignInDto idToken);




    }
}
