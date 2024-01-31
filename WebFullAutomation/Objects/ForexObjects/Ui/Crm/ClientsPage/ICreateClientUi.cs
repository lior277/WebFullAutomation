using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage
{
    public interface ICreateClientUi
    {
        IClientsPageUi ClickOnSaveButton();
        ICreateClientUi SetCountry(string country = null);
        ICreateClientUi SetCurrency(string currency = null);
        ICreateClientUi SetEmail(string email = null);
        ICreateClientUi SetFirstName(string firstName);
        ICreateClientUi SetGmtTimeZone(string gmtTimezone = null);
        ICreateClientUi SetLastName(string lastName = null);
        ICreateClientUi SetPhone(string phone = null);
        IClientsPageUi CreateClientUiPipe(string clientName);
        IClientsPageUi CreateCbdClientUiPipe(string url, string clientName);
        T ChangeContext<T>(IWebDriver driver) where T : class;
    }
}