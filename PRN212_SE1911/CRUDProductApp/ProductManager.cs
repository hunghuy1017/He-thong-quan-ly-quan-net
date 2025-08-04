using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public static class ProductManager
{
    public static void AddProduct(Product product)
    {
        using var conn = DatabaseHelper.GetConnection();
        string query = "INSERT INTO Products (ProductName, Price, IsAvailable) VALUES (@name, @price, @avail)";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@name", product.ProductName);
        cmd.Parameters.AddWithValue("@price", product.Price);
        cmd.Parameters.AddWithValue("@avail", product.IsAvailable);
        cmd.ExecuteNonQuery();
    }

    public static List<Product> GetAllProducts()
    {
        var list = new List<Product>();
        using var conn = DatabaseHelper.GetConnection();
        string query = "SELECT * FROM Products";
        using var cmd = new SqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Product
            {
                ProductID = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Price = reader.GetDecimal(2),
                IsAvailable = reader.GetBoolean(3)
            });
        }
        return list;
    }

    public static void DeleteProduct(int id)
    {
        using var conn = DatabaseHelper.GetConnection();
        string query = "DELETE FROM Products WHERE ProductID = @id";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateProduct(Product product)
    {
        using var conn = DatabaseHelper.GetConnection();
        string query = "UPDATE Products SET ProductName = @name, Price = @price, IsAvailable = @avail WHERE ProductID = @id";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@name", product.ProductName);
        cmd.Parameters.AddWithValue("@price", product.Price);
        cmd.Parameters.AddWithValue("@avail", product.IsAvailable);
        cmd.Parameters.AddWithValue("@id", product.ProductID);
        cmd.ExecuteNonQuery();
    }

    public static Product? GetById(int id)
    {
        using var conn = DatabaseHelper.GetConnection();
        string query = "SELECT * FROM Products WHERE ProductID = @id";
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Product
            {
                ProductID = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Price = reader.GetDecimal(2),
                IsAvailable = reader.GetBoolean(3)
            };
        }
        return null;
    }
}
