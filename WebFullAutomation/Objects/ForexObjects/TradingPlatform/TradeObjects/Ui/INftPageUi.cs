using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public interface INftPageUi
    {
        NftPageUi BuyNftPipe(string assetName);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        NftPageUi ClickOnBuyNft(string nftName);
        INftPageUi ClickOnCloseDealNft();
        INftPageUi VerifyMessages(string message);
    }
}