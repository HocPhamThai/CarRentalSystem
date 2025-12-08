using CarRentalSystem.DTOs;
using CarRentalSystem.Helper;
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
    public partial class CarsFr : Form
    {
        private readonly CarService _carService = new CarService();
        private readonly MainFr mainFr;
        private readonly Role _role;

        public CarsFr(MainFr mainFr, Role roleId)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this._role = roleId;
        }

        private void ResetTextBox()
        {
            tbCarid.Text = string.Empty;
            tbBrand.Text = string.Empty;
            tbModel.Text = string.Empty;
            cbAvailable.Text = string.Empty;
            tbPrice.Text = string.Empty;
        }

        private void LoadCars()
        {
            try
            {
                List<CarDTO> cars = _carService.GetAllCars();
                DataTable dt = ConvertListToDataTable(cars);
                carDGV.DataSource = dt;

                carDGV.Columns["CarId"].HeaderText = "Car ID";
                carDGV.Columns["Brand"].HeaderText = "Brand";
                carDGV.Columns["Model"].HeaderText = "Model";
                carDGV.Columns["Category"].HeaderText = "Category";
                carDGV.Columns["Available"].HeaderText = "Available";
                carDGV.Columns["Price"].HeaderText = "Price";
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading car data.");
            }
        }

        private void Car_Load(object sender, EventArgs e)
        {
            LoadCars();
            tbCarid.Hide();
        }

        private CarDTO BuildDto()
        {
            return new CarDTO
            {
                CarId = string.IsNullOrEmpty(tbCarid.Text) ? 0 : int.Parse(tbCarid.Text),
                Brand = tbBrand.Text,
                Model = tbModel.Text,
                Category = tbType.Text,
                Available = cbAvailable.Text,
                Price = int.Parse(tbPrice.Text)
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _carService.AddCar(BuildDto());
                MessageBox.Show("Car added!");
                LoadCars();
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
                _carService.UpdateCar(BuildDto());
                MessageBox.Show("Car updated!");
                LoadCars();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCarid.Text))
            {
                MessageBox.Show("Select car first");
                return;
            }

            DialogResult r = MessageBox.Show("Delete this customer?", "Confirm", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes) return;

            try
            {
                _carService.DeleteCar(int.Parse(tbCarid.Text));
                MessageBox.Show("Car deleted!");
                LoadCars();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void carDGV_CellClick(object sender, DataGridViewCellEventArgs e)
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
            LoadCars();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string col = cbSearch.SelectedItem.ToString().ToLower();
            carDGV.DataSource = _carService.SearchCars(col, tbSearch.Text);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)carDGV.DataSource;
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
                                string brand = reader.GetString(1);
                                string model = reader.GetString(2);
                                string category = reader.GetString(3);
                                string available = reader.GetString(4);
                                int price = 0;
                                if (reader.GetValue(5) != null && int.TryParse(reader.GetValue(4).ToString(), out int parsedPrice))
                                {
                                    price = parsedPrice;
                                }

                                using (SqlConnection connection = new SqlConnection(AppConfigHelper.ConnectionString))
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
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            mainFr.RefreshRoleUI();
            mainFr.Show();
        }
        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
