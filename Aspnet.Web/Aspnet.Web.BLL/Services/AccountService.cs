using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspnet.Web.BLL.Abstractions;
using Aspnet.Web.Common;
using Aspnet.Web.DAL.Abstractions;
using Aspnet.Web.Model;

namespace Aspnet.Web.BLL.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(IDbConnection connection, IUserRepository userRepository, IRoleRepository roleRepository) : base(connection)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<User>> AccountListAsync(int pageIndex, int pageSize)
        {
            return _userRepository.GetUserListAsync(pageIndex, pageSize);
        }

        public async Task<(User user, string key, string message)> LoginAsync(string userName, string password)
        {
            try
            {
                _connection.Open();
                var user = await _userRepository.GetUserAsync(userName);
                if (user == null)
                {
                    return (null, nameof(userName), "用户名错误！");
                }
                else if (password.MD5() != user.Password)
                {
                    return (null, nameof(password), "密码错误！");
                }
                else
                {
                    await _userRepository.UpdateLastLoginTimeAsync(user.Id);
                    return (user, null, null);
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<(int userId, string message)> RegisterAsync(User user)
        {
            try
            {
                _connection.Open();
                if (await _userRepository.CheckUserNameAsync(user.Name))
                {
                    return (0, "用户名已存在！");
                }
                else
                {
                    user.Id = await _userRepository.AddUserAsync(user);
                    return (user.Id, null);
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<(Role role, Permission[] permissions)> UserRolePermission(int userId)
        {
            try
            {
                _connection.Open();
                var role = await _roleRepository.GetUserRoleAsync(userId);
                if (role == null)
                {
                    return (null, null);
                }
                else
                {
                    return (role, await _roleRepository.GetRolePermissionAsync(role.Id));
                }
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
