using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Ui.Crm.Accounts.UsersMenuUi
{
    public interface IUserUi
    {
        IUserUi AddAsChild(bool addAsChild);
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IUsersPageUi ClickOnCloseUserCardButton();
        IUserUi ClickOnGenerateButton();
        IUsersPageUi ClickOnSaveButton();
        int CheckPasswordPolicy();
        IUserUi ClickOnSendApiKeyBtn();
        Dictionary<string, bool> CheckIfPasswordValid(List<string> passwords);
        IUsersPageUi CreateUserUiPipe(string url, string userName, string password = null);
        string GetApiKey();
        string GetGeneratedApiKeyBodyPopup();
        IUserUi SaveAndCreateAnotherUserButton();
        IUserUi SendApiKey(string setApiKey = null);
        IUserUi SetAccountType(string accountType = null);
        IUserUi SetActive(bool active);
        IUserUi SetAllowedIpAddresses(string[] allowedIpAddresses = null);
        IUserUi SetApiKey(string setApiKey = null);
        IUserUi SetCountry(string country = null);
        IUserUi SetEmail(string email = null);
        IUserUi SetExtention(string extention = null);
        IUserUi SetFirstName(string firstName = null);
        IUserUi SetGmtTimezone(string gmtTimezone = null);
        IUserUi SetLastName(string lastName = null);
        IUserUi SetOffice(string office = null);
        IUserUi SetPassword(string password = null);
        IUserUi SetPhone(string phone = null);
        IUserUi SetRole(string role = null);
        IUserUi SetSalaryType(string salaryType = null);
        IUserUi SetUserName(string userName);
        IUserUi SetYourStatus(bool? active = null, bool? addAsChild = null);
    }
}