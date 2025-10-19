using Api1.DTO;
using Api1.Model;
using Azure.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using MimeKit;
using System.Text;

namespace Api1.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IEmailBuilderService emailBuilder;

        public EmailServices(UserManager<ApplicationUser> userManager, IConfiguration config, IEmailBuilderService emailBuilder)
        {
            this.userManager = userManager;
            this.config = config;
            this.emailBuilder = emailBuilder;
        }


        public async Task<GeneralResponse> SendConfirmationEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return new GeneralResponse { IsSuccess = false, Message = "User not found." };

            if (user.EmailConfirmed)
                return new GeneralResponse { IsSuccess = false, Message = "Email already confirmed." };

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = System.Net.WebUtility.UrlEncode(token);


            var body = await emailBuilder.ConfirmEmailTemplateAsync(user, token);

            return await SendEmailAsync(email, "Confirm your email", body);
        }



        public async Task<GeneralResponse> SendEmailAsync(string toEmail, string subject, string htmlBody)
        {

            try
            {


                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Student Management System", config["EmailConfiguration:From"]));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = htmlBody };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(config["EmailConfiguration:From"], config["EmailConfiguration:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);


                return new GeneralResponse
                {
                    IsSuccess = true,
                    Message = "Email sent successfully. Please check your inbox to confirm your account."
                };

            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to send email. Error: {ex.Message}"
                };
            }

        }



        public async Task<GeneralResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                if (user.EmailConfirmed)
                    return new GeneralResponse
                    {
                        IsSuccess = true,
                        Message= "Email is already confirmed."

                    };


                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {

                    return new GeneralResponse
                    {
                        IsSuccess = true,
                        Message = "Email Confirmed Successfully."
                    };

                }
            }

            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Email Confirmation Failed.",
            };

        }

        public async Task<GeneralResponse> ForgotPasswordEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return new GeneralResponse { IsSuccess = false, Message = "User not found." };

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            token = System.Net.WebUtility.UrlEncode(token);


            var body = await emailBuilder.ResetPasswordTemplateasync(user, token);

            return await SendEmailAsync(email, "Reset Password Confirmation Email", body);
        }
    }
}
