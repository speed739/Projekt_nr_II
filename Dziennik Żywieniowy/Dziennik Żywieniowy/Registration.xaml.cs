using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Dziennik_Żywieniowy
{
    /// <summary>
    /// Logika interakcji dla klasy Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {

        SqlQueries query = new SqlQueries();
        List<CheckBox> activity_chbox_list;
        List<TextBox> textBoxes_list;
        private double height, weight, bmi, bmr, age, actv_l = 1.2;
        public Registration()
        {
            InitializeComponent();
            activity_chbox_list = new List<CheckBox>() { chbox_L, chbox_M, chbox_H };
            textBoxes_list = new List<TextBox>() { txt_age, txt_height, txt_weight, txt_BMI, txt_BMR };
        }
        private void chbox_L_Checked(object sender, RoutedEventArgs e)
        {
            actv_l = 1.2;
            chbox_M.IsChecked = false;
            chbox_H.IsChecked = false;
            chbox_L.IsChecked = true;
        }
        private void chbox_M_Checked(object sender, RoutedEventArgs e)
        {
            actv_l = 1.5;
            chbox_L.IsChecked = false;
            chbox_H.IsChecked = false;
            chbox_M.IsChecked = true;
        }
        private void chbox_H_Checked(object sender, RoutedEventArgs e)
        {
            actv_l = 1.8;
            chbox_M.IsChecked = false;
            chbox_L.IsChecked = false;
            chbox_H.IsChecked = true;
        }
        private void chbox_Woman_Checked(object sender, RoutedEventArgs e)
        {
            chbox_Man.IsChecked = false;
            chbox_Woman.IsChecked = true;
        }
        private void chbox_Man_Checked(object sender, RoutedEventArgs e)
        {
            chbox_Woman.IsChecked = false;
            chbox_Man.IsChecked = true;
        }
        private void Only_numbers(object sender, TextCompositionEventArgs e) => e.Handled = Global_Methods.IsTextAllowed(e.Text);
        private bool Check_login()
        {
            if (!string.IsNullOrWhiteSpace(txt_user.Text))
            {
                if (query.CheckUser(txt_user.Text) > 0)
                {
                    MessageBox.Show("Username is taken, please change it ", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }
            return false;
        }
        private bool Check_password()
        {
            if (string.IsNullOrWhiteSpace(txt_pass.Password))
            {
                return false;
            }
            return true;
        }
        private bool Check_required_fields()
        {
            bool result = true;
            textBoxes_list.ForEach(x =>
            {
                if (string.IsNullOrWhiteSpace(x.Text))
                    result = false;
            });
            return result;
        }
        private void btn_exit_Click(object sender, RoutedEventArgs e) => Global_Methods.Exit_method();
        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            List<Window> windows = new List<Window>();
            windows = Application.Current.Windows.OfType<Window>().ToList();
            if (windows.Count == 2)
            {
                Login log = new Login();
                Close();
                log.ShowDialog();
            }
            else
            {
                Close();
            }
        }
        private void btn_registration_Click(object sender, RoutedEventArgs e)
        {
            if (Check_login() && Check_password() && Check_required_fields())
            {
                Create();
                MessageBox.Show("Account has been created", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("All fields are required to complete registration", "FoodDiary", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (message == MessageBoxResult.Cancel)
                {
                    Close();
                }
            }
        }
        private void Create()
        {
            query.CreateUser(txt_user.Text, txt_pass.Password, weight, height, Sex_Value(), Activity_Value(), age, bmi, bmr);
        }
        private string Sex_Value()
        {
            if (chbox_Man.IsChecked == true)
            {
                return "Man";
            }
            return "Woman";
        }
        private string Activity_Value()
        {
            string result = string.Empty;
            activity_chbox_list.ForEach(x =>
            {
                if (x.IsChecked == true)
                    result = x.Content.ToString();
            });
            return result;
        }
        private void btn_count_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_height.Text) && !string.IsNullOrWhiteSpace(txt_weight.Text) && !string.IsNullOrWhiteSpace(txt_age.Text))
            {
                height = Convert.ToDouble(txt_height.Text) / 100;
                weight = Convert.ToDouble(txt_weight.Text);
                age = Convert.ToDouble(txt_age.Text);
                bmi = weight / (height * height); // wzór na BMI
                txt_BMI.Text = bmi.ToString("0.00");

                if (chbox_Woman.IsChecked == true)
                {
                    height = Convert.ToDouble(txt_height.Text);
                    bmr = (655 + (9.6 * weight) + (1.8 * height) - (4.7 * age)) * actv_l; // wzór na BMR dla W
                    txt_BMR.Text = bmr.ToString("0.0" + " kcal");
                }
                if (chbox_Man.IsChecked == true)
                {
                    height = Convert.ToDouble(txt_height.Text);
                    bmr = ((9.99 * weight) + (6.25 * height) - (4.92 * age) + 5) * actv_l; // wzór na BMR dla M
                    txt_BMR.Text = bmr.ToString("0.0" + " kcal");
                }
            }
            else
            {
                MessageBox.Show("Before you can use Calculate, please compleate all fields.", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
