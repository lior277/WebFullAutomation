using System.Collections.Generic;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm
{
    public interface ISalesPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ISalesPageUi ClickOnSalesModeFilterByName(string filterName);
        ISalesPageUi ClickOnSalesProgressFilterByName(string filterName);
        ISalesPageUi ClickOnShowManager();
        ISalesPageUi ClickOnShowOffice();
        ISalesPageUi ClickOnShowSeller();
        List<string> GetBoxesTitles();
        List<string> GetBoxesValues();
        List<string> GetSalesProgressData();
    }
}