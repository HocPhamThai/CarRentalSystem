using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CarRentalSystem
{
    public partial class BookingsFr : Form
    {

        string connectionString = "Data Source=HocPham\\SQLEXPRESS;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlDataReader reader;
        private SqlCommandBuilder commandBuilder;
        private Dictionary<string, int> decsFeatureAndFuels = new Dictionary<string, int>();
        private int totalPriceFuelAndFeature = 0;
        private int totalPriceBetweenDate = 0;
        private int totalPrice = 0; // equal priceOFcarPerDate * Date rent (totalPriceBetweenDate) + totalPriceFuelAndFeature;
        private int priceOfCarPerDate = 0;
        private MainFr mainFr;
        private int role;

        public BookingsFr(MainFr main, int roleid)
        {
            InitializeComponent();
            this.mainFr = main;
            this.role = roleid;
        }

        private void resetTextBox()
        {
            tbBookingId.Text = string.Empty;
            cbCarId.SelectedValue = -1;
            tbBrand.Text = string.Empty;
            tbModel.Text = string.Empty;
            tbFee.Text = string.Empty;
            cBCusId.SelectedValue = -1;
            tbName.Text = string.Empty;
            //reset checkbox
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.Checked = false;
                }
                if(control is RadioButton)
                {
                    RadioButton radioButton = (RadioButton)control;
                    radioButton.Checked = false;
                }
            }
            tbFromPlace.Text = string.Empty;
            tbToPlace.Text = string.Empty;
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            tbPrice.Text = string.Empty;
            //tbRentFee.Text = string.Empty;
        }

        private void loadBookings()
        {
            try
            {
                string query = "SELECT bookingId, carId , B.CusId, CusName , fromDate, toDate, status, description, totalCost FROM Bookings B, Customers Cus Where B.CusId = Cus.CusId";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    bookingDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillComboCarId()
        {
            string query = "Select CarId from Cars";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("carId", typeof(int));
                    dt.Load(reader);
                    cbCarId.ValueMember = "carId";
                    cbCarId.DataSource = dt;
                }
            }
        }


        private void fetchCustomer()
        {
            string query = "Select CusName From Customers where CusId = @CusId";
            using (SqlConnection conn = new SqlConnection(connectionString))                                       
            {
                conn.Open();
                using(SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CusId", cBCusId.Text);    
                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach(DataRow row in dt.Rows)
                    {
                        tbName.Text = row["CusName"].ToString();
                    }
                }
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
                    cBCusId.ValueMember = "CusId";
                    cBCusId.DataSource = dt;
                }
            }
        }

        private void Rental_Load(object sender, EventArgs e)
        {

            tbBookingId.Hide();
            fillComboCarId();
            fillCustomer();
            loadBookings();
        }



        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ckbMap_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckbMap.Text, ckbMap.Checked);
            loadTotalPrice();
        }

        private void ckBBluetooth_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBBluetooth.Text, ckBBluetooth.Checked);
            loadTotalPrice();
        }

        private void ckBRearCamera_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBRearCamera.Text, ckBRearCamera.Checked);
            loadTotalPrice();
        }

        private void ckBSideView_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBSideView.Text, ckBSideView.Checked);
            loadTotalPrice();
        }

        private void ckBDashboard_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBDashboard.Text, ckBDashboard.Checked);
            loadTotalPrice();
        }

        private void ckBSpeedAlert_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBSpeedAlert.Text, ckBSpeedAlert.Checked);
            loadTotalPrice();
        }

        private void ckTirePressure_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckTirePressure.Text, ckTirePressure.Checked);
            loadTotalPrice();
        }

        private void ckBCollinsion_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBCollinsion.Text, ckBCollinsion.Checked);
            loadTotalPrice();
        }

        private void ckBSunroof_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBSunroof.Text, ckBSunroof.Checked);
            loadTotalPrice();
        }

        private void ckBGPSNavigation_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBGPSNavigation.Text , ckBGPSNavigation.Checked);
            loadTotalPrice();
        }

        private void ckBUSBPort_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBUSBPort.Text , ckBUSBPort.Checked);
            loadTotalPrice();
        }

        private void ckBAllWheel_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBAllWheel.Text , ckBAllWheel.Checked);
            loadTotalPrice();
        }

        private void ckBPickupBed_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckBPickupBed.Text , ckBPickupBed.Checked);
            loadTotalPrice();
        }

        private void ckB360_CheckedChanged(object sender, EventArgs e)
        {
            loadFeaturePrice(ckB360.Text , ckB360.Checked);
            loadTotalPrice();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // get descripton for Bookings
            string desc = "";
            foreach (var pair in decsFeatureAndFuels)
            {
                desc += $"{pair.Key} - {pair.Value}\n";
            }
            // check radiobutton check or not
            int checkedCount = 0;

            foreach (RadioButton radioButton in this.Controls.OfType<RadioButton>())
            {
                if (radioButton.Checked)
                {
                    checkedCount++;
                }
            }
            if (String.IsNullOrEmpty(cbCarId.Text) || String.IsNullOrEmpty(dtpFromDate.Value.ToString())
           || String.IsNullOrEmpty(dtpToDate.Value.ToString())
           || String.IsNullOrEmpty(cBCusId.Text)
           || String.IsNullOrEmpty(tbPrice.Text)
           || String.IsNullOrEmpty(tbFromPlace.Text)
           || String.IsNullOrEmpty(tbToPlace.Text)  
           || checkedCount != 1 
           )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                DateTime fromDate = dtpFromDate.Value;
                DateTime toDate = dtpToDate.Value;
                if (Utils.Utils.IsCarAvailableForBooking(connectionString, fromDate, toDate, cbCarId.Text))
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(@connectionString))
                        {
                            conn.Open();
                            SqlTransaction transaction = conn.BeginTransaction();
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                int bookingID = -1;

                                // Command 1: Add a Booking
                                cmd.Transaction = transaction;
                                cmd.CommandText = "INSERT INTO Bookings(fromDate, toDate, status, cusId, carId, description, totalCost) " +
                                    "OUTPUT INSERTED.bookingId " +
                                    "VALUES (@fromDate, @toDate, @status, @cusId, @carId, @description, @totalCost)";
                                cmd.Parameters.AddWithValue("@fromDate", dtpFromDate.Value.ToString());
                                cmd.Parameters.AddWithValue("@toDate", dtpToDate.Value.ToString());
                                cmd.Parameters.AddWithValue("@status", "In Rental");
                                cmd.Parameters.AddWithValue("@cusId", cBCusId.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@carId", cbCarId.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@description", desc);
                                cmd.Parameters.AddWithValue("@totalCost", totalPrice);

                                bookingID = (int)cmd.ExecuteScalar();

                                if (bookingID != -1) // Check if bookingID was successfully retrieved
                                {
                                    // Command 2: Add a Schedule
                                    cmd.CommandText = "INSERT INTO Schedules(fromPlace, toPlace, dateDelay, dateReturn, fineCost, totalCost, bookingId, carId) " +
                                        "VALUES (@fromPlace, @toPlace, @dateDelay, @dateReturn, @fineCost, @totalCost, @bookingId, @carId)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@fromPlace", tbFromPlace.Text);
                                    cmd.Parameters.AddWithValue("@toPlace", tbToPlace.Text);
                                    cmd.Parameters.AddWithValue("@dateDelay", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@dateReturn", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@fineCost", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@totalCost", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@carId", cbCarId.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@bookingId", bookingID); // Use the obtained booking ID

                                    int rowsEffected = cmd.ExecuteNonQuery();
                                    if (rowsEffected > 0)
                                    {
                                        transaction.Commit();
                                        MessageBox.Show("Created Successfully");
                                        loadBookings();
                                        //UpdateAvailable();
                                        fillComboCarId();
                                        resetTextBox();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        MessageBox.Show("Failed to create the Schedule!!!");
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("Failed to retrieve BookingID.");
                                }
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }
                } else
                {
                    MessageBox.Show("Car is Rented!!. Please check FromDate and ToDate for this car.");
                }
            }
        }

        private void cbCarId_SelectedValueChanged(object sender, EventArgs e)
        {
            fillTextBoxCarInfo();
        }

        private void fillTextBoxCarInfo()
        {
            string query2 = "Select * From Cars Where CarId = @CarId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query2, conn))
                {
                    cmd.Parameters.AddWithValue("@CarId", cbCarId.Text);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string carBrand = reader["brand"].ToString();
                        string carModel = reader["model"].ToString();
                        string carCategory = reader["category"].ToString();
                        int price = Convert.ToInt32(reader["price"]);
                        tbModel.Text = carModel;
                        tbBrand.Text = carBrand;
                        tbSeat.Text = carCategory;
                        priceOfCarPerDate = price;
                        tbFee.Text = price.ToString();
                    }
                }
            }
        }

        private void cBCusId_SelectedValueChanged(object sender, EventArgs e)
        {
            fetchCustomer();
        }

        private void loadFeaturePrice(string featureName, bool isChecked)
        {
            string query = "Select featurePrice from Features where featureName = '"+featureName+"'";
            using (SqlConnection conn = new SqlConnection(@connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //cmd.Parameters.AddWithValue("@featureName", featureName);
                    SqlDataReader reader = cmd.ExecuteReader(); 
                    while(reader.Read())
                    {
                        int featurePrice = reader.GetInt32(reader.GetOrdinal("featurePrice")); ;
                        if(isChecked)
                        {
                            totalPriceFuelAndFeature += featurePrice;
                            decsFeatureAndFuels[featureName] = featurePrice;
                        } else
                        {
                            totalPriceFuelAndFeature -= featurePrice; 
                            decsFeatureAndFuels.Remove(featureName);
                        }
                    }
                }
            }
        }

        private void loadFuelPrice(string fuelName, bool isChecked)
        {
            string query = "Select fuelPrice from Fuels where fuelName = '" + fuelName + "'";
            using (SqlConnection conn = new SqlConnection(@connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //cmd.Parameters.AddWithValue("@featureName", featureName);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int fuelPrice = reader.GetInt32(reader.GetOrdinal("fuelPrice")); ;
                        if (isChecked)
                        {
                            totalPriceFuelAndFeature += fuelPrice;
                            decsFeatureAndFuels[fuelName] = fuelPrice;
                        }
                        else
                        {
                            totalPriceFuelAndFeature -= fuelPrice;
                            decsFeatureAndFuels.Remove(fuelName);
                        }
                    }
                }
            }
            
        }

        private void rBAll_CheckedChanged(object sender, EventArgs e)
        {
            loadFuelPrice(rBAll.Text, rBAll.Checked);
            loadTotalPrice();
        }

        private void rBGas_CheckedChanged(object sender, EventArgs e)
        {
            loadFuelPrice(rBGas.Text, rBGas.Checked);
            loadTotalPrice();
        }

        private void rBDiesel_CheckedChanged(object sender, EventArgs e)
        {
            loadFuelPrice(rBDiesel.Text, rBDiesel.Checked);
            loadTotalPrice();
        }

        private void rBElectric_CheckedChanged(object sender, EventArgs e)
        {
            loadFuelPrice(rBElectric.Text, rBElectric.Checked);
            loadTotalPrice();
        }

        private void bookingDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    DataGridViewCell cell = bookingDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cell.Style.SelectionBackColor = Color.Red;
                }
                if (e.RowIndex >= 0)
                {
                    DisplayBookingDetails(Convert.ToInt32(bookingDGV.Rows[e.RowIndex].Cells[0].Value.ToString()));
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Pleae selected agains");
            }
        
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dtpFromDate.Value.Date;
            DateTime currentDate = DateTime.Now.Date;

            if (selectedDate < currentDate)
            {
                dtpFromDate.ValueChanged -= dtpFromDate_ValueChanged; // Temporarily detach the event handler
                dtpFromDate.Value = currentDate;
                dtpFromDate.ValueChanged += dtpFromDate_ValueChanged; // Reattach the event handler
                MessageBox.Show("The FromDate must be from now time!!");
            }

            loadTotalPerDate();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            loadTotalPerDate();
        }
        private void loadTotalPerDate()
        {
            DateTime d1 = dtpFromDate.Value.Date;
            DateTime d2 = dtpToDate.Value.Date;
            if(d1 <= d2)
            {
                TimeSpan ts = d2 - d1;

                int dayDiff = ts.Days;
                totalPriceBetweenDate = (dayDiff + 1) * priceOfCarPerDate;
            } else
            {
                dtpFromDate.ValueChanged -= dtpFromDate_ValueChanged;
                dtpToDate.ValueChanged -= dtpToDate_ValueChanged;
                dtpFromDate.Value = DateTime.Now; dtpToDate.Value = DateTime.Now;
                dtpFromDate.ValueChanged += dtpFromDate_ValueChanged;
                dtpToDate.ValueChanged += dtpToDate_ValueChanged;
                MessageBox.Show("From date must before or equal to 'To date'.");
                tbPrice.Text = "0";
            }
            loadTotalPrice();
        }

        private void loadTotalPrice()
        {
            totalPrice = totalPriceFuelAndFeature + totalPriceBetweenDate;
            tbPrice.Text = totalPrice.ToString();
        }

        //booking details
        private void DisplayBookingDetails(int bookingId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Bookings WHERE bookingId = @bookingId", connection))
                {
                    cmd.Parameters.AddWithValue("@bookingId", bookingId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime fromDate = (DateTime)reader["fromDate"];
                            DateTime toDate = (DateTime)reader["toDate"];
                            TimeSpan duration = toDate - fromDate;
                            int days = duration.Days;
                            string bookingDetails =
                                $"Booking ID: {bookingId}\n" +
                                $"From Date: {reader["fromDate"]}\n" +
                                $"To Date: {reader["toDate"]}\n" +
                                $"Status: {reader["status"]}\n" +
                                $"Customer Name: {GetCustomerName(Convert.ToInt32(reader["cusId"]))}\n" +
                                $"Car Details: \n{GetCarDetails(Convert.ToInt32(reader["carId"]))} ({days + 1} Days)\n" +
                                $"{reader["description"]}\n" +
                                $"Total Cost: {reader["totalCost"]}$";

                            MessageBox.Show(bookingDetails);
                        }
                        else
                        {
                            MessageBox.Show("Booking not found.");
                        }
                    }
                }
            }
        }

        private string GetCustomerName(int customerId)
        {
            string query = "Select CusName From Customers where CusId = @CusId";
            string result = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CusId", customerId);
                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        result = row["CusName"].ToString();
                    }
                }
            }
            return result;
        }

        private string GetCarDetails(int carId)
        {
            string query2 = "Select * From Cars Where CarId = @CarId";
            string result = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query2, conn))
                {
                    cmd.Parameters.AddWithValue("@CarId", carId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string carBrand = reader["brand"].ToString();
                        string carModel = reader["model"].ToString();
                        string carCategory = reader["category"].ToString();
                        int price = Convert.ToInt32(reader["price"]);
                        result += carModel + " ";
                        result += carBrand + " ";
                        result += carCategory + " ";
                        result += price + "$/ Day";
                    }
                }
            }
            return result;
        }
    }
}
