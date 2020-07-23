using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Dziennik_Żywieniowy
{
    public class SqlQueries
    {
        string sql;
        int result;

        private int ReturnUserID(string user)
        {
            int id;
            string sql = "SELECT ID_User FROM Users WHERE Username = @user";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user", user);
            id = (int)command.ExecuteScalar();

            DB.Connection_Close(DB.Connection());
            return id;
        }

        public int Login(string username, string password)
        {
            sql = "SELECT COUNT(*) FROM Users WHERE Username = @user AND Password = HASHBYTES('SHA1','@password')";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user", username);
            command.Parameters.AddWithValue("@password", password);
            result = (int)command.ExecuteScalar();

            DB.Connection_Close(DB.Connection());
            return result;
        }

        public int CheckUser(string username)
        {
            sql = "SELECT COUNT(*) FROM Users WHERE Username = @user";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user", username);
            result = (int)command.ExecuteScalar();

            DB.Connection_Close(DB.Connection());
            return result;
        }

        public void CreateUser(string user, string pass, double weight, double height, string sex, string activity, double age, double bmi, double bmr)
        {
            sql = "INSERT INTO Users(Username,Password,Weight,Height,Sex,Age,ActivityLevel,BMI,BMR)" +
                "VALUES (@user,HASHBYTES('SHA1','@password'),@weight,@height,@sex,@age,@active,@bmi,@bmr)";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user", user);
            command.Parameters.AddWithValue("@password", pass);
            command.Parameters.AddWithValue("@weight", weight);
            command.Parameters.AddWithValue("@height", height);
            command.Parameters.AddWithValue("@sex", sex);
            command.Parameters.AddWithValue("@active", activity);
            command.Parameters.AddWithValue("@age", (int)age);
            command.Parameters.AddWithValue("@bmi", bmi);
            command.Parameters.AddWithValue("@bmr", bmr);
            command.ExecuteNonQuery();

            DB.Connection_Close(DB.Connection());
            CreateDiary(user);
        }

        private void CreateDiary(string user)
        {
            sql = "INSERT INTO Diary(ID_User) VALUES (@id)";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@id", ReturnUserID(user));
            command.ExecuteNonQuery();

            DB.Connection_Close(DB.Connection());
        }
        public void CreateProduct(string p_name, string protein, string carbs, string fat, string kcal)
        {
            sql = "INSERT INTO Products(ProductName,Protein,Carbohydrates,Fat,Kcal) VALUES(@p_name,@protein,@carbs,@fat,@kcal)";
            SqlCommand command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@p_name", p_name);
            command.Parameters.AddWithValue("@protein", float.Parse(protein));
            command.Parameters.AddWithValue("@carbs ", float.Parse(carbs));
            command.Parameters.AddWithValue("@fat", float.Parse(fat));
            command.Parameters.AddWithValue("@kcal", float.Parse(kcal));
            command.ExecuteNonQuery();

            DB.Connection_Close(DB.Connection());
            MessageBox.Show("Adding your product is compleat", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        public bool ProductExist(string p_name)
        {
            sql = "SELECT COUNT(*) FROM Products WHERE ProductName = @p_name";
            SqlCommand command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@p_name", p_name);
            result = (int)command.ExecuteScalar();
            DB.Connection_Close(DB.Connection());

            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public void AddProductToDiary(int id, string p_name, float kcal)
        {
            sql = "INSERT INTO DiaryDetails(ID_Diary,ID_Product,Kcal)" +
                                "VALUES (@id_diary,@id_product,@kcal)";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@id_diary", ReturnUserIDdiary(id));
            command.Parameters.AddWithValue("@id_product", ReturnProductID(p_name));
            command.Parameters.AddWithValue("@kcal", kcal);
            command.ExecuteNonQuery();

            DB.Connection_Close(DB.Connection());
        }
        private int ReturnProductID(string p_name)
        {
            int id_product;
            sql = "Select ID_Product From Products Where ProductName = @p_name";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@p_name", p_name);
            id_product = (int)command.ExecuteScalar();

            DB.Connection_Close(DB.Connection());
            return id_product;
        }
        public int ReturnUserIDdiary(int user_id)
        {
            int id_diary;
            sql = "Select ID_Diary From Diary Where ID_User = @user";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user", user_id);
            id_diary = (int)command.ExecuteScalar();

            DB.Connection_Close(DB.Connection());
            return id_diary;
        }

        public int CheckDiaryDetails(int id_diary)
        {
            sql = "SELECT COUNT(*) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
            var command_count = new SqlCommand(sql, DB.Connection());
            command_count.Parameters.AddWithValue("@diary", id_diary);
            result = (int)command_count.ExecuteScalar();
            DB.Connection_Close(DB.Connection());

            return result;
        }
        public decimal CalculateKcal(int id_diary)
        {
            decimal kcal_value;
            sql = "SELECT SUM(Kcal) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@diary", id_diary);
            kcal_value = (decimal)command.ExecuteScalar();

            DB.Connection_Close(DB.Connection());
            return kcal_value;
        }
        public void UpdateProduct(string p_name, string protein, string carbs, string fat, string kcal, string old_productname)
        {
            sql = " UPDATE Products SET ProductName = @p_name,Protein = @protein,Carbohydrates = @carbs,Fat = @fat,Kcal = @kcal WHERE ProductName = @old_p_name";
            SqlCommand command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@p_name", p_name);
            command.Parameters.AddWithValue("@protein", float.Parse(protein));
            command.Parameters.AddWithValue("@carbs", float.Parse(carbs));
            command.Parameters.AddWithValue("@fat", float.Parse(fat));
            command.Parameters.AddWithValue("@kcal", float.Parse(kcal));
            command.Parameters.AddWithValue("@old_p_name", old_productname);
            command.ExecuteNonQuery();

            DB.Connection_Close(DB.Connection());
            MessageBox.Show("Updating product - " + old_productname + " is compleat", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public double CalculateKcal(int id_diary, DateTime date)
        {
            double result = 0;
            string sql = "Select SUM(Kcal) From DiaryDetails WHERE AddData = @data AND ID_Diary = @id_diary";
            SqlCommand command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@id_diary", id_diary);
            command.Parameters.AddWithValue("@data", date);

            if (!Convert.IsDBNull(command.ExecuteScalar()))
            {
                return result = Convert.ToDouble(command.ExecuteScalar());
            }
            else
            {
                return result = 0;
            }
        }
        public DateTime CalculateStartDate(int user_id)
        {
            DateTime result;
            string sql = "Select MIN(AddData) From DiaryDetails WHERE ID_Diary = @user_id";
            SqlCommand command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user_id", user_id);

            if (!Convert.IsDBNull(command.ExecuteScalar()))
            {
                return result = Convert.ToDateTime(command.ExecuteScalar());
            }
            else
            {
                return result = DateTime.Today;
            }
        }
        public DateTime CalculateEndDate(int user_id)
        {
            DateTime result;
            string sql = "Select MAX(AddData) From DiaryDetails WHERE ID_Diary = @user_id";
            SqlCommand command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@user_id", user_id);

            if (!Convert.IsDBNull(command.ExecuteScalar()))
            {
                return result = Convert.ToDateTime(command.ExecuteScalar());
            }
            else
            {
                return result = DateTime.Today;
            }
        }
    }
}
