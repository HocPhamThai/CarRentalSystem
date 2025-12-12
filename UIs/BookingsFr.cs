using CarRentalSystem.DTOs;
using CarRentalSystem.Helper;
using CarRentalSystem.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;

namespace CarRentalSystem
{
    public partial class BookingsFr : Form
    {
        private readonly BookingService _bookingService = new BookingService();
        private readonly CarService _carService = new CarService();
        private readonly MainFr _mainFr;
        private readonly Role _role;
        private Dictionary<string, int> _descFeatureAndFuels = new Dictionary<string, int>();
        private int _totalPriceFuelAndFeature = 0;
        private int _totalPriceBetweenDate = 0;
        private int _totalPrice = 0;
        private int _priceOfCarPerDate = 0;

        public BookingsFr(MainFr main, Role role)
        {
            InitializeComponent();
            _mainFr = main;
            _role = role;
        }

        private void ResetTextBox()
        {
            tbBookingId.Text = string.Empty;
            cbCarId.SelectedValue = -1;
            tbBrand.Text = string.Empty;
            tbModel.Text = string.Empty;
            tbFee.Text = string.Empty;
            cBCusId.SelectedValue = -1;
            tbName.Text = string.Empty;

            // Reset checkboxes and radio buttons
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
                else if (control is RadioButton radioButton)
                {
                    radioButton.Checked = false;
                }
            }

            tbFromPlace.Text = string.Empty;
            tbToPlace.Text = string.Empty;
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            tbPrice.Text = string.Empty;

            // Reset price tracking
            _totalPriceFuelAndFeature = 0;
            _totalPriceBetweenDate = 0;
            _totalPrice = 0;
            _descFeatureAndFuels.Clear();
        }

        private void LoadBookings()
        {
            try
            {
                List<BookingDTO> bookings = _bookingService.GetAllBookings();
                DataTable dt = ConvertListToDataTable(bookings);
                bookingDGV.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading booking data: " + ex.Message);
            }
        }

        private void FillComboCarId()
        {
            try
            {
                List<CarDTO> cars = _carService.GetAllCars();
                DataTable dt = new DataTable();
                dt.Columns.Add("carId", typeof(int));

                foreach (var car in cars)
                {
                    dt.Rows.Add(car.CarId);
                }

                cbCarId.ValueMember = "carId";
                cbCarId.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cars: " + ex.Message);
            }
        }

