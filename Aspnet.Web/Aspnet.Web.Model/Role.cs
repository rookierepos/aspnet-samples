using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspnet.Web.Common;

namespace Aspnet.Web.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; } = Status.Normal;
        public DateTime CreatedTime { get; set; }
        public DateTime ChangedTime { get; set; }
    }

    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; } = Status.Normal;
        public DateTime CreatedTime { get; set; }
        public DateTime ChangedTime { get; set; }
    }

    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
