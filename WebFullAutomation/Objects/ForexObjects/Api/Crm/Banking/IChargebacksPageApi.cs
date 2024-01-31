using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking
{
    public interface IChargebacksPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string GetChargebackDataFromBankingRequest(string url, string apiKey, bool checkStatusCode = true);
        IChargebacksPageApi PatchAssignChargebackToUserRequest(string url, string chargebackId, string userId, string apiKey = null);
        IChargebacksPageApi PostExportChargebacksTableRequest(string url, string clientEmail, string userEmail, string apiKey = null);
    }
}