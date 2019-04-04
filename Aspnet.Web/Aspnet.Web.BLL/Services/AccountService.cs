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

        public AccountService(IDbConnection connection, IUserRepository userRepository) : base(connection)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> AccountListAsync(int pageIndex, int pageSize)
        {
            return _userRepository.GetUserListAsync(pageIndex, pageSize);
        }

        public async Task<(User user, string key, string message)> LoginAsync(string userName, string password)
        {
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
                await _userRepository.UpdateLastLoginTimeAsync(user.Id, null);
                return (user, null, null);
            }
        }

        public async Task<(int userId, string message)> RegisterAsync(User user)
        {
            if (await _userRepository.CheckUserNameAsync(user.Name, null))
            {
                return (0, "用户名已存在！");
            }
            else
            {
                user.Id = await _userRepository.AddUserAsync(user, null);
                return (user.Id, null);
            }
        }
    }
}
