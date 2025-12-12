using CarRentalSystem.DTOs;
using CarRentalSystem.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;

namespace CarRentalSystem
{
    public partial class SchedulesFr : Form
    {
        private readonly ScheduleService _scheduleService = new ScheduleService();
        private readonly MainFr _mainFr;
        private readonly Role _role;
        private int _totalCarCost = 0;

        public SchedulesFr(MainFr mainFr, Role role)
        {
            InitializeComponent();
            _mainFr = mainFr;
            _role = role;
        }

        private void ResetTextBox()
        {
            tbCarId.Text = string.Empty;
            tbDateDelay.Text = string.Empty;
            tbFineCost.Text = string.Empty;
            tbName.Text = string.Empty;
            tbStatus.Text = string.Empty;
            tbTotalCost.Text = string.Empty;
            cBCusId2.Text = string.Empty;
            tbBookingID.Text = string.Empty;
            tbScheduleID.Text = string.Empty;
            schedulesDGV.DataSource = null;
            schedulesDGV.Rows.Clear();
            _totalCarCost = 0;
        }

        private void LoadScheduleById(int customerId)
        {
            try
            {
                List<ScheduleViewDTO> schedules = _scheduleService.GetSchedulesByCustomerId(customerId);
                DataTable dt = ConvertListToDataTable(schedules);
                schedulesDGV.DataSource = dt;

                // Set column headers
                if (schedulesDGV.Columns.Count > 0)
                {
                    schedulesDGV.Columns["StartDate"].HeaderText = "Start Date";
                    schedulesDGV.Columns["EndDate"].HeaderText = "End Date";
                    schedulesDGV.Columns["CarID"].HeaderText = "Car ID";
                    schedulesDGV.Columns["Status"].HeaderText = "Status";
                    schedulesDGV.Columns["PickupLocation"].HeaderText = "Pickup Location";
                    schedulesDGV.Columns["ReturnLocation"].HeaderText = "Return Location";
                    schedulesDGV.Columns["TotalCarCost"].HeaderText = "Total Car Cost";
                    schedulesDGV.Columns["BookingID"].HeaderText = "Booking ID";
                    schedulesDGV.Columns["ScheduleID"].HeaderText = "Schedule ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading schedules: " + ex.Message);
            }
        }

        private void FillCustomer()
        {
            try
            {
                DataTable dt = _scheduleService.GetAllCustomerIds();
                DataRow defaultRow = dt.NewRow();
                defaultRow["CusId"] = DBNull.Value; // or use -1 if the column doesn't allow null
                dt.Rows.InsertAt(defaultRow, 0);

                cBCusId2.ValueMember = "CusId";
                cBCusId2.DisplayMember = "CusId"; // Add this to show the value
                cBCusId2.DataSource = dt;

                cBCusId2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
        }

        private void FetchCustomer()
        {
            if (string.IsNullOrEmpty(cBCusId2.Text)) return;

            try
            {
                string customerName = _scheduleService.GetCustomerName(int.Parse(cBCusId2.Text));
                tbName.Text = customerName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching customer: " + ex.Message);
            }
        }

        private ScheduleDTO BuildScheduleDto()
        {
            return new ScheduleDTO
            {
                ScheduleId = string.IsNullOrEmpty(tbScheduleID.Text) ? 0 : int.Parse(tbScheduleID.Text),
                DateDelay = string.IsNullOrEmpty(tbDateDelay.Text) || tbDateDelay.Text == "No Delay" 
                    ? null 
                    : tbDateDelay.Text,
                DateReturn = dtpReturnDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                FineCost = string.IsNullOrEmpty(tbFineCost.Text) ? 0 : int.Parse(tbFineCost.Text),
                TotalCost = string.IsNullOrEmpty(tbTotalCost.Text) ? 0 : int.Parse(tbTotalCost.Text),
                BookingId = string.IsNullOrEmpty(tbBookingID.Text) ? 0 : int.Parse(tbBookingID.Text),
                CarId = string.IsNullOrEmpty(tbCarId.Text) ? 0 : int.Parse(tbCarId.Text)
            };
        }

        private bool ValidateScheduleInput()
        {
            if (string.IsNullOrEmpty(tbScheduleID.Text))
            {
                MessageBox.Show("Please select a schedule first.");
                return false;
            }

            if (string.IsNullOrEmpty(tbBookingID.Text))
            {
                MessageBox.Show("Booking ID is missing.");
                return false;
            }

            if (string.IsNullOrEmpty(tbTotalCost.Text))
            {
                MessageBox.Show("Total cost is not calculated.");
                return false;
            }

            if (dtpReturnDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Return date cannot be in the past.");
                return false;
            }

            return true;
        }

        private void CalculatePayment()
        {
            try
            {
                DateTime returnDate = dtpReturnDate.Value.Date;
                
                if (returnDate < DateTime.Now.Date)
                {
                    MessageBox.Show("Please check Date Return!");
                    dtpReturnDate.Value = DateTime.Now;
                    tbTotalCost.Text = string.Empty;
                    return;
                }

                DateTime endDate = dtpEndDate.Value.Date;

                var (delayDays, fineCost, totalCost) = _scheduleService.CalculatePayment(
                    endDate, 
                    returnDate, 
                    _totalCarCost);

                if (delayDays <= 0)
                {
                    tbDateDelay.Text = "No Delay";
                    tbFineCost.Text = "0";
                }
                else
                {
                    tbDateDelay.Text = delayDays.ToString();
                    tbFineCost.Text = fineCost.ToString();
                }

                tbTotalCost.Text = totalCost.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating payment: " + ex.Message);
            }
        }

        // Event Handlers
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
            FillCustomer();

            cBCusId2.SelectedValueChanged += cBCusId_SelectedValueChanged;
        }

