using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings
{
    public interface IHistoryPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IDeclarationOfDepositUi ClickOnSignDod();
        IHistoryPageUi VerifyApprovedSignaturAlert(string message);
        IHistoryPageUi VerifySignatur(string verifySignaturText = "YES");
    }
}