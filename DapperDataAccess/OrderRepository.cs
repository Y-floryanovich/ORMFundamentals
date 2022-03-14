using Dapper;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDataAccess
{
    public interface IOrderRepository
    {
        Task<int> ExecuteAsyncWithInsert<T>(T entity);
        Task<int> ExecuteAsyncWithUpdate<T>(T entity);
        Task<int> ExecuteAsyncWithDelete<T>(T entity);
        Task<Order> QueryFirst<T>(T entity);
    }
    public class OrderRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        protected OrderRepository(
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

        public async Task<Order> QueryFirst<T>(T entity)
        {
            var sql = @"SELECT TOP 1 [Id]
                                          ,[Status]
                                          ,[CreatedDate]
                                          ,[UpdatedDate]
                                          ,[ProductId]
                                       FROM [dbo].[Order]
                                       WHERE Id = @id";

            using var connection = _connectionProvider.CreateConnection();
            var result = await connection.QueryFirstAsync<Order>(sql, entity);
            return result;
        }
    }
}
