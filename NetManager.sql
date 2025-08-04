-- ===========================================
-- 🎯 NetCafeDB - FULL DATABASE SCRIPT
-- Bao gồm: Users, Roles, Computers, Sessions,
-- Products, Orders, OrderDetails, Payments
-- ===========================================

-- 🗃️ Tạo Database (nếu chưa có)
CREATE DATABASE NetManagement;
GO

USE NetManagement;
GO


-- ========================
-- 📌 1. Roles
-- ========================
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);
GO

-- ========================
-- 📌 2. Users
-- ========================
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100),
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID),
    IsActive BIT DEFAULT 1
);
GO
ALTER TABLE Users
ADD LastModified DATETIME NULL;




CREATE TABLE ComputerTypes (
    TypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(50) NOT NULL,
    HourlyRate DECIMAL(10,2) NOT NULL
);
Go

-- ========================
-- 📌 3. Computers
-- ========================
CREATE TABLE Computers (
    ComputerID INT PRIMARY KEY IDENTITY(1,1),
    ComputerName NVARCHAR(50) NOT NULL,
    TypeID INT FOREIGN KEY REFERENCES ComputerTypes(TypeID),
    Status NVARCHAR(20) CHECK (Status IN ('Available', 'InUse', 'Maintenance'))
);
Go 

-- ========================
-- 📌 4. Sessions
-- ========================
CREATE TABLE Sessions (
    SessionID INT PRIMARY KEY IDENTITY(1,1),
    ComputerID INT FOREIGN KEY REFERENCES Computers(ComputerID),
    StaffID INT FOREIGN KEY REFERENCES Users(UserID),
    StartTime DATETIME NOT NULL,
    EndTime DATETIME,
    TotalCost DECIMAL(10,2)
);
GO

-- ========================
-- 📌 5. Products
-- ========================
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    IsAvailable BIT DEFAULT 1
);
GO

-- ========================
-- 📌 6. Orders
-- ========================
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    StaffID INT FOREIGN KEY REFERENCES Users(UserID),
    OrderTime DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2)
);
GO

-- ========================
-- 📌 7. OrderDetails
-- ========================
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL
);
GO

-- ========================
-- 📌 8. Payments
-- ========================
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    PaymentTime DATETIME DEFAULT GETDATE(),
    PaymentType NVARCHAR(50) CHECK (PaymentType IN ('Session', 'Order')),
    RefID INT NOT NULL, -- ID của Session hoặc Order
    Amount DECIMAL(10,2),
    StaffID INT FOREIGN KEY REFERENCES Users(UserID)
);
GO


ALTER TABLE Orders
ADD SessionID INT FOREIGN KEY REFERENCES Sessions(SessionID);

-- ===============================
-- ✅ 1. Roles (GIỮ NGUYÊN)
-- ===============================
INSERT INTO Roles (RoleName)
VALUES 
('Admin'), 
('Staff');

-- ===============================
-- ✅ 2. Users (10 Users)
-- ===============================
INSERT INTO Users (UserName, PasswordHash, FullName, RoleID, IsActive)
VALUES
('admin', 'hash_admin', N'Admin Chính', 1, 1),
('staff1', 'hash1', N'Nguyễn Văn A', 2, 1),
('staff2', 'hash2', N'Trần Thị B', 2, 1),
('staff3', 'hash3', N'Lê Văn C', 2, 1),
('staff4', 'hash4', N'Phạm Minh D', 2, 1),
('staff5', 'hash5', N'Hoàng Thị E', 2, 1),
('staff6', 'hash6', N'Đặng Văn F', 2, 1),
('staff7', 'hash7', N'Ngô Thị G', 2, 1),
('staff8', 'hash8', N'Đỗ Văn H', 2, 1),
('staff9', 'hash9', N'Bùi Thị I', 2, 1);

-- ===============================
-- ✅ 3. Computers (10 PCs)
-- ===============================


