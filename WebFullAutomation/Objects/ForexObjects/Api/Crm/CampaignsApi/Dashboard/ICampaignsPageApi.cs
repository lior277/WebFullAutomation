using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public interface ICampaignsPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string DeleteCampaignRequest(string uri, string CampaignId,
            string apiKey, bool checkStatusCode = true);

        GeneralResult<List<GetCampaignByIdResponse>> GetCampaignsRequest(string url,
             string apiKey = null, bool checkStatusCode = true);

        GeneralResult<GetCampaignByIdResponse> GetCampaignByIdRequest(string url,
             string campaignId, string apiKey = null, bool checkStatusCode = true);
    }
}