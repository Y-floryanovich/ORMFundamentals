using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DapperDataAccess
{
    public interface IConnectionProvider
    {
        IDbConnection CreateConnection();
    }
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IConfiguration _configuration;
        public ConnectionProvider(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("Default");
            return new SqlConnection(connectionString);
        }
    }
}
