using System;
using System.Data;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;

namespace CarRentalSystem
{
    public partial class MainFr : Form
    {
        private readonly int _userId;
        private readonly Role _role;

        public MainFr(int userId, Role role)
        {
            InitializeComponent();
            this._userId = userId;
            this._role = role;

            UpdateUIBasedOnRole();
        }

        private void UpdateUIBasedOnRole()
        {
            btnUser.Visible = false; btnDashBoard.Visible = false;

            if (_role == Role.Admin)
            {
                btnUser.Visible = true; btnDashBoard.Visible = true;
            }
        }

        public void RefreshRoleUI()
        {
            UpdateUIBasedOnRole();
        }

        private void NavigateTo(Form nextForm)
        {
            this.Hide();
            nextForm.FormClosed += (s, args) => this.Show();  // Khi form con đóng → quay lại MainFr
            nextForm.Show();
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            NavigateTo(new CarsFr(this, _role));
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            NavigateTo(new CustomersFr(this, _role));
        }

        private void btnBookings_Click(object sender, EventArgs e)
        {
            NavigateTo(new BookingsFr(this, _role));
        }

        private void btnSchedules_Click(object sender, EventArgs e)
        {
            NavigateTo(new SchedulesFr(this, _role));
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            NavigateTo(new UsersFr(this, _role));
        }

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            NavigateTo(new DashBoardFr(this, _role));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            var loginFr = new LoginFr();
            loginFr.Show();
        }
        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
