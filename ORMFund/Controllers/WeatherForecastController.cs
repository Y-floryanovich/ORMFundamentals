using DapperDataAccess;
using Domain;
using EFDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORMFund.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConnectionProvider _connectionProvider;
        private IProductRepository<Product> _productRepository;
        private IOrderRepository _orderRepository;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IProductRepository<Product> productRepository, IConnectionProvider connectionProvider, IOrderRepository orderRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _connectionProvider = connectionProvider;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            var id = 1;
            //var product = new Product { Name = "First", Description = "First", Height = 10, Length = 10, Weight = 10, Width = 10 };
            //var product2 = new Product { Name = "Second", Description = "Second", Height = 20, Length = 20, Weight = 20, Width = 20 };
            //var insertProduct = await _productRepository.InsertAsync(product);
            //var insertProduct2 = await _productRepository.InsertAsync(product2);
            //var updateProduct = await _productRepository.UpdateAsync(new Product { Id = 1, Name = "First", Description = "UpdateFirst", Height = 10, Length = 10, Weight = 10, Width = 10 });
            //var getById = await _productRepository.GetByIdAsync(id);

            var rng = new Random();

            var all = _productRepository.GetAll().ToList();

            var newOrder = new Order()
            {
                Status = Status.NotStarted,
                CreatedDate = System.DateTime.Now,
                UpdatedDate = System.DateTime.Now.AddMinutes(10),
                ProductId = 1,
            };
            var insertOrder = await _orderRepository.ExecuteAsyncWithInsert(newOrder);
            var updateOrder = await _orderRepository.ExecuteAsyncWithUpdate(new Order()
            {
                Status = Status.Unloading,
                CreatedDate = System.DateTime.Now,
                UpdatedDate = System.DateTime.Now.AddMinutes(10),
                ProductId = 1,
            });
            var getByIdOrder = await _orderRepository.QueryFirst<Order>();


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
