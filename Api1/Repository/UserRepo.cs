using Api1.DTO;
using Api1.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Api1.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepo(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> AddToFavAsync(string userId, int productId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false; // User not found
            }

            user.FavouriteProducts.Add(new FavouriteProduct { UserId = userId , ProductId=productId , AddedOn=DateTime.Now });
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<List<FavProductsResponse>> GetUserFavProductsAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            var result = await _userManager.Users
                .Where(u => u.Id == id)
                .Select(u => u.FavouriteProducts.Select(fp => new FavProductsResponse {
                    ProductId = fp.ProductId,
                    Category_Name = fp.Product.Category.Name,
                    Name = fp.Product.Name,
                    Price = fp.Product.Price,
                    AddedOn = fp.AddedOn,
                }).ToList()
                ).FirstOrDefaultAsync();

            return result;

        }

        public List<string> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public string GetUserById(string id)
        {
            throw new NotImplementedException();
        }


    }
}
