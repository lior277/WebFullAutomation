using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking
{
    public interface IDepositsPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class; GeneralResult<SearchResultOnBanking> GetDepositByFiltersRequest(string url,
          Dictionary<string, string> filtersValues, string apiKey = null,
          bool checkStatusCode = true);
        GeneralResult<SearchResultOnBanking> GetDepositDataFromBankingRequest(string url, string filterName, string filterValue, string apiKey = null, bool checkStatusCode = true);
        string PatchAssignDepositToUserRequest(string url, string depositId, string userId, string apiKey = null, bool checkStatusCode = true);
        string ExportDepositsTablePipe(string url, string clientEmail, string userEmail, string apiKey = null, bool checkStatusCode = true);
    }
}