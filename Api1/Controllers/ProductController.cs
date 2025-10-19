using Api1.DTO;
using Api1.Model;
using Api1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProductController : ControllerBase
    {
        private readonly IProductRepo productRepo;

        public ProductController(IProductRepo productRepo)
        {
            this.productRepo = productRepo;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var products = productRepo.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = productRepo.GetById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(NewProductModel product)
        {
            if (product == null)
                return BadRequest();

            int neww = productRepo.Create(product);

            return  CreatedAtAction("GetById", new { id = neww }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            bool updated = productRepo.Update(id, product);
            return updated ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = productRepo.Delete(id);
            return deleted ? Ok("Deleted Successfully") : NotFound();
        }


    }
}
