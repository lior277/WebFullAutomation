// Ignore Spelling: App

using System;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApp
{
    public class KillProcess
    {
       public static void Main(string[] args)
        {
            Process.GetProcesses()
               .Where(x => x.ProcessName
               .Contains("chromedriver", StringComparison.CurrentCultureIgnoreCase))
               .ToList()
               .ForEach(x => x.Kill());

            Process.GetProcesses()
                .Where(x => x.ProcessName
                .Contains("geckodriver", StringComparison.CurrentCultureIgnoreCase))
                .ToList()
                .ForEach(x => x.Kill());
        }
    }   
}
