using System;
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
        public string Username, Sex, ActivityLevel;
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
            string sql = "SELECT ID_User,Weight,Height,Sex,Age,ActivityLevel,BMI,BMR FROM Users WHERE Username = @user";
            var command = new SqlCommand(sql, DBconnection.Connection());
            command.Parameters.AddWithValue("@user", username);
            SqlDataReader reader = command.ExecuteReader();
            DBconnection.Connection_Close(DBconnection.Connection());

            while (reader.Read())
            {
                user.ID_User = reader.GetInt32(0);
                user.Weight = reader.GetDouble(1);
                user.Height = reader.GetDouble(2);
                user.Sex = reader.GetString(3);
                user.Age = reader.GetByte(4);
                user.ActivityLevel = reader.GetString(5);
                user.BMI = reader.GetDecimal(6);
                user.BMR = reader.GetDecimal(7);

            }
            return user;
        }
    }
}
