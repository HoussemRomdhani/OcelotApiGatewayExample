using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using products.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace products.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductsController : ControllerBase
    {
        private readonly IHttpContextAccessor _ctxAccessor;
        public ProductsController(IHttpContextAccessor ctxAccessor)
        {
            _ctxAccessor = ctxAccessor ?? throw new ArgumentNullException(nameof(ctxAccessor));
        }
        [HttpGet]
        public IActionResult Get()
        {
            KeyValuePair<string, StringValues>[]
              requestHeaders = _ctxAccessor.HttpContext.Request.Headers.ToArray();

            string headerValue = null;
            foreach (var pair in requestHeaders)
            {
                if (pair.Key == "userId")
                    headerValue = pair.Value;
            }
           
            //  var headerValues = HttpHeadersHelper.ExtractHeaders(requestHeaders);
            var result = BookStore.GetAll(int.Parse(headerValue));
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
