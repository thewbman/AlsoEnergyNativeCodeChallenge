using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part1_ConsoleApp
{
    class ServerResponseLogClass
    {
        public DateTime start_time;
        public DateTime? end_time;
        public Int16 status_code;       //can be small int to save on DB
        public string response_text;
        public ErrorCodeEnum error_code;      //can be small int to save on DB
    }
}
