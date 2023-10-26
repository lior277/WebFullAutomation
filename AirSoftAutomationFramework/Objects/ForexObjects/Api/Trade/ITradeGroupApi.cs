using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Trade
{
    public interface ITradeGroupApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ITradeGroupApi DeleteTradeGroupRequest(string uri, List<string> groupsIds, bool checkStatusCode = true);
        string DeleteTradeGroupRequest(string uri, string groupId, string apiKey = null, bool checkStatusCode = true);
        GetAuditTradeGroupResponse GetTradeGroupsByRevisionIdRequest(string uri, string tradeGroupId, int revisionId);
        GeneralResult<List<TradeGroup>> GetTradeGroupsRequest(string uri, string apiKey = null, bool checkStatusCode = true);
        List<GetTradeGroupsRevisionsResponse> GetTradeGroupsRevisionsRequest(string uri, string tradeGroupId, bool checkStatusCode = true);
        string PostCreateTradeGroupRequest(string uri, List<object> assetsTypes, string groupName, string apiKey = null, bool checkStatusCode = true);
        string PutEditTradeGroupRequest(string uri, string groupId, TradeGroup CryptoGroup, string apiKey = null, bool checkStatusCode = true);
    }
}