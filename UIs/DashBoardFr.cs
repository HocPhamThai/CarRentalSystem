using CarRentalSystem.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static CarRentalSystem.Utils.Utils;

namespace CarRentalSystem
{
    public partial class DashBoardFr : Form
    {
        private readonly MainFr _mainFr;
        private readonly Role _role;

        public DashBoardFr(MainFr mainFr, Role role)
        {
            InitializeComponent();
            _mainFr = mainFr;
            _role = role;
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DashBoardFr_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDashboardData()
        {
            LoadBookingStatistics();
            LoadCategoryChart();
            LoadRevenueChart();
        }

        private void LoadBookingStatistics()
        {
            // Brands
            string queryBrands = @"
                SELECT c.brand AS Brand,
                       COUNT(b.bookingId) AS TotalBookings,
                       SUM(b.totalCost) AS TotalRevenue 
                FROM Bookings b 
                INNER JOIN Cars c ON b.carId = c.carId
                WHERE status = 'Paymented'
                GROUP BY c.brand 
                ORDER BY TotalRevenue DESC";
            LoadDataGridView(queryBrands, dgvBrands);

            // Models
            string queryModels = @"
                SELECT c.model AS Model,
                       COUNT(b.bookingId) AS TotalBookings,
                       SUM(b.totalCost) AS TotalRevenue 
                FROM Bookings b
                INNER JOIN Cars c ON b.carId = c.carId 
                WHERE status = 'Paymented'
                GROUP BY c.model 
                ORDER BY TotalRevenue DESC";
            LoadDataGridView(queryModels, dgvModels);

            // Times
            string queryTimes = @"
                SELECT b.fromDate AS BookingDate,  
                       COUNT(b.bookingId) AS TotalBookings,
                       SUM(b.totalCost) AS TotalRevenue 
                FROM Bookings b 
                INNER JOIN Cars c ON b.carId = c.carId
                WHERE status = 'Paymented'
                GROUP BY b.fromDate
                ORDER BY BookingDate DESC";
            LoadDataGridView(queryTimes, dgvTimes);
        }

        private void LoadCategoryChart()
        {
            string categoryQuery = @"
                SELECT category, COUNT(*) AS count
                FROM Cars
                GROUP BY category";

            DataTable dt = ExecuteQuery(categoryQuery);

            chartCategory.DataSource = dt;
            chartCategory.Series["Amount"].XValueMember = "category";
            chartCategory.Series["Amount"].YValueMembers = "count";
            
            if (chartCategory.Titles.Count == 0)
            {
                chartCategory.Titles.Add("Category Car Chart");
            }
        }

        private void LoadRevenueChart()
        {
            string revenueQuery = @"
                SELECT 
                    DATEPART(MONTH, fromDate) AS Month, 
                    DATEPART(YEAR, fromDate) AS Year, 
                    SUM(totalCost) AS TotalRevenue
                FROM Bookings
                WHERE status = 'Paymented'
                GROUP BY DATEPART(YEAR, fromDate), DATEPART(MONTH, fromDate)
                ORDER BY Year, Month";

            DataTable dt = ExecuteQuery(revenueQuery);

            chartMonthYear.DataSource = dt;
            chartMonthYear.Series["Month"].XValueMember = "Month";
            chartMonthYear.Series["Month"].YValueMembers = "TotalRevenue";
            
            if (chartMonthYear.Titles.Count == 0)
            {
                chartMonthYear.Titles.Add("Revenue Chart by Month/Year");
            }
        }

        private void LoadDataGridView(string query, DataGridView dgv)
        {
            try
            {
                DataTable dt = ExecuteQuery(query);
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(AppConfigHelper.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            _mainFr.RefreshRoleUI();
            _mainFr.Show();
        }
    }
}
