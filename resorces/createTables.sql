CREATE TABLE [dbo].[Users] (
     [UserID]       NVARCHAR (128) Primary KEY  NOT NULL,
    	[UserName]      NCHAR (50) NOT NULL,
	[Email]        	NCHAR (50) NOT NULL UNIQUE,
	[Adress]        NCHAR (50) NOT NULL,
	[City]        	NCHAR (50) NOT NULL,
	[ZipCode]       NCHAR (15) NOT NULL
	
);
CREATE TABLE [dbo].[ProductType]
(
	[ProductTypeID] INT NOT NULL PRIMARY KEY IDENTITY, 
    	[ProductTypeName] NCHAR(40) NOT NULL
);
CREATE TABLE [dbo].[Categories]
(
	[CategoryID] 	INT NOT NULL PRIMARY KEY IDENTITY, 
    	[CategoryName] 	NCHAR(40) NOT NULL
);
CREATE TABLE [dbo].[Products]
(
	[ProductID] 	INT NOT NULL PRIMARY KEY IDENTITY, 
    	[ProductName] 	NCHAR(40) NOT NULL,
	[MovieDescription] 	NCHAR(256) NOT NULL,
	[Price] 	INT NOT NULL,
	[NrInStore] 	INT NOT NULL DEFAULT 0,
	[Rating] 	FLOAT NOT NULL,
	[ProductTypeID] INT NOT NULL,
 	CONSTRAINT 	[FK_Products_ProductTypeID] FOREIGN KEY ([ProductTypeID]) REFERENCES [ProductType]([ProductTypeID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[ProdCat]
(
	[ProdCatID] 	INT NOT NULL PRIMARY KEY IDENTITY, 
    	[ProductID] 	INT NOT NULL,
	[CategoryID] 	INT NOT NULL,
	CONSTRAINT 	[FK_ProdCat_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products]([ProductID]) ON DELETE CASCADE,
	CONSTRAINT 	[FK_ProdCat_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [Categories]([CategoryID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Orders]
(
	[OrderID] 	INT NOT NULL PRIMARY KEY IDENTITY, 
	[UserID] 	NVARCHAR (128) NOT NULL,
    	[OrderSent] 	BIT NOT NULL DEFAULT 0,
	[OrderSentDate]	DATE NOT NULL,
    	[OrderDateTime]	DATETIME DEFAULT (GETDATE()) NOT NULL,
	CONSTRAINT 	[FK_Orders_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[ProdOrder]
(
	[ProdOrderID] 	INT NOT NULL PRIMARY KEY IDENTITY, 
    	[ProductID] 	INT NOT NULL,
	[OrderID] 	INT NOT NULL,
	CONSTRAINT 	[FK_ProdOrder_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products]([ProductID]) ON DELETE CASCADE,
	CONSTRAINT 	[FK_ProdOrder_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Orders]([OrderID]) ON DELETE CASCADE
);