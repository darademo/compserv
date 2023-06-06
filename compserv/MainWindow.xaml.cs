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
                row.VisitsID = (from v in db.Visits select v.VisitsID).Max()+1;
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
            //выводит информацию из datagrid в книгу Excel
            //Excel.Application ExcelApp = new Excel.Application();
            //ExcelApp.Application.Workbooks.Add(Type.Missing);
            //ExcelApp.Visible = true;
            //Объявляем приложение
            Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
            //Отобразить Excel
            ex.Visible = true;
            //Количество листов в рабочей книге
            ex.SheetsInNewWorkbook = 2;
            //Добавить рабочую книгу
            Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            //Отключить отображение окон с сообщениями
            ex.DisplayAlerts = false;
            //Получаем первый лист документа (счет начинается с 1)
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
            //Название листа (вкладки снизу)
            sheet.Name = "Отчет";
            //Пример заполнения ячеек
            List<Visits> visits = db.Visits.ToList();
            foreach (Visits visit in visits)
            {
                sheet.Cells[1, 1] = String.Format(tbx_number.Text);
                sheet.Cells[1, 2] = String.Format(tbx_serv.Text);
                sheet.Cells[1, 3] = String.Format(tbx_date.Text);
            }
            ////Захватываем диапазон ячеек
            //Excel.Range range1 = sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[9, 9]);
            ////Шрифт для диапазона
            //range1.Cells.Font.Name = "Tahoma";
            ////Размер шрифта для диапазона
            //range1.Cells.Font.Size = 10;
            ////Захватываем другой диапазон ячеек
            //Excel.Range range2 = sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[9, 2]);
            //range2.Cells.Font.Name = "Times New Roman";
        

            Plan.SelectAllCells();
            Plan.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, Plan);
            String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            String result = (string)Clipboard.GetData(DataFormats.Text);
            Plan.UnselectAllCells();
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\tests\test.xls");
            file.WriteLine(result.Replace(',', ' '));


            MessageBox.Show(" Экспорт данных в созданный файл Excel прошел успешно");



        }
    }
}
    


