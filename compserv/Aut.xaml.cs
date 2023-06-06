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
using System.Runtime.InteropServices;
    
namespace compserv
{
    /// <summary>
    /// Логика взаимодействия для Aut.xaml
    /// </summary>
    public partial class Aut : Window
    {
        public Aut()
        {
            InitializeComponent();
        }
        DBEntities db = new DBEntities();
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ddbClone;Integrated Security=True");


        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Auth(object sender, RoutedEventArgs e)
        {
            if (tbxLogin.Text == "" || tbxPass.Password == "")
            {
                MessageBox.Show("Пустые поля");

            }
            else
            {
                var emp = db.Employ.Where(item => item.Login == tbxLogin.Text && item.Password == tbxPass.Password).FirstOrDefault();
                if (emp != null)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Логин или пароль введены не верно");
                }
            }
        }
    }
    }

