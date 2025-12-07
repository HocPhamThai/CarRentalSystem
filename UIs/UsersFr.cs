using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;

namespace CarRentalSystem
{
    public partial class UsersFr : Form
    {
        string connectionString = "Data Source=HocPham\\SQLEXPRESS;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;
        private MainFr mainFr;
        private readonly Role _role;
        public UsersFr(MainFr mainFr, Role role)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this._role = role;
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loadUser()
        {
            try
            {
                string query = "SELECT userId as \"User ID\",username as \"User Name\", userpassword as \"User Password\", role as \"Role\" FROM Users";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    userDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resetTextBox()
        {
            tbUsername.Text = String.Empty;
            tbPassword.Text = String.Empty;
            tbRole.Text = String.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbUsername.Text) || String.IsNullOrEmpty(tbPassword.Text)
                || String.IsNullOrEmpty(tbRole.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Users(username, userpassword, role) VALUES (@username, @userpassword, @role)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", tbUsername.Text);
                            cmd.Parameters.AddWithValue("@userpassword", tbPassword.Text);
                            cmd.Parameters.AddWithValue("@role", tbRole.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("User successfully Added!!!");
                                loadUser();
                                resetTextBox();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Information!!!");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void userDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    DataGridViewCell cell = userDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cell.Style.SelectionBackColor = Color.Red;
                }
                if (e.RowIndex >= 0)
                {
                    tbUserId.Text = userDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                    tbUsername.Text = userDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                    tbPassword.Text = userDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                    tbRole.Text = userDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                }
            }
            catch {
                MessageBox.Show("Please seleted again");
            }
        } 

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbUserId.Text) || String.IsNullOrEmpty(tbUsername.Text) || String.IsNullOrEmpty(tbPassword.Text)
                || String.IsNullOrEmpty(tbRole.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "Update Users SET Username = @Username, userpassword = @userpassword, role = @role Where userId = @userId";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@userId", tbUserId.Text);
                            cmd.Parameters.AddWithValue("@Username", tbUsername.Text);
                            cmd.Parameters.AddWithValue("@Userpassword", tbPassword.Text);
                            cmd.Parameters.AddWithValue("@role", tbRole.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
                                loadUser();
                                resetTextBox();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Information!!!");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbUserId.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete user with ID: {tbUserId.Text}", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM USERS WHERE userId = @userId";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@userId", tbUserId.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete user successfully!!!");
                                    loadUser();
                                    resetTextBox();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Information!!!");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                else if (result == DialogResult.No) { }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            mainFr.RefreshRoleUI();
            mainFr.Show();
        }

        private void User_Load(object sender, EventArgs e)
        {
            tbUserId.Hide();
            loadUser();
        }
    }
}
