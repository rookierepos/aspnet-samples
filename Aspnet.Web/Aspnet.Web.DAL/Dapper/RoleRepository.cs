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

        public bool CheckPermissionName(string name, IDbTransaction transaction)
        {
            try
            {
                return _connection.QueryFirstOrDefault<int>(
                           $"SELECT Id FROM {_permissionTableName} WHERE Name=@Name AND Status!=@Status LIMIT 1;",
                           new {Name = name, Status = Status.Deleted}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("检察权限名", ex);
            }
        }

        public async Task<bool> CheckPermissionNameAsync(string name, IDbTransaction transaction)
        {
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<int>(
                           $"SELECT Id FROM {_permissionTableName} WHERE Name=@Name AND Status!=@Status LIMIT 1;",
                           new {Name = name, Status = Status.Deleted}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("检察权限名", ex);
            }
        }

        public int AddPermission(Permission permission, IDbTransaction transaction)
        {
            try
            {
                return _connection.QuerySingle<int>(
                    $@"INSERT INTO {_permissionTableName}(Name, CreatedTime, ChangedTime, Status)
                     VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                     SELECT LAST_INSERT_ID();",
                    new
                    {
                        Name = permission.Name,
                        CreatedTime = permission.CreatedTime,
                        Status = permission.Status
                    }, transaction);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("添加权限", ex);
            }
        }

        public Task<int> AddPermissionAsync(Permission permission, IDbTransaction transaction)
        {
            try
            {
                return _connection.QuerySingleAsync<int>(
                    $@"INSERT INTO {_permissionTableName}(Name, CreatedTime, ChangedTime, Status)
                     VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                     SELECT LAST_INSERT_ID();",
                    new
                    {
                        Name = permission.Name,
                        CreatedTime = permission.CreatedTime,
                        Status = permission.Status
                    }, transaction);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("添加权限", ex);
            }
        }

        public bool CheckRoleName(string name, IDbTransaction transaction)
        {
            try
            {
                return _connection.QueryFirstOrDefault<int>(
                           $"SELECT Id FROM {_roleTableName} WHERE Name=@Name AND Status!=@Status LIMIT 1;",
                           new {Name = name, Status = Status.Deleted}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("检查角色名", ex);
            }
        }

        public async Task<bool> CheckRoleNameAsync(string name, IDbTransaction transaction)
        {
            try
            {
                return await _connection.QueryFirstOrDefaultAsync<int>(
                           $"SELECT Id FROM {_roleTableName} WHERE Name=@Name AND Status!=@Status LIMIT 1;",
                           new {Name = name, Status = Status.Deleted}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("检查角色名", ex);
            }
        }

        public int AddRole(Role role, IDbTransaction transaction)
        {
            try
            {
                return _connection.QuerySingle<int>(
                    $@"INSERT INTO {_roleTableName}(Name, CreatedTime, ChangedTime, Status)
                     VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                     SELECT LAST_INSERT_ID();",
                    new
                    {
                        Name = role.Name,
                        CreatedTime = role.CreatedTime,
                        Status = role.Status
                    }, transaction);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("添加角色", ex);
            }
        }

        public Task<int> AddRoleAsync(Role role, IDbTransaction transaction)
        {
            try
            {
                return _connection.QuerySingleAsync<int>(
                    $@"INSERT INTO {_roleTableName}(Name, CreatedTime, ChangedTime, Status)
                     VALUES(@Name, @CreatedTime, @CreatedTime, @Status);
                     SELECT LAST_INSERT_ID();",
                    new
                    {
                        Name = role.Name,
                        CreatedTime = role.CreatedTime,
                        Status = role.Status
                    }, transaction);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("添加角色", ex);
            }
        }

        public int AddRoleWithPermission(int roleId, int[] permissionId, IDbTransaction transaction)
        {
            object[] param = new object[permissionId.Length];
            for (int i = 0; i < permissionId.Length; i++)
            {
                param[i] = new { RoleId = roleId, PermissionId = permissionId[i] };
            }
            try
            {
                return _connection.Execute(
                    $"REPLACE INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);",
                    param,
                    transaction);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("添加角色权限", ex);
            }
        }

        public Task<int> AddRoleWithPermissionAsync(int roleId, int[] permissionId, IDbTransaction transaction)
        {
            object[] param = new object[permissionId.Length];
            for (int i = 0; i < permissionId.Length; i++)
            {
                param[i] = new { RoleId = roleId, PermissionId = permissionId[i] };
            }
            try
            {
                return _connection.ExecuteAsync(
                    $"REPLACE INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);",
                    param,
                    transaction);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("添加角色权限", ex);
            }
        }

        public bool RemovePermission(int permissionId, IDbTransaction transaction)
        {
            try
            {
                return _connection.Execute(
                           $@"UPDATE {_permissionTableName} SET Status=@Status, ChangedTime=@ChangedTime WHERE Id=@Id;
                            DELETE FROM {_rolePermissionTableName} WHERE PermissionId=@Id;",
                           new {Id = permissionId, Status = Status.Deleted, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("删除权限", ex);
            }
        }

        public async Task<bool> RemovePermissionAsync(int permissionId, IDbTransaction transaction)
        {
            try
            {
                return await _connection.ExecuteAsync(
                           $@"UPDATE {_permissionTableName} SET Status=@Status, ChangedTime=@ChangedTime WHERE Id=@Id;
                            DELETE FROM {_rolePermissionTableName} WHERE PermissionId=@Id;",
                           new {Id = permissionId, Status = Status.Deleted, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("删除权限", ex);
            }
        }

        public bool RemoveRole(int roleId, IDbTransaction transaction)
        {
            try
            {
                return _connection.Execute(
                           $@"UPDATE {_roleTableName} SET Status=@Status, ChangedTime=@ChangedTime WHERE Id=@Id;
                            DELETE FROM {_rolePermissionTableName} WHERE RoleId=@Id;",
                           new {Id = roleId, Status = Status.Deleted, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("删除角色", ex);
            }
        }

        public async Task<bool> RemoveRoleAsync(int roleId, IDbTransaction transaction)
        {
            try
            {
                return await _connection.ExecuteAsync(
                           $@"UPDATE {_roleTableName} SET Status=@Status, ChangedTime=@ChangedTime WHERE Id=@Id;
                            DELETE FROM {_rolePermissionTableName} WHERE RoleId=@Id;",
                           new {Id = roleId, Status = Status.Deleted, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("删除角色", ex);
            }
        }

        public bool UpdatePermissionName(int permissionId, string name, IDbTransaction transaction)
        {
            try
            {
                return _connection.Execute($"UPDATE {_permissionTableName} SET Name=@Name, ChangedTime=@ChangedTime WHERE Id=@Id;",
                           new {Id = permissionId, Name = name, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("修改权限名", ex);
            }
        }

        public async Task<bool> UpdatePermissionNameAsync(int permissionId, string name, IDbTransaction transaction)
        {
            try
            {
                return await _connection.ExecuteAsync($"UPDATE {_permissionTableName} SET Name=@Name, ChangedTime=@ChangedTime WHERE Id=@Id;",
                           new {Id = permissionId, Name = name, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("修改权限名", ex);
            }
        }

        public bool UpdateRoleName(int roleId, string name, IDbTransaction transaction)
        {
            try
            {
                return _connection.Execute($"UPDATE {_roleTableName} SET Name=@Name, ChangedTime=@ChangedTime WHERE Id=@Id;",
                           new {Id = roleId, Name = name, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("修改角色名", ex);
            }
        }

        public async Task<bool> UpdateRoleNameAsync(int roleId, string name, IDbTransaction transaction)
        {
            try
            {
                return await _connection.ExecuteAsync($"UPDATE {_roleTableName} SET Name=@Name, ChangedTime=@ChangedTime WHERE Id=@Id;",
                           new {Id = roleId, Name = name, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("修改角色名", ex);
            }
        }

        public bool UpdateRolePermission(int roleId, int[] newPermission, IDbTransaction transaction)
        {
            List<int> oldPermission;
            try
            {
                oldPermission = _connection.Query<int>(
                    $"SELECT PermissionId FROM {_rolePermissionTableName} WHERE RoleId=@RoleId;",
                    new {RoleId = roleId}, transaction).AsList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("查看现有权限", ex);
            }

            GetList(newPermission, oldPermission, out List<int> preAdd, out List<int> preDelete);

            bool result = false;

            if (preAdd.Count > 0)
            {
                object[] param = new object[preAdd.Count];
                for (int i = 0; i < preAdd.Count; i++)
                {
                    param[i] = new { RoleId = roleId, PermissionId = preAdd[i] };
                }
                try
                {
                    result = _connection.Execute(
                                 $"INSERT INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);",
                                 param, transaction) > 0;
                }
                catch (Exception ex)
                {
                    throw new RepositoryException("添加角色权限", ex);
                }
            }

            if (result && preDelete.Count > 0)
            {
                try
                {
                    result = _connection.Execute(
                                 $"DELETE FROM {_rolePermissionTableName} WHERE RoleId=@RoleId AND PermissionId IN @PermissionId",
                                 new {RoleId = roleId, PermissionId = preDelete.ToArray()}, transaction) > 0;
                }
                catch (Exception ex)
                {
                    throw new RepositoryException("删除角色权限", ex);
                }
            }
            return result;
        }

        public async Task<bool> UpdateRolePermissionAsync(int roleId, int[] newPermission, IDbTransaction transaction)
        {
            List<int> oldPermission;
            try
            {
                oldPermission = (await _connection.QueryAsync<int>(
                    $"SELECT PermissionId FROM {_rolePermissionTableName} WHERE RoleId=@RoleId;",
                    new { RoleId = roleId }, transaction)).AsList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("查看现有权限", ex);
            }

            GetList(newPermission, oldPermission, out List<int> preAdd, out List<int> preDelete);

            bool result = false;

            if (preAdd.Count > 0)
            {
                object[] param = new object[preAdd.Count];
                for (int i = 0; i < preAdd.Count; i++)
                {
                    param[i] = new { RoleId = roleId, PermissionId = preAdd[i] };
                }
                try
                {
                    result = await _connection.ExecuteAsync(
                                 $"INSERT INTO {_rolePermissionTableName}(RoleId, PermissionId)VALUES(@RoleId, @PermissionId);",
                                 param, transaction) > 0;
                }
                catch (Exception ex)
                {
                    throw new RepositoryException("添加角色权限", ex);
                }
            }

            if (result && preDelete.Count > 0)
            {
                try
                {
                    result = await _connection.ExecuteAsync(
                                 $"DELETE FROM {_rolePermissionTableName} WHERE RoleId=@RoleId AND PermissionId IN @PermissionId",
                                 new {RoleId = roleId, PermissionId = preDelete.ToArray()}, transaction) > 0;
                }
                catch (Exception ex)
                {
                    throw new RepositoryException("删除角色权限", ex);
                }
            }
            return result;
        }

        private static void GetList(int[] newPermission, List<int> oldPermission, out List<int> preAdd, out List<int> preDelete)
        {
            int oldCount = oldPermission.Count;
            if (oldCount > 0)
            {
                preAdd = new List<int>();
                if (newPermission.Length > 0)
                {
                    for (int i = 0; i < newPermission.Length; i++)
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
            try
            {
                return _connection.Execute($"UPDATE {_userTableName} SET RoleId=@RoleId, ChangedTime=@ChangedTime WHERE Id=@Id;",
                           new {Id = userId, RoleId = roleId, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("修改用户角色", ex);
            }
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, int roleId, IDbTransaction transaction)
        {
            try
            {
                return await _connection.ExecuteAsync($"UPDATE {_userTableName} SET RoleId=@RoleId, ChangedTime=@ChangedTime WHERE Id=@Id;",
                           new {Id = userId, RoleId = roleId, ChangedTime = DateTime.Now}, transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("修改用户角色", ex);
            }
        }

        public Role GetUserRole(int userId, IDbTransaction transaction = null)
        {
            try
            {
                var roleId = _connection.Query<int>($"SELECT RoleId FROM {_userTableName} WHERE Id=@Id;",
                    new {Id = userId}, transaction).FirstOrDefault();
                if (roleId > 0)
                {
                    return _connection.Query<Role>($"SELECT * FROM {_roleTableName} WHERE Id=@Id AND Status!=@Status;",
                        new {Id = roleId, Status = Status.Deleted}, transaction).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("获取用户角色", ex);
            }
        }

        public async Task<Role> GetUserRoleAsync(int userId, IDbTransaction transaction = null)
        {
            try
            {
                var roleId = (await _connection.QueryAsync<int>($"SELECT RoleId FROM {_userTableName} WHERE Id=@Id;",
                    new {Id = userId}, transaction)).FirstOrDefault();
                if (roleId > 0)
                {
                    return (await _connection.QueryAsync<Role>($"SELECT * FROM {_roleTableName} WHERE Id=@Id AND Status!=@Status;",
                        new {Id = roleId, Status = Status.Deleted}, transaction)).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("获取用户角色", ex);
            }
        }

        public Permission[] GetRolePermission(int roleId, IDbTransaction transaction = null)
        {
            try
            {
                return _connection.Query<Permission>(
                    $@"SELECT p.* FROM {_permissionTableName} p 
                        INNER JOIN {_rolePermissionTableName} rp ON p.Id = rp.PermissionId
                        WHERE rp.RoleId=@RoleId;",
                    new {RoleId = roleId}, transaction).ToArray();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("获取角色权限", ex);
            }
        }

        public async Task<Permission[]> GetRolePermissionAsync(int roleId, IDbTransaction transaction = null)
        {
            try
            {
                return (await _connection.QueryAsync<Permission>(
                    $@"SELECT p.* FROM {_permissionTableName} p 
                        INNER JOIN {_rolePermissionTableName} rp ON p.Id = rp.PermissionId
                        WHERE rp.RoleId=@RoleId;",
                    new {RoleId = roleId}, transaction)).ToArray();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("获取角色权限", ex);
            }
        }
    }
}
