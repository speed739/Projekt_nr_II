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
using LiveCharts;
using LiveCharts.Wpf;
using Dziennik_Żywieniowy.ViewModels;

namespace Dziennik_Żywieniowy
{
    /// <summary>
    /// Logika interakcji dla klasy UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            //Username.Content = UserName.username;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Global_Methods.Exit_method();
        }
        private void log_out_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Login log = new Login();
            log.ShowDialog();
        }
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            Settings setting = new Settings();
            setting.ShowDialog();
        }

        private void add_Product_Click(object sender, RoutedEventArgs e)
        {
            Add_Product add_Product = new Add_Product();
            add_Product.ShowDialog();
        }
    
        
    
    }

}
