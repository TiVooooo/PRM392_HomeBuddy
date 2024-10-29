using HomeBuddy.Common;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface ILoginService
    {
        Task<IBusinessResult> Login(LoginModel login);
    }
    public class LoginService : ILoginService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public LoginService(UnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<IBusinessResult> Login(LoginModel login)
        {
            var userList = await _unitOfWork.UserRepository.GetAllAsync();

            if(login.Email == null|| login.Password == null)
            {
                return new BusinessResult("Email and Password must be filled");
            } 
            var checkedUser = userList.Where(x => x.Email == login.Email && x.Password ==  login.Password)
                .FirstOrDefault();
            if(checkedUser == null)
            {
                return new BusinessResult(Const.ERROR_EXEPTION, "Not found user");
            } 
            if(!string.IsNullOrEmpty(login.DeviceToken))
            {
                checkedUser.DeviceToken = login.DeviceToken;
                await _unitOfWork.UserRepository.UpdateAsync(checkedUser);
            }
            var role = checkedUser.Role;

            var authClaims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, checkedUser.Name),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, checkedUser.Id.ToString())
            };

            var token = GenerateToken(authClaims);

            return new BusinessResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = role,
                UserId = checkedUser.Id,
                expiration = token.ValidTo
            });
        }

        public JwtSecurityToken GenerateToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddDays(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}
