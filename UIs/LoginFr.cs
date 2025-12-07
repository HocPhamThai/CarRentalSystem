using CarRentalSystem.DTOs;
using CarRentalSystem.Services;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;

namespace CarRentalSystem
{
    public partial class LoginFr : Form
    {
        private readonly AuthService _authService;

        public LoginFr()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text.Length < 6 || String.IsNullOrEmpty(tbUsername.Text))
            {
                MessageBox.Show("Username or passwrod invalid");
            }

            var request = new LoginRequestDto { Username = tbUsername.Text.Trim(), Password = tbPassword.Text.Trim() };

            var result = _authService.Login(request);

            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage);
                return;
            }

            // Login success
            this.Hide();
            new MainFr(result.UserId, (Role)result.Role).Show();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbClear_Click(object sender, EventArgs e)
        {
            tbUsername.Text = String.Empty; tbPassword.Text = String.Empty;
        }
    }
}
