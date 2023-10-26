// Ignore Spelling: Forex Api

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi.Settings
{
    public class HistoryPageApi : IHistoryPageApi
    {
        #region Members  
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public HistoryPageApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public IHistoryPageApi PatchSignDodFileRequest(string url, int transactionId,
            string dodId, GetLoginResponse loginData)
        {
            var route = $"{ApiRouteAggregate.PatchKycFile()}/{transactionId}?dod_id={dodId}";
            route = url + route;

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                DataRep.PdfFileNameToUpload);

            var documentFileContent = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .ConvertToBytesArray(path);

            documentFileContent.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("form-data")
                {
                    Name = "dod",
                    FileName = $"{transactionId}.pdf"
                };

            documentFileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            var form = new MultipartFormDataContent();
            form.Add(documentFileContent);

            var response = _apiAccess.ExecutePatchEntry(route, form, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public Transactions GetTransactionsRequest(string url,
            GetLoginResponse loginData)
        {
            var route = url + ApiRouteAggregate.GetTransactionsFromTp();
            var response = _apiAccess.ExecuteGetEntry(route, loginData);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            _apiAccess.CheckStatusCode(route, response);

            return JsonConvert.DeserializeObject<Transactions>(json);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
