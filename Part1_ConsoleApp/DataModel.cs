using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part1_ConsoleApp
{
    class DataModel

    {

        public DataModel()
        {
            TaskList = new TaskTracker[2];
        }
        private string _StringListOfNumbers;
        public string StringListOfNumbers
        {
            get
            {
                return _StringListOfNumbers;
            }
            set
            {
                _StringListOfNumbers = value;
            }
        }

        public List<Int32> NumberList;

        public bool ParseStringIntoList()
        {
            //Parse the comma seperated string list into int
            //Returns bool true when success, otherwise false
            this.NumberList = new List<int>();
            string[] stringArray = StringListOfNumbers.Split(',');

            foreach (var n in stringArray)
            {
                int i;
                if (Int32.TryParse(n, out i))
                    this.NumberList.Add(i);
                else
                {
                    this.NumberList.Clear();
                    return false;
                }

            }

            return true;

        }

        public int GetEvenSum()
        {
            int s = 0;

            foreach (var n in NumberList)
            {
                if ((n % 2) == 0)
                    s += n;
            }

            return s;
        }

        private string _GetEndpoint;
        public string GetEndpoint
        {
            get
            {
                return _GetEndpoint;
            }
            set
            {
                _GetEndpoint = value;
            }
        }

        private int _DelayMilliseconds;


        public int DelayMilliseconds
        {
            get
            {
                return _DelayMilliseconds;
            }
            set
            {
                _DelayMilliseconds = value;
            }
        }

        public void StartNewTask(int taskNum, string taskDisplayName)
        {
            if (taskNum <= TaskList.Length)
            {
                TaskList[taskNum - 1] = new TaskTracker(taskDisplayName,DateTime.Now);
            }
            else
            {
                throw new IndexOutOfRangeException("taskNum out of range");
            }
        }

        public void CompleteTask(int taskNum)
        {
            if (taskNum <= TaskList.Length)
            {
                TaskList[taskNum - 1].EndTime = DateTime.Now;
            }
            else
            {
                throw new IndexOutOfRangeException("taskNum out of range");
            }
        }

        private TaskTracker[] TaskList;



        class TaskTracker
        {
            public string name;
            public DateTime StartTime;
            public DateTime EndTime;

            public TaskTracker()
            {
            }

            public TaskTracker(string name, DateTime startTime)
            {
                this.name = name;
                StartTime = startTime;
            }
        }

        public string SummarizeTaskTracker()
        {
            //Print out summary of parallel tasks run times
            StringBuilder sb = new StringBuilder();
            int i = 0;
            sb.AppendLine("");

            foreach (var t in TaskList.OrderBy( c => c.EndTime).ToArray())
            {
                sb.AppendLine("Task " + t.name + " started " + t.StartTime.ToString("s") + " and finished " + t.EndTime.ToString() + (i == 0 ? " which was first" : ""));
                i++;
            }


            return sb.ToString();
        }
    }
}
