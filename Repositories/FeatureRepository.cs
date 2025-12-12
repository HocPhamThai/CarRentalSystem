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
    public class FeatureRepository
    {
        public int GetFeaturePrice(string featureName)
        {
            string query = "SELECT featurePrice FROM Features WHERE featureName = @featureName";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@featureName", featureName));

            if (dt.Rows.Count == 0)
                return 0;

            return (int)dt.Rows[0]["featurePrice"];
        }
    }
}
