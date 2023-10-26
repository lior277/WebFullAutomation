// Ignore Spelling: Api

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard.DocumentsAndFilesTab
{
    public class DocumentsAndFilesTabApi : IDocumentsAndFilesTabApi
    {
        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private readonly IWebDriver _driver;
        private readonly IApiAccess _apiAccess;
        private readonly string _crmUrl = Config.appSettings.CrmUrl;
        private readonly string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public DocumentsAndFilesTabApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IDocumentsAndFilesTabApi VerifyFileDonloaded(string url, string clientId,
            string fileName, string apiKey)
        {
            var route = ApiRouteAggregate.GetClientFile(clientId, fileName);
            route = $"{url}{route}&api_key={_apiKey}";
           var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }
    }
}
