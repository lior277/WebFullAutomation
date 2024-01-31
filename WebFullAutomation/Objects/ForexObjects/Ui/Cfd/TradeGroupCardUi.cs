using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using OpenQA.Selenium;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public class TradeGroupCardUi : ITradeGroupCardUi
    {
        #region Members
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _cryptoGroupTabs = "//a[contains(.,'{0}')]";
        private string _editAssetType = "//td[contains(.,'{0}')]//parent::tr//a";
        private string _editAsset = "//td[contains(.,'{0}')]//parent::tr//a[contains(.,'Edit')]";
        public static By DefaultCheckboxExp = By.CssSelector("label[class*='minotaur-checkbox']");
        public static By checkboxExp = By.CssSelector("input[type='checkbox']");
        #endregion Members

        public TradeGroupCardUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's
        private readonly By SpreadInputOnGeneralExp = By.CssSelector(
            "input[id='spread'][disabled]");

        private readonly By DataLoaderExp = By.CssSelector("div[class='loader']");

        private readonly By SpreadInputInAssetTypesExp = By.CssSelector(
         "div[class*='updated'] input[disabled]");

        private readonly By SpreadInputInAssetsExp = By.CssSelector(
          "form[novalidate] input[disabled][formcontrolname='spread']");

        private readonly By EditTypeIndicesBtnExp = By.XPath(
         "//td[contains(.,'indices')]//parent::tr//a");

        private readonly By OkBtnExp = By.XPath("//button[contains(.,'Ok')]");
        public static By SaveBtnExp = By.CssSelector("button[type='submit']");

        private readonly By EditAssetBtnExp = By.XPath(
           "//td[contains(.,'Apple')]//parent::tr//a[contains(.,'Edit')]");
        #endregion

        public ITradeGroupCardUi VerifyGeneralSpreadIsDisable()
        {
            _driver.SearchElement(SpreadInputOnGeneralExp);

            return this;
        }

        public ITradeGroupCardUi VerifyAssetTypeSpreadIsDisable()
        {
            _driver.SearchElement(SpreadInputInAssetTypesExp);

            return this;
        }

        public ITradeGroupCardUi VerifyAssetSpreadIsDisable()
        {
            _driver.SearchElement(SpreadInputInAssetsExp);

            return this;
        }

        public ITradeGroupCardUi ClickOnCryptoGroupTabByName(string tabName)
        {
            var cryptoGroupTabExt = By.XPath(string.Format(_cryptoGroupTabs, tabName));

            _driver.SearchElement(cryptoGroupTabExt)
                .ForceClick(_driver, cryptoGroupTabExt);

            return this;
        }

        public ITradeGroupCardUi ClickOnEditAssetType(string assetType)
        {
            var assetTypeExt = By.XPath(string.Format(_editAssetType, assetType));

            _driver.SearchElement(assetTypeExt)
                .ForceClick(_driver, assetTypeExt);

            return this;
        }

        public ITradeGroupCardUi ClickOnDefaultCheckbox()
        {
            var elementsToCheck = _driver.SearchElements(DefaultCheckboxExp);
            var checkboxs = _driver.SearchElements(checkboxExp);

            for (var i = 0; i < checkboxs.Count; i++)
            {
                elementsToCheck[i].ForceClick(_driver);

                if (!checkboxs[i].Selected)
                {
                    elementsToCheck[i].ForceClick(_driver);
                }
            }

            return this;
        }

        private ITradeGroupCardUi WaitForSavingLoader()
        {
            var elements = _driver.SearchElements(DataLoaderExp).Count;

            if (elements > 0)
            {
                _driver.SearchElement(DataLoaderExp);
                _driver.WaitForElementNotExist(DataLoaderExp);
            }

            return this;
        }

        public ITradeGroupCardUi ClickOnOkBtn()
        {
            var element = _driver.SearchElement(OkBtnExp);
            element.ForceClick(_driver, OkBtnExp);

            //for (int i = 0; i < 4; i++)
            //{
            //    if (element.Displayed)
            //    {
            //        Thread.Sleep(100);
            //        element.ForceClick(_driver, OkBtnExp);

            //        continue;
            //    }

            //    break;
            //}

            return this;
        }

        public ITradeGroupsUi ClickOnSaveBtn()
        {
            _driver.SearchElement(SaveBtnExp)
                .ForceClick(_driver, SaveBtnExp);

            WaitForSavingLoader();

            return _apiFactory.ChangeContext<ITradeGroupsUi>(_driver);
        }

        public ITradeGroupCardUi ClickOnEditAsset(string assetName)
        {
            var assetExt = By.XPath(string.Format(_editAsset, assetName));

            _driver.SearchElement(assetExt)
                .ForceClick(_driver, assetExt);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
