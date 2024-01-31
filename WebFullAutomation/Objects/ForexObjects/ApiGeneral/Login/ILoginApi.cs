// Ignore Spelling: apikey Crm Forex Api

using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral.Login
{
    public interface ILoginApi
    {
        GeneralResult<CreateLoginResponse> PostLoginCrmRequest(string url, string email, string fingerprint, string password = null, string apikey = null, bool checkStatusCode = true);
        GeneralResult<GetLoginResponse> PostLoginToTradingPlatform(string uri, string expectedEmail, string password = null, bool checkStatusCode = true);
    }
}