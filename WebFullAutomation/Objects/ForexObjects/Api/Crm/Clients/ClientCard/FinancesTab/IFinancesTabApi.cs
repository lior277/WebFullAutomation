// Ignore Spelling: Api psp Crm Forex

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Net.Http;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.FinancesTab
{
    public interface IFinancesTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        int DeleteChargeBackDepositRequest(string url, string clientId, int depositAmount, string depositId, string apiKey = null);
        string DeleteChargeBackDepositRequest(string url, string clientId, int depositAmount, string depositId, string apiKey, bool checkStatusCode = true);
        string DeleteFinanceItemRequest(string url, string financeItemId, string apiKey = null, bool checkStatusCode = true);
        HttpResponseMessage DeleteResetAccountRequest(string url, string clientId);
        IFinancesTabApi DeleteWithdrawalRequest(string url, string withdrawalId, string apiKey = null);
        GeneralResult<GetAvailableWithdrawalByUserIdResponse> GetAvailableWithdrawalByClientIdRequest(string url, string clientId, string apiKey = null, bool checkStatusCode = true);
        List<string> GetCreateDepositCurrenciesRequest(string url);
        GeneralResult<List<GetFinanceDataResponse.FinanceData>> GetFinanceDataRequest(string url, string clientId, string apiKey = null, bool checkStatusCode = true);
        IFinancesTabApi PatchAssignWithdrawalRequest(string url, string withdrawalId, string assignWithdrawalTitle = "fees", string apiKey = null);
        string PatchWithdrawalStatusRequest(string url, string clientId, string withdrawalId, string withdrawalStatus = "approved", string apiKey = null, bool checkStatusCode = true);
        IFinancesTabApi PostBonusRequest(string url, List<string> clientIds, int actualAmount = 1000, string actualMethod = null, string apiKey = null, bool checkStatusCode = true);
        GeneralResult<GeneralDto> PostBonusRequest(string url, string clientId, int actualAmount = 1000, string actualMethod = null, string apiKey = null, bool checkStatusCode = true);
        IFinancesTabApi PostCreateDepositWithWooCommerceUserRequest(string url, string clientName, int depositAmount, string apiKey, string payment_method = "bacs", string currency = "USD", int id = 1572, string product = "Beginner courses");
        List<string> PostDepositRequest(string url, List<string> clientIds, int actualAmount, string lastDigits = null, string transactionType = null, string actualMethod = null, string originalCurrency = null, string nameForMethod = null, string pspInstanceId = null, string pspTransactionId = null, string apiKey = null);
        string PostDepositRequest(string url, string clientId, double actualAmount, string lastDigits = null, string transactionType = null, string actualMethod = null, string originalCurrency = null, string nameForMethod = null, string pspInstanceId = null, string pspTransactionId = null, string apiKey = null, bool checkStatusCode = true);
        IFinancesTabApi PostSplitPendingWithdrawalRequest(string url, string clientId, string withdrawalId, int withdrawalSplitsAmount, string apiKey = null);
        string PostWithdrawalBonusRequest(string url, string clientId, int withdrawalBonusAmount, string apiKey = null, bool checkStatusCode = true);
        string PostWithdrawalDepositRequest(string url, string clientId, string userId, int withdrawalAmount, string currencyCode = "USD", string apiKey = null, bool checkStatusCode = true);
        IFinancesTabApi PutLastDepositDateRequest(string url, string clientId, string lastDepositDate, string apiKey = null);
    }
}