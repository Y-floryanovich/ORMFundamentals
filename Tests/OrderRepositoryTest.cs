using DapperDataAccess;
using NUnit.Framework;
using System.Configuration;

namespace Tests
{
    [TestFixture]
    class OrderRepositoryTest
    {
        private IOrderRepository _repository;

        private readonly OrderRepository _orderRepository;
        private const string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ORMFund;Integrated Security=True";

        public OrderRepositoryTest()
        {
            //_orderRepository = new OrderRepository(new ConnectionProvider(new Configuration()));
            //_orderRepository.Delete();
        }
    }
}