-- Loại máy tính
INSERT INTO ComputerTypes (TypeName, HourlyRate)
VALUES 
(N'Thường', 8000),
(N'VIP', 12000),
(N'Gaming', 15000);
Go 
INSERT INTO Computers (ComputerName, TypeID, Status)
VALUES
('PC01', 1, 'Available'),
('PC02', 2, 'InUse'),
('PC03', 1, 'Available'),
('PC04', 3, 'Maintenance'),
('PC05', 2, 'InUse'),
('PC06', 3, 'Available'),
('PC07', 1, 'Available'),
('PC08', 2, 'InUse'),
('PC09', 3, 'Available'),
('PC10', 1, 'Maintenance');


-- ===============================
-- ✅ 4. Products (10 Món ăn/uống)
-- ===============================
INSERT INTO Products (ProductName, Price, IsAvailable)
VALUES
(N'Coca Cola', 10000, 1),
(N'Mì ly Hảo Hảo', 15000, 1),
(N'Bánh Snack', 8000, 1),
(N'Nước suối', 7000, 1),
(N'Cà phê lon', 12000, 1),
(N'Khoai tây chiên', 20000, 1),
(N'Sữa tươi', 12000, 1),
(N'Nước tăng lực', 15000, 1),
(N'Bánh mì kẹp', 18000, 1),
(N'Trà sữa', 22000, 1);

-- ===============================
-- ✅ 5. Sessions (10 phiên chơi)
-- ===============================
INSERT INTO Sessions (ComputerID, StaffID, StartTime, EndTime, TotalCost)
VALUES
(1, 2, '2025-07-07 08:00', '2025-07-07 09:00', 10000),
(2, 3, '2025-07-07 09:10', '2025-07-07 10:00', 12000),
(3, 4, '2025-07-07 10:00', '2025-07-07 11:30', 13500),
(4, 5, '2025-07-07 11:00', '2025-07-07 12:00', 8000),
(5, 6, '2025-07-07 12:15', '2025-07-07 13:00', 11000),
(6, 7, '2025-07-07 13:10', '2025-07-07 14:00', 13000),
(7, 8, '2025-07-07 14:20', '2025-07-07 15:00', 12500),
(8, 9, '2025-07-07 15:30', '2025-07-07 16:00', 10500),
(9, 10, '2025-07-07 16:15', '2025-07-07 17:00', 11500),
(10, 2, '2025-07-07 17:15', NULL, NULL); -- đang chơi

-- ===============================
-- ✅ 6. Orders (10 đơn hàng)
-- ===============================
INSERT INTO Orders (StaffID, OrderTime, TotalAmount)
VALUES
(2, '2025-07-07 08:10', 25000),
(3, '2025-07-07 09:30', 30000),
(4, '2025-07-07 10:30', 20000),
(5, '2025-07-07 11:45', 28000),
(6, '2025-07-07 13:00', 32000),
(7, '2025-07-07 14:10', 18000),
(8, '2025-07-07 15:40', 35000),
(9, '2025-07-07 16:10', 40000),
(10, '2025-07-07 17:00', 17000),
(2, '2025-07-07 17:30', 21000);

-- ===============================
-- ✅ 7. OrderDetails (10 món)
-- ===============================
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
VALUES
(1, 1, 1, 10000),
(1, 2, 1, 15000),
(2, 3, 2, 8000),
(3, 5, 1, 12000),
(4, 6, 1, 20000),
(5, 7, 2, 12000),
(6, 9, 1, 18000),
(7, 10, 1, 22000),
(8, 4, 2, 7000),
(9, 8, 1, 15000);

-- ===============================
-- ✅ 8. Payments (10 thanh toán)
-- ===============================
INSERT INTO Payments (PaymentTime, PaymentType, RefID, Amount, StaffID)
VALUES
('2025-07-07 09:05', 'Session', 1, 10000, 2),
('2025-07-07 10:05', 'Session', 2, 12000, 3),
('2025-07-07 11:35', 'Session', 3, 13500, 4),
('2025-07-07 12:05', 'Session', 4, 8000, 5),
('2025-07-07 13:05', 'Session', 5, 11000, 6),
('2025-07-07 08:15', 'Order', 1, 25000, 2),
('2025-07-07 09:40', 'Order', 2, 30000, 3),
('2025-07-07 10:40', 'Order', 3, 20000, 4),
('2025-07-07 11:50', 'Order', 4, 28000, 5),
('2025-07-07 13:05', 'Order', 5, 32000, 6);
