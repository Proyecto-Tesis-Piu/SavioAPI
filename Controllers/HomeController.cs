using System;
using Microsoft.AspNetCore.Mvc;

namespace MonetaAPI.Controllers
{
    [Route("api/external")]
    [ApiController]
    public class HomeController
    {
        public HomeController() { }

        // GET: api/external
        [HttpGet]
        public ActionResult<string> WakeUp()
        {
            return DateTime.Now.ToString();
        }
    }
}
