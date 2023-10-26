using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public interface IAffiliatesRequestsApi
    {
        List<GetClientRegistrationResponse> GetClientRegistrationRequest(string uri, string campaignId, string apiKey = null);
    }
}