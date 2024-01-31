using System;

namespace AirSoftAutomationFramework.Internals.DAL.Logger
{
    public interface ILog4Net
    {
        void Debug(string message);
        void Error(string message, Exception ex = null);
        void Info(string message);
    }
}