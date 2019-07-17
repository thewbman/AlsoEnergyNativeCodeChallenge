using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Part2_WebApp.Controllers;
using Part2_WebApp.Models;
using System.Web.Http.Results;
using System.Web.Http;
using System.Net;
using System.Threading;

namespace Part2_WebApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetHello_ShouldReturnValue()
        {
            var testHello = new HelloClass();
            var controller = new HelloController();

            var actionResult = controller.Get() as IHttpActionResult;
            var contentResult = actionResult as OkNegotiatedContentResult<HelloClass>;
            Assert.IsNotNull(actionResult);

            //Times should be very close
            var ts = DateTime.Parse(contentResult.Content.CurrentDateTime) - DateTime.Parse(testHello.CurrentDateTime);
            Assert.IsTrue( Math.Abs(ts.TotalMilliseconds) < 100);

        }
        [TestMethod]
        public void GetHello_ShouldForceError()
        {
            var controller = new HelloController();

            //Pass forceError = true
            var actionResult = controller.Get(true) as IHttpActionResult;

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(ExceptionResult));

        }
    }
}
