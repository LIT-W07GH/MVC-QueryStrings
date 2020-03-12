using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication13.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCountry { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class NorthwindDb
    {
        private readonly string _connectionString;

        public NorthwindDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Order> GetOrders()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Orders";
            connection.Open();
            List<Order> result = new List<Order>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Order
                {
                    Id = (int)reader["OrderId"],
                    OrderDate = (DateTime)reader["OrderDate"],
                    ShipAddress = (string)reader["ShipAddress"],
                    ShipName = (string)reader["ShipName"],
                    ShipCountry = (string)reader["ShipCountry"]
                });
            }

            connection.Close();
            connection.Dispose();
            return result;
        }

        public List<Order> GetOrdersForYear(int year, string shipCountry)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Orders 
WHERE DATEPART(Year, OrderDate) = @year AND ShipCountry = @country";
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@country", shipCountry);
            connection.Open();
            List<Order> result = new List<Order>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Order
                {
                    Id = (int)reader["OrderId"],
                    OrderDate = (DateTime)reader["OrderDate"],
                    ShipAddress = (string)reader["ShipAddress"],
                    ShipName = (string)reader["ShipName"],
                    ShipCountry = (string)reader["ShipCountry"]
                });
            }

            connection.Close();
            connection.Dispose();
            return result;
        }

        public List<Category> GetCategories()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories";
            conn.Open();
            List<Category> categories = new List<Category>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = (int)reader["CategoryId"],
                    Name = (string)reader["CategoryName"],
                    Description = (string)reader["Description"]
                });
            }

            conn.Close();
            conn.Dispose();
            return categories;
        }

        public List<Product> GetProductsForCategory(int categoryId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products WHERE CategoryId = @catID";
            cmd.Parameters.AddWithValue("@catID", categoryId);
            connection.Open();
            List<Product> products = new List<Product>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Product p = new Product
                //{
                //    Id = (int)reader["ProductId"],
                //    Name = (string)reader["ProductName"],
                //    UnitPrice = (decimal)reader["UnitPrice"]
                //};
                //object quantityPerUnit = reader["QuantityPerUnit"];
                //if (quantityPerUnit != DBNull.Value)
                //{
                //    p.QuantityPerUnit = (string)quantityPerUnit;
                //}
                //products.Add(p);

                products.Add(new Product
                {
                    Id = (int)reader["ProductId"],
                    Name = (string)reader["ProductName"],
                    UnitPrice = (decimal)reader["UnitPrice"],
                    QuantityPerUnit = reader.GetOrNull<string>("QuantityPerUnit")
                });
            }

            connection.Close();
            connection.Dispose();
            return products;
        }

        public List<Product> Search(string searchText)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products WHERE ProductName LIKE @searchText";
            cmd.Parameters.AddWithValue("@searchText", $"%{searchText}%");
            connection.Open();
            List<Product> products = new List<Product>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                products.Add(new Product
                {
                    Id = (int)reader["ProductId"],
                    Name = (string)reader["ProductName"],
                    UnitPrice = (decimal)reader["UnitPrice"],
                    QuantityPerUnit = reader.GetOrNull<string>("QuantityPerUnit")
                });
            }

            connection.Close();
            connection.Dispose();
            return products;
        }

        public string GetCategoryName(int catId)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT CategoryName FROM Categories WHERE CategoryId = @catid";
            cmd.Parameters.AddWithValue("@catid", catId);
            connection.Open();
            string name = (string)cmd.ExecuteScalar();
            connection.Close();
            connection.Dispose();
            return name;
        }
    }

    public static class Extensions
    {
        public static T GetOrNull<T>(this SqlDataReader reader, string column)
        {
            object obj = reader[column];
            if (obj == DBNull.Value)
            {
                return default(T);
            }

            return (T)obj;
        }
    }
}
