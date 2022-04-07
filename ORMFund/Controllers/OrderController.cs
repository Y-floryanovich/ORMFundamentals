using System.Collections.Generic;
using System.Linq;
using DapperDataAccess;
using Domain;
using EFDataAccess;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private IOrderRepository _repository;

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            var orders = _repository.Read();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = _repository.Read().FirstOrDefault(x=> x.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public ActionResult<Product> PostOrder([FromBody] Order order)
        {
            _repository.ExecuteAsyncWithInsert(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public ActionResult PutOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var toUpdate = _repository.Read().FirstOrDefault(x=>x.Id == id);
            if (toUpdate != null)
            {
                _repository.ExecuteAsyncWithUpdate(toUpdate);
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            var order = _repository.Read().FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            _repository.Delete(order.Id);

            return NoContent();
        }
    }
}
