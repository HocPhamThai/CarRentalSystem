using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CarRentalSystem
{
    public partial class DashBoardFr : Form
    {
        string connectionString = "Data Source=HocPham\\SQLEXPRESS;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private MainFr mainFr;
        private int role;

        public DashBoardFr(MainFr mainFr, int role)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this.role = role;
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void DashBoardFr_Load(object sender, EventArgs e)
        {
            //Display Count and revenue
            //Brands
            string queryBrands = @"
                SELECT c.brand AS Brand,
                        Count(b.bookingId) AS TotalBookings,
                        Sum(b.totalCost) AS TotalRevenue 
                From Bookings b 
                INNER JOIN Cars c ON b.carId = c.carId
                Where status = 'Paymented'
                Group by c.brand 
                Order by TotalRevenue DESC";
            loadDataGridView(connectionString, queryBrands, dgvBrands);
            //Models
            string queryModels = @"
                Select c.model AS Model,
                        Count(b.bookingId) AS TotalBookings,
                        Sum(b.totalCost) AS TotalRevenue 
                From Bookings b
                INNER JOIN Cars c ON b.carId = c.carId 
                Where status = 'Paymented'
                Group by c.Model 
                Order by TotalRevenue DESC";
            loadDataGridView(connectionString, queryModels, dgvModels);
            //Times
            string queryTimes = @"
                SELECT b.fromDate AS BookingDate,  
                        Count(b.bookingId) AS TotalBookings,
                        Sum(b.totalCost) AS TotalRevenue 
                From Bookings b 
                INNER JOIN Cars c ON b.carId = c.carId
                Where status = 'Paymented'
                Group by b.fromDate
                Order by BookingDate DESC";
            loadDataGridView(connectionString, queryTimes, dgvTimes);

            //Chart
            //Category
            string categoryQuery = @"
                SELECT category, COUNT(*) AS count
                FROM Cars
                GROUP BY category
            ";
            using (SqlConnection conn = new SqlConnection(@connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(categoryQuery, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(categoryQuery, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    chartCategory.DataSource = dt;

                    chartCategory.Series["Amount"].XValueMember = "category";  
                    chartCategory.Series["Amount"].YValueMembers = "count";     

                    chartCategory.Titles.Add("Category Car Chart");
                }
            }
            //Revenue
            string revenueQuery = @"
                SELECT 
                    DATEPART(MONTH, fromDate) AS Month, 
                    DATEPART(YEAR, fromDate) AS Year, 
                    SUM(totalCost) AS TotalRevenue
                FROM Bookings
                WHERE status = 'Paymented'
                GROUP BY DATEPART(YEAR, fromDate), DATEPART(MONTH, fromDate)
                ORDER BY Year, Month
            ";

            using (SqlConnection conn = new SqlConnection(@connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(revenueQuery, conn)) 
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd); 
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    chartMonthYear.DataSource = dt;

                    chartMonthYear.Series["Month"].XValueMember = "Month";

                    chartMonthYear.Series["Month"].YValueMembers = "TotalRevenue";

                    chartMonthYear.Titles.Add("Revenue Chart by Month/Year");
                }
            }
        }

        private void loadDataGridView(string connectionString, string query, DataGridView dgv)
        {
            using(SqlConnection conn = new SqlConnection(@connectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgv.DataSource = dt;
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
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
    }
}
