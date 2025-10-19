using Api1.Model;

namespace Api1.Services
{
    public class EmailBuilderService : IEmailBuilderService
    {
        private readonly IConfiguration _config;

        public EmailBuilderService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<string> ConfirmEmailTemplateAsync(ApplicationUser user, string token)
        {
            var frontendUrl = _config["ClientURL"];
            var confirmationLink = $"{frontendUrl}/ConfirmEmail?userId={user.Id}&token={token}";


            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplates", "ConfirmEmailTemplate.html");
            var body = await File.ReadAllTextAsync(templatePath);

            body = body.Replace("{{UserName}}", user.UserName)
                       .Replace("{{ConfirmationLink}}", confirmationLink).ToString();

            return body;

        }

        public async Task<string> ResetPasswordTemplateasync(ApplicationUser user, string token)
        {
            var frontendUrl = _config["ClientURL"];

            var ResetPasswordLink = $"{frontendUrl}/ResetPassword?token={token}";

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplates", "ResetPasswordEmailTemplate.html");

            var body = await  File.ReadAllTextAsync(templatePath);
            body = body.Replace("{{UserName}}", user.UserName)
                       .Replace("{{ResetPasswordLink}}", ResetPasswordLink).ToString();

            return body;

        }
    }
}
