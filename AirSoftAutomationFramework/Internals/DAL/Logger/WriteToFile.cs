
using AirSoftAutomationFramework.Internals.Factory;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AirSoftAutomationFramework.Internals.DAL.Logger
{
    public class WriteToFile : ApplicationFactory, IWriteToFile
    {
        private string filePath = @"C:\lior\Test.txt";
        public void WriteText(string text)
        {
            try
            {
                //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                //{
                //    filePath = "/tmp/Test.txt"; // dont know why, the report is exported to /tmp/report (something removing the bla). 
                //}
                //else
                //{
                //    filePath = "reports";
                //}
                //Pass the file path and filename to the StreamWriter Constructor
                //C:\temp\
                using var streamWriter = new StreamWriter(filePath, append: true);
                streamWriter.WriteLine($"{text} {DateTime.Now}");
                streamWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
    }
}
