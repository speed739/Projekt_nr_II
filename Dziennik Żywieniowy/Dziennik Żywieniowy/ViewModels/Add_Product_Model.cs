using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik_Żywieniowy.ViewModels
{
    public class Add_Product_Model : Screen
    {
        BindableCollection<ProductModel> Products { get; set; }
        public Add_Product_Model()
        {
            ProductModel pr = new ProductModel();
            Products = new BindableCollection<ProductModel>(pr.GetProducts());
        }
    }
}

