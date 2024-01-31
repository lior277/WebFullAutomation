using System.IO;
using System.Reflection;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Path = System.IO.Path;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Profile
{
    public class ProfilePageApi : IProfilePageApi
    {

        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public ProfilePageApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IProfilePageApi PatchKycFileRequest(string url, string fileName,
            string fileContentName, GetLoginResponse loginData)
        {
            var route = url + ApiRouteAggregate.PatchKycFile();

            var path = Path.Combine(Path.GetDirectoryName(Assembly.
                GetExecutingAssembly().Location), fileName);

            var fileContent = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .UploadFilePipe(path, fileContentName, "image/png");

            var response = _apiAccess.ExecutePatchEntry(route, fileContent, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public Stream GetGeneralDtoRequest(string url,
            GetLoginResponse loginData)
        {
            var route = $"{url}{ApiRouteAggregate.GetCompanyDodFile()}";
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return response.Content.ReadAsStreamAsync().Result;
        }

        public GetClientProfileResponse GetClientProfileRequest(string url, GetLoginResponse loginData)
        {
            var route = url + ApiRouteAggregate.ClientProfileOnTradingPlatform();
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            return JsonConvert.DeserializeObject<GetClientProfileResponse>(json);
        }

        public IProfilePageApi PatchClientProfileRequest(string url,
            GetClientProfileResponse getClientProfileResponse, GetLoginResponse loginData)
        {
            var route = url + ApiRouteAggregate.ClientProfileOnTradingPlatform();

            var patchClientProfil = new
            {
                first_name = getClientProfileResponse.first_name,
                last_name = getClientProfileResponse.last_name,
                country = getClientProfileResponse.country,
                currency_code = getClientProfileResponse.currency_code
            };

            var response = _apiAccess.ExecutePatchEntry(route, patchClientProfil, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
