// Ignore Spelling: Cfd api

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factorys;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Ui.Risk
{
    public class AssetsCfdUi : IAssetsCfdUi
    {
        private IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;

        private string _openAssetGroupGroupBtn = "//li[@class='ui-treenode']/div[contains(.,'{0}')]" +
            "/span[contains(@class,'ui-tree-toggler')]";

        private string _removeAssetFromFront = "//div[contains(.,'{0}')]" +
            "/span[@class='asset-remove']";

        private string _addAssetBtn = "//td[contains(.,'{0}')]//following::" +
            "a[contains(@class,'addTableAssetbtn')][1]";

        private string _chooseAssetGroup = "//li[@class='ui-treenode']/div[contains(.,'{0}')]";

        public AssetsCfdUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's          
        private static By SaveBtnExp = By.CssSelector("button[class='btn btn-success']");
        #endregion Locator's     

        public IAssetsCfdUi OpenAssetGroupByName(string assetGroupName)
        {
            var assetGroupExp = By.XPath(string.Format(_openAssetGroupGroupBtn, assetGroupName));

            _driver.SearchElement(assetGroupExp)
                .ForceClick(_driver, assetGroupExp);

            return this;
        }

        public IAssetsCfdUi ChooseAssetGroupByName(string subGroupName)
        {
            var assetGroupExp = By.XPath(string.Format(_chooseAssetGroup, subGroupName));

            _driver.SearchElement(assetGroupExp)
                .ForceClick(_driver, assetGroupExp);

            return this;
        }

        public IAssetsCfdUi RemoveAssetByName(string assetName)
        {
            var removeAssetExp = By.XPath(string.Format(_removeAssetFromFront, assetName));

            _driver.SearchElement(removeAssetExp)
                .ForceClick(_driver, removeAssetExp);

            return this;
        }

        public IAssetsCfdUi AddAssetToFrontOrderByName(string assetName)
        {
            var addAssetBtnExp = By.XPath(string.Format(_addAssetBtn, assetName));

            _driver.SearchElement(addAssetBtnExp)
                .ForceClick(_driver, addAssetBtnExp);

            return this;
        }

        public IAssetsCfdUi ClickOnSaveButton()
        {
            _driver.SearchElement(SaveBtnExp)
                .ForceClick(_driver, SaveBtnExp);

            return this;
        }

        public IAssetsCfdUi AddAssetPipe(string groupName,
            string subGroupName, string assetName)
        {
            OpenAssetGroupByName(groupName);
            OpenAssetGroupByName(subGroupName);
            ChooseAssetGroupByName(subGroupName);
            AddAssetToFrontOrderByName(assetName);
            ClickOnSaveButton();

            return this;
        }

        public IAssetsCfdUi RemoveAssetPipe(string groupName,
            string subGroupName, string assetName)
        {
            OpenAssetGroupByName(groupName);
            OpenAssetGroupByName(subGroupName);
            ChooseAssetGroupByName(subGroupName);
            AddAssetToFrontOrderByName(assetName);
            RemoveAssetByName(assetName);
            ClickOnSaveButton();

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
