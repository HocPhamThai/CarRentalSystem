using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalSystem.Utils
{
    public static class Utils
    {
        public enum Role
        {
            Admin = 0,
            Employee = 1
        }

        public static bool IsCarAvailableForBooking(string connectionString, DateTime fromDate, DateTime toDate, string carId)
        {
            using (SqlConnection connection = new SqlConnection(@connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Cars WHERE available = 'YES' " +
                                "AND @carId NOT IN (SELECT C.carId FROM Cars C " +
                                "INNER JOIN Bookings B ON C.carId = B.carId " +
                                "WHERE (B.fromDate <= @toDate AND B.toDate >= @fromDate) OR (B.fromDate <= GETDATE() AND B.toDate >= GETDATE())" +
                                "AND B.status = 'In Rental')";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@carId", Convert.ToInt32(carId));
                    SqlDateTime from = new SqlDateTime(fromDate.Year, fromDate.Month, fromDate.Day);
                    command.Parameters.AddWithValue("@fromDate", from);
                    SqlDateTime to = new SqlDateTime(fromDate.Year, fromDate.Month, fromDate.Day);
                    command.Parameters.AddWithValue("@toDate", to);

                    int count = (int)command.ExecuteScalar();

                    connection.Close();
                    return count > 0;
                }
            }
        }

        public static bool IsCarAvailableForBooking(int carId)
        {
            string connectionString = CarRentalSystem.Helper.AppConfigHelper.ConnectionString;
            using (SqlConnection connection = new SqlConnection(@connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Cars WHERE available = 'YES' " +
                                "AND @carId NOT IN (SELECT C.carId FROM Cars C " +
                                "INNER JOIN Bookings B ON C.carId = B.carId " +
                                "WHERE (B.fromDate <= GETDATE() AND B.toDate >= GETDATE())" +
                                "AND B.status = 'In Rental')";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@carId", carId);
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0;
                }
            }
        }
    }
}
