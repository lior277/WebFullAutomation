

using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface IBannerTabApi
    {
        GeneralResult<GeneralDto> GetClientBannerRequest(string url, string clientId, bool checkStatusCode = true);
        string PutBannerRequest(string url, string clientId, string bannerId, string apiKey = null, bool checkStatusCode = true);
    }
}