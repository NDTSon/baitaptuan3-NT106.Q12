using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class DbHelper
{
    // Chỉnh connectionString theo máy bạn
    public static string ConnectionString = "Server=localhost\\SQLEXPRESS;Database=UserManagerDB;Integrated Security=True;TrustServerCertificate=True;";

    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (byte b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    public static bool UsernameExists(string username)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @u";
            cmd.Parameters.AddWithValue("@u", username);
            conn.Open();
            int c = (int)cmd.ExecuteScalar();
            return c > 0;
        }
    }

    public static void AddUser(string username, string hashedPassword, string email)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = "INSERT INTO Users (Username, PasswordHash, Email) VALUES (@u, @p, @e)";
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", hashedPassword);
            cmd.Parameters.AddWithValue("@e", email);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static bool ValidateUser(string username, string hashedPassword)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = conn.CreateCommand())
        {
            // Đúng tên cột trong SQL là PasswordHash
            cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username=@u AND PasswordHash=@p";
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", hashedPassword);
            conn.Open();
            int c = (int)cmd.ExecuteScalar();
            return c > 0;
        }
    }

    public static string GetEmailByUsername(string username)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT Email FROM Users WHERE Username=@u";
            cmd.Parameters.AddWithValue("@u", username);
            conn.Open();
            object o = cmd.ExecuteScalar();
            return o == null ? string.Empty : o.ToString();
        }
    }
}
