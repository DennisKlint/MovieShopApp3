using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieShopApp3.Models
{
    public class OrderAndProductsModel
    {
        private List<Products> products = new List<Products>();


        public int OrderID { get; set; }
        public string UserID { get; set; }
        public bool OrderSent { get; set; }
        public Nullable<System.DateTime> OrderSentDate { get; set; }
        public string OrderDateTime { get; set; }
        public List<Products> Product
        {
            get { return products; }
            set { products = value; }
        }

    }
}