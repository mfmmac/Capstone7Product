using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Capstone7ProductAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Capstone7ProductAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("Category/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategoryId(int id)
        {
            var products = await _context.Products.Where(c => c.CategoryId == id).ToListAsync();

            bool isEmpty = !products.Any();

            if (isEmpty)
            {
                return NotFound();
            }

            return products;
        }

        [HttpGet("Price/{maxPrice}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByMaxPrice(decimal maxPrice)
        {
            var products = await _context.Products.Where(c => c.UnitPrice <= maxPrice).ToListAsync();

            bool isEmpty = !products.Any();

            if (isEmpty)
            {
                return NotFound();
            }

            return products;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducts", new { id = product.ProductId }, product);
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}