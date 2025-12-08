using CarRentalSystem.Helper;
using System;
using System.Collections.Generic;
using System.Data;
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
            Admin = 1,
            Employee = 2
        }

        public static bool IsCarAvailableForBooking(string connectionString, DateTime fromDate, DateTime toDate, string carId)
        {
            string query = "SELECT COUNT(*) FROM Cars WHERE available = 'YES' " +
                            "AND @carId NOT IN (SELECT C.carId FROM Cars C " +
                            "INNER JOIN Bookings B ON C.carId = B.carId " +
                            "WHERE (B.fromDate <= @toDate AND B.toDate >= @fromDate) OR (B.fromDate <= GETDATE() AND B.toDate >= GETDATE())" +
                            "AND B.status = 'In Rental')";

            int count = (int)SQLHelper.ExecuteScalar(query, 
                new SqlParameter("@carId", Convert.ToInt32(carId)),
                new SqlParameter("@fromDate", new SqlDateTime(fromDate.Year, fromDate.Month, fromDate.Day)),
                new SqlParameter("@toDate", new SqlDateTime(toDate.Year, toDate.Month, toDate.Day))
            );

            return count > 0;
        }


        public static bool IsCarAvailableForBooking(int carId)
        {
            string query = "SELECT COUNT(*) FROM Cars WHERE available = 'YES' " +
                            "AND @carId NOT IN (SELECT C.carId FROM Cars C " +
                            "INNER JOIN Bookings B ON C.carId = B.carId " +
                            "WHERE (B.fromDate <= GETDATE() AND B.toDate >= GETDATE())" +
                            "AND B.status = 'In Rental')";
            int count = (int)SQLHelper.ExecuteScalar(query,
                new SqlParameter("@carId", carId)
            );

            return count > 0;
        }

        public static DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dt = new DataTable();

            // Get all public properties
            var properties = typeof(T).GetProperties();

            // Add columns based on properties
            foreach (var prop in properties)
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Add rows
            foreach (var item in items)
            {
                var row = dt.NewRow();
                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
