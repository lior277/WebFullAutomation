using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings
{
    public interface ISavingAccountTpApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string PostTransferToSavingAccountFromTpRequest(string url, string clientId, int sAAmount, GetLoginResponse loginData = null, bool checkStatusCode = true);
        string PostTransferToBalanceFromTpRequest(string url, string clientId,
           int amount, GetLoginResponse loginData = null, bool checkStatusCode = true);
    }
}