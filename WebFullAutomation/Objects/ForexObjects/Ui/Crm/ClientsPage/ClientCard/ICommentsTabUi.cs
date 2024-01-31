using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.ClientsPage.ClientCard
{
    public interface ICommentsTabUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ICommentsTabUi ClickOnSaveButton();
        ICommentsTabUi SetComment(string comment = null);
        ICommentsTabUi SetPlaningTime(string planingTime = null);
        ICommentsTabUi CreateCommentPipe();
    }
}