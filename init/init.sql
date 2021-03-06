use database;

CREATE TABLE IF NOT EXISTS Users (
	Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	Name varchar(50),
	Password varchar(50)
);

CREATE TABLE IF NOT EXISTS Shops
(
	Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	Name varchar(50),
	Address varchar(50),
	PhoneNumber varchar(12),
	Password varchar(50)
);

CREATE TABLE IF NOT EXISTS Products (
	Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	ShopId INT UNSIGNED,
	Name varchar(50),
	Category varchar(50),
	Description varchar(50),
	Count REAL,
	Cost REAL,
	FOREIGN KEY (ShopId) REFERENCES Shops (Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Checks (
	Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	ShopId INT UNSIGNED,
	UserId INT UNSIGNED,
	ShopName varchar(50),
	ShopAddress varchar(50),
	UserName varchar(50),
	PurchaseDate Date
);

CREATE TABLE IF NOT EXISTS Purchases (
	Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	CheckId INT UNSIGNED,
	Count INT UNSIGNED,
	Cost REAL,
	Name varchar(50),
	Category varchar(50),
	PaymentMethod varchar(50),
	FOREIGN KEY (CheckId) REFERENCES Checks (Id) ON DELETE CASCADE
);