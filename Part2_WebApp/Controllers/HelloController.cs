using Part2_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Part2_WebApp.Controllers
{
    public class HelloController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(bool forceError = false, bool forceTimeout = false)
        {
            if (forceError)
                return InternalServerError(new Exception("Forced error"));
            else if(forceTimeout)
            {
                //Sleep for 30 seconds, will cause timout on requests
                Thread.Sleep(30000);
                return Ok(new HelloClass());
            }
            else
                return Ok(new HelloClass());
        }
    }
}
