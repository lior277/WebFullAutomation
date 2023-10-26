// Ignore Spelling: erp Countrys api Forex Crm

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public interface ICampaignPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string ComposePixel(List<string> emailBodyVariables);
        ICampaignPageApi DeleteCampaignRequest(string url, string CampaignId, string apiKey = null);
        GetCampaignByIdResponse GetCampaignByIdRequest(string url, string CampaignId, string apiKey = null);
        List<string> GetCampaignsByAffiliateRequest(string url, string apiKey);
        List<GetCampaignByDialerResponse> GetCampaignsByDialerRequest(string url, string apiKey);
        GeneralResult<List<TransactionByCampaignResponse>> GetDepositsByCampaignIdRequest(string url, string campaignId, string apiKey, bool checkStatusCode = true);
        string PostCreateCampaignRequest(string uri, string campaignName, string erpUserId, int? leadsNum = null, string userEmail = null, string[] blockedCountrys = null, bool acceptingLeadsHoursActive = false, string acceptingLeadsHoursFrom = "12:00", string acceptingLeadsHoursTo = "12:00", bool stopTraffic = true, bool sendEmail = true, string limitCountry = null, string timeFrame = null, string campaignCode = null, string apiKey = null, bool checkStatusCode = true);
        string PutCampaignByIdRequest(string url, GetCampaignByIdResponse campaignDada, string affiliateId, string apiKey = null, bool checkStatusCode = true);
        ICampaignPageApi SetCampaignName(string campaignName);
        string SetCurrency(string currency = null);
        string SetDeal(string deal = null);
        ICampaignPageApi SetErpUserId(string erpUserId);
    }
}