        private void cBCusId_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cBCusId2.SelectedValue == null || cBCusId2.SelectedValue == DBNull.Value || cBCusId2.SelectedIndex == 0)
            {
                tbName.Text = string.Empty;
                schedulesDGV.DataSource = null;
                return;
            }

            if (int.TryParse(cBCusId2.Text, out int id))
            {
                FetchCustomer();
                LoadScheduleById(id);
            }
            else
            {
                MessageBox.Show("Invalid customer ID. Please enter a valid numeric value.");
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
                    DataGridViewRow row = schedulesDGV.Rows[e.RowIndex];
                    
                    dtpFromdate.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
                    dtpToDate.Value = Convert.ToDateTime(row.Cells["EndDate"].Value);
                    dtpEndDate.Value = Convert.ToDateTime(row.Cells["EndDate"].Value);
                    
                    tbCarId.Text = row.Cells["CarID"].Value.ToString();
                    _totalCarCost = Convert.ToInt32(row.Cells["TotalCarCost"].Value);
                    tbBookingID.Text = row.Cells["BookingID"].Value.ToString();
                    tbScheduleID.Text = row.Cells["ScheduleID"].Value.ToString();
                    
                    // Reset payment fields
                    tbDateDelay.Text = string.Empty;
                    tbFineCost.Text = string.Empty;
                    tbTotalCost.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select again: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateScheduleInput()) return;

            try
            {
                ScheduleDTO scheduleDto = BuildScheduleDto();
                int totalCost = int.Parse(tbTotalCost.Text);

                _scheduleService.CompleteScheduleWithPayment(
                    scheduleDto.ScheduleId,
                    scheduleDto.BookingId,
                    scheduleDto,
                    totalCost);

                MessageBox.Show("Payment processed successfully!");
                
                // Reload schedules for current customer
                if (int.TryParse(cBCusId2.Text, out int customerId))
                {
                    LoadScheduleById(customerId);
                }
                
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCarId.Text))
            {
                MessageBox.Show("Please select a schedule first.");
                return;
            }

            try
            {
                if (IsCarAvailableForBooking(
                    Helper.AppConfigHelper.ConnectionString,
                    dtpFromdate.Value.Date,
                    dtpToDate.Value.Date,
                    tbCarId.Text))
                {
                    tbStatus.Text = "Available";
                }
                else
                {
                    tbStatus.Text = "In Rental";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking car status: " + ex.Message);
            }
        }

        private void dtpReturnDate_ValueChanged(object sender, EventArgs e)
        {
            CalculatePayment();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            _mainFr.RefreshRoleUI();
            _mainFr.Show();
        }

        private void lbExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
