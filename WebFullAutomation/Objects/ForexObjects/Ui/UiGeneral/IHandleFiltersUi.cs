// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface IHandleFiltersUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IHandleFiltersUi CheckBoxFilterOnHiddenFiltersWindow(string filterName);
        int CheckIfFilterExist(string filterName);
        IHandleFiltersUi ClickOnApplyBtn();
        IHandleFiltersUi SelectUnselectElementInMultiSelect(string filterName,
            string valueToCheck, string valueToUncheck = null);

        IHandleFiltersUi ClickOnHiddenFiltersBtn();
        IHandleFiltersUi ClickOnHideFilterBtn(string filterName);
        IHandleFiltersUi ClickToOpenFiltersMenu();
        ISearchResultsUi MultiSelectDropDownPipe(string filterName, string value);
        ISearchResultsUi MultiSelectDropDownPipe(string filterName, string[] values);
        IHandleFiltersUi SearchValueInMultiSelectFilter(string filterName, string value);
        ISearchResultsUi SetTableColumnVisibility(string columnName);
        ISearchResultsUi SetTimeFilter(string time);
        ISearchResultsUi SingleSelectDropDownPipe(string filterName, string valueToCheck, string valueToUncheck = null);
        ISearchResultsUi UnassignPipe(string filterName);
        bool VerifyButtonEnable(string buttonName);
        IHandleFiltersUi WaitForTableToLoad();
    }
}