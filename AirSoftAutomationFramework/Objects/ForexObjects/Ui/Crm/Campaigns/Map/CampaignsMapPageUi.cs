using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Map
{
    public class CampaignsMapPageUi : ICampaignsMapPageUi
    {
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _filterBtn = "label[btnradio='{0}']";
        private string _countryOnTheMap = "path[class*='{0}'][style*='rgb(11, 52, 117)']";
        private string _countryNameOnTheMap = "path[class*='{0}']";

        public CampaignsMapPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's      
        private readonly By CountryTitleExp = By.CssSelector("span[class='iso-name']");
        private readonly By CampaignTitleExp = By.CssSelector("span[class*='sale-title']");
        private readonly By CountryAndCampaignValuesExp = By.CssSelector("div[class*='campaign-progress'] span[class*='sale-percent']");
        private readonly By InActiveCampaignTableExp = By.CssSelector("table[id='inActiveCampaignTable'] td");
        #endregion Locator's

        public CampaignsMapPageUi ClickOnFilterButtonByName(string filterButtonName)
        {
            var filterBtnExp = By.CssSelector(string.Format(_filterBtn, filterButtonName));

            _driver.SearchElement(filterBtnExp)
                .ForceClick(_driver, filterBtnExp);

            return this;
        }

        public string GetCountryName()
        {
            return _driver.SearchElement(CountryTitleExp)
                .GetElementText(_driver);
        }

        public string GetCampaignName()
        {
            return _driver.SearchElement(CampaignTitleExp)
                .GetElementText(_driver);
        }

        public List<string> GetFilterValues()
        {
            var data = new List<string>();

            _driver.SearchElements(CountryAndCampaignValuesExp)
                .ForEach(p => data.Add(p.GetElementText(_driver)));

            return data;
        }

        public List<string> GetiInActiveCampaignTableValues()
        {
            var data = new List<string>();

            _driver.SearchElements(InActiveCampaignTableExp)
                .ForEach(p => data.Add(p.GetElementText(_driver)));

            return data;
        }

        public ICampaignsMapPageUi VerifyCountryHighlightedBySymble(string countrySymble)
        {
            var countryOnTheMapHoverExp =
                By.CssSelector(string.Format(_countryOnTheMap, countrySymble));

            _driver.WaitForExactNumberOfElements(countryOnTheMapHoverExp, 1);

            return this;
        }

        public string GetCountryNameFromMapByContrySymble(string countrySymble)
        {
            var _countryNameOnTheMapExp =
              By.CssSelector(string.Format(_countryNameOnTheMap, countrySymble));

            return _driver.SearchElement(_countryNameOnTheMapExp)
                .GetElementText(_driver);
        }

        public List<string> GetFilterDataPipe()
        {
            var data = new List<string>();
            data.AddRange(GetFilterValues());
            data.Add(GetCountryName());
            data.Add(GetCampaignName());

            return data;
        }
    }
}
