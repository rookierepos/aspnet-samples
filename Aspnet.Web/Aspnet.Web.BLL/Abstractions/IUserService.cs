using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspnet.Web.Model;

namespace Aspnet.Web.BLL.Abstractions
{
    public interface IUserService
    {
        int AddUser(User user);
        Task<int> AddUserAsync(User user);
        User GetUser(int id);
        Task<User> GetUserAsync(int id);
        User GetUser(string name);
        Task<User> GetUserAsync(string name);
        bool CheckUserName(string name);
        Task<bool> CheckUserNameAsync(string name);
        (bool result, string message) ChangePassword(int id, string oldPassword, string newPassword);
        Task < (bool result, string message) > ChangePasswordAsync(int id, string oldPassword, string newPassword);
        bool UpdateLastLoginTime(int id);
        Task<bool> UpdateLastLoginTimeAsync(int id);
        IEnumerable<User> GetUserList(int pageSize, int pageIndex);
        Task<IEnumerable<User>> GetUserListAsync(int pageSize, int pageIndex);
    }
}
