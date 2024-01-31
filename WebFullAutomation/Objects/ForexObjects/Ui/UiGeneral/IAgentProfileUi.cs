// Ignore Spelling: Forex

using System.Collections.Generic;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface IAgentProfileUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IAgentProfileUi CloseAgentProfile();
        List<string> GetPrevMouthData();
        List<string> GetSellerRoundProgressData();
        List<string> GetTopTenDeposits();
        List<string> GetTopTenPnl();
        bool IsChartExist();
        int GetTopStatisticsBoxCurrencySign();
    }
}