using System.Collections.Generic;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public interface IChronoPageUi
    {
        IChronoPageUi BuyChronoPipe(string assetName, string multiplier = null, string timeLimit = null);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        bool CheckIfChronoMenuItemExist();
        IChronoPageUi ClickBuyButton();
        IChronoPageUi ClickOnBoostByName(string boostName);
        IChronoPageUi ClickOnConfirmTradeButton();
        IChronoPageUi ClickOnStopChronoTradeButton();
        string GetBlockTradeMessage();
        IList<IWebElement> GetDisabledBoosts();
        string GetEarlyStopPnlRgbColor();
        string GetTradeConfirmationBodyPopup();
        IChronoPageUi SetMultiplier(string multiplierId = null);
        IChronoPageUi SetTimeLimit(string timeLimitId = null);
        IChronoPageUi VerifyBlockChronoTradeMessage(string message);
        IChronoPageUi VerifyBoostDisableByName(string boostName);
        IChronoPageUi VerifyBoostEnableByName(string boostName);
        IChronoPageUi VerifyChronoOrderActivatedMessage(string message);
        IChronoPageUi WaitForChronoTimerToFinish(string expectedWait);
    }
}