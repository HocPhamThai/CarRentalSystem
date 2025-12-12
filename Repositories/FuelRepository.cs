using CarRentalSystem.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Repositories
{
    public class FuelRepository
    {
        public int GetFuelPrice(string fuelName)
        {
            string query = "SELECT fuelPrice FROM Fuels WHERE fuelName = @fuelName";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@fuelName", fuelName));

            if (dt.Rows.Count == 0)
                return 0;

            return (int)dt.Rows[0]["fuelPrice"];
        }
    }
}
