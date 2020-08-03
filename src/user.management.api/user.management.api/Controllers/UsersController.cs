using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using user.management.api.Models;

namespace user.management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //[HttpGet("")]
        //public ActionResult<IEnumerable<User>> Get()
        //{
        //    var user = UserManager.GetById(1);
        //    if (user == null)
        //        return NotFound();

        //    return Ok(user);
        //}

        [HttpGet("login")]
        public IActionResult Get(string name, [DataType(DataType.Password)] string password)
        {
            var response = UserManager.Login(name);
            return Ok(response);
        }
    }
}
