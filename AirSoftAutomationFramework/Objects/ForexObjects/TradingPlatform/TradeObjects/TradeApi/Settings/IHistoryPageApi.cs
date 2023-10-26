using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings
{
    public interface IHistoryPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IHistoryPageApi PatchSignDodFileRequest(string url,
            int transactionId, string dodId, GetLoginResponse loginData);

        Transactions GetTransactionsRequest(string url,
            GetLoginResponse loginData);
    }
}