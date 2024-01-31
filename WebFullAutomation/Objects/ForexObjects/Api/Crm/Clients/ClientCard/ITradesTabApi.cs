using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface ITradesTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;

        ITradesTabApi DeleteTradeRequest(string url, string tradeId);
        string DeleteTradeRequest(string url, string tradeId, string apiKey, bool checkStatusCode = true);
        GeneralResult<List<GetTradesResponse>> GetTradesRequest(string url,
            string clientId, string apiKey = null, bool checkStatusCode = true);

        string PachtEditTradeByIdRequest(string url, string tradeId,
            double swapCommission, double swapLong,
            double swapShort, string status, int commision, string closeAtLoss = null,
            string closeAtProfit = null, string apiKey = null, bool checkStatusCode = true);

        public string PachtEditSwapByTradeIdRequest(string url, string tradeId,
           double swapCommission, double swapLong,
           double swapShort, string apiKey = null, bool checkStatusCode = true);
        ITradesTabApi PachtReOpenTradeRequest(string url, string tradeId);
    }
}