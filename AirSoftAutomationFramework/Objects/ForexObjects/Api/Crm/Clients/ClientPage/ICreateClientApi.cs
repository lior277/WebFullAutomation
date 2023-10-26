// Ignore Spelling: Forex Api Crm app referance Perfix

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage
{
    public interface ICreateClientApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        List<string> CreateClientRequest(string url, List<string> clientsNames, string currency = null, string emailPrefix = null, string phone = null, string phone2 = null, string country = null, string gmtTimezone = null, string password = null, string apiKey = null, bool checkStatusCode = true, string freeText = null, string freeText2 = null, string freeText3 = null, string freeText4 = null, string freeText5 = null);
        string CreateClientRequest(string url, string firstName, string lastName = null, string currency = null, string emailPrefix = null, string emailSuffix = null, string phone = null, string phone2 = null, string country = null, string gmtTimezone = null, string password = null, string apiKey = null, bool checkStatusCode = true, string freeText = null, string freeText2 = null, string freeText3 = null, string freeText4 = null, string freeText5 = null);
        string CreateClientWithCampaign(string url, string clientName, string campaignId, string freeText = null, string currency = null, string emailPrefix = null, string country = null, string apiKey = null, bool checkStatusCode = true, string note = "Automation note", string freeText2 = null, string freeText3 = null, string freeText4 = null, string freeText5 = null, string subCampaign = null);
        List<string> GetClientCurrenciesRequest(string url);
        Dictionary<string, string> GetClientsIds();
        ICreateClientApi GetLogoutTreadingPlatformRequest(string uri, GetLoginResponse loginData);
        List<GetPixelResponse> GetPixelRequest(string url, string referanceId, int expectedNumOfPixels);
        GeneralDto PostCreateLoginLinkRequest(string url, string clientId, string apiKey = null);
        string RegisterClientWithCampaign(string url, string country = null, string clientName = null, string emailPrefix = null, string campaignId = null, string referanceId = null, string actualUrlParams = null);
        string RegisterClientWithPromoCode(string url, string country = null, string clientName = null, string referanceId = null, string actualUrlParams = null, string promoCode = "promoCode");
    }
}