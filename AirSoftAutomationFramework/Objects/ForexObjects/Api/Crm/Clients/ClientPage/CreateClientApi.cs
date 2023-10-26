// Ignore Spelling: Forex Api Crm app referance Perfix

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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage
{
    public class CreateClientApi : ICreateClientApi
    {
        public CreateClientApi(IApplicationFactory appFactory,
          IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        #region Members
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private string _password = DataRep.Password;
        //private string _apiRoutePostCreateClient = Config.GetValue(nameof(Config.ApiRoutePostCreateClient));
        private readonly IApplicationFactory _apiFactory;
        private Dictionary<string, string> _ClientsIds = new Dictionary<string, string>();
        private IWebDriver _driver;
        #endregion Members

        public string CreateClientRequest(string url, string firstName,
            string lastName = null, string currency = null, string emailPrefix = null,
            string emailSuffix = null, string phone = null, string phone2 = null,
            string country = null, string gmtTimezone = null, string password = null,
            string apiKey = null, bool checkStatusCode = true, string freeText = null,
            string freeText2 = null, string freeText3 = null,
            string freeText4 = null, string freeText5 = null)
        {
            string json;
            _apiKey = apiKey ?? _apiKey;
            emailPrefix ??= DataRep.EmailPrefix;
            var route = $"{url}{ApiRouteAggregate.PostCreateClient()}?api_key={_apiKey}";
            emailSuffix ??= firstName;
            var email = emailSuffix + emailPrefix;
            lastName ??= firstName;

            var reqDto = new CreateClientRequest
            {
                country = country ?? "afghanistan",
                currency_code = currency ?? DataRep.DefaultUSDCurrencyName,
                email = email,
                first_name = firstName,
                last_name = lastName,
                phone = phone ?? DataRep.UserDefaultPhone,
                phone_2 = phone2 ?? DataRep.UserDefaultPhone,
                gmt_timezone = gmtTimezone ?? "04:30",
                password = password ?? DataRep.Password,
                free_text = "free_text",
                free_text_2 = "free_text2",
                free_text_3 = "free_text3",
                free_text_4 = "free_text4",
                free_text_5 = "free_text5",
            };

            var response = _apiAccess.ExecutePostEntry(route, reqDto);

            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JsonConvert.DeserializeObject<GeneralDto>(json)
                    .ClientId;
            }

            return json;
        }

        public List<string> CreateClientRequest(string url,
            List<string> clientsNames, string currency = null,
            string emailPrefix = null, string phone = null,
            string phone2 = null, string country = null,
            string gmtTimezone = null, string password = null,
            string apiKey = null, bool checkStatusCode = true, string freeText = null,
            string freeText2 = null, string freeText3 = null,
            string freeText4 = null, string freeText5 = null)
        {
            var ids = new List<string>();

            foreach (var name in clientsNames)
            {
                var response = CreateClientRequest(url, name, currency, apiKey: apiKey);

                ids.Add(response);
            }

            return ids;
        }

        public string CreateClientWithCampaign(string url, string clientName,
           string campaignId, string freeText = null, string currency = null,
           string emailPrefix = null, string country = null, string apiKey = null,
           bool checkStatusCode = true, string note = "Automation note",
           string freeText2 = null, string freeText3 = null, string freeText4 = null,
           string freeText5 = null, string subCampaign = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateClient()}?api_key={_apiKey}";
            emailPrefix ??= DataRep.EmailPrefix;
            var email = clientName + emailPrefix;
            string json;

            var reqDto = new CreateClientWithCampaignRequest
            {
                country = country ?? "afghanistan",
                currency_code = currency ?? DataRep.DefaultUSDCurrencyName,
                email = email,
                first_name = clientName,
                free_text = freeText ?? DataRep.ClientFreeText,
                free_text_2 = freeText2 ?? "free_text2",
                free_text_3 = freeText3 ?? "free_text3",
                free_text_4 = freeText4 ?? "free_text4",
                free_text_5 = freeText5 ?? "free_text5",
                last_name = clientName,
                phone = "22254445",
                gmt_timezone = "04:30",
                password = DataRep.Password,
                campaign_id = campaignId,
                sub_campaign_key = subCampaign,
                note = note
            };

            var response = _apiAccess.ExecutePostEntry(route, reqDto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JsonConvert.DeserializeObject<GeneralDto>(json)
                    .ClientId;
            }

            return json;
        }

        public string RegisterClientWithPromoCode(string url, string country = null,
            string clientName = null, string referanceId = null,
            string actualUrlParams = null, string promoCode = "promoCode")
        {
            clientName ??= TextManipulation.RandomString();
            var route = url + ApiRouteAggregate.PostRegisterClient();
            var email = clientName + DataRep.EmailPrefix;

            var registerClientDto = new
            {
                country = country ?? "afghanistan",
                currency_code = DataRep.DefaultUSDCurrencyName,
                email = email,
                first_name = clientName,
                last_name = clientName,
                phone = "22254445",
                gmt_timezone = "04:30",
                password = DataRep.Password,
                lang = "en",
                free_text = "free text",
                urlParams = actualUrlParams,
                emailLang = "en",
                promo_code = promoCode // this code should also in the campaign
            };
            var response = _apiAccess.ExecutePostEntry(route, registerClientDto);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json).ClientId;
        }

        public string RegisterClientWithCampaign(string url, string country = null,
            string clientName = null, string emailPrefix = null, string campaignId = null,
            string referanceId = null, string actualUrlParams = null)
        {
            var route = url + ApiRouteAggregate.PostRegisterClient();
            clientName ??= TextManipulation.RandomString();
            emailPrefix ??= DataRep.EmailPrefix;
            var email = clientName + emailPrefix;

            var registerClientDto = new
            {
                country = country ?? "afghanistan",
                currency_code = DataRep.DefaultUSDCurrencyName,
                email = email,
                first_name = clientName,
                last_name = clientName,
                phone = "22254445",
                gmt_timezone = "04:30",
                password = _password,
                lang = "en",
                free_text = "free text",
                urlParams = actualUrlParams,
                emailLang = "en",
                campaign_id = campaignId
            };
            var response = _apiAccess.ExecutePostEntry(route, registerClientDto);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JObject.Parse(json)["client_id"].ToString();
        }

        public ICreateClientApi GetLogoutTreadingPlatformRequest(string uri,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetLogout();
            route = uri + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<GetPixelResponse> GetPixelRequest(string url, string referanceId,
            int expectedNumOfPixels)
        {
            var route = $"{url}{ApiRouteAggregate.GetPixel()}?referance_id={referanceId}";
            List<GetPixelResponse> pixelResponse = null;
            var timer = new Stopwatch();

            try
            {
                for (var i = 0; i < 40; i++)
                {
                    timer.Start();
                    var response = _apiAccess.ExecuteGetEntry(route);
                    _apiAccess.CheckStatusCode(route, response);
                    var json = response.Content.ReadAsStringAsync().Result;
                    pixelResponse = JsonConvert.DeserializeObject<List<GetPixelResponse>>(json);

                    if (pixelResponse == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (pixelResponse.Count != expectedNumOfPixels)
                        {
                            Thread.Sleep(1000);

                            continue;
                        }
                    }

                    timer.Stop();

                    break;
                }

                if (pixelResponse.Count != expectedNumOfPixels)
                {
                    var timeTaken = timer.Elapsed;
                    var time = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");

                    var exceMessage = ($" actual num of pixel:" +
                        $" {pixelResponse.Count}, expected num of pixel:: {expectedNumOfPixels}, timer: {time}");

                    throw new Exception(exceMessage);
                }
            }
            catch (Exception ex)
            {
                var exceMessage = ($"ex.Message: {ex.Message}, Email is empty , no email recived, referance_Id: {referanceId}");

                throw new Exception(exceMessage, ex);
            }

            return pixelResponse;
        }

        public GeneralDto PostCreateLoginLinkRequest(string url,
            string clientId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateLoginLink()}?api_key={_apiKey}";

            var registerClientDto = new
            {
                user_id = clientId,
                //redirect_url = "/settings/deposit"
            };
            var response = _apiAccess.ExecutePostEntry(route, registerClientDto);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<GeneralDto>(json);
        }

        public Dictionary<string, string> GetClientsIds()
        {
            return _ClientsIds;
        }

        public List<string> GetClientCurrenciesRequest(string url)
        {
            var currencies = new List<string>();
            var route = $"{url}{ApiRouteAggregate.GetClientCurrencies()}?api_key={_apiKey}";

            typeof(Currencies)
                .GetProperties()
                .ForEach(p => currencies.Add(p.Name));

            return currencies;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
