using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.TradingPlatform.TradeObjects.Ui
{
    public interface IProfilePageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        bool CheckIfProofOfIdentitySectionExist(int numOfElement);
        IProfilePageUi ClickOnFirstNamePencil();
        IProfilePageUi ClickOnKycSection(string sectionName);
        IProfilePageUi ClickOnSaveProfilebutton();
        string GetProfileName(string firstName);
        IProfilePageUi SetFirstName(string firstName);
        IProfilePageUi UploadKycFile();
        IProfilePageUi VerifyMessages(string message);
        IProfilePageUi VerifyPendingStatus();
    }
}