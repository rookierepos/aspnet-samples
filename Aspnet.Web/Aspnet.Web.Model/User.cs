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
        public DateTime CreatedTime { get; set; }
        public DateTime ChangedTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime? LockedDate { get; set; }
        public Status Status { get; set; } = Status.Normal;
        public int RoleId { get; set; } = 0;
    }

    public enum Status : short
    {
        Normal = 0b0001,
        Locked = 0b0010,
        Deleted = 0b0100
    }
}
