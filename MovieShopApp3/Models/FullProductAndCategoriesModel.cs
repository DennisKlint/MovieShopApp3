using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieShopApp3.Models
{
    public class FullProductAndCategoriesModel
    {
        private List<Categories> productCategories = new List<Categories>();

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string MovieDescription { get; set; }
        public int Price { get; set; }
        public int NrInStore { get; set; }
        public double Rating { get; set; }
        public int ProductTypeID { get; set; }
        public List<Categories> Category
        {
            get { return productCategories; }
            set { productCategories = value; }
        }

        public virtual ProductType ProductType { get; set; }
    }
}