using Microsoft.AspNetCore.Identity;

namespace Api1.Model
{
    public class ApplicationUser:IdentityUser
    {

        public string? Address { get; set; }

        public List<RefreshTokens> RefreshTokens { get; set; } = new List<RefreshTokens>();

        public List<FavouriteProduct> FavouriteProducts { get; set; } = new List<FavouriteProduct>();

    }
}
