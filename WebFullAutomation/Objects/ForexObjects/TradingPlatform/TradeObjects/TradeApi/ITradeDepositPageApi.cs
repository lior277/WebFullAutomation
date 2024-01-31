using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.TradeApi
{
    public interface ITradeDepositPageApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        object GetDepositPageRequest(string url, GetLoginResponse loginData, bool checkStatusCode = true);
        GeneralDto PostCreatePaymentRequestPipe(string url, GetLoginResponse loginData, double? depositAmount = null, bool checkStatusCode = true);
        ITradeDepositPageApi PostSendCustomPspBankDetailsRequest(string url, string pspBody, string pspTitle, GetLoginResponse loginData);
        ITradeDepositPageApi PostSendCustomPspWalletDetailsRequest(string url, string pspBody, string pspTitle, string pspFooter, string pspWallet, GetLoginResponse loginData);
    }
}