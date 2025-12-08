using CarRentalSystem.DTOs;
using CarRentalSystem.Services;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;
using Excel = Microsoft.Office.Interop.Excel;

namespace CarRentalSystem
{
    public partial class UsersFr : Form
    {
        private readonly UserService _userService = new UserService();
        private MainFr mainFr;
        private readonly Role _role;

        public UsersFr(MainFr mainFr, Role role)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this._role = role;
        }

        private void ResetTextBox()
        {
            tbUserId.Text = string.Empty;
            tbUsername.Text = string.Empty;
            tbPassword.Text = string.Empty;
            tbRole.Text = string.Empty;
        }

        private void LoadUsers()
        {
            try
            {
                List<UserDTO> users = _userService.GetAllUsers();
                DataTable dt = ConvertListToDataTable(users);
                userDGV.DataSource = dt;

                userDGV.Columns["UserId"].HeaderText = "User ID";
                userDGV.Columns["Username"].HeaderText = "Username";
                userDGV.Columns["UserPassword"].HeaderText = "Password";
                userDGV.Columns["Role"].HeaderText = "Role";
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading user data.");
            }
        }

        private void User_Load(object sender, EventArgs e)
        {
            LoadUsers();
            tbUserId.Hide();
        }

        private UserDTO BuildDto()
        {
            return new UserDTO
            {
                UserId = string.IsNullOrEmpty(tbUserId.Text) ? 0 : int.Parse(tbUserId.Text),
                Username = tbUsername.Text,
                UserPassword = tbPassword.Text,
                Role = int.Parse(tbRole.Text)
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _userService.AddUser(BuildDto());
                MessageBox.Show("User added!");
                LoadUsers();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                _userService.UpdateUser(BuildDto());
                MessageBox.Show("User updated!");
                LoadUsers();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbUserId.Text))
            {
                MessageBox.Show("Select user first");
                return;
            }

            DialogResult r = MessageBox.Show("Delete this user?", "Confirm", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes) return;

            try
            {
                _userService.DeleteUser(int.Parse(tbUserId.Text));
                MessageBox.Show("User deleted!");
                LoadUsers();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            catch (Exception ex)
            {
                MessageBox.Show("Please select again", ex.Message);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string col = cbSearch.SelectedItem.ToString().ToLower();
            userDGV.DataSource = _userService.SearchUsers(col, tbSearch.Text);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
                openFileDialog.Title = "Select an Excel File";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    tbFilePath.Text = selectedFilePath;
                }
            }
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)userDGV.DataSource;
            Export(dt);
        }

        public void Export(DataTable tbl)
        {
            try
            {
                if (tbl == null || tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                var excelApp = new Excel.Application();
                var workbook = excelApp.Workbooks.Add();

                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                for (var i = 0; i < tbl.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                }

                for (var i = 0; i < tbl.Rows.Count; i++)
                {
                    for (var j = 0; j < tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                    }
                }

                try
                {
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "UserData";
                    saveFileDialog.DefaultExt = ".xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    excelApp.Quit();
                    Console.WriteLine("Excel file saved!");
                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                    + ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string excelFilePath = tbFilePath.Text;

            if (string.IsNullOrWhiteSpace(excelFilePath))
            {
                MessageBox.Show("Please select an Excel file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        bool isFirstRow = true;
                        do
                        {
                            while (reader.Read())
                            {
                                if (isFirstRow)
                                {
                                    isFirstRow = false;
                                    continue; // Skip header row
                                }
                                string username = reader.GetString(1);
                                string password = reader.GetString(2);
                                int role = 0;
                                if (reader.GetValue(3) != null && int.TryParse(reader.GetValue(2).ToString(), out int parsedRole))
                                {
                                    role = parsedRole;
                                }

                                var user = new UserDTO
                                {
                                    Username = username,
                                    UserPassword = password,
                                    Role = role
                                };

                                _userService.AddUser(user);
                            }
                        } while (reader.NextResult());
                    }
                }

                MessageBox.Show("Data imported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            mainFr.RefreshRoleUI();
            mainFr.Show();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
