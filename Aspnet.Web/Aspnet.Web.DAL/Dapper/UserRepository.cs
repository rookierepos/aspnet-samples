using System;
using System.Data;
using System.Threading.Tasks;
using Aspnet.Web.Common;
using Aspnet.Web.DAL.Abstractions;
using Aspnet.Web.Model;
using Dapper;

namespace Aspnet.Web.DAL.Dapper
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly string _userTableName;

        public UserRepository(IDbConnection connection) : base(connection)
        {
            _userTableName = typeof(User).GetTableName();
        }

        public int AddUser(User user, IDbTransaction transaction = null)
        {
            int userId = _connection.QuerySingle<int>($@"
                INSERT INTO {_userTableName}(Name, Password, Nick, Admin, CreateTime, Status)
                VALUES(@Name, @Password, @Nick, @Admin, @CreateTime, @Status);
                SELECT LAST_INSERT_ID();",
                new
                {
                    Name = user.Name,
                    Password = user.Password,
                    Nick = user.Nick,
                    Admin = user.Admin,
                    CreateTime = user.CreateTime,
                    Status = user.Status
                }, transaction);
            return userId;
        }

        public async Task<int> AddUserAsync(User user, IDbTransaction transaction = null)
        {
            int userId = await _connection.QuerySingleAsync<int>($@"
                INSERT INTO {_userTableName}(Name, Password, Nick, Admin, CreateTime, Status)
                VALUES(@Name, @Password, @Nick, @Admin, @CreateTime, @Status);
                SELECT LAST_INSERT_ID();",
                new
                {
                    Name = user.Name,
                    Password = user.Password,
                    Nick = user.Nick,
                    Admin = user.Admin,
                    CreateTime = user.CreateTime,
                    Status = user.Status
                }, transaction);
            return userId;
        }

        public(bool result, string message) ChangePassword(int id, string oldPassword, string newPassword, IDbTransaction transaction)
        {
            var nowPassword = _connection.QueryFirstOrDefault<string>($"SELECT Password FROM {_userTableName} WHERE Id=@Id LIMIT 1;",
                new { Id = id }, transaction);
            if (string.IsNullOrEmpty(nowPassword))
                return (false, "未找到该用户");
            else if (nowPassword != oldPassword)
                return (false, "旧密码错误");
            var result = _connection.Execute($"UPDATE {_userTableName} SET Password=@newPassword WHERE Id=@Id;",
                new { Id = id, newPassword = newPassword }, transaction);
            if (result > 0)
                return (true, "修改成功");
            else
                return (false, "修改成功");
        }

        public async Task < (bool result, string message) > ChangePasswordAsync(int id, string oldPassword, string newPassword, IDbTransaction transaction)
        {
            var nowPassword = await _connection.QueryFirstOrDefaultAsync<string>($"SELECT Password FROM {_userTableName} WHERE Id=@Id LIMIT 1;",
                new { Id = id }, transaction);
            if (string.IsNullOrEmpty(nowPassword))
                return (false, "未找到该用户");
            else if (nowPassword != oldPassword)
                return (false, "旧密码错误");
            var result = await _connection.ExecuteAsync($"UPDATE {_userTableName} SET Password=@newPassword WHERE Id=@Id;",
                new { Id = id, newPassword = newPassword }, transaction);
            if (result > 0)
                return (true, "修改成功");
            else
                return (false, "修改成功");
        }

        public User GetUser(int id)
        {
            return _connection.QueryFirstOrDefault<User>($"SELECT * FROM {_userTableName} WHERE Id=@Id;",
                new { Id = id });
        }

        public Task<User> GetUserAsync(int id)
        {
            return _connection.QueryFirstOrDefaultAsync<User>($"SELECT * FROM {_userTableName} WHERE Id=@Id;",
                new { Id = id });
        }

        public User GetUser(string name)
        {
            return _connection.QueryFirstOrDefault<User>($"SELECT * FROM {_userTableName} WHERE Name=@Name LIMIT 1;",
                new { Name = name });
        }

        public Task<User> GetUserAsync(string name)
        {
            return _connection.QueryFirstOrDefaultAsync<User>($"SELECT * FROM {_userTableName} WHERE Name=@Name LIMIT 1;",
                new { Name = name });
        }

        public bool UpdateLastLoginTime(int id, IDbTransaction transaction)
        {
            var result = _connection.Execute($"UPDATE {_userTableName} SET LastLoginTime=@LastLoginTime WHERE Id=@Id;",
                new { Id = id, LastLoginTime = DateTime.Now }, transaction);
            return result > 0;
        }

        public async Task<bool> UpdateLastLoginTimeAsync(int id, IDbTransaction transaction)
        {
            var result = await _connection.ExecuteAsync($"UPDATE {_userTableName} SET LastLoginTime=@LastLoginTime WHERE Id=@Id;",
                new { Id = id, LastLoginTime = DateTime.Now }, transaction);
            return result > 0;
        }

        public bool CheckUserName(string name)
        {
            return _connection.QueryFirstOrDefault<int>($"SELECT Id FROM {_userTableName} WHERE Name=@Name LIMIT 1;",
                new { Name = name }) > 0;
        }

        public async Task<bool> CheckUserNameAsync(string name)
        {
            return await _connection.QueryFirstOrDefaultAsync<int>($"SELECT Id FROM {_userTableName} WHERE Name=@Name LIMIT 1;",
                       new { Name = name }) > 0;
        }
    }
}
