using Api1.Model;
using Api1.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        ICategoryRepo categoryRepo;

        public CategoryController(ICategoryRepo categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = categoryRepo.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var result = categoryRepo.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]

        public IActionResult Create(Category category)
        {
           bool created = categoryRepo.Create(category);

            return created ? CreatedAtAction("GetById", new {id=category.Id},category) : BadRequest();

        }

        [HttpPut("{id}")]

        public IActionResult Update(int id, Category category)
        {
            bool updated = categoryRepo.Update(id, category);
            return updated ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = categoryRepo.Delete(id);
            return deleted ? Ok() : NotFound();
        }

        [HttpGet("{categoryId}/products")]

        public IActionResult GetProductsByCategoryId(int categoryId)
        {
            var products = categoryRepo.GetProductsByCategoryId(categoryId);
            return Ok(products);
        }



    }
}
