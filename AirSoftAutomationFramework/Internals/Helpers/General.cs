// Ignore Spelling: api

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using AirSoftAutomationFramework.Objects.DTOs;
using static AirSoftAutomationFramework.Internals.Enums.EnumFactory;
using AirSoftAutomationFramework.Internals.Factory;

namespace AirSoftAutomationFramework.Internals.Helpers
{
    public class General : IGeneral
    {

        #region Members       
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        private string _apiKey = Config.appSettings.ApiKey;
        #endregion Members

        public General(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public decimal PostCurrencyConversionRequest(string url, double expectedAmount,
            string expectedFromCurrency, string expectedToCurrency)
        {
            var route = ApiRouteAggregate.Convert();
            route = url + route;

            var conversion = new
            {
                amount = expectedAmount,
                from_currency = expectedFromCurrency,
                to_currency = expectedToCurrency
            };
            var response = _apiAccess.ExecutePostEntry(route, conversion);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            var value = JsonConvert.DeserializeObject<GeneralDto>(json).Converted;

            return value.MathRoundFromGeneric(2);
        }

        public IGeneral SwitchToExistingWindow(TabToSwitch tabToSwitch)
        {
            _driver.SwitchToExistingWindow(tabToSwitch);

            return this;
        }

        public double GetRandomDoubleNumber(double minimum = 0, double maximum = 1)
        {
            var random = new Random();
            var temp = (random.NextDouble() * (maximum - minimum)) + minimum;

            return (double)temp.MathRoundFromGeneric(3);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
