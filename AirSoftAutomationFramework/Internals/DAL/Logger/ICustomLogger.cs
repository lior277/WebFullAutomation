namespace AirSoftAutomationFramework.Internals.DAL.Logger
{
    public interface ICustomLogger
    {
        string GetCallingMethodName(string expectedMethodName);
        void Log(string text);
    }
}