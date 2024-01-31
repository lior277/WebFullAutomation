using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.IO;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public class ClientCardApi : IClientCardApi
    {
        #region Members
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IWebDriver _driver;
        #endregion Members

        public ClientCardApi(IApplicationFactory appFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = appFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IClientCardApi WaitForBalanceInClientCardToBeUpdated(string url,
            string clientId, double amount)
        {
            for (var i = 0; i < 1000; i++)
            {
                // wait for procedure to update the _balance
                Thread.Sleep(100);

                var clientSearchBalance = _apiFactory
                    .ChangeContext<IClientsApi>()
                    .GetClientRequest(url, clientId)
                    .GeneralResponse
                    .data
                    .FirstOrDefault()
                    .balance
                    .Split('$')[0]
                    .TrimEnd();

                if (clientSearchBalance.Contains(amount.ToString()))
                {
                    break;
                }

            }

            return this;
        }

        public IClientCardApi PatchSetClientPasswordRequest(string url, string clientId,
            string clientName, string newPassword = null, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchReSetClientPassword(clientId)}?api_key={_apiKey}";

            var patchReSetCilentDto = new
            {
                first_name = clientName,
                last_name = clientName,
                password = newPassword ?? DataRep.Password
            };
            var response = _apiAccess.ExecutePatchEntry(route, patchReSetCilentDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IClientCardApi PatchSetClientCurrencyRequest(string url, string clientId,
            string currencyCode, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchSetClientCurrency(clientId)}?api_key={_apiKey}";

            var PatchSetCilentCurrency = new
            {
                currency_code = currencyCode
            };
            var response = _apiAccess.ExecutePatchEntry(route, PatchSetCilentCurrency);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IClientCardApi PostForgotPasswordRequest(string url, string clientEmail,
            GetLoginResponse loginData)
        {
            var route = url + ApiRouteAggregate.PostForgotPasswordTp();

            var forgetPasswordRequestDto = new
            {
                email = clientEmail,
                req_from = "login-page"
            };
            var response = _apiAccess.ExecutePostEntry(route, forgetPasswordRequestDto, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PostMakeCallRequest(string url, string clientId, string pbxName,
            string phone, string apiKey = null, bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.PostMakeCall()}{clientId}?api_key={apiKey}";
            string json;

            var createCallDto = new
            {
                phone = phone,
                pbx_name = pbxName
            };

            var response = _apiAccess.ExecutePostEntry(route, createCallDto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public IClientCardApi PostResetPasswordRequest(string url, string clientId)
        {
            var route = $"{url}{ApiRouteAggregate.PostResetPassword()}?api_key={_apiKey}";

            var forgetPasswordRequestDto = new
            {
                id = clientId,
                req_from = "client-profile"
            };
            var response = _apiAccess.ExecutePostEntry(route, forgetPasswordRequestDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IClientCardApi PostSendDirectEmailRequest(string url, string clientId)
        {
            var route = $"{url}{ApiRouteAggregate.PostSendDirectEmail()}?api_key={_apiKey}";

            var postSendDirectEmail = new
            {
                type = "custom",
                name = "Direct Email Automation",
                language = "en",
                subject = "Direct Email Automation",
                body = "PHVsIF9uZ2NvbnRlbnQtYzI5PSIiIGNsYXNzPSJ2YXJpYWJsZXMtbGlzdCIgc3R5bGU9ImNvbG9yOiBy" +
                "Z2IoOTcsIDExMSwgMTE5KTsiPjxsaSBfbmdjb250ZW50LWMyOT0iIiBjbGFzcz0ibmctc3Rhci1pbnNlcnRlZCI+PHNw" +
                "YW4gX25nY29udGVudC1jMjk9IiI+e1NFTExFUl9GSVJTVF9OQU1FfTwvc3Bhbj48L2xpPjwvdWw+",
                active = true,
                user_id = clientId
            };
            var response = _apiAccess.ExecutePostEntry(route, postSendDirectEmail);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string ExportClientCardPipe(string url, string clientId, string userEmail,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostExportClientCard(clientId)}?api_key={_apiKey}";

            var routeForExport = ApiRouteAggregate.ExportUser(clientId);

            var exportClientCardDto = new
            {
                export_email = userEmail,
                service_url = routeForExport
            };

            var response = _apiAccess.ExecutePostEntry(route, exportClientCardDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        //public GetClientCardByIdResponse GetClientCardByIdRequest(string url, string clientId)
        //{
        //    var route = ApiRouteAggregate.GetClientById();
        //    route = $"{url}{route}{clientId}?api_key={_apiKey}";
        //    var response = _apiAccess.ExecuteGetEntry(route);
        //    _apiAccess.CheckStatusCode(route, response);
        //    var json = response.Content.ReadAsStringAsync().Result;

        //    return JsonConvert.DeserializeObject<GetClientCardByIdResponse>(json);
        //}

        public GeneralResult<GeneralDto> GetExportClientCardRequest(string url, string exportLink,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            exportLink = url + exportLink;
            var route = $"{exportLink.TrimEnd()}&api_key={_apiKey}";
            var generalResult = new GeneralResult<GeneralDto>();
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                generalResult.Stream = response.Content.ReadAsStreamAsync().Result;
            }
            else
            {
                generalResult.Message = response.Content.ReadAsStringAsync().Result;
            }

            return generalResult;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
