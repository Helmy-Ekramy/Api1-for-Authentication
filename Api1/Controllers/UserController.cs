using Api1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("GetUserFavProducts")]
        public async Task<IActionResult> GetUserFavProducts(string userId)
        {
             var result = await _userRepo.GetUserFavProductsAsync(userId);

            if (result == null)
            {
                return NotFound("User not found");
            }
            return Ok(result);
        }

        [HttpPost("AddToFav")]
        public async Task<IActionResult> AddToFav(string userId, int productId)
        {
            var success = await _userRepo.AddToFavAsync(userId, productId);
            if (!success)
            {
                return NotFound("User not found or could not add to favorites.");
            }
            return Ok("Product added to favorites.");
        }
    }
    }
