using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class LoginFr : Form
    {
        string connectionString = "Data Source=HocPham\\SQLEXPRESS;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;

        public LoginFr()
        {
            InitializeComponent();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text.Length < 6 || String.IsNullOrEmpty(tbUsername.Text))
            {
                MessageBox.Show("Username or passwrod invalid");
            }
            else { 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT userId, role FROM Users WHERE username = @username AND userpassword = @password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", tbUsername.Text);
                        command.Parameters.AddWithValue("@password", tbPassword.Text);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            int userId = reader.GetInt32(0);
                            int role = reader.GetInt32(1);
                            this.Hide();
                            MainFr mainForm = new MainFr(userId, role);
                            mainForm.Show();
                        }
                        else
                        {
                            // Failed login
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                }
            }
        }

        private void lbClear_Click(object sender, EventArgs e)
        {
            tbUsername.Text = String.Empty; tbPassword.Text = String.Empty;
        }
    }
}
