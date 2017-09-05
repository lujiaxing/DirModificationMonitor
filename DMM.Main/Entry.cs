using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMM
{
    public class Entry
    {
        public static void Main(String[] args)
        {
            
            MonitorController.Init();

            foreach(var i in MonitorController.Monitors)
                Console.WriteLine(i.DirectoryToMonitor + " In Monitoring.");

            Thread.Sleep(-1);
        }
    }
}
