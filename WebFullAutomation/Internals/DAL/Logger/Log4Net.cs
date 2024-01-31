using System;
using System.Reflection;

namespace AirSoftAutomationFramework.Internals.DAL.Logger
{
    public class Log4Net : ILog4Net
    {
        private readonly log4net.ILog _logger;
        public Log4Net()
        {
            _logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        }
        public void Debug(string message)
        {
            _logger?.Debug(message);
        }
        public void Info(string message)
        {
            _logger?.Info(message);
        }
        public void Error(string message, Exception ex = null)
        {
            _logger?.Error(message, ex?.InnerException);
        }
    }
}
