using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard
{
    public interface ICreateAffiliateUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ICampaignsPageUi ClickOnSaveButton();
        ICampaignsPageUi CreateCbdAffiliateUiPipe(string url, string userName);
        ICampaignsPageUi CreateAffiliateUiPipe(string url, string userName);
        ICreateAffiliateUi SetAffiliateManager(string affiliateManager);
        ICreateAffiliateUi SetAffiliateName(string affiliateName = null);
        ICreateAffiliateUi SetEmail(string email = null);
        ICreateAffiliateUi SetFirstName(string firstName = null);
        ICreateAffiliateUi SetIpAddresses(string ipAddresses = null);
        ICreateAffiliateUi SetLastName(string lastName = null);
        ICreateAffiliateUi SetOffice(string office = null);
        ICreateAffiliateUi SetPassword(string password = null);
        ICreateAffiliateUi SetPhone(string phone = null);
        ICreateAffiliateUi SetRole(string role = null);
        ICreateAffiliateUi SetUserName(string userName);
    }
}