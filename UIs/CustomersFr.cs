using CarRentalSystem.DTOs;
using CarRentalSystem.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static CarRentalSystem.Utils.Utils;
using Excel = Microsoft.Office.Interop.Excel;

namespace CarRentalSystem
{
    public partial class CustomersFr : Form
    {
        private readonly CustomerService _customerService = new CustomerService();
        private readonly MainFr mainFr;
        private readonly Role _role;


        public CustomersFr(MainFr mainFr, Role role)
        {
            InitializeComponent();
            this.mainFr = mainFr;
            this._role = role;
        }

        private void ResetTextBox()
        {
            tbCusId.Text = String.Empty;
            tbCusName.Text = String.Empty;
            tbCusAdd.Text = String.Empty;
            tbPhone.Text = String.Empty;
        }

        private void LoadCustomers()
        {
            try
            {
                List<CustomerDTO> customers = _customerService.GetAll();
                DataTable dt = ConvertListToDataTable(customers);
                cusDGV.DataSource = dt;

                cusDGV.Columns["CusId"].HeaderText = "Customer ID";
                cusDGV.Columns["CusName"].HeaderText = "Name";
                cusDGV.Columns["CusAdd"].HeaderText = "Address";
                cusDGV.Columns["Phone"].HeaderText = "Phone";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private CustomerDTO BuildDto()
        {
            return new CustomerDTO
            {
                CusId = string.IsNullOrEmpty(tbCusId.Text) ? 0 : int.Parse(tbCusId.Text),
                CusName = tbCusName.Text,
                CusAdd = tbCusAdd.Text,
                Phone = tbPhone.Text
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _customerService.AddCustomer(BuildDto());
                MessageBox.Show("Customer added!");

                LoadCustomers();
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
                _customerService.UpdateCustomer(BuildDto());
                MessageBox.Show("Customer updated!");

                LoadCustomers();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbCusId.Text))
            {
                MessageBox.Show("Select a customer first!");
                return;
            }

            DialogResult r = MessageBox.Show("Delete this customer?", "Confirm", MessageBoxButtons.YesNo);
            if (r != DialogResult.Yes) return;

            try
            {
                _customerService.DeleteCustomer(int.Parse(tbCusId.Text));
                MessageBox.Show("Customer deleted!");

                LoadCustomers();
                ResetTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cusDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    DataGridViewCell cell = cusDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cell.Style.SelectionBackColor = Color.Red;
                }
                if (e.RowIndex >= 0)
                {
                    tbCusId.Text = cusDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                    tbCusName.Text = cusDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                    tbCusAdd.Text = cusDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                    tbPhone.Text = cusDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                }
            } catch {
                MessageBox.Show("please selected again");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string col = cbSearch.SelectedItem.ToString().ToLower();
            cusDGV.DataSource = _customerService.SearchCustomers(col, tbSearch.Text);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)cusDGV.DataSource;
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
                        saveFileDialog.FileName = "CustomersData";
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
