using System.Data;
using System.Threading.Tasks;
using Aspnet.Web.Model;

namespace Aspnet.Web.DAL.Abstractions
{
    public interface IUserRepository
    {
        int AddUser(User user, IDbTransaction transaction);
        Task<int> AddUserAsync(User user, IDbTransaction transaction);
        User GetUser(int id);
        Task<User> GetUserAsync(int id);
        User GetUser(string name);
        Task<User> GetUserAsync(string name);
        bool CheckUserName(string name);
        Task<bool> CheckUserNameAsync(string name);
        (bool result, string message) ChangePassword(int id, string oldPassword, string newPassword, IDbTransaction transaction);
        Task < (bool result, string message) > ChangePasswordAsync(int id, string oldPassword, string newPassword, IDbTransaction transaction);
        bool UpdateLastLoginTime(int id, IDbTransaction transaction);
        Task<bool> UpdateLastLoginTimeAsync(int id, IDbTransaction transaction);
    }
}
