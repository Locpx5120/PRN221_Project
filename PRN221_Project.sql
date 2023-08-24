create database PRN221_Project
GO

use PRN221_Project
GO

-- TableFood
Create table TableFood
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
	status NVARCHAR(100) NOT NULL 	--trống || có người 
)
GO

--Account
Create table Account
(
	id INT IDENTITY PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL,
	UserName NVARCHAR(100) NOT NULL,
	Password NVARCHAR(1000) NOT NULL,
	Type INT NOT NULL DEFAULT 0		--1: ADMIN, 0: STAFF
)
GO

-- FoodCategory
Create table FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL
)
-- FOOD
Create table Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL,
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0

	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

-- Bill
Create table Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT getDate(),
	DateCheckOut DATE,
	idTable INT NOT NULL,
	Status INT NOT NULL,		--1: LÀ ĐÃ THANH TOÁN && 0: CHƯA THANH TOÁN

	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

--BillDetails
Create Table BillDetails
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO


--insert account
INSERT INTO Account (DisplayName, UserName, Password, Type)
VALUES ('Pham Loc', 'admin', '123', 1);
INSERT INTO Account (DisplayName, UserName, Password, Type)
VALUES ('Pham Vinh', 'emp', '123', 0);
