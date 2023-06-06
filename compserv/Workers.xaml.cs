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
            string sql = "SELECT EmployID as 'Номер' ,[Lname] as 'Фамилия',[Name] as 'имя',[SurName] as 'отчество',Role as 'роль' FROM [Employ]".ToString();
            DataTable dataTable = new DataTable();
            dataTable = Database(sql);
            Plan.ItemsSource = dataTable.DefaultView;
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
        private void Plan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        }

        private void btn_del(object sender, RoutedEventArgs e)
        {

        }

        private void btn_ref(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