        private void FillCustomer()
        {
            try
            {
                DataTable dt = _bookingService.GetAllCustomerIds();
                cBCusId.ValueMember = "CusId";
                cBCusId.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
        }

        private void FetchCustomer()
        {
            if (string.IsNullOrEmpty(cBCusId.Text)) return;

            try
            {
                string customerName = _bookingService.GetCustomerName(int.Parse(cBCusId.Text));
                tbName.Text = customerName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching customer: " + ex.Message);
            }
        }

        private void FillTextBoxCarInfo()
        {
            if (string.IsNullOrEmpty(cbCarId.Text)) return;

            try
            {
                CarDTO car = _bookingService.GetCarById(int.Parse(cbCarId.Text));
                if (car != null)
                {
                    tbModel.Text = car.Model;
                    tbBrand.Text = car.Brand;
                    tbSeat.Text = car.Category;
                    _priceOfCarPerDate = car.Price;
                    tbFee.Text = car.Price.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading car info: " + ex.Message);
            }
        }

        private BookingDTO BuildBookingDto()
        {
            string desc = string.Join("\n", _descFeatureAndFuels.Select(p => $"{p.Key} - {p.Value}"));

            return new BookingDTO
            {
                BookingId = string.IsNullOrEmpty(tbBookingId.Text) ? 0 : int.Parse(tbBookingId.Text),
                CarId = int.Parse(cbCarId.SelectedValue.ToString()),
                CusId = int.Parse(cBCusId.SelectedValue.ToString()),
                FromDate = dtpFromDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                ToDate = dtpToDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                Status = "In Rental",
                Description = desc,
                TotalCost = _totalPrice
            };
        }

        private ScheduleDTO BuildScheduleDto(int carId)
        {
            return new ScheduleDTO
            {
                FromPlace = tbFromPlace.Text,
                ToPlace = tbToPlace.Text,
                CarId = carId
            };
        }

        private bool ValidateBookingInput()
        {
            int checkedRadioButtons = this.Controls.OfType<RadioButton>().Count(rb => rb.Checked);

            if (string.IsNullOrEmpty(cbCarId.Text) ||
                string.IsNullOrEmpty(cBCusId.Text) ||
                string.IsNullOrEmpty(tbPrice.Text) ||
                string.IsNullOrEmpty(tbFromPlace.Text) ||
                string.IsNullOrEmpty(tbToPlace.Text) ||
                checkedRadioButtons != 1)
            {
                MessageBox.Show("Missing Information");
                return false;
            }

            return true;
        }

        private void LoadFeaturePrice(string featureName, bool isChecked)
        {
            try
            {
                int featurePrice = _bookingService.GetFeaturePrice(featureName);

                if (isChecked)
                {
                    _totalPriceFuelAndFeature += featurePrice;
                    _descFeatureAndFuels[featureName] = featurePrice;
                }
                else
                {
                    _totalPriceFuelAndFeature -= featurePrice;
                    _descFeatureAndFuels.Remove(featureName);
                }

                LoadTotalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading feature price: " + ex.Message);
            }
        }

        private void LoadFuelPrice(string fuelName, bool isChecked)
        {
            try
            {
                int fuelPrice = _bookingService.GetFuelPrice(fuelName);

                if (isChecked)
                {
                    _totalPriceFuelAndFeature += fuelPrice;
                    _descFeatureAndFuels[fuelName] = fuelPrice;
                }
                else
                {
                    _totalPriceFuelAndFeature -= fuelPrice;
                    _descFeatureAndFuels.Remove(fuelName);
                }

                LoadTotalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading fuel price: " + ex.Message);
            }
        }

        private void LoadTotalPerDate()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;

            if (fromDate <= toDate)
            {
                TimeSpan duration = toDate - fromDate;
                int days = duration.Days;
                _totalPriceBetweenDate = (days + 1) * _priceOfCarPerDate;
            }
            else
            {
                dtpFromDate.ValueChanged -= dtpFromDate_ValueChanged;
                dtpToDate.ValueChanged -= dtpToDate_ValueChanged;
                dtpFromDate.Value = DateTime.Now;
                dtpToDate.Value = DateTime.Now;
                dtpFromDate.ValueChanged += dtpFromDate_ValueChanged;
                dtpToDate.ValueChanged += dtpToDate_ValueChanged;
                MessageBox.Show("From date must be before or equal to 'To date'.");
                tbPrice.Text = "0";
            }

            LoadTotalPrice();
        }

        private void LoadTotalPrice()
        {
            _totalPrice = _totalPriceFuelAndFeature + _totalPriceBetweenDate;
            tbPrice.Text = _totalPrice.ToString();
        }

        private void DisplayBookingDetails(int bookingId)
        {
            try
            {
                BookingDTO booking = _bookingService.GetBookingById(bookingId);

                if (booking == null)
                {
                    MessageBox.Show("Booking not found.");
                    return;
                }

                DateTime fromDate;
                DateTime toDate;
                int days = 0;

                // Safe date parsing
                if (DateTime.TryParse(booking.FromDate, out fromDate) && 
                    DateTime.TryParse(booking.ToDate, out toDate))
                {
                    days = (toDate - fromDate).Days;
                }
                else
                {
                    MessageBox.Show("Invalid date format in booking data.");
                    return;
                }

                string customerName = _bookingService.GetCustomerName(booking.CusId) ?? "Unknown";
                CarDTO car = _bookingService.GetCarById(booking.CarId);
                string carDetails = car != null 
                    ? $"{car.Model} {car.Brand} {car.Category} {car.Price}$/Day" 
                    : "Car details not available";

                // Safe conversion of TotalCost
                string totalCostDisplay = booking.TotalCost.ToString() ?? "0";

                string bookingDetails =
                    $"Booking ID: {booking.BookingId}\n" +
                    $"From Date: {booking.FromDate ?? "N/A"}\n" +
                    $"To Date: {booking.ToDate ?? "N/A"}\n" +
                    $"Status: {booking.Status ?? "Unknown"}\n" +
                    $"Customer Name: {customerName}\n" +
                    $"Car Details: \n\t{carDetails} ({days + 1} Days)\n" +
                    $"Decriptions: {booking.Description ?? "No description"}\n" +
                    $"Total Cost: {totalCostDisplay}$";

                MessageBox.Show(bookingDetails);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying booking details: " + ex.Message);
            }
        }

        // Event Handlers
        private void Rental_Load(object sender, EventArgs e)
        {
            tbBookingId.Hide();
            FillComboCarId();
            FillCustomer();
            LoadBookings();

            btnAdd.BringToFront();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateBookingInput()) return;

            try
            {
                BookingDTO bookingDto = BuildBookingDto();
                ScheduleDTO scheduleDto = BuildScheduleDto(int.Parse(cbCarId.SelectedValue.ToString()));

                _bookingService.AddBooking(bookingDto, scheduleDto);

                MessageBox.Show("Booking created successfully!");
                LoadBookings();
                FillComboCarId();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbCarId_SelectedValueChanged(object sender, EventArgs e)
        {
            FillTextBoxCarInfo();
        }

        private void cBCusId_SelectedValueChanged(object sender, EventArgs e)
        {
            FetchCustomer();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dtpFromDate.Value.Date;
            DateTime currentDate = DateTime.Now.Date;

            if (selectedDate < currentDate)
            {
                dtpFromDate.ValueChanged -= dtpFromDate_ValueChanged;
                dtpFromDate.Value = currentDate;
                dtpFromDate.ValueChanged += dtpFromDate_ValueChanged;
                MessageBox.Show("The FromDate must be from now time!");
            }

            LoadTotalPerDate();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            LoadTotalPerDate();
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
                    int bookingId = Convert.ToInt32(bookingDGV.Rows[e.RowIndex].Cells[0].Value);
                    DisplayBookingDetails(bookingId);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select again");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            _mainFr.RefreshRoleUI();
            _mainFr.Show();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Feature CheckBox Event Handlers
        private void ckbMap_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckbMap.Text, ckbMap.Checked);
        }

        private void ckBBluetooth_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBBluetooth.Text, ckBBluetooth.Checked);
        }

        private void ckBRearCamera_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBRearCamera.Text, ckBRearCamera.Checked);
        }

        private void ckBSideView_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBSideView.Text, ckBSideView.Checked);
        }

        private void ckBDashboard_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBDashboard.Text, ckBDashboard.Checked);
        }

        private void ckBSpeedAlert_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBSpeedAlert.Text, ckBSpeedAlert.Checked);
        }

        private void ckTirePressure_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckTirePressure.Text, ckTirePressure.Checked);
        }

        private void ckBCollinsion_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBCollinsion.Text, ckBCollinsion.Checked);
        }

        private void ckBSunroof_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBSunroof.Text, ckBSunroof.Checked);
        }

        private void ckBGPSNavigation_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBGPSNavigation.Text, ckBGPSNavigation.Checked);
        }

        private void ckBUSBPort_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBUSBPort.Text, ckBUSBPort.Checked);
        }

        private void ckBAllWheel_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBAllWheel.Text, ckBAllWheel.Checked);
        }

        private void ckBPickupBed_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckBPickupBed.Text, ckBPickupBed.Checked);
        }

        private void ckB360_CheckedChanged(object sender, EventArgs e)
        {
            LoadFeaturePrice(ckB360.Text, ckB360.Checked);
        }

        // Fuel RadioButton Event Handlers
        private void rBAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadFuelPrice(rBAll.Text, rBAll.Checked);
        }

        private void rBGas_CheckedChanged(object sender, EventArgs e)
        {
            LoadFuelPrice(rBGas.Text, rBGas.Checked);
        }

        private void rBDiesel_CheckedChanged(object sender, EventArgs e)
        {
            LoadFuelPrice(rBDiesel.Text, rBDiesel.Checked);
        }

        private void rBElectric_CheckedChanged(object sender, EventArgs e)
        {
            LoadFuelPrice(rBElectric.Text, rBElectric.Checked);
        }
    }
}
