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
using System.Data.SqlClient;
using System.Data;
using LiveCharts.Helpers;

namespace Dziennik_Żywieniowy
{
    /// <summary>
    /// Logika interakcji dla klasy UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        UserName_model user_model = new UserName_model();
        private double LBM, Fat;
        public UserWindow()
        {
            InitializeComponent();
            username.Content = Global_Methods.username;
            user_model = user_model.Return_info(Global_Methods.username);
        }
        /*Wzory na skład ciała dla Mężczyzny*/
        private void UpdatePie_ChartValues_Man()
        {
            LBM = (user_model.Weight * 0.278) + (0.22 * user_model.Height);
            Fat = user_model.Weight - LBM;

            List<double> list_LBM = new List<double>() { LBM };
            List<double> list_Fat = new List<double>() { Fat };
            part_green.Values = list_LBM.AsChartValues();
            part_yellow.Values = list_Fat.AsChartValues();

        }
        /*Wzory na skład ciała dla Kobiety*/
        private void UpdatePie_ChartValues_Woman()
        {
            LBM = (user_model.Weight * 0.384) + (0.129 * user_model.Height);
            Fat = user_model.Weight - LBM;

            List<double> list_LBM = new List<double>() { LBM };
            List<double> list_Fat = new List<double>() { Fat };
            part_green.Values = list_LBM.AsChartValues();
            part_yellow.Values = list_Fat.AsChartValues();
        }
        private void UpdateHalfdounat_ChartValues()
        {
            int id_diary;
            decimal kcal_value;

            string sql = "Select ID_Diary From Diary Where ID_User = @user";
            var command_diary = new SqlCommand(sql, DBconnection.Connection());
            command_diary.Parameters.AddWithValue("@user", user_model.ID_User);
            id_diary = (int)command_diary.ExecuteScalar();

            string sql1 = "SELECT COUNT(*) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
            var command_count = new SqlCommand(sql1, DBconnection.Connection());
            command_count.Parameters.AddWithValue("@diary", id_diary);

            if ((int)command_count.ExecuteScalar() > 0)
            {
                string sql2 = "SELECT SUM(Kcal) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
                var command_kcal = new SqlCommand(sql2, DBconnection.Connection());
                command_kcal.Parameters.AddWithValue("@diary", id_diary);

                kcal_value = (decimal)command_kcal.ExecuteScalar();
                half_dounat_chart.Value = (double)(kcal_value / user_model.BMR) * 100;
                half_dounat_chart.Value = Convert.ToDouble(String.Format("{0:0.##}", half_dounat_chart.Value));
            }
            else
            {
                half_dounat_chart.Value = 0;
            }
        }
        private void Charts_Loaded(object sender, RoutedEventArgs e)
        {
            if (user_model.Sex == "Man")
            {
                UpdatePie_ChartValues_Man();
            }
            else
            {
                UpdatePie_ChartValues_Woman();
            }
            UpdateHalfdounat_ChartValues();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Global_Methods.Exit_method();
        }
        private void log_out_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            Close();
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
            half_dounat_chart.Value = (double)Global_Methods.chartvalue * 100;
            half_dounat_chart.Value = Convert.ToDouble(String.Format("{0:0.#}", half_dounat_chart.Value));
        }
    }
}
