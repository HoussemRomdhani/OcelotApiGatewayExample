using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using user.management.api.Models;

namespace user.management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("")]
        public ActionResult<IEnumerable<User>> Get()
        {
            var user = UserManger.GetById(1);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Get(string name, string password)
        {
            var response = UserManger.Login(name, password);
            return Ok(response);
        }
    }
}
