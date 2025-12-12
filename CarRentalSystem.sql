IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CarRentalDb')
    BEGIN
        CREATE DATABASE CarRentalDb;
    END

USE CarRentalDb

CREATE TABLE Users
(
  userId INT IDENTITY(1,1) NOT NULL,
  username VARCHAR(200) NOT NULL,
  userpassword VARCHAR(200) NOT NULL,
  role INT NOT NULL,
  PRIMARY KEY (userId)
);

CREATE TABLE [dbo].[Customers] (
    [cusId]   INT           IDENTITY (1, 1) NOT NULL,
    [cusName] VARCHAR (200) NOT NULL,
	[cusAdd]  VARCHAR (200) NOT NULL,
    [phone]   VARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([cusId] ASC)
);

CREATE TABLE [dbo].[Cars] (
    [carId]      INT           IDENTITY (1, 1) NOT NULL,
    [brand]      VARCHAR (200) NOT NULL,
    [model]      VARCHAR (200) NOT NULL,
    [category]   VARCHAR (200) NOT NULL,
    [available]  VARCHAR (200) NOT NULL,
    [price] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([carId] ASC)
);

CREATE TABLE Features
(
    featureId INT IDENTITY(1,1) NOT NULL,
    featureName VARCHAR(200) NOT NULL,
    featurePrice INT NOT NULL,
    PRIMARY KEY (featureId)
);

CREATE TABLE [dbo].[Fuels]
(
    fuelId INT IDENTITY(1,1) NOT NULL,
    fuelName VARCHAR(200) NOT NULL,
    fuelPrice INT NOT NULL
    PRIMARY KEY (fuelId)
)

CREATE TABLE [dbo].[Bookings]
(
  bookingId INT IDENTITY(1,1) NOT NULL,
  fromDate DATE NOT NULL,
  toDate DATE NOT NULL,
  status VARCHAR(50) NOT NULL,
  carId INT NOT NULL,
  cusId INT NOT NULL,
  totalCost DECIMAL NOT NULL,
  description VARCHAR(250) NULL,

  PRIMARY KEY (bookingId),
  FOREIGN KEY (carId) REFERENCES Cars(carId),
  FOREIGN KEY (cusId) REFERENCES Customers(cusId)
);

CREATE TABLE [dbo].[Invoices] (
    [invoiceId]     INT  IDENTITY (1, 1) NOT NULL,
    [dateEstablish] DATE NOT NULL,
    [totalAmount]   INT  NOT NULL,
    [bookingId]     INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([invoiceId] ASC),
    UNIQUE NONCLUSTERED ([bookingId] ASC),
    FOREIGN KEY ([bookingId]) REFERENCES [dbo].[Bookings] ([bookingId])
);

CREATE TABLE Schedules
(
  fromPlace VARCHAR(200) NOT NULL,
  toPlace VARCHAR(200) NOT NULL,
  scheduleId INT IDENTITY(1,1) NOT NULL,
  dateDelay DATE NULL,
  dateReturn DATE NULL,
  fineCost INT NULL,
  totalCost INT NULL,
  carId INT NOT NULL,
  bookingId INT NOT NULL,
  PRIMARY KEY (scheduleId),
  FOREIGN KEY (carId) REFERENCES Cars(carId),
  FOREIGN KEY (bookingId) REFERENCES Bookings(bookingId)
);

SET IDENTITY_INSERT [dbo].[Cars] ON
INSERT INTO [dbo].[Cars] ([carId], [brand], [model], [category], [available], [price]) VALUES (4, N'Audi', N'Mini', N'4 Seat', N'YES', 300)
INSERT INTO [dbo].[Cars] ([carId], [brand], [model], [category], [available], [price]) VALUES (5, N'Audi', N'Sedan', N'4 Seat', N'YES', 350)
INSERT INTO [dbo].[Cars] ([carId], [brand], [model], [category], [available], [price]) VALUES (6, N'Audi', N'Hatchback', N'4 Seat', N'YES', 400)
INSERT INTO [dbo].[Cars] ([carId], [brand], [model], [category], [available], [price]) VALUES (7, N'Audi', N'CUV High roar', N'5 Seat', N'YES', 450)
INSERT INTO [dbo].[Cars] ([carId], [brand], [model], [category], [available], [price]) VALUES (8, N'Audi', N'SUV High roar', N'7 Seat', N'YES', 500)
INSERT INTO [dbo].[Cars] ([carId], [brand], [model], [category], [available], [price]) VALUES (9, N'Audi', N'MPV Low roar', N'7 Seat', N'YES', 550)
INSERT INTO [dbo].[Cars] ([carId], [brand], [model], [category], [available], [price]) VALUES (10, N'Audi', N'Pickup Truck', N'Pickup Truck', N'YES', 600)
SET IDENTITY_INSERT [dbo].[Cars] OFF


INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Map', 200);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Bluetooth', 50);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Rearview Camera', 120);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Side-view Camera', 120);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Dashboard Camera', 120);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Speed Alert', 40);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Tire Pressure Sensor', 80);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Collision Sensor', 90);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Sunroof', 150);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('GPS Navigation', 100);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('USB Port', 30);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('All-Wheel Drive', 200);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('Pickup Truck Bed Cover', 200);
INSERT INTO [dbo].[Features] ([featureName], [featurePrice]) VALUES ('360-degree Camera', 150);

INSERT INTO [dbo].[Fuels] ([fuelName], [fuelPrice]) VALUES ('Gasoline', 150), ('Diesel', 140), ('Electric', 100);

--------------------------------------------------------
-- Users
--------------------------------------------------------
CREATE INDEX IX_Users_Username ON Users(username);

--------------------------------------------------------
-- Customers
--------------------------------------------------------
CREATE INDEX IX_Customers_cusName ON Customers(cusName);
CREATE INDEX IX_Customers_phone ON Customers(phone);

--------------------------------------------------------
-- Cars
--------------------------------------------------------
CREATE INDEX IX_Cars_Brand ON Cars(brand);
CREATE INDEX IX_Cars_Category ON Cars(category);
CREATE INDEX IX_Cars_Available ON Cars(available);
CREATE INDEX IX_Cars_Brand_Category ON Cars(brand, category);

--------------------------------------------------------
-- Bookings
--------------------------------------------------------
CREATE INDEX IX_Bookings_carId ON Bookings(carId);
CREATE INDEX IX_Bookings_cusId ON Bookings(cusId);
CREATE INDEX IX_Bookings_bookingDate ON Bookings(fromDate, toDate);
CREATE INDEX IX_Bookings_status ON Bookings(status);

--------------------------------------------------------
-- Invoices
--------------------------------------------------------
CREATE INDEX IX_Invoices_dateEstablish ON Invoices(dateEstablish);

--------------------------------------------------------
-- Schedules
--------------------------------------------------------
CREATE INDEX IX_Schedules_carId ON Schedules(carId);
CREATE INDEX IX_Schedules_bookingId ON Schedules(bookingId);
CREATE INDEX IX_Schedules_from_to ON Schedules(fromPlace, toPlace);