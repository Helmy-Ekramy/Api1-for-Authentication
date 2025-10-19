using Api1.Context;
using Api1.DTO;
using Api1.Model;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Prng;
using System.Net;
using System.Security.Cryptography;
namespace Api1.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IjwtServices _jwt;
        private readonly IEmailServices _emailServices;
        private readonly Api1Context db;
        private object jwt;

        public AccountServices(UserManager<ApplicationUser> userManager, IjwtServices jwt, IEmailServices emailServices, Api1Context db)
        {
            _userManager = userManager;
            _jwt = jwt;
            _emailServices = emailServices;
            this.db = db;
        }



        public async Task<GeneralResponse> RegisterUserAsync(RegisterModel registerUser)
        {
            var User = new ApplicationUser();
            User.Email = registerUser.Email;
            User.UserName = registerUser.Name;
            User.Address = registerUser.Address;



            var result = await _userManager.CreateAsync(User, registerUser.Password);

            if (result.Succeeded)
            {

                return await _emailServices.SendConfirmationEmailAsync(User.Email);

            }
            return new GeneralResponse
            {
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };

        }

        public async Task<GeneralResponse> LoginUserAsync(LoginModel loginUser)
        {

            var User = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.UserName == loginUser.UserName);

            if (User != null)
            {
                bool found = await _userManager.CheckPasswordAsync(User, loginUser.Password);

                if (found)
                {

                    if (!User.EmailConfirmed)
                    {
                        return new GeneralResponse
                        {
                            IsSuccess = false,
                            Message = "Email not confirmed yet"
                        };
                    }


                    var oldRefreshTokens = User.RefreshTokens.Where(r => r.IsActive).ToList();

                    foreach (var item in oldRefreshTokens)
                    {
                        item.Revoked = DateTime.Now;
                    }





                    // generate jwt token 
                    var jwtToken = await _jwt.GenerateTokenAsync(User);
                    var refreshToken = GenerateRefreshToken();
                    User.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(User);

                    return new GeneralResponse
                    {
                        IsSuccess = true,
                        JwtToken = jwtToken.JwtToken,
                        RefreshToken = refreshToken.Token,
                        Expiration = refreshToken.Expires,
                        Message = "Login Success"
                    };

                }
            }
            return new GeneralResponse
            {
                IsSuccess = false,
                Message = "Invalid UserName or Password",
            };

        }

        public async Task<GeneralResponse> ResendEmailAsync(string email)
        {
            return await _emailServices.SendConfirmationEmailAsync(email);
        }

        public async Task<GeneralResponse> ConfirmEmailAsync(string userId, string token)
        {

            return await _emailServices.ConfirmEmailAsync(userId, token);
        }




        public async Task<GeneralResponse> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }
            // here send email for reset password

            return await _emailServices.ForgotPasswordEmailAsync(email);
        }




        public async Task<GeneralResponse> ResetPasswordAsync([FromQuery] string token, [FromBody] ResetPasswordModel resetModel)
        {
            var user = await _userManager.FindByEmailAsync(resetModel.Email);

            if (user == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, token, resetModel.NewPassword);
            if (result.Succeeded)
            {
                return new GeneralResponse
                {
                    IsSuccess = true,
                    Message = "Password reset successful"
                };
            }

            return new GeneralResponse
            {
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public RefreshTokens GenerateRefreshToken()
        {

            var newRefreshToken = new RefreshTokens();
            newRefreshToken.Token = Guid.NewGuid().ToString();
            newRefreshToken.Created = DateTime.Now;
            newRefreshToken.Expires = DateTime.Now.AddDays(7);

            return newRefreshToken;

        }


        public async Task<GeneralResponse> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == token));

            if (user == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "Invalid token"
                };
            }

            var refreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == token);
            if (!refreshToken.IsActive)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "Inactive token"
                };
            }


            // Revoke current refresh token

            var oldRefreshTokens = user.RefreshTokens.Where(r => r.IsActive).ToList();
            foreach (var item in oldRefreshTokens)
            {
                item.Revoked = DateTime.Now;
            }
            //refreshToken.Revoked = DateTime.Now;

            // generate new refresh token
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            // generate new jwt token
            var newAccessToken = await _jwt.GenerateTokenAsync(user);

            return new GeneralResponse
            {
                IsSuccess = true,
                JwtToken = newAccessToken.JwtToken,
                RefreshToken = newRefreshToken.Token,
                Expiration = newRefreshToken.Expires,
                Message = "Token refreshed"
            };

        }


        public async Task<GeneralResponse> GoogleSignInAsync(GoogleSignInDto idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken.token, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { "255560464621-bl13orub7g4o8mrop5mce38qh02trerj.apps.googleusercontent.com" }
            });

            var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = payload.Email.Split('@')[0],
                    Email = payload.Email,
                    EmailConfirmed = payload.EmailVerified
                };


                await _userManager.CreateAsync(user);



            }


            var oldRefreshTokens = user.RefreshTokens.Where(r => r.IsActive).ToList();
            foreach (var item in oldRefreshTokens)
            {
                item.Revoked = DateTime.Now;
            }


            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            var token = await _jwt.GenerateTokenAsync(user);


            return new GeneralResponse
            {
                IsSuccess = true,
                JwtToken = token.JwtToken,
                RefreshToken = refreshToken.Token,
                Expiration = refreshToken.Expires,
                Message = "Google Sign-In successful"
            }
            ;
        }

        public async Task<GeneralResponse> LogoutUserAsync(string userId)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }


            user.RefreshTokens.Clear();
            await _userManager.UpdateAsync(user);

            return new GeneralResponse
            {
                IsSuccess = true,
                Message = "Logout successful"
            };


        }
    }
}
