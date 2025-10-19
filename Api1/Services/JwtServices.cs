using Api1.DTO;
using Api1.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api1.Services
{
    public class JWTServices : IjwtServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configuration;
        public JWTServices(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this.configuration = configuration;
        }
        public async Task<GeneralResponse> GenerateTokenAsync(ApplicationUser User)
        {
            var userRoles = await _userManager.GetRolesAsync(User);

            var Claims = new List<Claim>();
            Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            Claims.Add(new Claim(ClaimTypes.Name, User.UserName));
            Claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));

            foreach (var item in userRoles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, item));
            }



            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])); // >=16 bit

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: configuration["JWT:IssuerIP"],
                    audience:configuration["JWT:AudIP"],
                    claims: Claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signingCredentials
                );


            return new GeneralResponse
            {
                IsSuccess = true,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Message = DateTime.Now.AddMinutes(5).ToString()
            };
                
        }
    }
}
