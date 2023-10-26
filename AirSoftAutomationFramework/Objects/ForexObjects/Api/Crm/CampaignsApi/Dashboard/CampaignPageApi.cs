// Ignore Spelling: erp Countrys api Forex Crm

using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using static AirSoftAutomationFramework.Objects.DTOs.CreateCampaignRequest;
using Cap = AirSoftAutomationFramework.Objects.DTOs.CreateCampaignRequest.Cap;
using Limitation = AirSoftAutomationFramework.Objects.DTOs.CreateCampaignRequest.Limitation;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public class CampaignPageApi : ICampaignPageApi
    {
        #region Members
        private static string _erpUserId;
        private string _campaignName;
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public CampaignPageApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string PostCreateCampaignRequest(string uri,
            string campaignName, string erpUserId,
            int? leadsNum = null, string userEmail = null,
            string[] blockedCountrys = null,
            bool acceptingLeadsHoursActive = false,
            string acceptingLeadsHoursFrom = "12:00",
            string acceptingLeadsHoursTo = "12:00",
            bool stopTraffic = true, bool sendEmail = true,
            string limitCountry = null, string timeFrame = null,
            string campaignCode = null,
            string apiKey = null, bool checkStatusCode = true)
        {
            string json;
            _apiKey = apiKey ?? _apiKey;

            var minDeposit = new Min_Deposit()
            {
                USD = 0,
                EUR = 0,
                GBP = 0,
                CAD = 0,
                JPY = 0,
                CHF = 0,
                CNY = 0,
                RUB = 0,
                BTC = 0,
                USDT = 0
            };
            //var blockedCountries = new object.First();
            var route = $"{uri}{ApiRouteAggregate.PostCreateCampaign()}?api_key={_apiKey}";

            var limitation = new Limitation()
            {
                country = limitCountry ?? DataRep.UserDefaultCountry,
                timeframe = timeFrame ?? "Daily",
                leads_num = leadsNum ?? 100
            };

            var cap = new Cap
            {
                stop_traffic = stopTraffic,
                send_email = sendEmail,
                limitations = new Limitation[] { limitation },
                email = userEmail ?? "wrongEmail@wrongEmail.com"
            };

            var createCampaignDto = new CreateCampaignRequest
            {
                erp_user_id = erpUserId,
                deal = SetDeal(),
                name = campaignName,
                currency = SetCurrency(),
                cap = cap,
                code = campaignCode,
                blocked_countries = blockedCountrys ?? System.Array.Empty<string>(),
                account_type_id = null,
                payment = 0,
                min_deposit = minDeposit,
                compliance_status = "Active",
                accepting_leads_hours_active = acceptingLeadsHoursActive,
                accepting_leads_hours_from = acceptingLeadsHoursFrom,
                accepting_leads_hours_to = acceptingLeadsHoursTo
            };

            var response = _apiAccess.ExecutePostEntry(route, createCampaignDto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JObject.Parse(json)["_id"].ToString();
            }

            return json;
        }

        public GeneralResult<List<TransactionByCampaignResponse>>
            GetDepositsByCampaignIdRequest(string url,
            string campaignId, string apiKey, bool checkStatusCode = true)
        {
            var generalResult = new GeneralResult<List<TransactionByCampaignResponse>>();
            var route = $"{url}{ApiRouteAggregate.GetDepositsByCampaignId(campaignId)}?api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                generalResult.GeneralResponse = JsonConvert
                    .DeserializeObject<List<TransactionByCampaignResponse>>(json);
            }
            else
            {
                generalResult.Message = json;
            }

            return generalResult;
        }

        public List<GetCampaignByDialerResponse> GetCampaignsByDialerRequest(string url,
            string apiKey)
        {
            var campaigns = new List<string>();
            var expandoObject = new ExpandoObject();
            var route = $"{url}{ApiRouteAggregate.GetCampaignsByDialer()}?api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetCampaignByDialerResponse>>(json);
            //var jsonArray = JArray.Parse(json);
            //jsonArray.ForEach(p => campaigns.Add(p.SelectToken("name").ToString()));

            //return campaigns.Count();
        }

        public List<string> GetCampaignsByAffiliateRequest(string url, string apiKey)
        {
            var campaigns = new List<string>();
            var route = $"{url}{ApiRouteAggregate.GetCampaignsByAffiliate()}?api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;
            var jsonArray = JArray.Parse(json);
            jsonArray.ForEach(p => campaigns.Add(p.SelectToken("_id").ToString()));

            return campaigns;
        }

        public GetCampaignByIdResponse GetCampaignByIdRequest(string url, string CampaignId,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateCampaign()}/{CampaignId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content?.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetCampaignByIdResponse>(json);
        }

        public ICampaignPageApi DeleteCampaignRequest(string url,
            string CampaignId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateCampaign()}/{CampaignId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string ComposePixel(List<string> emailBodyVariables)
        {
            var builder = new StringBuilder();
            string value;
            var baseUrl = $"{DataRep.TesimUrl}{ApiRouteAggregate.SendPixel()}?";

            foreach (var item in emailBodyVariables)
            {
                if (item.Contains('@'))
                {
                    var param = item.Split("@").Last();
                    value = param + "=" + "{PARAM@" + param + "}" + "&";
                    builder.Append(value);
                }
                else
                {
                    value = item + "=" + "{" + item + "}" + "&";
                    builder.Append(value);
                }
            }

            return baseUrl + builder.ToString().TrimEnd('&');
        }

        public string PutCampaignByIdRequest(string url, GetCampaignByIdResponse campaignDada,
            string affiliateId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateCampaign()}/{campaignDada.Id}?api_key={_apiKey}";

            var PutCampaignDto = new
            {
                erp_user_id = affiliateId,
                name = campaignDada.Name,
                deal = SetDeal(),
                code = campaignDada.Code,
                cap = campaignDada.cap,
                blocked_countries = campaignDada.BlockedCountries,
                account_type_id = campaignDada.AccountTypeId,
                currency = SetCurrency(),
                register_pixel = campaignDada.RegisterPixel,
                accepted_deposit_pixel = campaignDada.AcceptedDepositPixel,
                payment = campaignDada.Payment,
                compliance_status = "Active",
                accepting_leads_hours_active = campaignDada.AcceptingLeadsHoursActive,
                accepting_leads_hours_from = campaignDada.AcceptingLeadsHoursFrom,
                accepting_leads_hours_to = campaignDada.AcceptingLeadsHoursTo,
                min_deposit = campaignDada.MinDeposit,
            };
            var response = _apiAccess.ExecutePutEntry(route, PutCampaignDto);

            var json = response
              .Content
              .ReadAsStringAsync()
              .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public ICampaignPageApi SetErpUserId(string erpUserId)
        {
            _erpUserId = erpUserId ?? "New";

            return this;
        }

        public string SetDeal(string deal = null)
        {
            return deal ?? "cpa";
        }

        public string SetCurrency(string currency = null)
        {
            return currency ?? DataRep.DefaultUSDCurrencyName;
        }

        public ICampaignPageApi SetCampaignName(string campaignName)
        {
            _campaignName = campaignName;

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
