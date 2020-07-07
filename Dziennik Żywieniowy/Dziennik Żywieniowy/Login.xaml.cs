using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Dziennik_Żywieniowy
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void btn_Registration_Click_1(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.chbox_L.IsChecked = true;
            reg.chbox_Man.IsChecked = true;
            reg.ShowDialog();
        }
        private void btn_Submit_Click_1(object sender, RoutedEventArgs e)
        {
            WelcomeWindow main = new WelcomeWindow();
          
            if (txt_Username.Text.Length < 1  || txt_Password.Password.Length < 1)
            {
                MessageBoxResult msg = MessageBox.Show("Please insert login and password", "FoodDiary", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (msg != MessageBoxResult.OK)
                {
                    Close();
                    main.ShowDialog();
                }
            }
            else
            {
                string sql = "SELECT COUNT(*) FROM Users WHERE Username = @user AND Password = HASHBYTES('SHA1','@password')";
                var command = new SqlCommand(sql, DBconnection.Connection());
                command.Parameters.AddWithValue("@user", txt_Username.Text);
                command.Parameters.AddWithValue("@password", txt_Password.Password);

                int results = (int)command.ExecuteScalar();
                if (results > 0)
                {
                    UserWindow userwindow = new UserWindow();
                    Global_Methods.username = txt_Username.Text;
                    DBconnection.Connection_Close(DBconnection.Connection());
                    Close();
                    userwindow = new UserWindow();
                    userwindow.ShowDialog();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Wrong login or password", "FoodDiary", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (result != MessageBoxResult.OK)
                    {
                        DBconnection.Connection_Close(DBconnection.Connection());
                        Close();
                        main.ShowDialog();
                    }
                }
            }
        }
    }
}