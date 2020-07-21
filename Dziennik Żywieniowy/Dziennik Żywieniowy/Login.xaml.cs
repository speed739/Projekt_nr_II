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

            if (string.IsNullOrWhiteSpace(txt_Username.Text) || string.IsNullOrWhiteSpace(txt_Password.Password))
            {
                MessageBoxResult msg = MessageBox.Show("Please insert login and password", "FoodDiary", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (msg != MessageBoxResult.OK)
                {
                    WelcomeWindow main = new WelcomeWindow();
                    Close();
                    main.ShowDialog();
                }
            }
            else
            {
                SqlQueries query = new SqlQueries();
                if (query.Login(txt_Username.Text, txt_Password.Password) > 0)
                {
                    Global_Methods.username = txt_Username.Text;
                    UserWindow userwindow = new UserWindow();
                    Close();
                    userwindow.ShowDialog();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Wrong login or password", "FoodDiary", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (result != MessageBoxResult.OK)
                    {
                        WelcomeWindow main = new WelcomeWindow();
                        Close();
                        main.ShowDialog();
                    }
                }
            }
        }
    }
}