using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using EFDataAccess;
using Microsoft.AspNetCore.Mvc;

namespace ORMFund.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository<Product> _productRepository;

        public ProductController(IProductRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts(int pageNumber = 1, int pageSize = 10,
            int? orderId = null)
        {
            IQueryable<Product> products = _productRepository.GetAll();
            //if (orderId != null)
            //{
            //    products = products.Where(x => x.Orders.Contains(orderId));
            //}

            products = products
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            return Ok(products.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            await _productRepository.InsertAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var toUpdate = await _productRepository.GetByIdAsync(id);
            if (toUpdate != null)
            {
                await _productRepository.UpdateAsync(toUpdate);
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(product);

            return NoContent();
        }
    }
}


