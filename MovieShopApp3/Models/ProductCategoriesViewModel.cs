﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieShopApp3.Models
{
    /**
     * ProductCategoriesViewModel - This class will be holding the needed data from products, and their linked categories
     *                              You need to first get the data of the product you need, use the product ID to find linked
     *                              Categories in the relations table, then take the ID of the Correct Categories to the
     *                              Category table, and then save the right category into the Category List
     */
    public class ProductCategoriesViewModel
    {
        private List<Categories> productCategories = new List<Categories>();

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int ProductTypeID { get; set; }
        public List<Categories> Cagetory
        {
            get { return productCategories; }
            set { productCategories = value; }
        }

    }
}