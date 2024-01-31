using OpenQA.Selenium;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public interface IGeneral
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        decimal PostCurrencyConversionRequest(string url,
            double amont, string fromCurrency, string toCurrency);

        double GetRandomDoubleNumber(double minimum = 0, double maximum = 1);

        IGeneral SwitchToExistingWindow(TabToSwitch tabToSwitch);
    }
}