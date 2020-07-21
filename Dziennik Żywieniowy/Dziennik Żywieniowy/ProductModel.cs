using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Dziennik_Żywieniowy
{
    public class ProductModel
    {
        public string ProductName { get; set; }
        public float Weight { get; set; }
        public float Kcal { get; set; }
        public float Protein { get; set; }
        public float Carbohydrates { get; set; }
        public float Fat { get; set; }

        /*Funkcje poniżej bedą wykorzystywane w przyszłości*/
        private int Get_ID()
        {
            int id;
            string sql = "SELECT MAX(ID_Product) FROM Products";
            var command = new SqlCommand(sql, DB.Connection());
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                id = Convert.ToInt32(reader[0]);
                DB.Connection_Close(DB.Connection());
                return id;
            }
        }
        public List<ProductModel> GetProducts()
        {
            List<ProductModel> output = new List<ProductModel>();
            int max_id = Get_ID();

            for (int i = 1; i <= max_id; i++)
            {
                output.Add(GetProduct(i));
            }
            return output;
        }
        public ProductModel GetProduct(int i)
        {
            ProductModel output = new ProductModel();
            string sql = "SELECT ProductName,Weight,Kcal,Protein,Carbohydrates,Fat FROM Products WHERE ID_Product = @id";
            var command = new SqlCommand(sql, DB.Connection());
            command.Parameters.AddWithValue("@id", i);
            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                output.ProductName = reader["ProductName"].ToString();
                output.Weight = float.Parse(reader["Weight"].ToString());
                output.Kcal = float.Parse((reader["Kcal"]).ToString());
                output.Protein = float.Parse((reader["Protein"]).ToString());
                output.Carbohydrates = float.Parse((reader["Carbohydrates"]).ToString());
                output.Fat = float.Parse((reader["Fat"]).ToString());

                DB.Connection_Close(DB.Connection());
            }
            return output;
        }
    }
}