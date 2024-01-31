using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface ISalesTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        AccountTypeData CreateAutomationAccountTypePipe(string url);
        string CreateSalasAgentSalaryPipe(string url, int depositAmount, string apiKey = null);
        ISalesTabApi DeleteAccountTypeRequest(string url, string accountTypeId, string apiKey = null);
        ISalesTabApi DeleteSalasAgentSalaryRequest(string url, string salasAgentSalaryId, string apiKey = null);
        ISalesTabApi DeleteSavingAccountRequest(string url, string savingAccountTypeId, string apiKey = null);
        GetAccountTypesResponse GetAccountTypesRequest(string url);
        DepositGroupRangeResponse GetDepositRangeRequest(string url);
        GetRecaptchaResponse GetRecaptchaRequest(string url);
        List<GeneralDto> GetSalasAgentSalaryRequest(string url);
        GetSavingAccountResponse GetSavingAccountsRequest(string url, string apiKey = null);
        ISalesTabApi PostCreateAccountTypeRequest(string url, string accountTypeName = null, string apiKey = null);
        string PostCreateDepositRangePipe(string url);
        ISalesTabApi PostCreateDepositRangeRequest(string url);
        string PostCreateSavingAccountRequest(string url, bool isDefault = false, string apiKey = null);
        string PostSalasAgentSalaryRequest(string url, int depositAmount, string apiKey = null);
        ISalesTabApi PutAccountTypeRequest(string url, AccountTypeData accountTypeData, string apiKey = null);
        ISalesTabApi PutEditSavingAccountRequest(string url, string savingAccountId, string savingAccountName, string apiKey);
    }
}