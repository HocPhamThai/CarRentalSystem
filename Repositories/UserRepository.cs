using CarRentalSystem.DTOs;
using CarRentalSystem.Helper.CarRentalSystem.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Repositories
{
    public interface IUserRepository
    {
        LoginResultDto GetUser(string username, string password);
    }

    public class UserRepository : IUserRepository
    {
        public LoginResultDto GetUser(string username, string password)
        {
            string sql = "SELECT UserId, Username, UserPassword, Role FROM Users WHERE Username = @Username AND UserPassword = @Password";
            
            DataTable dt = SQLHelper.ExecuteQuery(sql,
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            );

            if (dt.Rows.Count == 0)
            {
                return new LoginResultDto
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password."
                };
            }

            return new LoginResultDto
            {
                Success = true,
                UserId = Convert.ToInt32(dt.Rows[0]["userId"]),
                Role = Convert.ToInt32(dt.Rows[0]["role"])
            };
        }
    }
}
