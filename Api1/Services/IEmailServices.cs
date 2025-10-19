using Api1.DTO;
using Api1.Model;

namespace Api1.Services
{
    public interface IEmailServices
    {
        Task<GeneralResponse> SendConfirmationEmailAsync(string email);
        Task<GeneralResponse> SendEmailAsync(string toEmail, string subject, string body);
        Task<GeneralResponse> ConfirmEmailAsync(string userId,string token);

        Task<GeneralResponse> ForgotPasswordEmailAsync(string email);



    }
}
