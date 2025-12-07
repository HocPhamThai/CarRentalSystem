using System.Configuration;

namespace CarRentalSystem.Helper
{
    public static class AppConfigHelper
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}
