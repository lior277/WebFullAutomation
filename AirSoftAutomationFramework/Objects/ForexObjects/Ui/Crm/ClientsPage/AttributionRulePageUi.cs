

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage
{
    public class AttributionRulePageUi : IAttributionRulePageUi
    {
        #region Members
        private IWebDriver _driver;
        private IHandleFiltersUi _handleFilters;
        private readonly IApplicationFactory _apiFactory;
        private string _split = "input[formcontrolname='split'][value='{0}']";
        private string _multiSelect = "angular2-multiselect[id='{0}']";
        private string _searchMultiSelect = "angular2-multiselect[id='{0}'] input[type='text']";
        private string _multiSelectValue = "//angular2-multiselect[@id='{0}']//descendant::label[contains(.,'{1}')]";
        #endregion

        public AttributionRulePageUi(IHandleFiltersUi handleFilters,
            IApplicationFactory apiFactory, IWebDriver driver)
        {
            _handleFilters = handleFilters;
            _driver = driver;
            _apiFactory = apiFactory;
        }

        #region Locator's    
        private readonly By NameExp = By.CssSelector("input[id='name']");
        private readonly By TypeExp = By.CssSelector("select[id='type']");
        private readonly By SaveBtnExp = By.CssSelector("button[type='submit']");
        private readonly By RetentionTypeExp = By.CssSelector("select[id='type']");
        private readonly By DuplicateAttributionMessageExp = By.CssSelector("h4[class='modal-title']");
        #endregion

        public IAttributionRulePageUi SetName(string name)
        {
            _driver.SearchElement(NameExp)
                .SendsKeysAuto(_driver, NameExp, name);

            return this;
        }

        public IAttributionRulePageUi SelectType(string typeValue)
        {

            _driver.SearchElement(RetentionTypeExp)
                .SelectElementFromDropDownByValue(_driver, typeValue);

            return this;
        }

        private IAttributionRulePageUi ClickOnMultiSelectById(string multiSelectId)
        {
            var multiSelectExp = By.CssSelector(string.Format(_multiSelect, multiSelectId));

            _driver.SearchElement(multiSelectExp)
                .ForceClick(_driver, multiSelectExp);

            return this;
        }

        private IAttributionRulePageUi SearchValueInMultiSelect(string multiSelectId, string valueToSearch)
        {
            var multiSelectSearchExp = By.CssSelector(string.Format(_searchMultiSelect, multiSelectId));

            _driver.SearchElement(multiSelectSearchExp)
                .SendsKeysAuto(_driver, multiSelectSearchExp, valueToSearch);

            return this;
        }

        private IAttributionRulePageUi ClickOnMultiSelectValue(string multiSelectId, string value)
        {
            var multiSelectValueExp = By.XPath(string.Format(_multiSelectValue, multiSelectId, value));

            _driver.SearchElement(multiSelectValueExp)
                .ForceClick(_driver, multiSelectValueExp);

            return this;
        }

        private IAttributionRulePageUi SelectRadio(string radioValue)
        {
            var multiSplitValueExp = By.CssSelector(string.Format(_split, radioValue));

            _driver.SearchElement(multiSplitValueExp)
                .ForceClick(_driver, multiSplitValueExp);

            return this;
        }

        public IAttributionRulePageUi SelectCampaignPipe(string campaignName)
        {
            ClickOnMultiSelectById("campaign_id");
            SearchValueInMultiSelect("campaign_id", campaignName);
            ClickOnMultiSelectValue("campaign_id", campaignName);
            ClickOnMultiSelectById("campaign_id");

            return this;
        }

        public IAttributionRulePageUi SelectCountryPipe(string countryName,
            bool needToCloseDialog = true)
        {
            ClickOnMultiSelectById("country");
            SearchValueInMultiSelect("country", countryName);
            ClickOnMultiSelectValue("country", countryName);

            if (needToCloseDialog)
            {
                ClickOnMultiSelectById("country");
            }

            return this;
        }

        public IAttributionRulePageUi ChooseSplit(string splitValue)
        {
            SelectRadio(splitValue);

            return this;
        }

        public IAttributionRulePageUi SelectFtdSellerPipe(string ftdSellerId)
        {
            ClickOnMultiSelectById("ftd_agent_id");
            SearchValueInMultiSelect("ftd_agent_id", ftdSellerId);
            ClickOnMultiSelectValue("ftd_agent_id", ftdSellerId);

            return this;
        }

        public IAttributionRulePageUi ChooseRetentionType(string RetentionTypeValue)
        {
            SelectRadio(RetentionTypeValue);

            return this;
        }

        public IAttributionRulePageUi SelectRetentionSellerPipe(string retentionSellerId)
        {
            ClickOnMultiSelectById("retention_agent_id");
            SearchValueInMultiSelect("retention_agent_id", retentionSellerId);
            ClickOnMultiSelectValue("retention_agent_id", retentionSellerId);

            return this;
        }

        public string GetDuplicateAttributionMessage()
        {
            return _driver.SearchElement(DuplicateAttributionMessageExp)
                .GetElementText(_driver, DuplicateAttributionMessageExp);
        }

        public ICreateClientUi ClickOnSaveButton()
        {
            _driver.SearchElement(SaveBtnExp)
                .ForceClick(_driver, SaveBtnExp);

            return _apiFactory.ChangeContext<ICreateClientUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
