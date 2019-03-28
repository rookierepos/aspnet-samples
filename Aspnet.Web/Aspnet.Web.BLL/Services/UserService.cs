using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspnet.Web.BLL.Abstractions;
using Aspnet.Web.Model;
using Aspnet.Web.DAL;
using Aspnet.Web.DAL.Abstractions;
using Aspnet.Web.DAL.Dapper;

namespace Aspnet.Web.BLL.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IDbConnection connection) : base(connection)
        {
            _userRepository = new UserRepository(_connection);
        }

        public int AddUser(User user)
        {
            return _userRepository.AddUser(user, null);
        }

        public Task<int> AddUserAsync(User user)
        {
            return _userRepository.AddUserAsync(user, null);
        }

        public bool CheckUserName(string name)
        {
            return _userRepository.CheckUserName(name);
        }

        public Task<bool> CheckUserNameAsync(string name)
        {
            return _userRepository.CheckUserNameAsync(name);
        }

        public(bool result, string message) ChangePassword(int id, string oldPassword, string newPassword)
        {
            return _userRepository.ChangePassword(id, oldPassword, newPassword, null);
        }

        public Task < (bool result, string message) > ChangePasswordAsync(int id, string oldPassword, string newPassword)
        {
            return _userRepository.ChangePasswordAsync(id, oldPassword, newPassword, null);
        }

        public User GetUser(int id)
        {
            return _userRepository.GetUser(id);
        }

        public User GetUser(string name)
        {
            return _userRepository.GetUser(name);
        }

        public Task<User> GetUserAsync(int id)
        {
            return _userRepository.GetUserAsync(id);
        }

        public Task<User> GetUserAsync(string name)
        {
            return _userRepository.GetUserAsync(name);
        }

        public bool UpdateLastLoginTime(int id)
        {
            return _userRepository.UpdateLastLoginTime(id, null);
        }

        public Task<bool> UpdateLastLoginTimeAsync(int id)
        {
            return _userRepository.UpdateLastLoginTimeAsync(id, null);
        }

        public IEnumerable<User> GetUserList(int pageSize, int pageIndex)
        {
            return _userRepository.GetUserList(pageSize, pageIndex);
        }

        public Task<IEnumerable<User>> GetUserListAsync(int pageSize, int pageIndex)
        {
            return _userRepository.GetUserListAsync(pageSize, pageIndex);
        }
    }
}
