using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Aspnet.Web.Model;

namespace Aspnet.Web.DAL.Abstractions
{
    public interface IUserRepository
    {
        int AddUser(User user, IDbTransaction transaction = null);
        Task<int> AddUserAsync(User user, IDbTransaction transaction = null);
        User GetUser(int id, IDbTransaction transaction = null);
        Task<User> GetUserAsync(int id, IDbTransaction transaction = null);
        User GetUser(string name, IDbTransaction transaction = null);
        Task<User> GetUserAsync(string name, IDbTransaction transaction = null);
        bool CheckUserName(string name, IDbTransaction transaction = null);
        Task<bool> CheckUserNameAsync(string name, IDbTransaction transaction = null);
        (bool result, string message) ChangePassword(int id, string oldPassword, string newPassword, IDbTransaction transaction = null);
        Task < (bool result, string message) > ChangePasswordAsync(int id, string oldPassword, string newPassword, IDbTransaction transaction = null);
        bool UpdateLastLoginTime(int id, IDbTransaction transaction = null);
        Task<bool> UpdateLastLoginTimeAsync(int id, IDbTransaction transaction = null);
        IEnumerable<User> GetUserList(int pageIndex, int pageSize);
        Task<IEnumerable<User>> GetUserListAsync(int pageIndex, int pageSize);
    }
}
