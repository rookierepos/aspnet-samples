using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspnet.Web.Model;

namespace Aspnet.Web.DAL.Abstractions
{
    public interface IRoleRepository
    {
        bool HasRoleName(string name, IDbTransaction transaction);
        Task<bool> HasRoleNameAsync(string name, IDbTransaction transaction);

        int AddRole(Role role, IDbTransaction transaction);
        Task<int> AddRoleAsync(Role role, IDbTransaction transaction);

        int AddRoleWithPermission(int roleId, int[] permissionId, IDbTransaction transaction);
        Task<int> AddRoleWithPermissionAsync(int roleId, int[] permissionId, IDbTransaction transaction);

        bool RemoveRole(int roleId, IDbTransaction transaction);
        Task<bool> RemoveRoleAsync(int roleId, IDbTransaction transaction);

        bool UpdateRoleName(int roleId, string name, IDbTransaction transaction);
        Task<bool> UpdateRoleNameAsync(int roleId, string name, IDbTransaction transaction);

        bool UpdateRolePermission(int roleId, int[] newPermission, IDbTransaction transaction);
        Task<bool> UpdateRolePermissionAsync(int roleId, int[] newPermission, IDbTransaction transaction);

        bool HasPermissionName(string name, IDbTransaction transaction);
        Task<bool> HasPermissionNameAsync(string name, IDbTransaction transaction);

        int AddPermission(Permission permission, IDbTransaction transaction);
        Task<int> AddPermissionAsync(Permission permission, IDbTransaction transaction);

        bool UpdatePermissionName(int permissionId, string name, IDbTransaction transaction);
        Task<bool> UpdatePermissionNameAsync(int permissionId, string name, IDbTransaction transaction);

        bool RemovePermission(int permissionId, IDbTransaction transaction);
        Task<bool> RemovePermissionAsync(int permissionId, IDbTransaction transaction);

        bool UpdateUserRole(int userId, int roleId, IDbTransaction transaction);
        Task<bool> UpdateUserRoleAsync(int userId, int roleId, IDbTransaction transaction);
    }
}
