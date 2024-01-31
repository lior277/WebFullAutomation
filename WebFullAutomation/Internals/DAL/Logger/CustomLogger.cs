using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace AirSoftAutomationFramework.Internals.DAL.Logger
{
    public class CustomLogger : ICustomLogger
    {

        public string filePath = @"C:\temp\Test.txt";
        public void Log(string text)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                filePath = "/tmp/Test.txt"; // dont know why, the report is exported to /tmp/report (something removing the bla). 
            }
            else
            {
                filePath = @"C:\temp\Test.txt";
            }

            using var streamWriter = new StreamWriter(filePath, append: true);
            streamWriter.WriteLine($"{text} {DateTime.Now}");
            streamWriter.Close();
        }

        public string GetCallingMethodName(string expectedMethodName)
        {
            // get call stack
            var stackTrace = new StackTrace();

            // get calling method name
            _ = stackTrace.GetFrames();

            return "";
        }
    }
}

