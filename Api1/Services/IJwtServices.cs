using Api1.DTO;
using Api1.Model;

namespace Api1.Services
{
    public interface IjwtServices
    {
         Task<GeneralResponse> GenerateTokenAsync(ApplicationUser user);
    }
}
