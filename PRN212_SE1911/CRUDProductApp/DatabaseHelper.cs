using System.Data.SqlClient;

public static class DatabaseHelper
{
    private static readonly string connectionString = "Server=localhost;Database=NetManagement;User Id=sa;Password=123;TrustServerCertificate=True";

    public static SqlConnection GetConnection()
    {
        var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}
