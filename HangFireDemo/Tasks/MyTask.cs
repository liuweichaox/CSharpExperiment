using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFireDemo.Tasks
{
    public class MyTask
    {
        public void DoWork()
        {
            Console.WriteLine("MyTask DoWork " + DateTime.Now);
        }
    }
}
