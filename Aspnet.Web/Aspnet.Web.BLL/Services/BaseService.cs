using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspnet.Web.Common;

namespace Aspnet.Web.BLL.Services
{
    public class BaseService
    {
        protected IDbConnection _connection;

        public BaseService()
        {
            _connection = ConnectionManager.GetConnection();
        }
    }
}
