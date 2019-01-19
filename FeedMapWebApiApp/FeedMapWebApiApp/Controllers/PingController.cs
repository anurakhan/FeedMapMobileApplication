using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        /// <summary>
        /// This method is for the user to validate that his current token has expired/not
        /// in regards to the api.
        /// </summary>
        /// <returns>The post.</returns>
        [HttpPost]
        public ActionResult Post()
        {
            return Ok();
        }
    }
}
