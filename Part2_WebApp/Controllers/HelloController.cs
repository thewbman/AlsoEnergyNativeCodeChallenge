using Part2_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Part2_WebApp.Controllers
{
    public class HelloController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(bool forceError = false)
        {
            if (forceError)
                return InternalServerError(new Exception("Forced error"));
            else
                return Ok(new HelloClass());
        }
    }
}
