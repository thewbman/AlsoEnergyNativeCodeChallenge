using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Part1_ConsoleApp
{
    class Program
    {

        static int Main(string[] args)
        {
            DateTime start = DateTime.UtcNow;
            Log("Starting Console: " + start.ToString());

            DataModel d = new DataModel();

            foreach (var c in ConfigurationManager.AppSettings)
            {
                Log("We have app.config value for '" + c.ToString() + "' with value '" + ConfigurationManager.AppSettings[c.ToString()] + "'");
            }

            Log("");

            var app = new CommandLineApplication();
            var argList = app.Option("-l | --list <value>", "List of numbers seperated by commas (e.g. '--list 1,2,3,4,5')", CommandOptionType.SingleValue);
            var argGetEndpoint = app.Option("-g | --get <value>", "API endpoint to call a GET request to (e.g. '--get http://worldtimeapi.org/api/timezone/Europe/London.json')", CommandOptionType.SingleValue);
            var argDelayMilliseconds = app.Option("-d | --delay <value>", "Delay interval in milliseconds between printing out numbers (e.g. '--delay 200')", CommandOptionType.SingleValue);
            var argRunSqlTest = app.Option("-r | --runSqlTest", "Run the use cases that simulate API calls of 200, 500 and timeout", CommandOptionType.NoValue);
            app.HelpOption("-? | -h | --help");

            app.OnExecute(() =>
            {
                //Parse list and sum evens
                d.StringListOfNumbers = argList.HasValue() ? argList.Value() : "1,2,3,4,5,6,7,8,9";
                if (d.ParseStringIntoList())
                {
                    Log("Using list of: '" + d.StringListOfNumbers + "' with length " + d.NumberList.Count.ToString());

                    Log("The sum of all even numbers in the list is: " + d.GetEvenSum().ToString());

                    Log("");

                }
                else
                {
                    LogError("Error parsing list into integers, please check your input: '" + d.StringListOfNumbers + "'");

                    return -1;
                }

                //Call REST endpoint
                d.GetEndpoint = argGetEndpoint.HasValue() ? argGetEndpoint.Value() : "http://worldtimeapi.org/api/timezone/Europe/London.json";
                Log("Calling endpoint: " + d.GetEndpoint);
                Log("Response: " + GetWebText(d.GetEndpoint));
                Log("");

                //Print out numbers from list with a delay between each
                //Need to confirm is int 
                int _delay;
                if (!argDelayMilliseconds.HasValue())
                {
                    //Default
                    _delay = 200;
                }
                else if (int.TryParse(argDelayMilliseconds.Value(), out _delay))
                {
                    //Success parsed into int
                }
                else
                {
                    LogError("Error parsing delay milliseconds into integer, please check your input: '" + argDelayMilliseconds.Value() + "'");

                    return -1;
                }

                Task t_user = Task.Factory.StartNew(() =>PrintListWithDelay(d.NumberList.ToArray(), _delay, "User chosen delay " + _delay.ToString() + "ms: ", false));
                Task.WaitAll(t_user);
                Log("");


                //Run multiple threads
                List<Task> taskList = new List<Task>();

                Task t1 = Task.Factory.StartNew(() => 
                {
                    d.StartNewTask(1,"t1");
                    PrintListWithDelay(d.NumberList.ToArray(), 500, "t1 - Delayed 500ms: ", false);
                    d.CompleteTask(1);
                });
                taskList.Add(t1);
                Task t2 = Task.Factory.StartNew(() =>
                {
                    d.StartNewTask(2,"t2");
                    PrintListWithDelay(d.NumberList.ToArray(), 1000, "t2 - Delayed 1000ms in reverse: ", true);
                    d.CompleteTask(2);
                });
                taskList.Add(t2);


                Task.WaitAll(taskList.ToArray());

                Log(d.SummarizeTaskTracker());


                if(argRunSqlTest.HasValue())
                {
                    //Simulate 200, 500 and timeout web calls
                    if (ConfigurationManager.ConnectionStrings["ae_code_challange"] == null)
                    {
                        LogError("Unable to find connection string for 'ae_code_challange' in app.config");
                    }
                    else
                    {
                        using (SqlConnection sqlcon_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ae_code_challange"].ConnectionString))
                        {
                            if (sqlcon_connection.State != System.Data.ConnectionState.Open)
                                sqlcon_connection.Open();

                            //Allow the input enpoint to be used, though it will not give the same 200/500/timeout responses
                            string baseEndpoint = argGetEndpoint.HasValue() ? argGetEndpoint.Value() : "http://localhost:49932/api/Hello";

                            string[] testEndpoints = new string[3];
                            testEndpoints[0] = baseEndpoint;                               //200
                            testEndpoints[1] = baseEndpoint + "?forceError=true";          //500
                            testEndpoints[2] = baseEndpoint + "?forceTimeout=true";        //timeout

                            foreach (string url in testEndpoints)
                            {
                                //Call http request and return log object
                                var logevent = GetWebTextReturningLog(url);

                                Log("Logging web request '" + url + "' to database with status: " + logevent.error_code.ToString());

                                string sql = "";
                                sql = @"spInsert_server_response_log";

                                SqlCommand command = new SqlCommand(sql, sqlcon_connection);
                                command.CommandType = System.Data.CommandType.StoredProcedure;

                                command.Parameters.Add("@start_time", SqlDbType.DateTime2).Value = logevent.start_time;
                                command.Parameters.Add("@end_time", SqlDbType.DateTime2).Value = logevent.end_time;
                                command.Parameters.Add("@status_code", SqlDbType.SmallInt).Value = logevent.status_code;
                                command.Parameters.Add("@response_text", SqlDbType.VarChar).Value = logevent.response_text;
                                command.Parameters.Add("@error_code", SqlDbType.SmallInt).Value = logevent.error_code;

                                command.ExecuteNonQuery();
                            }

                        }
                    }
                }

                Log("");
                Log("Program is complete.  Press any key to quit");
                Console.Read();

                return 0;
            });


            return app.Execute(args);

        }

        static public void Log(string s)
        {
            Console.WriteLine(s);
        }
        static public void LogError(string s)
        {
            Console.WriteLine("ERROR: "+s);
        }

        static public string GetWebText(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                return reader.ReadToEnd();
            }
            catch (UriFormatException uriEx)
            {
                return "URI exception: " + uriEx.Message;
            }
            catch (WebException webEx)
            {
                return "Web exception: " + webEx.Message;
            }
        }

        static public ServerResponseLogClass GetWebTextReturningLog(string url)
        {
            var log = new ServerResponseLogClass();
            log.start_time = DateTime.Now;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000; //5 seconds
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                log.response_text = reader.ReadToEnd();
                log.status_code = (Int16)response.StatusCode;
                log.end_time = DateTime.Now;
                log.error_code = ErrorCodeEnum.Success;
            }
            catch (UriFormatException uriEx)
            {
                log.response_text = "URI exception: " + uriEx.Message;
                log.error_code = ErrorCodeEnum.Error;
            }
            catch (WebException webEx)
            {

                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    log.response_text = "Web exception timeout: " + webEx.Message;
                    log.error_code = ErrorCodeEnum.Timeout;
                }
                else
                {
                    log.response_text = "Web exception: " + webEx.Message;

                    if (webEx.Response != null)
                        log.status_code = (Int16)((HttpWebResponse)webEx.Response).StatusCode;

                    log.error_code = ErrorCodeEnum.Error;
                }
            }

            return log;
        }

        static public int PrintListWithDelay(int[] myIntArray, int myDelayMilliseconds, string textPreface, bool reverseList)
        {
            int ErrorCount = 0;

            if (reverseList)
                myIntArray = myIntArray.Reverse<int>().ToArray();

            foreach(var i in myIntArray)
            {
                Log(textPreface + i.ToString());
                Thread.Sleep(myDelayMilliseconds);
            }


            return ErrorCount;

        }



    }
}
