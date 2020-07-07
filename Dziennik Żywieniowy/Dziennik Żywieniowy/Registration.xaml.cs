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
            if (txt_user.Text.Length > 0)
            {
                string sql;
                sql = "SELECT COUNT(*) FROM Users WHERE Username = @user";
                var command = new SqlCommand(sql, DBconnection.Connection());
                command.Parameters.AddWithValue("@user", txt_user.Text);

                int results = (int)command.ExecuteScalar();
                DBconnection.Connection_Close(DBconnection.Connection());
                if (results > 0)
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
            if (txt_pass.Password.Length == 0)
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
                if (x.Text.Length == 0)
                    result = false;
            });
            return result;
        }
        private void btn_exit_Click(object sender, RoutedEventArgs e) => Global_Methods.Exit_method();
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btn_registration_Click(object sender, RoutedEventArgs e)
        {
            if (Check_login() == true && Check_password() == true && Check_required_fields() == true)
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
            string sql = "INSERT INTO Users(Username,Password,Weight,Height,Sex,Age,ActivityLevel,BMI,BMR)" +
                "VALUES (@user,HASHBYTES('SHA1','@password'),@weight,@height,@sex,@age,@active,@bmi,@bmr)";
            var command = new SqlCommand(sql, DBconnection.Connection());
            command.Parameters.AddWithValue("@user", txt_user.Text);
            command.Parameters.AddWithValue("@password", txt_pass.Password);
            command.Parameters.AddWithValue("@weight", weight);
            command.Parameters.AddWithValue("@height", height);
            command.Parameters.AddWithValue("@sex", Sex_Value());
            command.Parameters.AddWithValue("@active", Activity_Value());
            command.Parameters.AddWithValue("@age", (int)age);
            command.Parameters.AddWithValue("@bmi", bmi);
            command.Parameters.AddWithValue("@bmr", bmr);
            command.ExecuteNonQuery();

            string sql1 = "INSERT INTO Diary(ID_User) VALUES (@id)";
            var command1 = new SqlCommand(sql1, DBconnection.Connection());
            command1.Parameters.AddWithValue("@id", Global_Methods.ReturnID_User(txt_user.Text));
            command1.ExecuteNonQuery();

            DBconnection.Connection_Close(DBconnection.Connection());
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
            string result = "";
            activity_chbox_list.ForEach(x =>
            {
                if (x.IsChecked == true)
                    result = x.Content.ToString();
            });
            return result;
        }
        private void btn_count_Click(object sender, RoutedEventArgs e)
        {
            if (txt_height.Text.Length > 0 && txt_weight.Text.Length > 0 && txt_age.Text.Length > 0)
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
