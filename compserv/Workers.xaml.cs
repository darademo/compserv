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

namespace compserv
{
    /// <summary>
    /// Логика взаимодействия для Workers.xaml
    /// </summary>
    public partial class Workers : Window
    {
        DBEntities db = new DBEntities();
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ddbClone;Integrated Security=True");
        public Workers()
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
        
        private void Update()
        {
            string sql = "SELECT EmployID as 'Номер' ,[Lname] as 'Фамилия',[Name] as 'имя',[SurName] as 'отчество',Role as 'роль' FROM [Employ]".ToString();
            DataTable dataTable = new DataTable();
            dataTable = Database(sql);
            Plan.ItemsSource = dataTable.DefaultView;
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void To_Main(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void To_Grafik(object sender, RoutedEventArgs e)
        {
            Grafik grafik = new Grafik();
            grafik.Show();
            this.Close();
        }

        private void btn_add(object sender, RoutedEventArgs e)
        {
            try
            {
                Employ row = new Employ();
                row.EmployID = (from v in db.Employ select v.EmployID).Max() + 1;
                row.Lname = tbxFam.Text;
                row.Name = tbxName.Text;
                row.SurName = tbxName.Text;
                if (row.Role == null)
                { row.Role = " "; }
                    else
                { row.Role = tbxRole.Text; }
                if (row.Login == null)
                { row.Login = " "; }
                else
                { row.Login = tbxLogin.Text; }
                if (row.Password == null) 
                { row.Password = " "; }
                else
                { row.Password = tbxPass.Text; }
                db.Employ.Add(row);
                db.SaveChanges();
                Update();
                MessageBox.Show("Данные Добавлены");
            }
            catch
            {
                MessageBox.Show("Данные введены некорректно");
            }
        }

        private void btn_del(object sender, RoutedEventArgs e)
        {
            int selectedinedx = Convert.ToInt32(Plan.Columns[0].GetCellContent(Plan.SelectedItem).Parent.ToString().Remove(0, 38));
            var row = db.Employ.Where(w => w.EmployID == selectedinedx).FirstOrDefault();
            db.Employ.Remove(row);
            db.SaveChanges();
            Update();
            MessageBox.Show("Данные скрыты");
        }

        private void btn_ref(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
