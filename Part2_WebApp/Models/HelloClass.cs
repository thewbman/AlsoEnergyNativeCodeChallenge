using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Part2_WebApp.Models
{
    public class HelloClass
    {
        public string CurrentDateTime
        {
            get
            {
                return DateTime.Now.ToString("");
            }
        }
    }
}