// Ignore Spelling: api Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface IBroadcastMessageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IBroadcastMessageUi CheckSelectedUser(string userNameToSelect);
        IBroadcastMessageUi ClickOnBroadcastButton();
        IBroadcastMessageUi ClickOnForewordButton();
        IBroadcastMessageUi ClickOnGotItButton();
        IBroadcastMessageUi ClickOnNextButton();
        IBroadcastMessageUi ClickOnSendButton();
        IBroadcastMessageUi SearchUser(string userName);
        IBroadcastMessageUi SetMessage(string message);
        IBroadcastMessageUi SetSubject(string subject);
        IBroadcastMessageUi VerifyBroadCastBody(string body);
        IBroadcastMessageUi VerifyBroadCastTitle(string title);
    }
}