using Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ADO
{
    public class ProductRep
    {
        private readonly string _connectionString;

        public ProductRep(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Product product)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var query = "Insert into Product " +
                        "(Name, Description, Weight, Height, Width, Length) " +
                        "values (@Name, @Description , @Weight, @Height, @Width, @Length); " +
                        "SELECT SCOPE_IDENTITY();";
            var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@Weight", product.Weight);
            cmd.Parameters.AddWithValue("@Height", product.Height);
            cmd.Parameters.AddWithValue("@Width", product.Width);
            cmd.Parameters.AddWithValue("@Length", product.Length);
            product.Id = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Dispose();
            con.Close();
        }

        public Product Read(int id)
        {
            Product product = null;

            using var con = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Product WHERE Id = " + id;
            var cmd = new SqlCommand(query, con);
            con.Open();
            var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                product = new Product();
                product.Id = Convert.ToInt32(rdr["Id"]);
                product.Name = rdr["Name"].ToString();
                product.Description = rdr["Description"].ToString();
                product.Weight = Convert.ToInt32(rdr["Weight"]);
                product.Height = Convert.ToInt32(rdr["Height"]);
                product.Width = Convert.ToInt32(rdr["Width"]);
                product.Length = Convert.ToInt32(rdr["Length"]);
            }
            cmd.Dispose();
            con.Close();
            return product;
        }

        public void Update(Product product)
        {
            using var con = new SqlConnection(_connectionString);
            var query = "update Product " +
                        "set Name = @Name, " +
                        "Description = @Description, " +
                        "Weight = @Weight, " +
                        "Height = @Height, " +
                        "Width = @Width, " +
                        "Length = @Length " +
                        "where Id = @Id";
            var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", product.Id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@Weight", product.Weight);
            cmd.Parameters.AddWithValue("@Height", product.Height);
            cmd.Parameters.AddWithValue("@Width", product.Width);
            cmd.Parameters.AddWithValue("@Length", product.Length);
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }

        public void Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            var query = "delete from Product where Id = " + id;
            var cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }

        public IEnumerable<Product> Read()
        {
            var products = new List<Product>();
            using var con = new SqlConnection(_connectionString);
            var query = "select * from Product";
            var cmd = new SqlCommand(query, con);
            con.Open();
            var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var product = new Product();
                product.Id = Convert.ToInt32(rdr["Id"]);
                product.Name = rdr["Name"].ToString();
                product.Description = rdr["Description"].ToString();
                product.Weight = Convert.ToInt32(rdr["Weight"]);
                product.Height = Convert.ToInt32(rdr["Height"]);
                product.Width = Convert.ToInt32(rdr["Width"]);
                product.Length = Convert.ToInt32(rdr["Length"]);
                products.Add(product);
            }
            cmd.Dispose();
            con.Close();
            return products;
        }

        public void Delete()
        {
            using var con = new SqlConnection(_connectionString);
            var query = "delete from Product";
            var cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }
}
