using Api1.DTO;
using Api1.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Repository
{    public interface IUserRepo
    {
        public List<string> GetAllUsers();
        public string GetUserById(string id);
        public Task<List<FavProductsResponse>> GetUserFavProductsAsync(string id);
        public Task<bool> AddToFavAsync(string userId, int productId);

    }
}
