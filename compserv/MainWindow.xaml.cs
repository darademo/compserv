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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DBEntities db = new DBEntities();
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ddbClone;Integrated Security=True");

        public class ClassForGrid
        {

            public string VisitsID { get; set; }
            public string ClientID { get; set; }
            public string ServisID { get; set; }
            public string DateVisit { get; set; }
            public string Quantity_product { get; set; }
            public string Date_manufacture { get; set; }

            public ClassForGrid(string VisitsID, string ClietID, string ServisID, string DateVisit, string Quantity_product, string Date_manufacture)
            {
                this.VisitsID = VisitsID;
                this.ClientID = ClientID;
                this.ServisID = ServisID;
                this.DateVisit = DateVisit;
                this.Quantity_product = Quantity_product;
                this.Date_manufacture = Date_manufacture;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Update();
        }


        private void Plan_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            //вытаскивание значения из datagrid
            //selected
            try
            {
                var rowcontent = Plan.Columns[0].GetCellContent(Plan.SelectedItem);
                var row1 = rowcontent != null ? rowcontent.Parent as System.Windows.Controls.DataGridCell : null;  //(переменная) = (условие) ? (значение если условие выполняется) : (значение если условие не выполняется)
                string str = row1.ToString();
                int selectedindex = Convert.ToInt32(str.Remove(0, 38));
            }
            catch
            {
                int selectedinedx = Convert.ToInt32(Plan.Columns[0].GetCellContent(Plan.SelectedItem).Parent.ToString().Remove(0, 38));
                var row = db.Visits.Where(w => w.VisitsID == selectedinedx).FirstOrDefault();
                tbx_number.Text = row.ClientID.ToString();
                tbx_serv.Text = row.ServisID.ToString();
                tbx_date.Text = row.DateVisit.ToString();


            }

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
            string sql = "  SELECT VisitsID AS 'Номер', Client.LName as 'Клиент Ф', Client.[Name] as 'И', Client.SurName as 'О', [Services].Name 'Услуга' ,DateVisit as 'Дата' FROM Visits  INNER JOIN Client on Visits.ClientID = Client.ClientID   INNER JOIN [Services] on [Services].ServicesID = Visits.ServisID ";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            DataTable dataTable = new DataTable("Table");
            adapter.Fill(dataTable);
            Plan.ItemsSource = dataTable.DefaultView;
            adapter.Update(dataTable);

            con.Close();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void To_Emp(object sender, RoutedEventArgs e)
        {
            Workers emp = new Workers();
            emp.Show();
            this.Close();
        }

        private void To_Clients(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            clients.Show();
            this.Close();
        }

        private void To_Uchet(object sender, RoutedEventArgs e)
        {
            Uchet uchet = new Uchet();
            uchet.Show();
            this.Close();
        }
        private void Service_click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT Name услуга,Price AS 'стоимость' FROM [Services]";
            DataTable dataTable = new DataTable();
            dataTable = Database(sql);
            Plan.ItemsSource = dataTable.DefaultView;
        }

        private void btn_add(object sender, RoutedEventArgs e)
        {
            try
            {
                Visits row = new Visits();
                row.VisitsID = (from v in db.Visits select v.VisitsID).Max() + 1;
                row.ClientID = Convert.ToInt32(tbx_number.Text);
                row.ServisID = Convert.ToInt32(tbx_serv.Text);
                row.DateVisit = Convert.ToDateTime(tbx_date.Text);
                db.Visits.Add(row);
                db.SaveChanges();
                Update();
                MessageBox.Show("Данные Добавлены");
            }
            catch
            {
                MessageBox.Show("Данные введены не коректно");
            }
        }
        private void btn_edit(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedinedx = Convert.ToInt32(Plan.Columns[0].GetCellContent(Plan.SelectedItem).Parent.ToString().Remove(0, 38));
                var row = db.Visits.Where(w => w.VisitsID == selectedinedx).FirstOrDefault();
                row.ClientID = Convert.ToInt32(tbx_number.Text);
                row.ServisID = Convert.ToInt32(tbx_serv.Text);
                row.DateVisit = Convert.ToDateTime(tbx_date.Text);
                db.SaveChanges();
                Update();
                MessageBox.Show("Выполненно");
            }
            catch
            {
                MessageBox.Show("Поле не выбрано");
            }
        }

        private void btn_del(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedinedx = Convert.ToInt32(Plan.Columns[0].GetCellContent(Plan.SelectedItem).Parent.ToString().Remove(0, 38));
                var row = db.Visits.Where(w => w.VisitsID == selectedinedx).FirstOrDefault();
                db.Visits.Remove(row);
                db.SaveChanges();
                Update();
                MessageBox.Show("Данные скрыты");
            }
            catch
            {

                MessageBox.Show("Поле не выбрано");
            }
        }

        private void btn_ref(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Poisk(object sender, RoutedEventArgs e)
        {
            string sql;
            DataTable dataTable = new DataTable();
            sql = " SELECT VisitsID AS 'Номер', Client.LName as 'Клиент Ф', Client.[Name] as 'И', Client.SurName as 'О' , [Services].Name 'Услуга' ,DateVisit as 'Дата' FROM Visits  INNER JOIN Client on Visits.ClientID = Client.ClientID   INNER JOIN [Services] on [Services].ServicesID = Visits.ServisID   WHERE Client.Lname Like '%" + tbxPoisk.Text + "%'";
            dataTable = Database(sql);
            Plan.ItemsSource = dataTable.DefaultView;
        }

        private void To_Excel(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            DataGrid dataGrid = Plan;
            // Создаем диалог сохранения файла Excel
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.Filter = "Excel Files|*.xlsx";
            saveDialog.Title = "Save an Excel File";
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    // Создаем новый файл Excel
                    var newFile = new FileInfo(saveDialog.FileName);

                    // Создаем пакет Excel
                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        // Создаем лист Excel
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        // Заполняем заголовки столбцов

                        int columnIndex = 1;
                        foreach (DataGridColumn column in dataGrid.Columns)
                        {
                            worksheet.Cells[1, columnIndex].Value = column.Header;
                            columnIndex++;
                        }

                        // Заполняем данные из DataGrid
                        int rowIndex = 2;
                        foreach (var item in dataGrid.Items)
                        {
                            columnIndex = 1;
                            foreach (DataGridColumn column in dataGrid.Columns)
                            {
                                // Получаем содержимое ячейки
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

                        // Сохраняем пакет Excel
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


