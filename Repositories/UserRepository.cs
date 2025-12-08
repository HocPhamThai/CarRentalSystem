using CarRentalSystem.DTOs;
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
    public class UserRepository
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

        public List<UserDTO> GetAll()
        {
            string query = "SELECT userId, username, userpassword, role FROM Users";
            DataTable dt = SQLHelper.ExecuteQuery(query);

            var users = new List<UserDTO>();
            foreach (DataRow row in dt.Rows)
            {
                users.Add(new UserDTO
                {
                    UserId = (int)row["userId"],
                    Username = row["username"].ToString(),
                    UserPassword = row["userpassword"].ToString(),
                    Role = (int)row["role"]
                });
            }

            return users;
        }

        public int Add(UserDTO user)
        {
            string query = @"INSERT INTO Users (username, userpassword, role)
                             VALUES (@Username, @UserPassword, @Role)";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@UserPassword", user.UserPassword),
                new SqlParameter("@Role", user.Role));
        }

        public int Update(UserDTO user)
        {
            string query = @"UPDATE Users 
                             SET username=@Username, userpassword=@UserPassword, role=@Role 
                             WHERE userId=@UserId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@UserPassword", user.UserPassword),
                new SqlParameter("@Role", user.Role));
        }

        public int Delete(int userId)
        {
            string query = "DELETE FROM Users WHERE userId=@UserId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@UserId", userId));
        }

        public List<UserDTO> Search(string column, string keyword)
        {
            column = column.ToLower();
            string query = $"SELECT userId, username, userpassword, role FROM Users WHERE {column} = '' OR {column} LIKE '%' + @keyword + '%'";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@keyword", keyword));

            var users = new List<UserDTO>();
            foreach (DataRow row in dt.Rows)
            {
                users.Add(new UserDTO
                {
                    UserId = (int)row["userId"],
                    Username = row["username"].ToString(),
                    UserPassword = row["userpassword"].ToString(),
                    Role = (int)row["role"]
                });
            }

            return users;
        }
    }
}
