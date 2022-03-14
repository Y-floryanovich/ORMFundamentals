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
        private IProductRepository<Product> _productRepository; 

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IProductRepository<Product> productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            var id = 1;
            var product = new Product { Name = "First", Description = "First", Height = 10, Length = 10, Weight = 10, Width = 10 };
            var product2 = new Product { Name = "Second", Description = "Second", Height = 20, Length = 20, Weight = 20, Width = 20 };
            var insert = await _productRepository.InsertAsync(product);
            var insert2 = await _productRepository.InsertAsync(product2);
            var update = await _productRepository.UpdateAsync(new Product { Id = 1, Name = "First", Description = "UpdateFirst", Height = 10, Length = 10, Weight = 10, Width = 10 });
            var getById = await _productRepository.GetByIdAsync(id);

            var rng = new Random();

            var all = _productRepository.GetAll().ToList();
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
