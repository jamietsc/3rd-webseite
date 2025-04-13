using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThirdWebseite.DataAccessLayer.Repositories
{
    //Repository for the Login and Register Methods
    public interface IUserData<T> where T : class
    {
        //Register Method
        Task<bool> RegisterUser(T user);

        //Login Method
        Task<bool> LoginUser(T user);
    }
}
