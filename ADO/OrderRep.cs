using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace ADO
{
    public class OrderRep
    {
        private readonly string _connectionString;

        public OrderRep(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Order order)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            var query = "Insert into [Order] " +
                        "(Status, CreatedDate, UpdatedDate, ProductId) " +
                        "values (@Status, @CreatedDate , @UpdatedDate, @ProductId); " +
                        "SELECT SCOPE_IDENTITY();";
            var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Status", (int)order.Status);
            cmd.Parameters.AddWithValue("@CreatedDate", order.CreatedDate);
            cmd.Parameters.AddWithValue("@UpdatedDate", order.UpdatedDate);
            cmd.Parameters.AddWithValue("@ProductId", order.ProductId);
            order.Id = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Dispose();
            con.Close();
        }

        public Order Read(int id)
        {
            Order order = null;

            using var con = new SqlConnection(_connectionString);
            var query = "SELECT * FROM [Order] WHERE Id = " + id;
            var cmd = new SqlCommand(query, con);
            con.Open();
            var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                order = new Order();
                order.Id = Convert.ToInt32(rdr["Id"]);
                order.Status = (Status)rdr["Status"];
                order.CreatedDate = Convert.ToDateTime(rdr["CreatedDate"]);
                order.UpdatedDate = Convert.ToDateTime(rdr["UpdatedDate"]);
                order.ProductId = Convert.ToInt32(rdr["ProductId"]);
            }
            cmd.Dispose();
            con.Close();
            return order;
        }

        public void Update(Order order)
        {
            using var con = new SqlConnection(_connectionString);
            var query = "update [Order] " +
                        "set Status = @Status, " +
                        "CreatedDate = @CreatedDate, " +
                        "UpdatedDate = @UpdatedDate, " +
                        "ProductId = @ProductId " +
                        "where Id = @Id";
            var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", order.Id);
            cmd.Parameters.AddWithValue("@Status", (int)order.Status);
            cmd.Parameters.AddWithValue("@CreatedDate", order.CreatedDate);
            cmd.Parameters.AddWithValue("@UpdatedDate", order.UpdatedDate);
            cmd.Parameters.AddWithValue("@ProductId", order.ProductId);
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }

        public void Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            var query = "delete from [Order] where Id = " + id;
            var cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }

        public IEnumerable<Order> Read(int? month = null,
            Status? status = null,
            int? year = null,
            int? productId = null)
        {
            var orders = new List<Order>();
            using var con = new SqlConnection(_connectionString);
            var cmd = new SqlCommand("GetOrders", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@status", (int?)status);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@productId", productId);
            con.Open();
            var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var order = new Order();
                order.Id = Convert.ToInt32(rdr["Id"]);
                order.Status = (Status)rdr["Status"];
                order.CreatedDate = Convert.ToDateTime(rdr["CreatedDate"]);
                order.UpdatedDate = Convert.ToDateTime(rdr["UpdatedDate"]);
                order.ProductId = Convert.ToInt32(rdr["ProductId"]);
                orders.Add(order);
            }
            cmd.Dispose();
            con.Close();
            return orders;
        }

        public void Delete(int? month = null,
            Status? status = null,
            int? year = null,
            int? productId = null)
        {

            using var con = new SqlConnection(_connectionString);
            SqlTransaction transaction = con.BeginTransaction();


            var cmd = new SqlCommand("DeleteOrders", con);
            cmd.Transaction = transaction;
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@status", (int?)status);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@productId", productId);
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
            }
            
        }
    }
}
