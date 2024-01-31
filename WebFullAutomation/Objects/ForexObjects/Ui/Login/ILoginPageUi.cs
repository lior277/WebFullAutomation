using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login
{
    public interface ILoginPageUi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        ILoginPageUi ChangePasswordPipe(string NewPassword, string currentUrl);
        ILoginPageUi ClickOnLoginButton();
        ILoginPageUi ClickOnApproveButton();
        ILoginPageUi ClickOnSignatureField();
        ILoginPageUi ClickOnDeclineButton(); 
        ILoginPageUi ClickOnLoginButtonForChangePassword(string currentUrl);
        ILoginPageUi ClickOnLogOut();
        ILoginPageUi DeleteAllCookies();
        string GetLoginMessage(string erorMessage);
        ILoginPageUi LoginPipe(string userName,
            string url = null, string password = null,
            bool deleteCookies = false, bool needRefresh = false, string apiKey = null);
        ILoginPageUi NavigateToUrl(string url);
        ILoginPageUi SetPassword(string password = null);
        ILoginPageUi SetUserName(string userName);
        ILoginPageUi WaitForUrlToChange(string expectedUrl);
    }
}