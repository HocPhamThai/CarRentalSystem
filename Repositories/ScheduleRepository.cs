using CarRentalSystem.DTOs;
using CarRentalSystem.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CarRentalSystem.Repositories
{
    public class ScheduleRepository
    {
        public List<ScheduleViewDTO> GetSchedulesByCustomerId(int customerId)
        {
            string query = @"SELECT B.fromDate AS StartDate, B.toDate AS EndDate, 
                           C.carId AS CarID, B.Status AS Status, 
                           S.fromPlace AS PickupLocation, S.toPlace AS ReturnLocation, 
                           B.totalCost AS TotalCarCost, B.BookingId AS BookingID, 
                           S.scheduleId AS ScheduleID 
                           FROM Bookings AS B 
                           INNER JOIN Cars AS C ON B.carId = C.carId 
                           INNER JOIN Schedules AS S ON B.bookingId = S.bookingId 
                           WHERE B.cusId = @cusId 
                           AND B.Status != 'Paymented'";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@cusId", customerId));

            var schedules = new List<ScheduleViewDTO>();
            foreach (DataRow row in dt.Rows)
            {
                schedules.Add(new ScheduleViewDTO
                {
                    StartDate = (DateTime)row["StartDate"],
                    EndDate = (DateTime)row["EndDate"],
                    CarID = (int)row["CarID"],
                    Status = row["Status"].ToString(),
                    PickupLocation = row["PickupLocation"].ToString(),
                    ReturnLocation = row["ReturnLocation"].ToString(),
                    TotalCarCost = (int)row["TotalCarCost"],
                    BookingID = (int)row["BookingID"],
                    ScheduleID = (int)row["ScheduleID"]
                });
            }

            return schedules;
        }

        public ScheduleDTO GetById(int scheduleId)
        {
            string query = "SELECT * FROM Schedules WHERE scheduleId = @scheduleId";

            DataTable dt = SQLHelper.ExecuteQuery(query,
                new SqlParameter("@scheduleId", scheduleId));

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];
            return new ScheduleDTO
            {
                ScheduleId = (int)row["scheduleId"],
                FromPlace = row["fromPlace"].ToString(),
                ToPlace = row["toPlace"].ToString(),
                DateDelay = row["dateDelay"] == DBNull.Value ? null : row["dateDelay"].ToString(),
                DateReturn = row["dateReturn"] == DBNull.Value ? null : row["dateReturn"].ToString(),
                FineCost = row["fineCost"] == DBNull.Value ? (int?)null : (int)row["fineCost"],
                TotalCost = row["totalCost"] == DBNull.Value ? (int?)null : (int)row["totalCost"],
                BookingId = (int)row["bookingId"],
                CarId = (int)row["carId"]
            };
        }

        public int Update(ScheduleDTO schedule)
        {
            string query = @"UPDATE Schedules 
                           SET dateDelay = @dateDelay, dateReturn = @dateReturn, 
                               fineCost = @fineCost, totalCost = @totalCost 
                           WHERE scheduleId = @scheduleId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@scheduleId", schedule.ScheduleId),
                new SqlParameter("@dateDelay", schedule.DateDelay ?? (object)DBNull.Value),
                new SqlParameter("@dateReturn", schedule.DateReturn ?? (object)DBNull.Value),
                new SqlParameter("@fineCost", schedule.FineCost ?? (object)DBNull.Value),
                new SqlParameter("@totalCost", schedule.TotalCost ?? (object)DBNull.Value));
        }

        public int CompleteScheduleWithPayment(int scheduleId, int bookingId, ScheduleDTO schedule, int totalCost)
        {
            using (SqlConnection conn = new SqlConnection(AppConfigHelper.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Update Booking status and total cost
                    string bookingQuery = @"UPDATE Bookings 
                                          SET status = @status, totalCost = @totalCost 
                                          WHERE bookingId = @bookingId";

                    using (SqlCommand cmd = new SqlCommand(bookingQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@bookingId", bookingId);
                        cmd.Parameters.AddWithValue("@totalCost", totalCost);
                        cmd.Parameters.AddWithValue("@status", "Paymented");

                        int bookingRowsAffected = cmd.ExecuteNonQuery();

                        if (bookingRowsAffected <= 0)
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }

                    // Update Schedule
                    string scheduleQuery = @"UPDATE Schedules 
                                           SET dateDelay = @dateDelay, dateReturn = @dateReturn, 
                                               fineCost = @fineCost, totalCost = @totalCost 
                                           WHERE scheduleId = @scheduleId";

                    using (SqlCommand cmd = new SqlCommand(scheduleQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
                        cmd.Parameters.AddWithValue("@dateDelay", schedule.DateDelay ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dateReturn", schedule.DateReturn ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fineCost", schedule.FineCost ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@totalCost", schedule.TotalCost ?? (object)DBNull.Value);

                        int scheduleRowsAffected = cmd.ExecuteNonQuery();

                        if (scheduleRowsAffected <= 0)
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }

                    transaction.Commit();
                    return 1;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int Delete(int scheduleId)
        {
            string query = "DELETE FROM Schedules WHERE scheduleId = @scheduleId";

            return SQLHelper.ExecuteNonQuery(query,
                new SqlParameter("@scheduleId", scheduleId));
        }
    }
}