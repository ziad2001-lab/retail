using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Task.Model;

namespace Task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly productContext _dbcontext;
        public ProductController(productContext context )
        {
            _dbcontext = context;
        }
        [HttpGet]
        public IActionResult GetProduct()
        {

            try
            {
                var products = _dbcontext.Products.ToList();
                if (products.Count == 0)
                {
                    return NotFound("Error");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult SearchProduct(int id)
        {
            try
            {
               
                var product = _dbcontext.Products.Find(id);
                if (product == null)
                {
                    return NotFound($"Product not found with id{id}");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{Name}")]
        public IActionResult SearchProductbyname(string Name)
        {
            try
            {
                if (_dbcontext.Products == null)
                {
                    return NotFound();
                }
                var products = _dbcontext.Products.Where(p => p.Name == Name).ToList();

                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public  IActionResult AddProduct(Product product)
        {
            try
            {
                        if (_dbcontext.Products.Any(p => p.Name == product.Name))
                        {
                            // Return a conflict response (HTTP 409) indicating that the resource already exists
                            return Conflict($"A product with the name '{product.Name}' already exists.");
                        }
                _dbcontext.Products.Add(product);
                _dbcontext.SaveChanges();
                //return CreatedAtAction(nameof(GetProduct), new { id = product.Id, product });
                return Ok("Product Created");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateProduct( Product model)
        {
            if (model == null|| model.Id==0)
            {
                if (model == null)
                  BadRequest();
            }
            try
            {
                if (_dbcontext.Products.Any(p => p.Name == model.Name))
                {
                    // Return a conflict response (HTTP 409) indicating that the resource already exists
                    return Conflict($"A product with the name '{model.Name}' already exists.");
                }
                var product = _dbcontext.Products.Find(model.Id);

                if (product == null)
                {
                    return NotFound($"Product not found with id{model.Id}");
                }
                product.Name = model.Name;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                _dbcontext.SaveChanges();
                return Ok("product updated");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {

            try
            {
                var product = _dbcontext.Products.Find(id);
                if (product == null)
                {
                    return NotFound($"Product not found with id{id}");
                }
                _dbcontext.Products.Remove(product);
                _dbcontext.SaveChanges();
                return Ok("Product deleted");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }



        }
    }
}
