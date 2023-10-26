

using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.sales
{
    public interface ISalesPageApi
    {
        GeneralResult<GetAgentProfileInfoResponse> GetAgentProfileInfoRequest(string url, string userId, bool checkStatusCode = true);
        string GetApprovedDepositRequest(string uri, string apiKey, bool checkStatusCode = true);
        string GetSalesPerformanceRequest(string uri, string apiKey, bool checkStatusCode = true);
    }
}