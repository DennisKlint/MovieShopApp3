using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieShopApp3.Models
{
    public class ProductsAndUserModel
    {
        private List<Products> products = new List<Products>();

        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public List<Products> Products
        {
            get { return products; }
            set { products = value; }
        }
    }
}