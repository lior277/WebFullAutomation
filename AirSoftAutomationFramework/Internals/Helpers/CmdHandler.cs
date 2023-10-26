using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public static class CmdHandler
    {
        public static string ExecuteCmdCommandAndGetTheOutPut(string cmdCommand)
        {
            var procStartInfo = new ProcessStartInfo("cmd", "/c " + cmdCommand);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            var proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            var result = proc.StandardOutput.ReadToEnd();
            var userName = Regex.Replace(result.Split(@"\")[1], @"\n|\r", "");
            proc.WaitForExit();

            return result;
        }
    }
}
