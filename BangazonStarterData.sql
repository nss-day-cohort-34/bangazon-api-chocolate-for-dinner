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
INSERT INTO Computer (PurchaseDate, Make, Manufacturer, IsDeleted) VALUES ( '20180101 12:00:00 AM', 'Alienware','Dell', 0);
INSERT INTO Computer (PurchaseDate, Make, Manufacturer, IsDeleted) VALUES ( '20180724 12:00:00 AM', 'MacBook Air','Apple', 0);
INSERT INTO Computer (PurchaseDate, Make, Manufacturer, IsDeleted) VALUES ( '20171212 12:00:00 AM', 'ZenBook','ASUS', 0);
INSERT INTO Computer (PurchaseDate, Make, Manufacturer, IsDeleted) VALUES ( '20180305 12:00:00 AM', 'Inspiron','Dell', 0);
INSERT INTO Computer (PurchaseDate, Make, Manufacturer, IsDeleted) VALUES ( '20160102 12:00:00 AM', 'IdeaPad','Lenovo', 0);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, IsDeleted) VALUES ( '2016-03-02 12:00:00 AM', '2018-10-30 12:00:00 AM','MacBook Pro','Apple', 0);

-- ComputerEmployee
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (1, 5, '20160301 12:00:00 AM', '20171001 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (2, 6, '20160401 12:00:00 AM', '20170423 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (2, 5, '20171002 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (1, 1, '20180102 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (3, 2, '20180815 12:00:00 AM');
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (4, 3, '20171224 12:00:00 AM' );

-- TrainingProgram
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees, IsDeleted) VALUES ('Creating SMART goals', '20191020 12:00:00 AM', '20191212 12:00:00 AM', 25, 0);
INSERT INTO TrainingProgram ([Name], StartDate, EndDate, MaxAttendees, IsDeleted) VALUES ('Code of Conduct', '20200123 12:00:00 AM', '20200201 12:00:00 AM', 30, 0);

-- EmployeeTraining
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 2);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (2, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (3, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (4, 2); 

-- Customer
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Bill', 'Jarverson', '20170714 12:00:00 AM', '20190822 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Susan', 'Hall', '20150204 12:00:00 AM', '20190619 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Jennifer', 'Timerson', '20181112 12:00:00 AM', '20191020 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('John', 'Jackson', '20140508 12:00:00 AM', '20191112 12:00:00 AM')
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Emily', 'Norlane', '20150211 12:00:00 AM', '20180416 12:00:00 AM')

-- ProductType
INSERT INTO ProductType (Name, IsDeleted) VALUES ('Clothing', 0)
INSERT INTO ProductType (Name, IsDeleted) VALUES ('Home', 0)
INSERT INTO ProductType (Name, IsDeleted) VALUES ('Active', 0)

-- Product
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity, IsDeleted) VALUES (1, 3, 3.99, 'Socks', 'These socks would make a great gift', 14, 0)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity, IsDeleted) VALUES (2, 2, 7.49, 'Digital Clock', 'Simple Design. Tells Time', 31, 0)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity, IsDeleted) VALUES (2, 1, 14.99, 'Mickey Mouse Night Light', 'Unlike Alexander and the terrible horrible no good very bad day, this one works great', 2, 0)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity, IsDeleted) VALUES (1, 5, 13.49, 'Jeans', 'They will keep your feet warm and make you look great', 21, 0)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity, IsDeleted) VALUES (3, 4, 22.49, 'Bike Helmet', 'You cannot put a price on a healthy brain', 8, 0)
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity, IsDeleted) VALUES (3, 2, 3.99, 'Tire Pump', 'Never have a flat', 42, 0)

-- PaymentType
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('3840482041122397', 'Visa', 1, 0)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('2738491039583948', 'MasterCard', 1, 0)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('9345739820374932', 'Visa', 2, 0)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('1638437291939493', 'American Express', 3, 0)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('5849498420943940', 'American Express', 4, 0)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('2348902492503303', 'MasterCard', 4, 0)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('8745938940298202', 'Visa', 4, 0)
INSERT INTO PaymentType (AcctNumber, Name, CustomerId, IsDeleted) VALUES ('2498928520495823', 'MasterCard', 5, 0)

-- Order
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

-- OrderProduct
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