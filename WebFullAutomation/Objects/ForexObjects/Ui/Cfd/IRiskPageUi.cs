using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public interface IRiskPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ISearchResultsUi SearchRisks(string searchText);
        IRiskPageUi WaitForNumOfElementInRiskTable(int expectedNumOfRows);
        IRiskPageUi WaitForRiskTableToLoad();
    }
}