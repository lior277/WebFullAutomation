using System.Collections.Generic;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public interface IMarketExposurePageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        Dictionary<string, int> GetFrontCardLongPositionData();
        Dictionary<string, int> GetFrontCardPendingOrdersData();
        Dictionary<string, int> GetFrontCardShortPositionData();
        Dictionary<string, int> GetFrontCardTotalExposureData();
    }
}