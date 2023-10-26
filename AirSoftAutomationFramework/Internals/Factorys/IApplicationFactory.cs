using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Internals.Factory
{
    public interface IApplicationFactory
    {
        T ChangeContext<T>(IWebDriver driver = null) where T : class;
    }
}
