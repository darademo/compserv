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
    /// Логика взаимодействия для Uchet.xaml
    /// </summary>
    public partial class Uchet : Window
    {
        DBEntities db = new DBEntities();
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ddbClone;Integrated Security=True");
        public Uchet()
        {
            InitializeComponent();
            Update();
        }
        private void Plan_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                var row = db.Consumables.Where(w => w.ConsumablesID == selectedinedx).FirstOrDefault();
                tbx_name.Text = row.Name.ToString();
                tbx_kolvo.Text = row.Ed_izmer.ToString();
                tbx_edizmer.Text = row.Colich.ToString();
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
            string sql = "  SELECT ConsumablesID as 'Номер', Name as 'Название', Edizmer.NameEdizmer as 'Ед измерения', Colich as 'Количество' FROM Consumables INNER JOIN Edizmer on [Edizmer].EdizmerID = [Consumables].Ed_izmer ".ToString();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            DataTable dataTable = new DataTable("Table");
            adapter.Fill(dataTable);
            Plan.ItemsSource = dataTable.DefaultView;
            adapter.Update(dataTable);

            con.Close();
        }
        private void to_main(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btn_del(object sender, RoutedEventArgs e)
        {

        }

        private void btn_add(object sender, RoutedEventArgs e)
        {

        }

        private void btn_edit(object sender, RoutedEventArgs e)
        {

        }

        private void btn_ref(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
