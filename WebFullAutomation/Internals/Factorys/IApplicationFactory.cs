using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Internals.Factorys
{
    public interface IApplicationFactory
    {
        T ChangeContext<T>(IWebDriver driver = null) where T : class;
    }
}
