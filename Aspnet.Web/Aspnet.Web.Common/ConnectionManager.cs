using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Aspnet.Web.Common
{
    public class ConnectionManager
    {
        private static readonly string connectionKey = "connection";
        private static string _connectionString;

        public static void InitConnectionString(string connection) => _connectionString = connection;

        public static IDbConnection GetConnection()
        {
            var connection = HttpContext.Current.Cache[connectionKey] as IDbConnection;
            if (connection == null)
            {
                connection = new MySqlConnection(_connectionString ?? throw new InvalidOperationException(nameof(_connectionString)));
                HttpContext.Current.Cache.Insert(connectionKey, connection);
            }

            return connection;
        }

        public static void ConnectionDispose()
        {
            var connection = HttpContext.Current.Cache[connectionKey] as IDbConnection;
            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
        }
    }
}
