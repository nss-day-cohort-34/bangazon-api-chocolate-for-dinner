USE MASTER
GO

IF NOT EXISTS (
    SELECT [name]
    FROM sys.databases
    WHERE [name] = N'BangazonAPI'
)
CREATE DATABASE BangazonAPI
GO

USE BangazonAPI
GO


CREATE TABLE Department (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL,
	Budget 	INTEGER NOT NULL
);

CREATE TABLE Employee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	DepartmentId INTEGER NOT NULL,
	IsSuperVisor BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_EmployeeDepartment FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
);

CREATE TABLE Computer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	PurchaseDate DATETIME NOT NULL,
	DecomissionDate DATETIME,
	Make VARCHAR(55) NOT NULL,
	Manufacturer VARCHAR(55) NOT NULL
);

CREATE TABLE ComputerEmployee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	ComputerId INTEGER NOT NULL,
	AssignDate DATETIME NOT NULL,
	UnassignDate DATETIME,
    CONSTRAINT FK_ComputerEmployee_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_ComputerEmployee_Computer FOREIGN KEY(ComputerId) REFERENCES Computer(Id)
);


CREATE TABLE TrainingProgram (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	MaxAttendees INTEGER NOT NULL
);

CREATE TABLE EmployeeTraining (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	TrainingProgramId INTEGER NOT NULL,
    CONSTRAINT FK_EmployeeTraining_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_EmployeeTraining_Training FOREIGN KEY(TrainingProgramId) REFERENCES TrainingProgram(Id)
);

CREATE TABLE ProductType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL
);

CREATE TABLE Customer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	CreationDate DATETIME NOT NULL,
	LastActiveDate DATETIME NOT NULL
);

CREATE TABLE Product (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	ProductTypeId INTEGER NOT NULL,
	CustomerId INTEGER NOT NULL,
	Price MONEY NOT NULL,
	Title VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	Quantity INTEGER NOT NULL,
    CONSTRAINT FK_Product_ProductType FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
    CONSTRAINT FK_Product_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);


CREATE TABLE PaymentType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	AcctNumber VARCHAR(55) NOT NULL,
	[Name] VARCHAR(55) NOT NULL,
	CustomerId INTEGER NOT NULL,
    CONSTRAINT FK_PaymentType_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);

CREATE TABLE [Order] (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INTEGER NOT NULL,
	PaymentTypeId INTEGER,
    CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id),
    CONSTRAINT FK_Order_Payment FOREIGN KEY(PaymentTypeId) REFERENCES PaymentType(Id)
);

CREATE TABLE OrderProduct (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	OrderId INTEGER NOT NULL,
	ProductId INTEGER NOT NULL,
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY(ProductId) REFERENCES Product(Id),
    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY(OrderId) REFERENCES [Order](Id)
);

INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Bill', 'Jarverson', '20170714 12:00:00 AM', '20190822 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Susan', 'Hall', '20150204 12:00:00 AM', '20190619 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Jennifer', 'Timerson', '20181112 12:00:00 AM', '20191020 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('John', 'Jackson', '20140508 12:00:00 AM', '20191112 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Emily', 'Norlane', '20150211 12:00:00 AM', '20180416 12:00:00 AM')

INSERT INTO ProductType (Name) VALUES ('Clothing')
INSERT INTO ProductType (Name) VALUES ('Home')
INSERT INTO ProductType (Name) VALUES ('Active')

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (1, 3, 3.99, 'Socks', 'These socks would make a great gift', 14)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (2, 2, 7.49, 'Digital Clock', 'Simple Design. Tells Time', 31)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (2, 1, 14.99, 'Mickey Mouse Night Light', 'Unlike Alexander and the terrible horrible no good very bad day, this one works great', 2)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (1, 5, 13.49, 'Jeans', 'They will keep your feet warm and make you look great', 21)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (3, 4, 22.49, 'Bike Helmet', 'You cannot put a price on a healthy brain', 8)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (3, 2, 3.99, 'Tire Pump', 'Never have a flat', 42)

INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('3840482041122397', 'Visa', 1)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('2738491039583948', 'MasterCard', 1)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('9345739820374932', 'Visa', 2)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('1638437291939493', 'American Express', 3)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('5849498420943940', 'American Express', 4)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('2348902492503303', 'MasterCard', 4)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('8745938940298202', 'Visa', 4)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId) VALUES ('2498928520495823', 'MasterCard', 5)

INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1, 1)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1, null)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (1, 2)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (2, 3)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (2, null)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (3, 4)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (4, 5)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (4, 6)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (4, null)
INSERT INTO [Order] (CustomerId, PaymentTypeId) VALUES (5, 8)

INSERT INTO OrderProduct (OrderId, ProductId) VALUES (1, 1)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (1, 2)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (2, 4)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (2, 5)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (3, 1)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (4, 5)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (5, 1)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (5, 3)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (5, 4)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (6, 3)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (7, 2)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (8, 1)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (8, 6)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (9, 1)
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (10, 3)

---------------------------------------------------------------

-- Department
INSERT INTO Department ([Name], Budget) VALUES ('Sales', 876534);
INSERT INTO Department ([Name], Budget) VALUES ('Customer Service', 56777);
INSERT INTO Department ([Name], Budget) VALUES ('Information Technology', 65400);
-- Employee
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor) VALUES ('Sarah', 'Fleming', 3, 1 );
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor) VALUES ('Brantley', 'Jones', 2,1 );
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor) VALUES ('Noah', 'Bartfield', 1, 1 );
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor) VALUES ('Michelle', 'Jimenez', 2,0 );
-- Computer
INSERT INTO Computer (PurchaseDate, Make, Manufacturer) VALUES ( '20180101 12:00:00 AM', 'Alienware','Dell');
INSERT INTO Computer (PurchaseDate, Make, Manufacturer) VALUES ( '20180724 12:00:00 AM', 'MacBook Air','Apple');
INSERT INTO Computer (PurchaseDate, Make, Manufacturer) VALUES ( '20171212 12:00:00 AM', 'ZenBook','ASUS');
INSERT INTO Computer (PurchaseDate, Make, Manufacturer) VALUES ( '20180305 12:00:00 AM', 'Inspiron','Dell');
INSERT INTO Computer (PurchaseDate, Make, Manufacturer) VALUES ( '20160102 12:00:00 AM', 'IdeaPad','Lenovo');
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer) VALUES ( '2016-03-02 12:00:00 AM', '2018-10-30 12:00:00 AM','MacBook Pro','Apple');
-- ComputerEmployee
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (1, 5, '20160301 12:00:00 AM', '20171001 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (2, 6, '20160401 12:00:00 AM', '20170423 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (2, 5, '20171002 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (1, 1, '20180102 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (3, 2, '20180815 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (4, 3, '20171224 12:00:00 AM' );
-- TrainingProgram
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees) VALUES ('Creating SMART goals', '20191020 12:00:00 AM', '20191212 12:00:00 AM', 25);
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees) VALUES ('Code of Conduct', '20180123 12:00:00 AM', '20180930 12:00:00 AM', 30);
-- EmployeeTraining
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 2);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (2, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (3, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (4, 2); 

