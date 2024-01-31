// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.UiGeneral
{
    public interface IChatUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IChatUi ClickOnChatIcon();
        IChatUi ClickOnChatParticipant(string clientFullame);
        string GetRevivedChatMessage(string chatMessage);
        IChatUi SetChatMessage(string chatMessage);
        IChatUi WaitForBadgeRedNotificationNumber();
        IChatUi WaitForLoaderForNotDisplay();
    }
}