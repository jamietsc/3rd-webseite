using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Dapper;

using ThirdWebseite.DataAccessLayer.Repositories;
using ThirdWebseite.DataAccessLayer.SqlDataAccess;
using ThirdWebseite.Models.ViewModels;
using System.Reflection;
using System.Data;




namespace ThirdWebseite.DataAccessLayer.Data
{
    public class UserData : IUserData<RegisterViewModel>
    {
        private readonly ISqlDataAccess _db;
        public UserData(ISqlDataAccess db)
        {
            _db = db;
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
                Console.WriteLine($"User ID: {userID}");
                if (userID != -1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{parameters}");
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Error occurred while registering the user.", ex);
            }
            return false;
        }

        public async Task<bool> LoginUser(RegisterViewModel user)
        {
            await Task.Delay(1000); // Simulate some delay
            return true;
        }
    }
}
