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
        bool CheckRoleName(string name, IDbTransaction transaction = null);
        Task<bool> CheckRoleNameAsync(string name, IDbTransaction transaction = null);

        int AddRole(Role role, IDbTransaction transaction = null);
        Task<int> AddRoleAsync(Role role, IDbTransaction transaction = null);

        int AddRoleWithPermission(int roleId, int[] permissionId, IDbTransaction transaction = null);
        Task<int> AddRoleWithPermissionAsync(int roleId, int[] permissionId, IDbTransaction transaction = null);

        bool RemoveRole(int roleId, IDbTransaction transaction = null);
        Task<bool> RemoveRoleAsync(int roleId, IDbTransaction transaction = null);

        bool UpdateRoleName(int roleId, string name, IDbTransaction transaction = null);
        Task<bool> UpdateRoleNameAsync(int roleId, string name, IDbTransaction transaction = null);

        bool UpdateRolePermission(int roleId, int[] newPermission, IDbTransaction transaction = null);
        Task<bool> UpdateRolePermissionAsync(int roleId, int[] newPermission, IDbTransaction transaction = null);

        bool CheckPermissionName(string name, IDbTransaction transaction = null);
        Task<bool> CheckPermissionNameAsync(string name, IDbTransaction transaction = null);

        int AddPermission(Permission permission, IDbTransaction transaction = null);
        Task<int> AddPermissionAsync(Permission permission, IDbTransaction transaction = null);

        bool UpdatePermissionName(int permissionId, string name, IDbTransaction transaction = null);
        Task<bool> UpdatePermissionNameAsync(int permissionId, string name, IDbTransaction transaction = null);

        bool RemovePermission(int permissionId, IDbTransaction transaction = null);
        Task<bool> RemovePermissionAsync(int permissionId, IDbTransaction transaction = null);

        bool UpdateUserRole(int userId, int roleId, IDbTransaction transaction = null);
        Task<bool> UpdateUserRoleAsync(int userId, int roleId, IDbTransaction transaction = null);
        
        Role GetUserRole(int userId, IDbTransaction transaction = null);
        Task<Role> GetUserRoleAsync(int userId, IDbTransaction transaction = null);
        
        Permission[] GetRolePermission(int roleId, IDbTransaction transaction = null);
        Task<Permission[]> GetRolePermissionAsync(int roleId, IDbTransaction transaction = null);
    }
}
