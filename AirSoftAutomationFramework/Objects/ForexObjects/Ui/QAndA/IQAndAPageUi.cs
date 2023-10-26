// Ignore Spelling: Forex

using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.QAndA
{
    public interface IQAndAPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IQAndAPageUi ClickOnAddQuestionAndAnswer();
        IQAndAPageUi ClickOnQuestionByName(string question);
        IQAndAPageUi ClickOnSave();
        IQAndAPageUi ClickUploadImage();
        IQAndAPageUi ClickOnConfirm();
        string GetAnswerImage();
        IQAndAPageUi SetAnswer(string answer);
        IQAndAPageUi SetQuestion(string question);
        IQAndAPageUi UploadImageToAnswer();
        IQAndAPageUi VerifyAnswerText(string answer);
    }
}