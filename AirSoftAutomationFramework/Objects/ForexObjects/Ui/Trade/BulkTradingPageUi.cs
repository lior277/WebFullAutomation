// Ignore Spelling: api Forex Crm

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm
{
    public class BulkTradePageUi : IBulkTradePageUi
    {
        #region Members
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _multiSelectValue = "//angular2-multiselect[@id='asset_symbol']//descendant::label[contains(.,'{0}')]";
        private string _openTradeRadios = "//label[contains(.,'{0}')]";

        #endregion Members

        public BulkTradePageUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's 
        private readonly By MultiSelectExp = By.CssSelector("angular2-multiselect[id='asset_symbol']");
        private readonly By SearchMultiSelectExp = By.CssSelector("angular2-multiselect[id='asset_symbol'] input[type='text']");
        private readonly By SelectBulkGroupExp = By.CssSelector("select[name='selectedMassGroup']");
        private readonly By OpenBulkTradeBtnExp = By.XPath("//button[contains(.,'Open trade')]");
        private readonly By OpenBulkGroupExp = By.XPath("//button[contains(.,'Bulk-group')]");
        private readonly By ExposureExp = By.CssSelector("input[id='exposure']");
        private readonly By ConfirmTradeExp = By.CssSelector("div[class='modal-content'] div[class*='action-alert']");

        #endregion

        public BulkTradePageUi SelectBulkGroup(string BulkGroupName)
        {
            _driver.SearchElement(SelectBulkGroupExp)
                .SelectElementFromDropDownByText(_driver, SelectBulkGroupExp, BulkGroupName);

            _driver.WaitForExactNumberOfElements(DataRep.ChooseClientCheckBoxExp, 2);

            return this;
        }

        public IBulkTradePageUi ClickOnClientCheckBox()
        {
            _driver.WaitForExactNumberOfElements(DataRep.ChooseClientCheckBoxExp, 2);

            _driver.SearchElements(DataRep.ChooseClientCheckBoxExp)
                .First()
                .ForceClick(_driver, DataRep.ChooseClientCheckBoxExp);

            return this;
        }

        public IBulkTradePageUi SelectAssetPipe(string value)
        {
            ClickOnMultiSelectById();
            SearchValueInMultiSelect(value);
            ClickOnMultiSelectValue(value);

            return this;
        }

        private IBulkTradePageUi ClickOnMultiSelectById()
        {
            _driver.SearchElement(MultiSelectExp)
                .ForceClick(_driver, MultiSelectExp);

            return this;
        }

        private IBulkTradePageUi SearchValueInMultiSelect(string valueToSearch)
        {
            _driver.SearchElement(SearchMultiSelectExp)
                .SendsKeysAuto(_driver, SearchMultiSelectExp, valueToSearch);

            return this;
        }

        private IBulkTradePageUi ClickOnMultiSelectValue(string value)
        {
            var multiSelectValueExp = By.XPath(string.Format(_multiSelectValue, value));

            _driver.SearchElement(multiSelectValueExp)
                .ForceClick(_driver, multiSelectValueExp);

            return this;
        }

        public IBulkTradePageUi ClickOnOpenTradeButton()
        {
            _driver.SearchElement(OpenBulkTradeBtnExp)
                .ForceClick(_driver, OpenBulkTradeBtnExp);

            return this;
        }

        public IBulkTradePageUi SetExposer(string exposerValue)
        {
            _driver.SearchElement(ExposureExp)
                .SendsKeysAuto(_driver, ExposureExp, exposerValue);

            return this;
        }

        private IBulkTradePageUi ClickOnTradeRadioByName(string radioName)
        {
            var multiSelectExp = By.CssSelector(string.Format(_openTradeRadios, radioName));

            _driver.SearchElement(multiSelectExp)
                .ForceClick(_driver, multiSelectExp);

            return this;
        }

        public IBulkTradePageUi ClickOnSaveOpenTradeButton()
        {
            _driver.SearchElement(DataRep.SaveExp)
                .ForceClick(_driver, DataRep.SaveExp);

            return this;
        }

        public ISearchResultsUi ClickOnConfirmButton()
        {
            _driver.SearchElement(DataRep.ConfirmExp)
                .WaitElementToStopMoving(_driver, ConfirmTradeExp)
                .ForceClick(_driver, DataRep.ConfirmExp);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
