using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings
{
    public interface IWithdrawalTpApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IWithdrawalTpApi CreateWithdrawalPipeApi(string url, GetLoginResponse loginData, int withdrawalAmount, TestCase.Steps steps = null);
        string PostCancelPendingWithdrawalRequest(string url, int WithdrawalId, GetLoginResponse loginData, bool checkStatusCode = true);
        string PostPendingWithdrawalRequest(string url, GetLoginResponse loginData, int amount = 10, bool checkStatusCode = true);
        string GetAvailableWithdrawalRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true);
        IWithdrawalTpApi SetAmount(int? amount);
    }
}