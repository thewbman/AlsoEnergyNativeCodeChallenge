using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public Timer t1;

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
            var argList = app.Option("-l | --list <value>", "List of numbers seperated by commas (e.g.  '--list 1,2,3,4,5')", CommandOptionType.SingleValue);
            var argGetEndpoint = app.Option("-g | --get <value>", "API endpoint to call a GET request to (e.g.  '--get http://worldtimeapi.org/api/timezone/Europe/London.json')", CommandOptionType.SingleValue);
            var argDelayMilliseconds = app.Option("-d | --delay <value>", "Delay interval in milliseconds between printing out numbers (e.g.  '--delay 200')", CommandOptionType.SingleValue);
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
                    Log("Error parsing list into integers, please check your input: '" + d.StringListOfNumbers + "'");

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
                    Log("Error parsing delay milliseconds into integer, please check your input: '" + argDelayMilliseconds.Value() + "'");

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

        static public string GetWebText(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.ContentType = "application/json";
                //request.Accept = "application/json";
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
