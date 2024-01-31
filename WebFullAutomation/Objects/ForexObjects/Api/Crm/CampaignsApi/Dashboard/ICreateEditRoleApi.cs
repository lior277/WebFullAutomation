using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public interface ICreateEditRoleApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ICreateEditRoleApi PostAffiliateRoleWithAllPermissionsRequest(string uri);
        ICreateEditRoleApi PostAffiliateRoleWithNoPermissionsRequest(string uri);
    }
}