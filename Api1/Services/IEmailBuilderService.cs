using Api1.Model;

namespace Api1.Services
{
    public interface IEmailBuilderService
    {

        Task<string> ConfirmEmailTemplateAsync(ApplicationUser user,string token);
        Task<string> ResetPasswordTemplateasync(ApplicationUser user, string token);

    }
}
