using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _service;
        public ShoppingCartController(IShoppingCartService service)
        {
            _service = service;
        }
        // GET api/shoppingcart
        [HttpGet]
        public ActionResult<IEnumerable<ShoppingItem>> Get()
        {
            var items = _service.GetAllItems();
            return Ok(items);
        }
        // GET api/shoppingcart/5
        [HttpGet("{id}")]
        public ActionResult<ShoppingItem> Get(Guid id)
        {
            var item = _service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        // POST api/shoppingcart
        [HttpPost]
        public ActionResult Post([FromBody] ShoppingItem value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _service.Add(value);
            return CreatedAtAction("Get", new { id = item.Id }, item);
        }
        // DELETE api/shoppingcart/5
        [HttpDelete("{id}")]
        public ActionResult Remove(Guid id)
        {
            var existingItem = _service.GetById(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            _service.Remove(id);
            return Ok();
        }
    }
}