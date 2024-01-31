using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Banking
{
    public interface IWithdrawalsPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IWithdrawalsPageApi ExportWithdrawalsTablePipe(string url, string clientEmail, string userEmail, string apiKey = null);
        GeneralResult<SearchResultOnBanking> GetWithdrawalDataFromBankingRequest(string url, string filterName, string filterValue, string apiKey = null, bool checkStatusCode = true);
    }
}