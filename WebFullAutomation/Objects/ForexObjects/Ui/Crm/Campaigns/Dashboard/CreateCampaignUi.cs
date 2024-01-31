using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Campaigns.Dashboard
{
    public class CreateCampaignUi : ICreateCampaignUi
    {
        #region Members
        public IWebDriver _driver;
        private static string _affiliateName;
        private string _deal;
        private string _currency;
        private string _campaignName;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public CreateCampaignUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's  
        private readonly By AffiliateNameExp = By.XPath("//select[contains(@class,'affiliate-name')]");
        private readonly By DealExp = By.XPath("//select[contains(@class,'campaign-deal')]");
        private readonly By CurrencyExp = By.XPath("//select[contains(@class,'campaign-currency')]");
        private readonly By CodeExp = By.CssSelector("input[class*='campaign-code']");
        private readonly By CampaignNameExp = By.CssSelector("input[class*='campaign-name']");
        #endregion Locator's

        public ICreateCampaignUi SetAffiliateName(string affiliateName)
        {
            _affiliateName = affiliateName ?? "new";

            _driver.SearchElement(AffiliateNameExp, 120)
                .SelectElementFromDropDownByText(_driver, AffiliateNameExp, _affiliateName);

            return this;
        }

        public ICreateCampaignUi SetDeal(string deal = null)
        {
            _deal = deal ?? "cpa";

            _driver.SearchElement(DealExp)
                .SelectElementFromDropDownByText(_driver, DealExp, _deal);

            return this;
        }

        public ICreateCampaignUi SetCurrency(string currency = null)
        {
            _currency = currency ?? DataRep.DefaultUSDCurrencyName;

            _driver.SearchElement(CurrencyExp)
                .SelectElementFromDropDownByText(_driver, CurrencyExp, _currency);

            return this;
        }

        public ICreateCampaignUi SetCode(string code = null)
        {
            code ??= TextManipulation.RandomString();

            _driver.SearchElement(CodeExp, 150)
               .SendsKeysAuto(_driver, CodeExp, code);

            return this;
        }

        public ICreateCampaignUi SetCampaignName(string campaigntName)
        {
            _campaignName = campaigntName;

            _driver.SearchElement(CampaignNameExp, 150)
                .SendsKeysAuto(_driver, CampaignNameExp, _campaignName, 150);

            return this;
        }

        public ICampaignsPageUi ClickOnSaveButton()
        {
            _driver.SearchElement(DataRep.SaveExp)
                .ForceClick(_driver, DataRep.SaveExp);
            //in cbd with firefox inactive campaign dont load
            _driver.WaitForElementNotExist(DataRep.SaveExp);

            return _apiFactory.ChangeContext<ICampaignsPageUi>(_driver);
        }

        public ICampaignsPageUi CreateCampaignUiPipe(string affiliateName, string campaignName)
        {
            SetAffiliateName(affiliateName);
            SetDeal();
            SetCode();
            SetCurrency();
            SetCampaignName(campaignName);

            return ClickOnSaveButton();
        }

        public ICampaignsPageUi CreateCampaignUiPipeCbd(string affiliateName, string campaignName)
        {
            SetAffiliateName(affiliateName);
            SetDeal();
            SetCode();
            SetCampaignName(campaignName);

            return ClickOnSaveButton();
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
