using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Cfd
{
    public interface ITradeGroupCardUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ITradeGroupCardUi ClickOnCryptoGroupTabByName(string tabName);
        ITradeGroupCardUi ClickOnEditAsset(string assetName);
        ITradeGroupCardUi ClickOnEditAssetType(string assetType);
        ITradeGroupCardUi ClickOnOkBtn();
        ITradeGroupsUi ClickOnSaveBtn();
        ITradeGroupCardUi ClickOnDefaultCheckbox();
        ITradeGroupCardUi VerifyAssetSpreadIsDisable();
        ITradeGroupCardUi VerifyAssetTypeSpreadIsDisable();
        ITradeGroupCardUi VerifyGeneralSpreadIsDisable();
    }
}