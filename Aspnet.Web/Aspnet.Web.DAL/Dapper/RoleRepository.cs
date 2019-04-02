using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspnet.Web.Common;
using Aspnet.Web.DAL.Abstractions;
using Aspnet.Web.Model;
using Dapper;

namespace Aspnet.Web.DAL.Dapper
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        private readonly string _userTableName;
        private readonly string _roleTableName;
        private readonly string _permissionTableName;
        private readonly string _rolePermissionTableName;

        public RoleRepository(IDbConnection connection) : base(connection)
        {
            _userTableName = typeof(User).GetTableName();
            _roleTableName = typeof(Role).GetTableName();
            _permissionTableName = typeof(Permission).GetTableName();
            _rolePermissionTableName = typeof(RolePermission).GetTableName();
        }

        public bool HasPermissionName(string name, IDbTransaction transaction)
        {
            return _connection.QueryFirstOrDefault<int>($"SELECT Id FROM {_permissionTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;", new { Name = name, Status = Status.Deleted }, transaction) > 0;
        }

        public async Task<bool> HasPermissionNameAsync(string name, IDbTransaction transaction)
        {
            return await _connection.QueryFirstOrDefaultAsync<int>($"SELECT Id FROM {_permissionTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;", new { Name = name, Status = Status.Deleted }, transaction) > 0;
        }

        public int AddPermission(Permission permission, IDbTransaction transaction)
        {
            int id = _connection.QuerySingle<int>($@"
                INSERT INTO {_permissionTableName}(Name, CreatedTime, ChangedTime, Status)
                VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                SELECT LAST_INSERT_ID();",
                new
                {
                    Name = permission.Name,
                    CreatedTime = permission.CreatedTime,
                    Status = permission.Status
                }, transaction);
            return id;
        }

        public async Task<int> AddPermissionAsync(Permission permission, IDbTransaction transaction)
        {
            int id = await _connection.QuerySingleAsync<int>($@"
                INSERT INTO {_permissionTableName}(Name, CreatedTime, ChangedTime, Status)
                VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                SELECT LAST_INSERT_ID();",
                new
                {
                    Name = permission.Name,
                    CreatedTime = permission.CreatedTime,
                    Status = permission.Status
                }, transaction);
            return id;
        }

        public bool HasRoleName(string name, IDbTransaction transaction)
        {
            return _connection.QueryFirstOrDefault<int>($"SELECT Id FROM {_roleTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;", new { Name = name, Status = Status.Deleted }, transaction) > 0;
        }

        public async Task<bool> HasRoleNameAsync(string name, IDbTransaction transaction)
        {
            return await _connection.QueryFirstOrDefaultAsync<int>($"SELECT Id FROM {_roleTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;", new { Name = name, Status = Status.Deleted }, transaction) > 0;
        }

        public int AddRole(Role role, IDbTransaction transaction)
        {
            int id = _connection.QuerySingle<int>($@"
                INSERT INTO {_roleTableName}(Name, CreatedTime, ChangedTime, Status)
                VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                SELECT LAST_INSERT_ID();",
                new
                {
                    Name = role.Name,
                    CreatedTime = role.CreatedTime,
                    Status = role.Status
                }, transaction);
            return id;
        }

        public async Task<int> AddRoleAsync(Role role, IDbTransaction transaction)
        {
            int id = await _connection.QuerySingleAsync<int>($@"
                INSERT INTO {_roleTableName}(Name, CreatedTime, ChangedTime, Status)
                VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                SELECT LAST_INSERT_ID();",
                new
                {
                    Name = role.Name,
                    CreatedTime = role.CreatedTime,
                    Status = role.Status
                }, transaction);
            return id;
        }

        public int AddRoleWithPermission(int roleId, int[] permissionId, IDbTransaction transaction)
        {
            object[] param = new object[permissionId.Length];
            for (int i = 0; i < permissionId.Length; i++)
            {
                param[i] = new { RoleId = roleId, PermissionId = permissionId[i] };
            }
            return _connection.Execute("REPLACE INTO test(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);", param, transaction);
        }

        public Task<int> AddRoleWithPermissionAsync(int roleId, int[] permissionId, IDbTransaction transaction)
        {
            object[] param = new object[permissionId.Length];
            for (int i = 0; i < permissionId.Length; i++)
            {
                param[i] = new { RoleId = roleId, PermissionId = permissionId[i] };
            }
            return _connection.ExecuteAsync("REPLACE INTO test(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);", param, transaction);
        }

        public bool RemovePermission(int permissionId, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemovePermissionAsync(int permissionId, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRole(int roleId, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveRoleAsync(int roleId, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePermissionName(string name, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePermissionNameAsync(string name, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRoleName(string name, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRoleNameAsync(string name, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRolePermission(int roleId, int[] newPermission, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRolePermissionAsync(int roleId, int[] newPermission, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserRole(int userId, int roleId, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserRoleAsync(int userId, int roleId, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
