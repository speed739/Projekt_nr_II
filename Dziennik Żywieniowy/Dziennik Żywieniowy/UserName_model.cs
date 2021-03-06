﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Dziennik_Żywieniowy
{
    public class UserName_model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int ID_User, Age;
        public string Username, ActivityLevel;
        private string _sex;
        public EnumSex Sex;
        public double Weight, Height;
        public decimal BMI, BMR;

        //public string username
        //{
        //    get
        //    {
        //        return username;
        //    }
        //    set
        //    {
        //        if (username != value)
        //        {
        //            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Username"));
        //            username = value;
        //        }
        //    }
        //}
        public UserName_model Return_info(string username)
        {
            UserName_model user = new UserName_model();
            string sql = "SELECT ID_User,Username,Weight,Height,Sex,Age,ActivityLevel,BMI,BMR FROM Users WHERE Username = @user";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user", username);
            SqlDataReader reader = command.ExecuteReader();
            DB.Connection_Close(DB.Connection());

            while (reader.Read())
            {
                user.ID_User = reader.GetInt32(0);
                user.Username = reader.GetString(1);
                user.Weight = reader.GetDouble(2);
                user.Height = reader.GetDouble(3);
                user._sex = reader.GetString(4);
                user.Age = reader.GetByte(5);
                user.ActivityLevel = reader.GetString(6);
                user.BMI = reader.GetDecimal(7);
                user.BMR = reader.GetDecimal(8);
            }
            Sex = (user._sex == "Man") ? EnumSex.Man : EnumSex.Woman;

            return user;
        }
    }
}
