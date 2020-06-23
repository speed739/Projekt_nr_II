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
            //this.Hide();
            reg.chbox_L.IsChecked = true;
            reg.chbox_Man.IsChecked = true;
            reg.ShowDialog();
        }
        private void btn_Submit_Click_1(object sender, RoutedEventArgs e)
        {
            if (true)
            {
                UserWindow main = new UserWindow();
                Close();
                main.ShowDialog();
            }
        }
    }
}
