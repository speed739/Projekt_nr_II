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

namespace Dziennik_Żywieniowy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }
        private void bnt_Signup(object sender, RoutedEventArgs e)
        {
            Login log_In = new Login();
            Close();
            log_In.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Global_Methods.Exit_method();
        private void bnt_registration_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            Close();
            reg.ShowDialog();
        }
    }
}
