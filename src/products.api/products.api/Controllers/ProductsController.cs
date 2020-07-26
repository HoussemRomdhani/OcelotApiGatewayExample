using Microsoft.AspNetCore.Mvc;
using products.api.Models;

namespace products.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Get()
        {
            var result = BookStore.GetAll();
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public IActionResult Get(int id)
        {
            var result = BookStore.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
