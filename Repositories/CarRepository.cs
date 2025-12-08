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
    public class CarRepository
    {
        public List<CarDTO> GetAll()
        {
            string query = "SELECT carId, brand, model, category, available, price FROM Cars";
            DataTable dt = SQLHelper.ExecuteQuery(query);

            var cars = new List<CarDTO>();
            foreach (DataRow row in dt.Rows)
            {
                cars.Add(new CarDTO
                {
                    CarId = (int)row["carId"],
                    Brand = row["brand"].ToString(),
                    Model = row["model"].ToString(),
                    Category = row["category"].ToString(),
                    Available = row["available"].ToString(),
                    Price = (int)row["price"]
                });
            }

            return cars;
        }

        public int Add(CarDTO car)
        {
            string query = @"INSERT INTO Cars (brand, model, category, available, price)
                             VALUES (@Brand, @Model, @Category, @Available, @Price)";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@Brand", car.Brand),
                new SqlParameter("@Model", car.Model),
                new SqlParameter("@Category", car.Category),
                new SqlParameter("@Available", car.Available),
                new SqlParameter("@Price", car.Price));
        }

        public int Update(CarDTO car)
        {
            string query = @"UPDATE Cars 
                             SET brand=@Brand, model=@Model, category=@Category, available=@Available, price=@Price 
                             WHERE carId=@CarId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@CarId", car.CarId),
                new SqlParameter("@Brand", car.Brand),
                new SqlParameter("@Model", car.Model),
                new SqlParameter("@Category", car.Category),
                new SqlParameter("@Available", car.Available),
                new SqlParameter("@Price", car.Price));
        }

        public int Delete(int carId)
        {
            string query = "DELETE FROM Cars WHERE carId=@CarId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@CarId", carId));
        }

        public List<CarDTO> Search(string column, string keyword)
        {
            column = column.ToLower();
            string query = $"SELECT carId, brand, model, category, available, price FROM Cars WHERE {column} = '' OR {column} LIKE '%' + @keyword + '%'";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@keyword", keyword));

            var cars = new List<CarDTO>();
            foreach (DataRow row in dt.Rows)
            {
                cars.Add(new CarDTO
                {
                    CarId = (int)row["carId"],
                    Brand = row["brand"].ToString(),
                    Model = row["model"].ToString(),
                    Category = row["category"].ToString(),
                    Available = row["available"].ToString(),
                    Price = (int)row["price"]
                });
            }

            return cars;
        }
    }
}
