using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public interface ITradeTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ITradeTabUi VerifyIdColumnExist();
    }
}