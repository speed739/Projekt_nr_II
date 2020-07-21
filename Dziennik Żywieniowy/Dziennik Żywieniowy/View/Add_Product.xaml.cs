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
        SqlQueries sql_query = new SqlQueries();
        DataTable dt = new DataTable("Products");
        List<TextBox> textBoxes;
        public Add_Product()
        {
            InitializeComponent();
            user_model = user_model.Return_info(Global_Methods.username);
            Fill_data();
            textBoxes = new List<TextBox>() { txt_carbohydrates, txt_fat, txt_kcal, txt_protein };
        }
        private void Fill_data()
        {
            DataTable products_datatable = new DataTable();
            string sql_comm = "SELECT ProductName,Protein,Carbohydrates,Fat,Kcal,Weight FROM Products";
            SqlCommand command = new SqlCommand(sql_comm, DB.Connection());
            SqlDataAdapter sda = new SqlDataAdapter(command);

            sda.Fill(products_datatable);
            dt = products_datatable;
            Products.ItemsSource = dt.DefaultView;
            DB.Connection_Close(DB.Connection());
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
            if (string.IsNullOrWhiteSpace(txt_productname.Text))
            {
                MessageBox.Show("Field:[Product name] can't be empty", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (CheckProduct(txt_productname.Text))
                {
                    MessageBox.Show("Product with the given name already exists, change the name or update the existing product", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    sql_query.CreateProduct(txt_productname.Text, txt_protein.Text, txt_carbohydrates.Text, txt_fat.Text, txt_kcal.Text);
                    textBoxes.ForEach(x =>
                    {
                        if (!string.IsNullOrWhiteSpace(x.Text))
                        {
                            x.Text = "0";
                        }
                    });
                    Fill_data();
                }
            }
        }
        private bool CheckProduct(string productname)
        {
            return sql_query.ProductExist(productname);
        }
        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Products.UnselectAllCells();
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
                    sql_query.AddProductToDiary(user_model.ID_User, product.ProductName, CalculateKcalValue());
                }
            }
        }
        private float CalculateKcalValue()
        {
            float result, weight;
            if (string.IsNullOrWhiteSpace(txt_weight.Text))
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
        private void Button_BackClick(object sender, RoutedEventArgs e)
        {
            UpdateChartValues();
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => UpdateChartValues();
        private void UpdateChartValues()
        {
            int id_diary = sql_query.ReturnUserIDdiary(user_model.ID_User);
            if (sql_query.CheckDiaryDetails(id_diary) > 0)
            {
                Global_Methods.chartvalue = sql_query.CalculateKcal(id_diary) / user_model.BMR;
            }
        }
        private void OnlyNumbers(object sender, TextCompositionEventArgs e) => e.Handled = Global_Methods.IsTextAllowed(e.Text);
        private void Button_UpdateClick(object sender, RoutedEventArgs e)
        {
            if (Products.SelectedItem == null) // jeśli jest coś zaznaczone
            {
                MessageBox.Show("Please select one, before updating", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txt_productname_update.Text))
                {
                    MessageBox.Show("Field:[Product name] can't be empty", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (txt_productname_update.Text == product.ProductName) //czy nazwa produktu pozostała bez zmian
                    {
                        UpdateProducts();
                    }
                    else
                    {
                        if (CheckProduct(txt_productname_update.Text))
                        {
                            MessageBox.Show("Product with the given name already exists ", "FoodDiary", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            UpdateProducts();
                        }
                    }
                }
            }
        }
        private void UpdateProducts()
        {
            sql_query.UpdateProduct(txt_productname_update.Text, txt_protein_update.Text, txt_carbohydrates_update.Text, txt_fat_update.Text, txt_kcal_update.Text, product.ProductName);
            Fill_data();
        }
    }
}

