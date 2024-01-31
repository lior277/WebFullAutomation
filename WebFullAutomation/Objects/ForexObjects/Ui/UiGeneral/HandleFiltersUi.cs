// Ignore Spelling: Forex api Unselect Uncheck Unassign

using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using OpenQA.Selenium;
using System.Threading;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public class HandleFiltersUi : IHandleFiltersUi
    {
        #region Members
        public IWebDriver _driver;
        private readonly IApplicationFactory _apiFactory;
        private string _clickOnMultiSelect = "//span[contains(@class,'c-angle-down')]" +
            "//ancestor::div[contains(@class,'sidebar-filter-multiselect {0}')]";

        private string _chooseColumn = "//a[contains(.,'{0}')]";
        private string _timeFilter = "label[btnradio='{0}']";
        private string _timeFilterPressed = "label[btnradio='{0}'][aria-pressed='true']";
        private string _search = "div[class*='sidebar-filter-multiselect {0}'] input[class*='c-input']";
        private string _selectElement = "//label[contains(.,'{0}')]";
        private string _filterButton = "//button[contains(.,' {0} ')]";

        private string _listAreaOpen = "div[class='sidebar-filter-multiselect" +
            " {0}'] div[class='dropdown-list']:not([hidden])";

        private string _singelSelectionFilterDisable = "div[class*='sidebar-filter-multiselect" +
            " {0}'] input[type='checkbox']:disabled";

        private string _filterIsClose = "//span[contains(@class,'c-angle-down')]" +
            "//ancestor::div[contains(@class,'sidebar-filter-multiselect {0}')]";

        private string _hideFilterBtn = "div[class*='{0}'] i[class*='fa-eye-slash']";
        private string _filter = "//div[contains(@class,'sidebar-filter-multiselect {0}')]";

        private string _hiddenFilterCheckbox = "//label[contains(.,'{0}')]" +
            "//parent::div[contains(@class,'filter-item')]//descendant" +
            "::span[@class='input-indicator']";
        #endregion Members

        public HandleFiltersUi(IApplicationFactory apiFactory, IWebDriver driver)
        {
            _apiFactory = apiFactory;
            _driver = driver;
        }

        #region Locator's   
        private readonly By ColumnVisibilityBtnExp = By.CssSelector("button[class*='buttons-colvis']");
        private readonly By DropdownListIsOpeExp = By.CssSelector("div[class='dropdown-list']:not([hidden])");
        private readonly By FiltersMenueBtnExp = By.CssSelector("button[class='filters-button']");
        private readonly By FiltersMenueCloseExp = By.CssSelector("button[class='filters-button'] img");
        private readonly By FiltersMenueOpenExp = By.CssSelector("button[class='filters-button']");
        private readonly By UnassignBtnExp = By.CssSelector("div[title='show unassigned clients']");
        private readonly By FilterIsOpenExp = By.CssSelector("span[class*='c-angle-up']");
        private readonly By DataLoaderExp = By.CssSelector("div[class='loader']");
        private readonly By HiddenFiltersBtnExp = By.CssSelector("button[title='Hidden filters']");
        private readonly By SaveFilterBtnExp = By.CssSelector("button[class*='save-filter-btn']");
        #endregion

        public IHandleFiltersUi WaitForTableToLoad()
        {
            var elements = _driver.SearchElements(DataLoaderExp).Count;

            if (elements > 0)
            {
                _driver.SearchElement(DataLoaderExp);
                _driver.WaitForElementNotExist(DataLoaderExp);
            }

            _apiFactory
                .ChangeContext<ISharedStepsGenerator>(_driver)
                .WaitForTableToLoad();

            return this;
        }

        public IHandleFiltersUi ClickToOpenFiltersMenu()
        {
            _driver.ClickAndWaitForNextElement(FiltersMenueBtnExp,
                FiltersMenueOpenExp);

            Thread.Sleep(2000);

            return this;
        }

        private IHandleFiltersUi ClickToOpenFilter(string filterName)
        {
            var MultiSelectDropDownExp = By
                .XPath(string.Format(_clickOnMultiSelect, filterName));

            var MultiSelectDropDownOpenExp = By
                .CssSelector(string.Format(_listAreaOpen, filterName));

            _driver.SearchElement(MultiSelectDropDownExp);
            _driver.ClickAndWaitForNextElement(MultiSelectDropDownExp, DropdownListIsOpeExp);

            //var elements = _driver.SearchElements(DropdownListIsOpeExp).Count;

            //for (var i = 0; i < 10; i++)
            //{
            //    if (elements == 0)
            //    {
            //        _driver.SearchElement(MultiSelectDropDownExp)
            //            .ForceClick(_driver, MultiSelectDropDownExp);

            //        Thread.Sleep(200);
            //        elements = _driver.SearchElements(DropdownListIsOpeExp).Count;

            //        continue;
            //    }

            //    break;
            //}

            //Thread.Sleep(500);

            return this;
        }

        public IHandleFiltersUi ClickOnHideFilterBtn(string filterName)
        {
            var hideFilterExp = By
                .CssSelector(string.Format(_hideFilterBtn, $"{filterName}"));

            _driver.SearchElement(hideFilterExp);
            _driver.MoveToElementAndClick(hideFilterExp);

            return this;
        }

        public IHandleFiltersUi ClickOnHiddenFiltersBtn()
        {
            _driver.SearchElement(HiddenFiltersBtnExp)
                .ForceClick(_driver, HiddenFiltersBtnExp);

            return this;
        }

        public IHandleFiltersUi ClickOnApplyBtn()
        {
            _driver.SearchElement(SaveFilterBtnExp)
                .ForceClick(_driver, SaveFilterBtnExp);

            return this;
        }

        public int CheckIfFilterExist(string filterName)
        {
            var FilterExp = By
               .XPath(string.Format(_filter, $"{filterName}"));

            return _driver.SearchElements(FilterExp).Count;
        }

        public IHandleFiltersUi CheckBoxFilterOnHiddenFiltersWindow(string filterName)
        {
            var hiddenFiltersExp = By
                .XPath(string.Format(_hiddenFilterCheckbox, $"{filterName}"));

            _driver.SearchElement(hiddenFiltersExp)
                .ForceClick(_driver, hiddenFiltersExp);

            return this;
        }

        private void OpenCloseFilter(string filterName)
        {
            var filterIsCloseExp = By
               .XPath(string.Format(_filterIsClose, $"{filterName}"));

            var numOfElements = _driver.SearchElements(filterIsCloseExp, 0.3).Count;

            if (numOfElements == 1)
            {
                ClickToOpenFilter(filterName);
            }
        }

        private bool CheckIfSingleSelectionFilterDisable(string filterName)
        {
            var selectElementExp = By.CssSelector(string.Format(_singelSelectionFilterDisable, $"{filterName}"));
            var isSelected = _driver.SearchElements(selectElementExp, 0.3).Count > 0;

            if (isSelected)
            {
                return true;
            }

            return false;
        }

        public IHandleFiltersUi SearchValueInMultiSelectFilter(string filterName, string value)
        {
            var SearchExp = By.CssSelector(string.Format(_search, $"{filterName}"));

            _driver.SearchElement(SearchExp)
                .SendsKeysAuto(_driver, SearchExp, value);

            return this;
        }

        private IHandleFiltersUi SelectUnselectElementSingelSelect(string filterName,
            string valueToCheck, string valueToUncheck = null)
        {
            var UncheckExp = By.XPath(string.Format(_selectElement, $"{valueToUncheck}"));

            if (CheckIfSingleSelectionFilterDisable(filterName))
            {
                _driver.SearchElement(UncheckExp)
                    .ForceClick(_driver, UncheckExp);

                WaitForTableToLoad();
            }

            var SelectElementExp = By.XPath(string.Format(_selectElement, $"{valueToCheck}"));
            OpenCloseFilter(filterName);
            _driver.ScrollUp(SelectElementExp);
            var element = _driver.SearchElement(SelectElementExp);
            element.ForceClick(_driver, SelectElementExp);
            WaitForTableToLoad();

            return this;
        }

        public IHandleFiltersUi SelectUnselectElementInMultiSelect(string filterName,
            string valueToCheck, string valueToUncheck = null)
        {
            var UncheckExp = By.XPath(string.Format(_selectElement, $"{valueToUncheck}"));
            var SelectElementExp = By.XPath(string.Format(_selectElement, $"{valueToCheck}"));
            OpenCloseFilter(filterName);

            if (valueToUncheck != null)
            {
                // Uncheck
                _driver.SearchElement(UncheckExp)
                    .ForceClick(_driver, UncheckExp);
            }

            _driver.ScrollUp(SelectElementExp);
            var element = _driver.SearchElement(SelectElementExp);
            element.ForceClick(_driver, SelectElementExp);
            WaitForTableToLoad();

            return this;
        }

        private IHandleFiltersUi SelectUnselectElementMultiSelect(string filterName, string value)
        {
            var SelectElementExp = By.XPath(string.Format(_selectElement, $"{value}"));
            //OpenCloseFilter(filterName);

            var element = _driver.SearchElement(SelectElementExp);
            element.ForceClick(_driver, SelectElementExp);
            WaitForTableToLoad();

            return this;
        }

        public ISearchResultsUi MultiSelectDropDownPipe(string filterName, string value)
        {
            ClickToOpenFilter(filterName);
            SearchValueInMultiSelectFilter(filterName, value);
            SelectUnselectElementMultiSelect(filterName, value);
            //_driver.WaitForAnimationToLoad();

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi SingleSelectDropDownPipe(string filterName,
            string valueToCheck, string valueToUncheck = null)
        {
            OpenCloseFilter(filterName);
            SelectUnselectElementSingelSelect(filterName, valueToCheck, valueToUncheck);
            WaitForTableToLoad();

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi UnassignPipe(string filterName)
        {
            ClickToOpenFilter(filterName);
            ClickOnUnassignBtn();
            WaitForTableToLoad();

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi MultiSelectDropDownPipe(string filterName, string[] values)
        {
            ClickToOpenFilter(filterName);

            foreach (var value in values)
            {
                SearchValueInMultiSelectFilter(filterName, value);
                SelectUnselectElementMultiSelect(filterName, value);
            }

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        private ISearchResultsUi ClickOnUnassignBtn()
        {
            _driver.SearchElement(UnassignBtnExp)
                .ForceClick(_driver, UnassignBtnExp);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public ISearchResultsUi SetTableColumnVisibility(string columnName)
        {
            var ChooseColumnExp = By.CssSelector(string.Format(_chooseColumn,
                $"{columnName.ToLower()}"));

            _driver.SearchElement(ColumnVisibilityBtnExp)
               .ForceClick(_driver, ColumnVisibilityBtnExp);

            _driver.SearchElement(ChooseColumnExp)
                .ForceClick(_driver, ChooseColumnExp);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        // TBD loop
        public ISearchResultsUi SetTimeFilter(string time)
        {
            var timeFilterExp = By.CssSelector(string.Format(_timeFilter, $"{time}"));
            var timeFilterPressedExp = By.CssSelector(string.Format(_timeFilterPressed, $"{time}"));

            _driver.SearchElement(timeFilterExp)
                .ForceClick(_driver, timeFilterExp);

            _driver.SearchElement(timeFilterPressedExp);

            return _apiFactory.ChangeContext<ISearchResultsUi>(_driver);
        }

        public bool VerifyButtonEnable(string buttonName)
        {
            var filterButtonExp = By.CssSelector(string.Format(_filterButton, buttonName));

            return _driver.SearchElement(filterButtonExp)
                .Enabled;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
