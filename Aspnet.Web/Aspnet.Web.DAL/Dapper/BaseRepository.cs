using System;
using System.Data;

namespace Aspnet.Web.DAL.Dapper
{
    public class BaseRepository
    {
        protected IDbConnection _connection;

        public BaseRepository(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
    }
}
