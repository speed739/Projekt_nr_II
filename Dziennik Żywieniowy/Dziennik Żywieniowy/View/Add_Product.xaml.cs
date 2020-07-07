using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data;
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
using Dziennik_Żywieniowy;
using System.Data.SqlClient;

namespace Dziennik_Żywieniowy
{
    /// <summary>
    /// Logika interakcji dla klasy Add_Product.xaml
    /// </summary>
    public partial class Add_Product : Window
    {
        ProductModel product = new ProductModel();
        UserName_model user_model = new UserName_model();
        DataTable dt = new DataTable("Products");
        List<TextBox> textBoxes;
        public Add_Product()
        {
            InitializeComponent();
            user_model = user_model.Return_info(Global_Methods.username);
            Fill_data();
            textBoxes = new List<TextBox>() { txt_carbohydrates, txt_fat, txt_kcal, txt_productname, txt_protein };
        }
        private void Fill_data()
        {
            DataTable products_datatable = new DataTable();
            string sql_comm = "SELECT ProductName,Protein,Carbohydrates,Fat,Kcal,Weight FROM Products";
            SqlCommand command = new SqlCommand(sql_comm, DBconnection.Connection());
            SqlDataAdapter sda = new SqlDataAdapter(command);

            sda.Fill(products_datatable);
            dt = products_datatable;
            Products.ItemsSource = dt.DefaultView;
            DBconnection.Connection_Close(DBconnection.Connection());
        }
        private void DataG_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            List<DataGridCellInfo> cell_infos = new List<DataGridCellInfo>();
            for (int i = 0; i < Products.SelectedCells.Count; i++)
            {
                cell_infos.Add(Products.SelectedCells[i]);
            }
            if (cell_infos.Count > 0 && Products.SelectedIndex > -1)
            {
                product.ProductName = (cell_infos[0].Column.GetCellContent(cell_infos[0].Item) as TextBlock).Text;
                product.Protein = float.Parse((cell_infos[1].Column.GetCellContent(cell_infos[0].Item) as TextBlock).Text);
                product.Carbohydrates = float.Parse((cell_infos[2].Column.GetCellContent(cell_infos[0].Item) as TextBlock).Text);
                product.Fat = float.Parse((cell_infos[3].Column.GetCellContent(cell_infos[0].Item) as TextBlock).Text);
                product.Kcal = float.Parse((cell_infos[4].Column.GetCellContent(cell_infos[0].Item) as TextBlock).Text);
                product.Weight = float.Parse((cell_infos[5].Column.GetCellContent(cell_infos[0].Item) as TextBlock).Text);

                txt_weight.Text = product.Weight.ToString();
                txt_carbohydrates_update.Text = product.Carbohydrates.ToString();
                txt_fat_update.Text = product.Fat.ToString();
                txt_productname_update.Text = product.ProductName.ToString();
                txt_protein_update.Text = product.Protein.ToString();
                txt_kcal_update.Text = product.Kcal.ToString();
            }
        }
        private void Button_AddProductToDataBase_Click(object sender, RoutedEventArgs e)
        {
            if (txt_productname.Text.Length == 0)
            {
                MessageBox.Show("Product name field can't be empty", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string sql_comm = "INSERT INTO Products(ProductName,Protein,Carbohydrates,Fat,Kcal) VALUES(@p_name,@protein,@carbs,@fat,@kcal)";
                SqlCommand command = new SqlCommand(sql_comm, DBconnection.Connection());
                command.Parameters.AddWithValue("@p_name", txt_productname.Text);
                command.Parameters.AddWithValue("@protein", float.Parse(txt_protein.Text));
                command.Parameters.AddWithValue("@carbs ", float.Parse(txt_carbohydrates.Text));
                command.Parameters.AddWithValue("@fat", float.Parse(txt_fat.Text));
                command.Parameters.AddWithValue("@kcal", float.Parse(txt_kcal.Text));
                command.ExecuteNonQuery();

                DBconnection.Connection_Close(DBconnection.Connection());
                MessageBoxResult result = MessageBox.Show("Adding your product is compleat", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                textBoxes.ForEach(x =>
                {
                    if (x.Text.Length > 0)
                    {
                        x.Clear();
                    }
                });
                Fill_data();
            }
        }
        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataTable product_table = dt;
            DataView dv = product_table.DefaultView;
            dv.RowFilter = string.Format("ProductName like '%{0}%'", txt_search.Text);
            Products.ItemsSource = dv;
        }
        private void Button_Add_to_FoodDiary_Click(object sender, RoutedEventArgs e)
        {
            if (Products.SelectedItem == null)
            {
               MessageBox.Show("Please select one, before adding", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want add this product: " + product.ProductName + " ?", "FoodDiary", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    string sql = "INSERT INTO DiaryDetails(ID_Diary,ID_Product,Kcal)" +
                                          "VALUES (@id_diary,@id_product,@kcal)";
                    var command = new SqlCommand(sql, DBconnection.Connection());
                    command.Parameters.AddWithValue("@id_diary", ReturnUserID_Diary());
                    command.Parameters.AddWithValue("@id_product", ReturnProductID());
                    command.Parameters.AddWithValue("@kcal", CalculateKcalValue());
                    command.ExecuteNonQuery();

                    DBconnection.Connection_Close(DBconnection.Connection());
                }
            }
        }
        private float CalculateKcalValue()
        {
            float result, weight;
            if (txt_weight.Text.Length == 0)
            {
                return 0;
            }
            else
            {
                weight = float.Parse(txt_weight.Text) / 100;
                result = product.Kcal * weight;
            }
            return result;
        }
        private int ReturnProductID()
        {
            int id_product;
            string sql = "Select ID_Product From Products Where ProductName = @p_name";
            var command = new SqlCommand(sql, DBconnection.Connection());
            command.Parameters.AddWithValue("@p_name", product.ProductName);
            id_product = (int)command.ExecuteScalar();

            DBconnection.Connection_Close(DBconnection.Connection());
            return id_product;
        }
        private int ReturnUserID_Diary()
        {
            int id_diary;
            string sql = "Select ID_Diary From Diary Where ID_User = @user";
            var command = new SqlCommand(sql, DBconnection.Connection());
            command.Parameters.AddWithValue("@user", user_model.ID_User);
            id_diary = (int)command.ExecuteScalar();

            DBconnection.Connection_Close(DBconnection.Connection());
            return id_diary;
        }
        private void Button_ExitClick(object sender, RoutedEventArgs e)
        {
            int command_result;
            decimal kcal_value;
            int id_diary = ReturnUserID_Diary();

            string sql1 = "SELECT COUNT(*) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
            var command_count = new SqlCommand(sql1, DBconnection.Connection());
            command_count.Parameters.AddWithValue("@diary", id_diary);
            command_result = (int)command_count.ExecuteScalar();
            DBconnection.Connection_Close(DBconnection.Connection());

            if (command_result > 0)
            {
                string sql = "SELECT SUM(Kcal) From DiaryDetails Where ID_Diary = @diary AND AddData = CONVERT(varchar, getdate(), 23)";
                var command = new SqlCommand(sql, DBconnection.Connection());
                command.Parameters.AddWithValue("@diary", id_diary);
                kcal_value = (decimal)command.ExecuteScalar();

                DBconnection.Connection_Close(DBconnection.Connection());
                Global_Methods.chartvalue = kcal_value / user_model.BMR;
                Close();
            }
            else
            {
                Close();
            }
        }
        private void OnlyNumbers(object sender, TextCompositionEventArgs e) => e.Handled = Global_Methods.IsTextAllowed(e.Text);
        private void Button_UpdateClick(object sender, RoutedEventArgs e)
        {
            if (Products.SelectedItem == null)
            {
                MessageBox.Show("Please select one, before updating", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (txt_productname_update.Text.Length == 0)
                {
                    MessageBox.Show("Product name field can't be empty", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string sql_comm = " UPDATE Products SET ProductName = @p_name,Protein = @protein,Carbohydrates = @carbs,Fat = @fat,Kcal = @kcal WHERE ProductName = @old_p_name";
                    SqlCommand command_delete = new SqlCommand(sql_comm, DBconnection.Connection());
                    command_delete.Parameters.AddWithValue("@p_name", txt_productname_update.Text);
                    command_delete.Parameters.AddWithValue("@protein", float.Parse(txt_protein_update.Text));
                    command_delete.Parameters.AddWithValue("@carbs", float.Parse(txt_carbohydrates_update.Text));
                    command_delete.Parameters.AddWithValue("@fat", float.Parse(txt_fat_update.Text));
                    command_delete.Parameters.AddWithValue("@kcal", float.Parse(txt_kcal_update.Text));
                    command_delete.Parameters.AddWithValue("@old_p_name", product.ProductName);
                    command_delete.ExecuteNonQuery();
           
                    DBconnection.Connection_Close(DBconnection.Connection());
                    MessageBox.Show("Updating product - " + product.ProductName + " is compleat", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                    Fill_data();
                }
            }
        }
    }
}
