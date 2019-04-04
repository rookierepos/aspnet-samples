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
            return _connection.QueryFirstOrDefault<int>(
                       $"SELECT Id FROM {_permissionTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;",
                       new {Name = name, Status = Status.Deleted}, transaction) > 0;
        }

        public async Task<bool> HasPermissionNameAsync(string name, IDbTransaction transaction)
        {
            return await _connection.QueryFirstOrDefaultAsync<int>(
                       $"SELECT Id FROM {_permissionTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;",
                       new {Name = name, Status = Status.Deleted}, transaction) > 0;
        }

        public int AddPermission(Permission permission, IDbTransaction transaction)
        {
            int id = _connection.QuerySingle<int>(
                $@"INSERT INTO {_permissionTableName}(Name, CreatedTime, ChangedTime, Status)
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
            int id = await _connection.QuerySingleAsync<int>(
                $@"INSERT INTO {_permissionTableName}(Name, CreatedTime, ChangedTime, Status)
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
            return _connection.QueryFirstOrDefault<int>(
                       $"SELECT Id FROM {_roleTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;",
                       new {Name = name, Status = Status.Deleted}, transaction) > 0;
        }

        public async Task<bool> HasRoleNameAsync(string name, IDbTransaction transaction)
        {
            return await _connection.QueryFirstOrDefaultAsync<int>(
                       $"SELECT Id FROM {_roleTableName} WHERE Name=@Name AND Status!=@Status  LIMIT 1;",
                       new {Name = name, Status = Status.Deleted}, transaction) > 0;
        }

        public int AddRole(Role role, IDbTransaction transaction)
        {
            int id = _connection.QuerySingle<int>(
                $@"INSERT INTO {_roleTableName}(Name, CreatedTime, ChangedTime, Status)
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
            int id = await _connection.QuerySingleAsync<int>(
                $@"INSERT INTO {_roleTableName}(Name, CreatedTime, ChangedTime, Status)
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

            return _connection.Execute(
                $"REPLACE INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);", param,
                transaction);
        }

        public Task<int> AddRoleWithPermissionAsync(int roleId, int[] permissionId, IDbTransaction transaction)
        {
            object[] param = new object[permissionId.Length];
            for (int i = 0; i < permissionId.Length; i++)
            {
                param[i] = new { RoleId = roleId, PermissionId = permissionId[i] };
            }

            return _connection.ExecuteAsync(
                $"REPLACE INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);", param,
                transaction);
        }

        public bool RemovePermission(int permissionId, IDbTransaction transaction)
        {
            return _connection.Execute($@"UPDATE {_permissionTableName} SET Status=@Status WHERE Id=@Id;
                DELETE FROM {_rolePermissionTableName} WHERE PermissionId=@Id;",
                       new {Id = permissionId, Status = Status.Deleted}, transaction) > 0;
        }

        public async Task<bool> RemovePermissionAsync(int permissionId, IDbTransaction transaction)
        {
            return await _connection.ExecuteAsync($@"UPDATE {_permissionTableName} SET Status=@Status WHERE Id=@Id;
                DELETE FROM {_rolePermissionTableName} WHERE PermissionId=@Id;",
                       new {Id = permissionId, Status = Status.Deleted}, transaction) > 0;
        }

        public bool RemoveRole(int roleId, IDbTransaction transaction)
        {
            return _connection.Execute($@"UPDATE {_roleTableName} SET Status=@Status WHERE Id=@Id;
                DELETE FROM {_rolePermissionTableName} WHERE RoleId=@Id;",
                       new {Id = roleId, Status = Status.Deleted},
                       transaction) > 0;
        }

        public async Task<bool> RemoveRoleAsync(int roleId, IDbTransaction transaction)
        {
            return await _connection.ExecuteAsync($@"UPDATE {_roleTableName} SET Status=@Status WHERE Id=@Id;
                DELETE FROM {_rolePermissionTableName} WHERE RoleId=@Id;",
                       new {Id = roleId, Status = Status.Deleted},
                       transaction) > 0;
        }

        public bool UpdatePermissionName(int permissionId, string name, IDbTransaction transaction)
        {
            return _connection.Execute($"UPDATE {_permissionTableName} SET Name=@Name WHERE Id=@Id;",
                       new {Id = permissionId, Name = name}, transaction) > 0;
        }

        public async Task<bool> UpdatePermissionNameAsync(int permissionId, string name, IDbTransaction transaction)
        {
            return await _connection.ExecuteAsync($"UPDATE {_permissionTableName} SET Name=@Name WHERE Id=@Id;",
                       new {Id = permissionId, Name = name}, transaction) > 0;
        }

        public bool UpdateRoleName(int roleId, string name, IDbTransaction transaction)
        {
            return _connection.Execute($"UPDATE {_roleTableName} SET Name=@Name WHERE Id=@Id;",
                       new {Id = roleId, Name = name}, transaction) > 0;
        }

        public async Task<bool> UpdateRoleNameAsync(int roleId, string name, IDbTransaction transaction)
        {
            return await _connection.ExecuteAsync($"UPDATE {_roleTableName} SET Name=@Name WHERE Id=@Id;",
                       new {Id = roleId, Name = name}, transaction) > 0;
        }

        public bool UpdateRolePermission(int roleId, int[] newPermission, IDbTransaction transaction)
        {
            var oldPermission = _connection.Query<int>(
                $"SELECT PermissionId FROM {_rolePermissionTableName} WHERE RoleId=@RoleId;",
                new {RoleId = roleId}, transaction).AsList();

            GetList(newPermission, oldPermission, out List<int> preAdd, out List<int> preDelete);

            bool result = false;

            if (preAdd.Count > 0)
            {
                object[] param = new object[preAdd.Count];
                for (int i = 0; i < preAdd.Count; i++)
                {
                    param[i] = new { RoleId = roleId, PermissionId = preAdd[i] };
                }

                result = _connection.Execute(
                             $"INSERT INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);",
                             param, transaction) > 0;
            }

            if (result && preDelete.Count > 0)
            {
                result = _connection.Execute(
                    $"DELETE FROM {_rolePermissionTableName} WHERE RoleId=@RoleId AND PermissionId IN @PermissionId",
                    new {RoleId = roleId, PermissionId = preDelete.ToArray()}, transaction) > 0;
            }
            return result;
        }

        public async Task<bool> UpdateRolePermissionAsync(int roleId, int[] newPermission, IDbTransaction transaction)
        {
            var oldPermission = (await _connection.QueryAsync<int>(
                $"SELECT PermissionId FROM {_rolePermissionTableName} WHERE RoleId=@RoleId;",
                new { RoleId = roleId }, transaction)).AsList();

            GetList(newPermission, oldPermission, out List<int> preAdd, out List<int> preDelete);

            bool result = false;

            if (preAdd.Count > 0)
            {
                object[] param = new object[preAdd.Count];
                for (int i = 0; i < preAdd.Count; i++)
                {
                    param[i] = new { RoleId = roleId, PermissionId = preAdd[i] };
                }
                result = await _connection.ExecuteAsync(
                             $"INSERT INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);",
                             param, transaction) > 0;
            }

            if (result && preDelete.Count > 0)
            {
                result = await _connection.ExecuteAsync(
                             $"DELETE FROM {_rolePermissionTableName} WHERE RoleId=@RoleId AND PermissionId IN @PermissionId",
                             new {RoleId = roleId, PermissionId = preDelete.ToArray()}, transaction) > 0;
            }
            return result;
        }

        private static void GetList(int[] newPermission, List<int> oldPermission, out List<int> preAdd, out List<int> preDelete)
        {
            int oldCount = oldPermission.Count;
            int newCount = newPermission.Length;
            if (oldCount > 0)
            {
                preAdd = new List<int>();
                if (newCount > 0)
                {
                    for (int i = 0; i < newCount; i++)
                    {
                        for (int j = 0; j < oldCount; j++)
                        {
                            if (newPermission[i] == oldPermission[j])
                            {
                                newPermission[i] = -1;
                                oldPermission.RemoveAt(j);
                                oldCount--;
                                break;
                            }
                        }
                        if (newPermission[i] != -1)
                        {
                            preAdd.Add(newPermission[i]);
                        }
                    }
                }
                preDelete = oldPermission;
            }
            else
            {
                preAdd = newPermission.ToList();
                preDelete = new List<int>();
            }
        }

        public bool UpdateUserRole(int userId, int roleId, IDbTransaction transaction)
        {
            return _connection.Execute($"UPDATE {_userTableName} SET RoleId=@RoleId WHERE Id=@Id;",
                       new {Id = userId, RoleId = roleId}, transaction) > 0;
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, int roleId, IDbTransaction transaction)
        {
            return await _connection.ExecuteAsync($"UPDATE {_userTableName} SET RoleId=@RoleId WHERE Id=@Id;",
                       new {Id = userId, RoleId = roleId}, transaction) > 0;
        }
    }
}
