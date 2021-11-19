using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracticeAPI.Models;
using PracticeAPI.Services;

namespace PracticeAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly iService<Product> _productService;

        public ProductsController(iService<Product> ProductService)
        {
            _productService = ProductService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _productService.GetAllWithData();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            return await _productService.GetByID(id);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            return await _productService.Create(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(Guid id, Product product)
        {
            if(id != product.Id)
                return BadRequest();

            var updatedEntity = await _productService.Update(product);

            if(updatedEntity == null)
                return NotFound();

            return updatedEntity;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if(await _productService.GetByID(id) == null)
                return NotFound();

            await _productService.Delete(id);

            return NoContent();
        }
    }
}