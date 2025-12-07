using ExcelDataReader;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;
using Excel = Microsoft.Office.Interop.Excel;


namespace CarRentalSystem
{
    public partial class CarsFr : Form
    {
        string connectionString = "Data Source=HocPham\\SQLEXPRESS;Initial Catalog=CarRentaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private SqlDataAdapter adapter;
        private SqlCommandBuilder commandBuilder;
        private readonly MainFr mainFr;
        private readonly Role _role;

        public CarsFr(MainFr mainFr, Role roleId)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this._role = roleId;

        }

        private void resetTextBox()
        {
            tbCarid.Text = string.Empty;
            tbBrand.Text = string.Empty;
            tbModel.Text = string.Empty;
            cbAvailable.Text = string.Empty;
            tbPrice.Text = string.Empty;
        }

        private void loadCars()
        {
            try
            {
                string query = "SELECT carId as 'Car ID', brand as 'Brand', model as 'Model', category as 'Category', available as 'Available', price as 'Price' FROM Cars";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    adapter = new SqlDataAdapter(query, conn);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    carDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Car_Load(object sender, EventArgs e)
        {
            loadCars();
            tbCarid.Hide();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbBrand.Text)
           || String.IsNullOrEmpty(tbModel.Text)
            || String.IsNullOrEmpty(tbType.Text)
           || String.IsNullOrEmpty(cbAvailable.SelectedItem.ToString())
           || String.IsNullOrEmpty(tbPrice.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "INSERT INTO Cars (brand, model, category, available, price) VALUES (@Brand, @Model, @Category, @Available, @Price)";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Brand", tbBrand.Text);
                            cmd.Parameters.AddWithValue("@Model", tbModel.Text);
                            cmd.Parameters.AddWithValue("@Category", tbType.Text);
                            cmd.Parameters.AddWithValue("@Available", cbAvailable.Text);
                            cmd.Parameters.AddWithValue("@Price", tbPrice.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Car successfully Added!!!");
                                loadCars();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbCarid.Text) || String.IsNullOrEmpty(tbBrand.Text)
            || String.IsNullOrEmpty(tbModel.Text)
            || String.IsNullOrEmpty(cbAvailable.SelectedItem.ToString())
            || String.IsNullOrEmpty(tbPrice.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    string query = "UPDATE Cars SET brand = @Brand, model = @Model, available = @Available, Price = @Price, category = @Category WHERE carId = @CarId";

                    using (SqlConnection conn = new SqlConnection(@connectionString))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CarId", tbCarid.Text);
                            cmd.Parameters.AddWithValue("@Brand", tbBrand.Text);
                            cmd.Parameters.AddWithValue("@Model", tbModel.Text);
                            cmd.Parameters.AddWithValue("@Available", cbAvailable.Text);
                            cmd.Parameters.AddWithValue("@Price", tbPrice.Text);
                            cmd.Parameters.AddWithValue("@Category", tbType.Text);

                            int rowEffected = cmd.ExecuteNonQuery();
                            if (rowEffected > 0)
                            {
                                MessageBox.Show("Edit successfully!!!");
                                loadCars();
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
            if (String.IsNullOrEmpty(tbCarid.Text))
            {
                MessageBox.Show("Missing ID Information");
            }
            else
            {
                DialogResult result = MessageBox.Show($"Are you sure to delete this car", "Delete Car", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Cars WHERE carId = @CarId";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(@connectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@CarId", tbCarid.Text);

                                int rowEffected = cmd.ExecuteNonQuery();
                                if (rowEffected > 0)
                                {
                                    MessageBox.Show("Delete car successfully!!!");
                                    loadCars();
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
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            mainFr.RefreshRoleUI();
            mainFr.Show();
        }

        private void cusDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    DataGridViewCell cell = carDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cell.Style.SelectionBackColor = Color.Red;
                }
                if (e.RowIndex >= 0)
                {
                    tbCarid.Text = carDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                    tbBrand.Text = carDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                    tbModel.Text = carDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                    tbType.Text = carDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                    cbAvailable.Text = carDGV.Rows[e.RowIndex].Cells[4].Value.ToString();
                    tbPrice.Text = carDGV.Rows[e.RowIndex].Cells[5].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select again", ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadCars();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbSearch.SelectedIndex == 1)
            {
                loadCars("brand");
            } 
            else if (cbSearch.SelectedIndex == 2)
            {
                loadCars("model");
            } 
            else if (cbSearch.SelectedIndex == 3) 
            {
                loadCars("category");
            } 
            else
            {
                loadCars();
            }
        }

        private void loadCars(string typeSearch)
        {
            try
            {
                string query = "SELECT * FROM Cars WHERE " + typeSearch + " = @SearchText";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@SearchText", tbSearch.Text);

                    adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    carDGV.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable) carDGV.DataSource;
            Export(dt);
        }
        public void Export(DataTable tbl)
        {
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
                        saveFileDialog.FileName = "CarData";
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
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string excelFilePath = tbFilePath.Text; // Use the selected file path from the textbox

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
                        do
                        {
                            while (reader.Read())
                            {
                                string brand = reader.GetString(0);
                                string model = reader.GetString(1);
                                string category = reader.GetString(2);
                                string available = reader.GetString(3);
                                int price = 0; 
                                if (reader.GetValue(4) != null && int.TryParse(reader.GetValue(4).ToString(), out int parsedPrice))
                                {
                                    price = parsedPrice;
                                }

                                using (SqlConnection connection = new SqlConnection(@connectionString))
                                {
                                    connection.Open();

                                    string query = "INSERT INTO Cars (brand, model, category, available, price) " +
                                                   "VALUES (@brand, @model, @category, @available, @price)";

                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        command.Parameters.AddWithValue("@brand", brand);
                                        command.Parameters.AddWithValue("@model", model);
                                        command.Parameters.AddWithValue("@category", category);
                                        command.Parameters.AddWithValue("@available", available);
                                        command.Parameters.AddWithValue("@price", price);

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                        } while (reader.NextResult());
                    }
                }

                MessageBox.Show("Data imported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
