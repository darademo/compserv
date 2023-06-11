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
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Window
    {
        DBEntities db = new DBEntities();
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ddbClone;Integrated Security=True");
        public Clients()
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
            string sql = "SELECT ClientID as 'Номер', Lname as 'Фамилия', Client.[Name] as 'Имя', SurName as 'Отчество', DateBirth as 'Дата рождения', Adress 'Адрес', PhonNumber as 'Телефон' FROM Client ;".ToString();
            DataTable dataTable = new DataTable();
            dataTable = Database(sql);
            Plan.ItemsSource = dataTable.DefaultView;
        }
        private void To_Main(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btn_add(object sender, RoutedEventArgs e)
        {
            try
            {
                Client row = new Client();
                row.ClientID = (from v in db.Client select v.ClientID).Max() + 1;
                row.SurName = tbx_otch.Text;
                row.Name = tbx_name.Text;
                row.Lname = tbx_fam.Text;
                row.PhonNumber = tbx_phone.Text;
                row.DateBirth = Convert.ToDateTime(tbx_date.Text);
                row.VidClient = Convert.ToInt32(tbx_vid.Text);
                row.TypeClient = Convert.ToInt32(tbx_type.Text);
                row.Adress = tbx_adress.Text;
                db.Client.Add(row);
                db.SaveChanges();
                Update();
                MessageBox.Show("Данные Добавлены");
            }
            catch
            {
                MessageBox.Show("Некорректные данные");
            }
        }

        private void btn_del(object sender, RoutedEventArgs e)
        {
            int selectedinedx = Convert.ToInt32(Plan.Columns[0].GetCellContent(Plan.SelectedItem).Parent.ToString().Remove(0, 38));
            var row = db.Client.Where(w => w.ClientID == selectedinedx).FirstOrDefault();
            db.Client.Remove(row);
            db.SaveChanges();
            Update();
            MessageBox.Show("Данные скрыты");
        }

        private void btn_ref(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
