using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Helper
{
    namespace CarRentalSystem.Helper
    {
        public static class DbExecutor
        {
            public static T Execute<T>(string connectionString, Func<SqlConnection, T> func)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return func(conn);
                }
            }
        }
    }

}
