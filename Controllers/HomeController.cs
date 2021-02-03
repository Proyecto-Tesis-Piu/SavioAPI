using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MonetaAPI.Controllers
{
    [Route("api/external")]
    [ApiController]
    public class HomeController
    {
        public HomeController() { }

        // GET: api/external
        [HttpGet]
        public ActionResult<String> WakeUp()
        {
            return JsonSerializer.Serialize(DateTime.Now.ToString());
        }
    }
}
