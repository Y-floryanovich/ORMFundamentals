using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDataAccess
{
    public interface IOrderRepository
    {
        Task<int> ExecuteAsyncWithInsert<T>(T entity);
        Task<int> ExecuteAsyncWithUpdate<T>(T entity);
        Task<int> ExecuteAsyncWithDelete<T>(T entity);
        Task<Order> QueryFirst<T>();
        IEnumerable<Order> Read(int? month = null, Status? status = null, int? year = null, int? productId = null);
        void Delete(int? month = null, Status? status = null, int? year = null, int? productId = null);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        public OrderRepository(
            IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<int> ExecuteAsyncWithInsert<T>(T entity)
        {
            var sql = @"INSERT INTO [dbo].[Order]
                                ([Status]
                                ,[CreatedDate]
                                ,[UpdatedDate]
                                ,[ProductId])
                      VALUES
                          (@status, @createdDate, @updatedDate, @productId)";

            using var connection = _connectionProvider.CreateConnection();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<int> ExecuteAsyncWithUpdate<T>(T entity)
        {
            var sql = @"UPDATE [dbo].[Order]
                        SET [Status] = @status
                           ,[CreatedDate] = @createdDate
                           ,[UpdatedDate] = @updatedDate
                           ,[ProductId] = @productId
                       WHERE Id = @id";

            using var connection = _connectionProvider.CreateConnection();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<int> ExecuteAsyncWithDelete<T>(T entity)
        {
            var sql = @"DELETE FROM [dbo].[Order]
                       WHERE Id = @id";

            using var connection = _connectionProvider.CreateConnection();
            var result = await connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<Order> QueryFirst<T>()
        {
            var sql = @"SELECT TOP 1 [Id]
                                          ,[Status]
                                          ,[CreatedDate]
                                          ,[UpdatedDate]
                                          ,[ProductId]
                                       FROM [dbo].[Order]";

            using var connection = _connectionProvider.CreateConnection();
            var result = await connection.QueryFirstAsync<Order>(sql);
            return result;
        }

        public IEnumerable<Order> Read(int? month = null, Status? status = null, int? year = null, int? productId = null)
        {
            var orders = new List<Order>();
            using var connection = _connectionProvider.CreateConnection();
            var cmd = new SqlCommand("GetOrders", (SqlConnection)connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@status", (int?)status);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@productId", productId);
            connection.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var order = new Order();
                order.Id = Convert.ToInt32(reader["Id"]);
                order.Status = (Status)reader["Status"];
                order.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                order.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                order.ProductId = Convert.ToInt32(reader["ProductId"]);
                orders.Add(order);
            }

            return orders;
        }
        
        public void Delete(int? month = null, Status? status = null, int? year = null, int? productId = null)
        {
            using var connection = _connectionProvider.CreateConnection();
            var cmd = new SqlCommand("DeleteOrders", (SqlConnection)connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
