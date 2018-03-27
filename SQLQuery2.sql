select Products.*,Categories.CategoryName FROM Products,Categories,ProdCat
WHERE Categories.CategoryID = ProdCat.CategoryID 
AND ProdCat.ProductID = Products.ProductID