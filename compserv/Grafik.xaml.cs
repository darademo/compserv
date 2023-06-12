using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using OfficeOpenXml;

namespace compserv
{
    /// <summary>
    /// Логика взаимодействия для Grafik.xaml
    /// </summary>
    public partial class Grafik : Window
    {
        DBEntities db = new DBEntities();
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ddbClone;Integrated Security=True");
        public Grafik()
        {
            InitializeComponent();
            Update();
        }
        public DataTable Database(string sql)
        {
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
        public void Update()
        {
            string sql = "SELECT GrafikID AS 'Номер', Employ.LName as 'Сотрудник Ф', Employ.[Name] as 'И', Employ.SurName as 'О', " +
                "Data_Start as 'Дата начала', Date_End as 'Дата окончания' FROM Grafik INNER JOIN Employ on Employ.EmployID =  Grafik.EmpID";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            DataTable dataTable = new DataTable("Table");
            adapter.Fill(dataTable);
            Plan.ItemsSource = dataTable.DefaultView;
            adapter.Update(dataTable);

            con.Close();
        }

        private void To_Main(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Add_btn(object sender, RoutedEventArgs e)
        {
            Grafik row = new Grafik();
            row.GrafikID = (from v in db.Employ select v.EmployID).Max() + 1;
            row.EmpID = Convert.ToInt32(tbx_number.Text);
            row.Data_Start = Convert.ToDateTime(tbx_ot.Text);
            row.Date_End = Convert.ToDateTime (tbx_to.Text);
            db.Grafik.Add(row);
            db.SaveChanges();
            Update();
            MessageBox.Show("Данные Добавлены");
        }

        private void Del_btn(object sender, RoutedEventArgs e)
        {
            int selectedinedx = Convert.ToInt32(Plan.Columns[0].GetCellContent(Plan.SelectedItem).Parent.ToString().Remove(0, 38));
            var row = db.Grafik.Where(w => w.GrafikID == selectedinedx).FirstOrDefault();
            db.Grafik.Remove(row);
            db.SaveChanges();
            Update();
            MessageBox.Show("Данные скрыты");
        }

        private void To_Excel(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            DataGrid dataGrid = Plan;
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.Filter = "Excel Files|*.xlsx";
            saveDialog.Title = "Save an Excel File";
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    var newFile = new FileInfo(saveDialog.FileName);
                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        int columnIndex = 1;
                        foreach (DataGridColumn column in dataGrid.Columns)
                        {
                            worksheet.Cells[1, columnIndex].Value = column.Header;
                            columnIndex++;
                        }
                        int rowIndex = 2;
                        foreach (var item in dataGrid.Items)
                        {
                            columnIndex = 1;
                            foreach (DataGridColumn column in dataGrid.Columns)
                            {

                                var cellContent = column.GetCellContent(item);
                                if (cellContent is TextBlock)
                                {
                                    worksheet.Cells[rowIndex, columnIndex].Value = (cellContent as TextBlock).Text;
                                }
                                else if (cellContent is CheckBox)
                                {
                                    worksheet.Cells[rowIndex, columnIndex].Value = (cellContent as CheckBox).IsChecked;
                                }
                                else if (cellContent is ComboBox)
                                {
                                    worksheet.Cells[rowIndex, columnIndex].Value = (cellContent as ComboBox).SelectedValue;
                                }
                                columnIndex++;
                            }
                            rowIndex++;
                        }
                        package.Save();
                    }
                    MessageBox.Show("Export to Excel completed successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting to Excel: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
