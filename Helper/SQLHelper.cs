using System.Data;
using System.Data.SqlClient;

namespace CarRentalSystem.Helper
{
    public static class SQLHelper
    {
        private static SqlConnection CreateConnection() => new SqlConnection(AppConfigHelper.ConnectionString);

        // -------------------------------
        // SELECT trả về DataTable
        // -------------------------------
        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // -------------------------------
        // SELECT trả về một giá trị duy nhất
        // -------------------------------
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        // -------------------------------
        // INSERT / UPDATE / DELETE
        // Trả về số dòng bị ảnh hưởng
        // -------------------------------
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
