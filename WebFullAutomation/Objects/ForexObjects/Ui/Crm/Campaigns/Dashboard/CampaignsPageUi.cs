using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard
{
    public class CampaignsPageUi : ICampaignsPageUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        public CampaignsPageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's    
        private readonly By CreateCampaignButtonExp = By.CssSelector("button[class*='create-campaign-btn']");
        private readonly By CreateAffiliateButtonExp = By.CssSelector("button[class*='create-affiliate-btn']");
        private readonly By ActiveCampaignGridSearchExp = By.CssSelector("input[aria-controls='activeCampaignTable']");
        private readonly By InActiveCampaignGridSearchExp = By.CssSelector("input[aria-controls='inActiveCampaignTable']");
        private readonly By InActiveCampaignDataTableRowsExp = By.CssSelector("table[class*='search-result-inactive-campaign'] tr[class='odd'],[class='even']");
        private readonly By ListBarsDiagramExp = By.CssSelector("div[class*='campaign-progress']");
        private readonly By DonutDiagramExp = By.CssSelector("div[id='donut'] svg");
        private readonly By CampaignsTitlesFromListBarsDiagramExp = By.CssSelector("span[class*='sale-title']");
        private readonly By WaitForProcessinginActiveCampaignExp = By.CssSelector("div[id='inActiveCampaignTable_processing'][style='display: none;']");
        #endregion Locator's     

        public ICreateCampaignUi ClickCreateCampaignButton()
        {
            WaitForCampaignTableToLoad();

            _driver.SearchElement(CreateCampaignButtonExp)
                .ForceClick(_driver, CreateCampaignButtonExp);

            return _apiFactory.ChangeContext<ICreateCampaignUi>(_driver);
        }

        public ICreateAffiliateUi ClickCreateAffiliateButton()
        {
            _driver.SearchElement(CreateAffiliateButtonExp)
                .ForceClick(_driver, CreateAffiliateButtonExp);

            return _apiFactory.ChangeContext<ICreateAffiliateUi>(_driver);
        }

        public ICampaignsPageUi VerifyListBarsDiagramExist()
        {
            _driver.SearchElement(ListBarsDiagramExp);

            return this;
        }

        public List<string> GetCampaignsTitlesFromListBarsDiagram()
        {
            var titlesList = new List<string>();
            _driver.WaitForExactNumberOfElements(CampaignsTitlesFromListBarsDiagramExp, 2);
            var elements = _driver.SearchElements(CampaignsTitlesFromListBarsDiagramExp);
            elements.ForEach(p => titlesList.Add(p.GetElementText(_driver)));

            return titlesList;
        }

        public ICampaignsPageUi VerifyDonutDiagramExist()
        {
            _driver.SearchElement(DonutDiagramExp);

            return this;
        }

        public ISearchResultsUi SearchActiveCampaign(string campaignName)
        {
            WaitForCampaignTableToLoad();

            _driver.SearchElement(ActiveCampaignGridSearchExp)
                .SendsKeysAuto(_driver, ActiveCampaignGridSearchExp, campaignName);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ICampaignsPageUi WaitForCampaignTableToLoad()
        {
            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public ISearchResultsUi SearchInActiveCampaign(string campaignName)
        {
            WaitForCampaignTableToLoad();

            _driver.SearchElement(InActiveCampaignGridSearchExp)
                .SendsKeysAuto(_driver, InActiveCampaignGridSearchExp, campaignName);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
