using System;
using Aspnet.Web.Common;

namespace Aspnet.Web.Model
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Nick { get; set; }
        public bool Admin { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime? LockedDate { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Normal;
    }

    public enum UserStatus : short
    {
        Normal = 0b0001,
        Locked = 0b0010,
        Deleted = 0b0100
    }
}
