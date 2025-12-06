using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CarRentalSystem
{
    public partial class SchedulesFr : Form
    {
        string connectionString = "Data Source=HocPham\\SQLEXPRESS;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlDataReader reader;
        private MainFr mainFr;
        private int role;
        private int totalCarCost = 0;

        public SchedulesFr(MainFr mainFr, int role)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this.role = role;
        }

        private void ResetTextBox() {
            tbCarId.Text = string.Empty;
            tbDateDelay.Text = string.Empty;
            tbFineCost.Text = string.Empty;
            tbName.Text = string.Empty;
            tbStatus.Text = string.Empty;
            tbTotalCost.Text = string.Empty;
            cBCusId2.Text = string.Empty;
            schedulesDGV.DataSource = null;
            schedulesDGV.Rows.Clear();
        }

        private void loadScheduleById(int id)
        {
            try
            {
                string query = "SELECT B.fromDate AS StartDate, B.toDate AS EndDate, C.carId AS CarID, B.Status AS Status, S.fromPlace AS PickupLocation, S.toPlace AS ReturnLocation, B.totalCost AS TotalCarCost, B.BookingId AS BookingID, S.scheduleId AS ScheduleID " +
                    "FROM Bookings AS B INNER JOIN Cars AS C ON B.carId = C.carId INNER JOIN Schedules AS S ON B.bookingId = S.bookingId " +
                    "WHERE B.cusId = @cusId " +
                    "AND B.Status != 'Paymented'";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {   
                        cmd.Parameters.AddWithValue("@CusId", id);
                        adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        schedulesDGV.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillCustomer()
        {
            string query = "Select CusId from Customers";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CusId", typeof(int));
                    dt.Load(reader);
                    cBCusId2.ValueMember = "CusId";
                    cBCusId2.DataSource = dt;
                }
            }
            cBCusId2.SelectedValueChanged += cBCusId_SelectedValueChanged;
        }


        private void fetchCustomer()
        {
            string query = "Select CusName From Customers where CusId = @CusId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CusId", cBCusId2.Text);
                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        tbName.Text = row["CusName"].ToString();
                    }
                }
            }
            
        }

        private void bookingDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    DataGridViewCell cell = schedulesDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cell.Style.SelectionBackColor = Color.Red;
                }
                if (e.RowIndex >= 0)
                {
                    dtpFromdate.Value = Convert.ToDateTime(schedulesDGV.Rows[e.RowIndex].Cells[0].Value);
                    dtpToDate.Value = Convert.ToDateTime(schedulesDGV.Rows[e.RowIndex].Cells[1].Value);
                    dtpEndDate.Text = schedulesDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                    totalCarCost = Convert.ToInt32(schedulesDGV.Rows[e.RowIndex].Cells[6].Value.ToString());
                    tbBookingID.Text = schedulesDGV.Rows[e.RowIndex].Cells[7].Value.ToString();
                    tbScheduleID.Text = schedulesDGV.Rows[e.RowIndex].Cells[8].Value.ToString();
                }
            } catch {
                MessageBox.Show("Please selected again");
            }
       
        }

        private void lbExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SchedulesFr_Load(object sender, EventArgs e)
        {
            tbDateDelay.ReadOnly = true;
            tbTotalCost.ReadOnly = true;
            tbFineCost.ReadOnly = true;
            cBCusId2.SelectedValueChanged -= cBCusId_SelectedValueChanged;
            tbScheduleID.Hide();
            tbBookingID.Hide();
            dtpFromdate.Hide();
            dtpToDate.Hide();
            dtpEndDate.Enabled = false;
            fillCustomer();
        }

        private void cBCusId_SelectedValueChanged(object sender, EventArgs e)
        {
            fetchCustomer();
            if (int.TryParse(cBCusId2.Text, out int id))
            {
                fetchCustomer();
                loadScheduleById(id);
            }
            else
            {
                MessageBox.Show("Invalid customer ID. Please enter a valid numeric value.");
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(@connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = "Update Bookings SET status = @status,totalCost = @totalCost Where bookingId = @bookingId";
                        cmd.Parameters.AddWithValue("@bookingId", tbBookingID.Text);
                        cmd.Parameters.AddWithValue("@totalCost", tbTotalCost.Text);
                        cmd.Parameters.AddWithValue("@status", "Paymented");


                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0) 
                        {
                            cmd.CommandText = "Update Schedules SET dateDelay = @dateDelay, dateReturn = @dateReturn, fineCost = @fineCost, totalCost = @totalCost Where scheduleId = @scheduleId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@scheduleId", tbScheduleID.Text);
                            cmd.Parameters.AddWithValue("@dateDelay", tbDateDelay.Text);
                            cmd.Parameters.AddWithValue("@dateReturn", dtpReturnDate.Value.ToString());
                            cmd.Parameters.AddWithValue("@fineCost", tbFineCost.Text);
                            cmd.Parameters.AddWithValue("@totalCost", tbTotalCost.Text);

                            int rowsEffected = cmd.ExecuteNonQuery();
                            if (rowsEffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Successfully");
                                ResetTextBox();
                            }
                            else
                            {
                                transaction.Rollback();
                                MessageBox.Show("Failed to update Schedule!!!");
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Failed to update Booking!!!");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            switch (role)
            {
                case 0:
                    this.Hide();
                    mainFr.Show();
                    mainFr.ShowAdminFeatures();
                    break;
                case 1:
                    this.Hide();
                    mainFr.Show();
                    mainFr.ShowEmployeeFeatures();
                    break;
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if(Utils.Utils.IsCarAvailableForBooking(connectionString,dtpFromdate.Value.Date, dtpToDate.Value.Date , tbCarId.Text))
            {
                tbStatus.Text = "Available";
            } else
            {
                tbStatus.Text = "In Rental";
            }
        }

        private void dtpReturnDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime dtpRetrnCheck = dtpReturnDate.Value.Date;
            if(dtpRetrnCheck < DateTime.Now.Date)
            {
                MessageBox.Show("Please check Date Return!!!");
                dtpReturnDate.Value = DateTime.Now;
                tbTotalCost.Text = "";
            } else
            {
                DateTime d2 = dtpReturnDate.Value.Date;
                DateTime d1 = dtpEndDate.Value.Date;
                TimeSpan ts = d2 - d1;
                int delayDays = (int)ts.TotalDays;
                if (delayDays <= 0)
                {
                    tbDateDelay.Text = "No Delay";
                    tbFineCost.Text = "0";
                }
                else
                {
                    tbDateDelay.Text = delayDays.ToString();
                    tbFineCost.Text = "" + delayDays * 250;
                }
                tbTotalCost.Text = totalCarCost + Convert.ToInt32(tbFineCost.Text) + "";
            }
        }
    }
}
