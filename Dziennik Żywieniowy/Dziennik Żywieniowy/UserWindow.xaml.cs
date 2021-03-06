﻿using System;
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
        int helper = 0; // zmienna pomocnicza do wyswietlania tylko pojedyńczego komunikatu 
        public Func<double, string> YFormatter { get; set; }
        int id_diary,range; //zmienna okreslajaca okres czasu pokazany na wykresie
        SqlQueries queries = new SqlQueries();

        public UserWindow()
        {
            InitializeComponent();
            username.Content = Global_Methods.username;
            user_model = user_model.Return_info(Global_Methods.username);
            id_diary = queries.ReturnUserIDdiary(user_model.ID_User);
            CalculateStartAndEndDate();
            RefreshChart();
        }

        private void CalculateStartAndEndDate()
        {
            data_od.DisplayDateStart = queries.CalculateStartDate(id_diary);
            data_od.DisplayDateEnd = queries.CalculateEndDate(id_diary);
            data_do.DisplayDateStart = data_od.DisplayDateStart;
            data_do.DisplayDateEnd = data_od.DisplayDateEnd;
           
            data_od.SelectedDate = data_od.DisplayDateStart;
            data_do.SelectedDate = data_do.DisplayDateEnd;
        }
        private void UpdatePie_ChartValues(EnumSex sex)
        {
            double a, b;

            a = (sex == EnumSex.Man) ? 0.278 : 0.384;
            b = (sex == EnumSex.Man) ? 0.22 : 0.129;

            LBM = (user_model.Weight * a) + (b * user_model.Height);
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
            var command_diary = new SqlCommand(sql, DB.Connection());
            command_diary.Parameters.AddWithValue("@user", user_model.ID_User);
            id_diary = (int)command_diary.ExecuteScalar();

            string sql1 = "SELECT COUNT(*) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
            var command_count = new SqlCommand(sql1, DB.Connection());
            command_count.Parameters.AddWithValue("@diary", id_diary);

            if ((int)command_count.ExecuteScalar() > 0)
            {
                string sql2 = "SELECT SUM(Kcal) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
                var command_kcal = new SqlCommand(sql2, DB.Connection());
                command_kcal.Parameters.AddWithValue("@diary", id_diary);

                kcal_value = (decimal)command_kcal.ExecuteScalar();
                half_dounat_chart.Value = (double)(kcal_value / user_model.BMR) * 100;
                half_dounat_chart.Value = Convert.ToDouble(String.Format("{0:0.#}", half_dounat_chart.Value));
                CompleatDaily();
            }
            else
            {
                half_dounat_chart.Value = 0;
            }
        }
        private void Charts_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePie_ChartValues(user_model.Sex);
            UpdateHalfdounat_ChartValues();
        }
        private void Exit_Click(object sender, RoutedEventArgs e) => Global_Methods.Exit_method();
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
            RefreshChart();
            CompleatDaily();
        }
        private void diary_Click(object sender, RoutedEventArgs e)
        {
            Diary diary = new Diary();
            diary.ShowDialog();

        }
        private void CompleatDaily()
        {
            if (half_dounat_chart.Value >= 100 && helper == 0)
            {
                helper = 1;
                MessageBox.Show("Congratulation !!!  today your daily calories are compleat ", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private ChartValues<double> LoadValuesToLineChart()
        {
            CalculateRange();
            ChartValues<double> values = new ChartValues<double>();

            if (range < 1)
            {
                data_od.SelectedDate = data_od.SelectedDate.Value.Date.AddDays(range - 1);
                CalculateRange();
            }
            if (range > 0)
            {
                string[] Labels = new string[range];
                for (int i = 0; i < range; i++)
                {
                    double result = queries.CalculateKcal(id_diary, data_od.SelectedDate.Value.AddDays(i));
                    Labels[i] = data_od.SelectedDate.Value.AddDays(i).ToShortDateString();
                    values.Add(result);
                    DB.Connection_Close(DB.Connection());

                }
                axisX.Labels = Labels;
            }
            return values;
        }
        private void CalculateRange()
        {
            range = data_do.SelectedDate.Value.Day - data_od.SelectedDate.Value.Day + 1;
        }
        private void RefreshChart()
        {
            SeriesCollection SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Eaten calories",
                    Values  = LoadValuesToLineChart()

                }
            };
            YFormatter = value => value.ToString();
            linechart.Series = SeriesCollection;
            DataContext = this;
        }
        private void data_od_CalendarClosed(object sender, RoutedEventArgs e)
        {
            RefreshChart();
        }
    }
}
