using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public interface ICreateAffiliateApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string CreateAffiliateApiPipe(string url, string userName, string roleName = null, string apiKey = null);
        string PostCreateAffiliateRequest(string uri, string apiKey = null);
        ICreateAffiliateApi SetAffiliateManager(string affiliateManager);
        ICreateAffiliateApi SetEmail(string email = null);
        ICreateAffiliateApi SetFirstName(string firstName = null);
        ICreateAffiliateApi SetIpAddresses(string[] ipAddresses = null);
        ICreateAffiliateApi SetLastName(string lastName = null);
        ICreateAffiliateApi SetOfficeId(string url, string office = null);
        ICreateAffiliateApi SetPassword(string password = null);
        ICreateAffiliateApi SetPhone(string phone = null);
        ICreateAffiliateApi SetRole(string role = null);
        ICreateAffiliateApi SetUserName(string userName);
    }
}