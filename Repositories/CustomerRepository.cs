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
    public class CustomerRepository
    {
        public List<CustomerDTO> GetAll()
        {
            string query = @"SELECT cusId, cusName, cusAdd, phone FROM Customers";
            DataTable dt = SQLHelper.ExecuteQuery(query);

            List<CustomerDTO> customers = new List<CustomerDTO>();
            foreach (DataRow row in dt.Rows)
            {
                customers.Add(new CustomerDTO
                {
                    CusId = Convert.ToInt32(row["cusId"]),
                    CusName = row["cusName"].ToString(),
                    CusAdd = row["cusAdd"].ToString(),
                    Phone = row["phone"].ToString()
                });
            }

            return customers;
        }

        public int Add(CustomerDTO customer)
        {
            string query = @"INSERT INTO Customers(cusName, cusAdd, phone) VALUES (@cusName, @cusAdd, @phone)";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@cusName", customer.CusName),
                new SqlParameter("@cusAdd", customer.CusAdd),
                new SqlParameter("@phone", customer.Phone)
            );
        }

        public int Update(CustomerDTO customer)
        {
            string query = @"UPDATE Customers SET cusName = @cusName, cusAdd = @cusAdd, phone = @phone WHERE cusId = @cusId";
            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@cusName", customer.CusName),
                new SqlParameter("@cusAdd", customer.CusAdd),
                new SqlParameter("@phone", customer.Phone),
                new SqlParameter("@cusId", customer.CusId)
            );
        }

        public int Delete(int customerID)
        {
            string query = @"DELETE FROM Customers WHERE cusId = @cusId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@cusId", customerID)
            );
        }

        public List<CustomerDTO> Search(string column, string keyword)
        {
            string query =
                $"SELECT cusId, cusName, cusAdd, phone FROM Customers WHERE {column} LIKE '%' + @keyword + '%'";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@keyword", keyword));

            var list = new List<CustomerDTO>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CustomerDTO
                {
                    CusId = (int)row["cusId"],
                    CusName = row["cusName"].ToString(),
                    CusAdd = row["cusAdd"].ToString(),
                    Phone = row["phone"].ToString()
                });
            }

            return list;
        }
    }
}
