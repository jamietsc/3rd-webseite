using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Dapper;

using ThirdWebseite.DataAccessLayer.Repositories;
using ThirdWebseite.DataAccessLayer.SqlDataAccess;
using ThirdWebseite.Models.ViewModels;
using System.Reflection;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;




namespace ThirdWebseite.DataAccessLayer.Data
{
    public class UserData : IUserData<RegisterViewModel>
    {
        private readonly ISqlDataAccess _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserData(ISqlDataAccess db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> RegisterUser(RegisterViewModel user)
        {
            var hasher = new PasswordHasher<RegisterViewModel>();
            user.Password = hasher.HashPassword(user, user.Password);

            var parameters = new
            {
                user.UserName,
                user.Email,
                PasswordHash = user.Password,
                user.Role
            };

            try
            {
                var userID = await _db.SaveDataReturnID("sp_RegisterUser", parameters);
                if (userID != -1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering the user.", ex);
            }
            return false;
        }

        public async Task<bool> LoginUser(LoginViewModel User)
        {
            var hasher = new PasswordHasher<RegisterViewModel>();

            var parameters = new { Username = User.UserName };

            try
            {
                var result = await _db.LoadData<RegisterViewModel, dynamic>("sp_Login", parameters);
                if (result == null || !result.Any())
                {
                    return false;
                }
                var passwordVerificationResult = hasher.VerifyHashedPassword(result.FirstOrDefault(), result.FirstOrDefault().Password, User.Password);
                Console.WriteLine($"Password Verification Result: {passwordVerificationResult}"); // Debugging line
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.FirstOrDefault().UserName),
                        new Claim(ClaimTypes.Role, result.FirstOrDefault().Role)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    return true;
                }

                    
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); // Debugging line
                throw new Exception("Error occurred while logging in the user.", ex);
            }
            return false;
        }
    }
}
