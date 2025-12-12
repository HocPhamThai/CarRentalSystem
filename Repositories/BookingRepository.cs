using CarRentalSystem.DTOs;
using CarRentalSystem.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CarRentalSystem.Repositories
{
    public class BookingRepository
    {
        public List<BookingDTO> GetAll()
        {
            string query = @"SELECT bookingId, carId, B.CusId, CusName, fromDate, toDate, 
                           status, description, totalCost 
                           FROM Bookings B 
                           INNER JOIN Customers Cus ON B.CusId = Cus.CusId";

            DataTable dt = SQLHelper.ExecuteQuery(query);

            var bookings = new List<BookingDTO>();
            foreach (DataRow row in dt.Rows)
            {
                bookings.Add(new BookingDTO
                {
                    BookingId = Convert.ToInt32(row["bookingId"]),
                    CarId = Convert.ToInt32(row["carId"]),
                    CusId = Convert.ToInt32(row["CusId"]),
                    CusName = row["CusName"].ToString(),
                    FromDate = ((DateTime)row["fromDate"]).ToString(),
                    ToDate = ((DateTime)row["toDate"]).ToString(),
                    Status = row["status"].ToString(),
                    Description = row["description"] == DBNull.Value ? string.Empty : row["description"].ToString(),
                    TotalCost = row["totalCost"] == DBNull.Value ? 0 : Convert.ToInt32(row["totalCost"])
                });
            }

            return bookings;
        }

        public BookingDTO GetById(int bookingId)
        {
            string query = "SELECT * FROM Bookings WHERE bookingId = @bookingId";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@bookingId", bookingId));

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];
            return new BookingDTO
            {
                BookingId = Convert.ToInt32(row["bookingId"]),
                CarId = Convert.ToInt32(row["carId"]),
                CusId = Convert.ToInt32(row["CusId"]),
                FromDate = ((DateTime)row["fromDate"]).ToString(),
                ToDate = ((DateTime)row["toDate"]).ToString(),
                Status = row["status"].ToString(),
                Description = row["description"] == DBNull.Value ? string.Empty : row["description"].ToString(),
                TotalCost = row["totalCost"] == DBNull.Value ? 0 : Convert.ToInt32(row["totalCost"])
            };
        }

        public int Add(BookingDTO booking, ScheduleDTO schedule)
        {
            using (SqlConnection conn = new SqlConnection(AppConfigHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    int bookingId;

                    // Insert booking
                    string bookingQuery = @"INSERT INTO Bookings(fromDate, toDate, status, cusId, carId, description, totalCost) 
                                          OUTPUT INSERTED.bookingId 
                                          VALUES (@fromDate, @toDate, @status, @cusId, @carId, @description, @totalCost)";

                    using (SqlCommand cmd = new SqlCommand(bookingQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@fromDate", booking.FromDate);
                        cmd.Parameters.AddWithValue("@toDate", booking.ToDate);
                        cmd.Parameters.AddWithValue("@status", booking.Status);
                        cmd.Parameters.AddWithValue("@cusId", booking.CusId);
                        cmd.Parameters.AddWithValue("@carId", booking.CarId);
                        cmd.Parameters.AddWithValue("@description", booking.Description ?? string.Empty);
                        cmd.Parameters.AddWithValue("@totalCost", booking.TotalCost);

                        bookingId = (int)cmd.ExecuteScalar();
                    }

                    // Insert schedule
                    string scheduleQuery = @"INSERT INTO Schedules(fromPlace, toPlace, dateDelay, dateReturn, 
                                           fineCost, totalCost, bookingId, carId) 
                                           VALUES (@fromPlace, @toPlace, @dateDelay, @dateReturn, 
                                           @fineCost, @totalCost, @bookingId, @carId)";

                    using (SqlCommand cmd = new SqlCommand(scheduleQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@fromPlace", schedule.FromPlace);
                        cmd.Parameters.AddWithValue("@toPlace", schedule.ToPlace);
                        cmd.Parameters.AddWithValue("@dateDelay", DBNull.Value);
                        cmd.Parameters.AddWithValue("@dateReturn", DBNull.Value);
                        cmd.Parameters.AddWithValue("@fineCost", DBNull.Value);
                        cmd.Parameters.AddWithValue("@totalCost", DBNull.Value);
                        cmd.Parameters.AddWithValue("@carId", schedule.CarId);
                        cmd.Parameters.AddWithValue("@bookingId", bookingId);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return bookingId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int Update(BookingDTO booking)
        {
            string query = @"UPDATE Bookings 
                           SET fromDate=@fromDate, toDate=@toDate, status=@status, 
                               cusId=@cusId, carId=@carId, description=@description, totalCost=@totalCost 
                           WHERE bookingId=@bookingId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@bookingId", booking.BookingId),
                new SqlParameter("@fromDate", booking.FromDate),
                new SqlParameter("@toDate", booking.ToDate),
                new SqlParameter("@status", booking.Status),
                new SqlParameter("@cusId", booking.CusId),
                new SqlParameter("@carId", booking.CarId),
                new SqlParameter("@description", booking.Description ?? string.Empty),
                new SqlParameter("@totalCost", booking.TotalCost));
        }

        public int Delete(int bookingId)
        {
            string query = "DELETE FROM Bookings WHERE bookingId=@bookingId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@bookingId", bookingId));
        }

        public List<BookingDTO> Search(string column, string keyword)
        {
            column = column.ToLower();
            string query = $@"SELECT bookingId, carId, B.CusId, CusName, fromDate, toDate, 
                            status, description, totalCost 
                            FROM Bookings B 
                            INNER JOIN Customers Cus ON B.CusId = Cus.CusId
                            WHERE {column} = '' OR {column} LIKE '%' + @keyword + '%'";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@keyword", keyword));

            var bookings = new List<BookingDTO>();
            foreach (DataRow row in dt.Rows)
            {
                bookings.Add(new BookingDTO
                {
                    BookingId = (int)row["bookingId"],
                    CarId = (int)row["carId"],
                    CusId = (int)row["CusId"],
                    CusName = row["CusName"].ToString(),
                    FromDate = ((DateTime)row["fromDate"]).ToString(),
                    ToDate = ((DateTime)row["toDate"]).ToString(),
                    Status = row["status"].ToString(),
                    Description = row["description"] == DBNull.Value ? string.Empty : row["description"].ToString(),
                    TotalCost = (int)row["totalCost"]
                });
            }

            return bookings;
        }

        public int UpdateStatus(int bookingId, string status, int totalCost)
        {
            string query = @"UPDATE Bookings 
                           SET status = @status, totalCost = @totalCost 
                           WHERE bookingId = @bookingId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@bookingId", bookingId),
                new SqlParameter("@status", status),
                new SqlParameter("@totalCost", totalCost));
        }
    }
}
