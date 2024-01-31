// Ignore Spelling: Api Crm Forex Dods

using AirSoftAutomationFramework.Objects.DTOs;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public interface IPlatformTabApi
    {
        T ChangeContext<T>(IWebDriver driver) where T : class;
        IPlatformTabApi ChangeDocumentStatusPipe(string url, string documentName, bool isActive = true);
        IPlatformTabApi ChangeDodStatusPipe(string url, string documentName, bool isActive = true);
        IPlatformTabApi ChangeEmailStatusPipe(string url, string emailType, bool isActive = true);
        List<string> CheckEmailBodyParams(Dictionary<string, string> emailBodyParams);
        string ComposeEmailBody(List<string> emailBodyVariables);
        string CreateBannerPipe(string url, string bannerName, string bannerBody = null, string apiKey = null);
        IPlatformTabApi CreateDocumentPipe(string url, Dictionary<string, string> emailParams, string documentBody, string apiKey = null);
        IPlatformTabApi CreateDodPipe(string url, Dictionary<string, string> emailParams, string documentBody);
        IPlatformTabApi CreateTermsAndConditionPipe(string url, Dictionary<string, string> emailParams, string documentBody, string apiKey = null);
        IPlatformTabApi DeleteBannerRequest(string url, List<string> bannerIds, string apiKey = null);
        IPlatformTabApi DeleteBannerRequest(string url, string bannerId, string apiKey = null);
        IPlatformTabApi DeleteCustomEmailRequest(string url, string customEmailId, string apiKey = null, bool checkStatusCode = true);
        IPlatformTabApi DeleteDocumentRequest(string url, string documentId, string apiKey = null);
        IPlatformTabApi DeleteDod(string url, List<string> dodsNames);
        IPlatformTabApi DeleteDod(string url, string dodName);
        List<Email> FilterEmailByBody(string url, string emailAddress, string subject, string body);
        Email FilterEmailByBodyPipe(string url, string emailAddress, string filter);
        List<Email> FilterEmailBySubject(string url, string emailAddress, string subject);
        List<GeneralDto> GetAutoEmailsRequest(string url, string apiKey = null);
        List<GetBannersResponse> GetBannersRequest(string url);
        GetCustomEmailsResponse GetCustomEmailByIdRequest(string url, string emailId);
        List<GetCustomEmailsResponse> GetCustomEmailsRequest(string url, string apiKey = null);
        List<GeneralDto> GetDocumentsRequest(string url, string apiKey = null);
        bool GetDocumentStatusByNameRequest(string url, string documentName);
        List<GeneralDto> GetDodsRequest(string url, string apiKey = null);
        bool GetDodStatusByNameRequest(string url, string dodName);
        List<Email> GetEmailsRequest(string url, string emailAddress, string subject = null);
        bool GetEmailStatusByNameRequest(string url, string emailType);
        List<GeneralDto> GetTermsAndConditionsRequest(string url, string apiKey = null);
        Dictionary<string, string> ParseMailToKeyValuePair(Email email);
        List<Dictionary<string, string>> ParseMailToKeyValuePair(List<Email> emails);
        IPlatformTabApi PatchEmailStatusRequest(string url, string documentId, bool isActive = true);
        IPlatformTabApi PostCreateBannerRequest(string url, string bannerName, string bannerBody = null, string apiKey = null);
        string PostCreateCustomSystemEmailRequest(string url, Dictionary<string, string> emailParams, string apiKey = null, bool checkStatusCode = true);
        string PostCreateSystemEmailRequest(string url, Dictionary<string, string> emailParams, string apiKey = null, bool checkStatusCode = true);
        IPlatformTabApi PostDocumentRequest(string url, Dictionary<string, string> emailParams, string documentBody, string apiKey = null);
        string PostDodRequest(string url, Dictionary<string, string> emailParams, string documentBody, string apiKey = null, bool checkStatusCode = true);
        string PostSaveCustomEmailRequest(string url, Dictionary<string, string> emailParams, string apiKey = null, bool checkStatusCode = true);
        string PostSendCustomEmailRequest(string url, string emailBody, string clientId, string emailName, string emailSubject, string apiKey = null, bool checkStatusCode = true);
        IPlatformTabApi PutBannerInSettingsRequest(string url, GetBannersResponse getBannersResponse, string apiKey = null);
        string PutCustomEmailRequest(string url, GetCustomEmailsResponse getCustomEmailsResponse, string apiKey = null, bool checkStatusCode = true);
        IPlatformTabApi PutEmailByIdTemplateRequest(string url, Dictionary<string, string> emailParams);
        string PutTermsAndConditionRequest(string url, bool termsConditions = true, string apiKey = null, bool checkStatusCode = true);
        IPlatformTabApi UpdateCustomEmailTemplatePipe(string url, Dictionary<string, string> emailParams, List<string> bodyEmailParams, string apiKey = null);
        IPlatformTabApi UpdateEmailTemplatePipe(string url, Dictionary<string, string> emailParams, List<string> bodyEmailParams);
        IPlatformTabApi WaitForNumOfEmails(string url, string emailAddress, int expectedNumOfEmails);
    }
}