

using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.SATab
{
    public interface ISATabApi
    {
        GetSaBalanceByUserIdResponse GetSaBalanceByClientIdRequest(string url,
            string clientId, string apiKey = null);

        string PostTransferToBalanceRequest(string url, string clientId,
            int amount, string apiKey = null, bool checkStatusCode = true);

        string PostTransferToSavingAccountRequest(string url,
            string clientId, int sAAmount, string apiKey = null, bool checkStatusCode = true);

        ISATabApi CreateSaProfit(string url,
            GetActivitiesResponse getActivitiesResponse,
            int amount, int balance, string actionType = "profit");
    }
}