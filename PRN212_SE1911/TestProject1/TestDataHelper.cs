using CRUDProductApp;
using System.Data.SqlClient;

namespace CRUDProductApp.Tests
{
    public static class TestDataHelper
    {
        public static void AddTestProduct(string name, decimal price)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void RemoveTestProduct(string name)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Products WHERE Name = @Name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
