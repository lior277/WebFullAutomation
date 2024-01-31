using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public static class ProcessHandler 
	{
		public static void NavigateToUrl(string url)
		{
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
        }		
	}
}
