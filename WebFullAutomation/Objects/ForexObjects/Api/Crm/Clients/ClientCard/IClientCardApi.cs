using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientCard
{
    public interface IClientCardApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        string ExportClientCardPipe(string url, string clientId, string userEmail,
            string apiKey = null, bool checkStatusCode = true);
        GeneralResult<GeneralDto> GetExportClientCardRequest(string url, string exportLink, string apiKey = null, bool checkStatusCode = true);
        IClientCardApi PatchSetClientCurrencyRequest(string url, string clientId, string currencyCode, string apiKey = null);
        IClientCardApi PatchSetClientPasswordRequest(string url, string clientId, string clientName, string newPassword = null, string apiKey = null);
        IClientCardApi PostForgotPasswordRequest(string url, string clientEmail, GetLoginResponse loginData);
        string PostMakeCallRequest(string url, string clientId, string pbxName, string phone, string apiKey = null, bool checkStatusCode = true);
        IClientCardApi PostResetPasswordRequest(string url, string clientId);
        IClientCardApi PostSendDirectEmailRequest(string url, string clientId);
        IClientCardApi WaitForBalanceInClientCardToBeUpdated(string url, string clientId, double amount);
    }
}