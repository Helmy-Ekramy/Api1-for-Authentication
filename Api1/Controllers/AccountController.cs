using Api1.Context;
using Api1.DTO;
using Api1.Model;
using Api1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices accountServices;

        public AccountController(IAccountServices accountServices)
        {
            this.accountServices = accountServices;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(RegisterModel registerModel)
        {


            if (ModelState.IsValid)
            {

                var result = await accountServices.RegisterUserAsync(registerModel);

                if (result.IsSuccess)
                {
                    return Ok(result.Message);
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item);
                }

            }

            return BadRequest(ModelState);

        }


        [HttpGet("/ConfirmEmail")]

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest();



            var result = await accountServices.ConfirmEmailAsync(userId, token);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            ModelState.AddModelError("", result.Message ?? "");

            return BadRequest(ModelState);
        }


        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                var result = await accountServices.LoginUserAsync(loginUser);
                if (result.IsSuccess)
                {
                    return Ok(new { JWT_Token = result.JwtToken, RefreshToken = result.RefreshToken, Expiration = result.Expiration.ToString() });
                }

                ModelState.AddModelError("", result.Message ?? "");
            }

            return BadRequest(ModelState);

        }

        [HttpDelete("/Logout")]
        public async Task<IActionResult> Logout([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            var result = await accountServices.LogoutUserAsync(token);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("/SendEmailConfirmation")]

        public async Task<IActionResult> SendEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var result = await accountServices.ResendEmailAsync(email);

            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }


        [HttpPost("/ForgetPassword")]

        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();


            var result = await accountServices.ForgetPasswordAsync(email);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("/ResetPassword")]

        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromBody] ResetPasswordModel resetModel)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(resetModel.NewPassword) || string.IsNullOrEmpty(resetModel.Email))
                return BadRequest();

            var result = await accountServices.ResetPasswordAsync(token, resetModel);

            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item);
            }


            return BadRequest(ModelState);


        }


        [HttpPost("/RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            var result = await accountServices.RefreshTokenAsync(token);
            if (result.IsSuccess)
            {
                return Ok(new { JWT_Token = result.JwtToken, RefreshToken = result.RefreshToken, Expiration = result.Expiration });
            }
            ModelState.AddModelError("", result.Message ?? "");

            return BadRequest(ModelState);
        }

        [HttpPost("/GoogleSignIn")]

        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInDto idToken)
        {
            var result = await accountServices.GoogleSignInAsync(idToken);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return Unauthorized("Invalid Sign In");

        }

    }
}
