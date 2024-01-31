using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui.Settings
{
    public interface IDeclarationOfDepositUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IHistoryPageUi ClickOnSaveSignatureButton();
        IDeclarationOfDepositUi SetSignature();
    }
}