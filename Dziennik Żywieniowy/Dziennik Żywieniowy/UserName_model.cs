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
        private string password;
        private float weight, height, bmi, bmr;
        private int age;
        public string username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Username"));
                    username = value;
                }
            }
        }
        public UserName_model Return_info()
        {
            UserName_model user = new UserName_model();
            string sql = "SELECT Weight,Height,Age FROM Users WHERE Username = @user";
            var command = new SqlCommand(sql, DBconnection.Connection());
            command.Parameters.AddWithValue("@user", username);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) 
            {
                user.weight = reader.GetFloat(0);
                user.height = reader.GetFloat(1);
                user.age = reader.GetInt32(2);
            }
            return user;
        }
    }
}
