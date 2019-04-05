using Aspnet.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Web.BLL.Abstractions
{
    public interface IAccountService
    {
        Task<(int userId, string message)> RegisterAsync(User user);
        Task<(User user, string key, string message)> LoginAsync(string userName, string password);
        Task<IEnumerable<User>> AccountListAsync(int pageIndex, int pageSize);
        Task<(Role role, Permission[] permissions)> UserRolePermission(int userId);
    }
}
