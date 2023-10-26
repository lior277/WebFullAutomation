// Ignore Spelling: Api Crm Forex Dods

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class PlatformTabApi : IPlatformTabApi
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public PlatformTabApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string ComposeEmailBody(List<string> emailBodyVariables)
        {
            var builder = new StringBuilder();

            foreach (var item in emailBodyVariables)
            {
                var value = item + "=" + "{" + item + "}" + ",";
                builder.Append(value);
            }

            return builder
                .ToString()
                .TrimEnd(',')
                .EncodeBase64();
        }

        public IPlatformTabApi PutEmailByIdTemplateRequest(string url,
            Dictionary<string, string> emailParams)
        {
            var emailExist = GetAutoEmailsRequest(url)
                .Where(p => p.Type == emailParams["type"])
                .Any();

            if (!emailExist)
            {
                PostCreateSystemEmailRequest(url, emailParams, _apiKey);
            }

            var emailId = GetAutoEmailsRequest(url)
                .Where(p => p.Type == emailParams["type"])?
                .FirstOrDefault()?
                .Id;

            var route = ApiRouteAggregate.PutEmailByIdTemplate(emailId);
            route = $"{url}{route}?api_key={_apiKey}";

            var PutEmailByIdTemplateDto = new
            {
                type = emailParams["type"],
                language = emailParams["language"],
                subject = emailParams["subject"],
                active = true,
                body = emailParams["body"]
            };
            var response = _apiAccess.ExecutePutEntry(route, PutEmailByIdTemplateDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IPlatformTabApi UpdateCustomEmailTemplatePipe(string url,
            Dictionary<string, string> emailParams,
            List<string> bodyEmailParams, string apiKey = null)
        {
            var emailBody = ComposeEmailBody(bodyEmailParams);
            emailParams.Add("body", emailBody);
            PostCreateCustomSystemEmailRequest(url, emailParams);

            return this;
        }

        // get system emails
        public List<GeneralDto> GetAutoEmailsRequest(string url, string apiKey = null)
        {

            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetAutoEmails();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json);
        }

        public List<GeneralDto> GetDocumentsRequest(string url, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetDocuments();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json);
        }

        public List<GetCustomEmailsResponse> GetCustomEmailsRequest(string url,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetCustomEmails();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetCustomEmailsResponse>>(json);
        }

        public GetCustomEmailsResponse GetCustomEmailByIdRequest(string url,
            string emailId)
        {
            var route = ApiRouteAggregate.PutEmailByIdTemplate(emailId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetCustomEmailsResponse>(json);
        }

        public List<GeneralDto> GetDodsRequest(string url, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetDods();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json);
        }

        public List<GeneralDto> GetTermsAndConditionsRequest(string url, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.GetTermsAndConditions();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json);
        }

        public bool GetDodStatusByNameRequest(string url, string dodName)
        {
            var route = ApiRouteAggregate.GetDods();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json)
                .Where(p => p.Name == dodName)
                .FirstOrDefault()
                .Active;
        }

        public bool GetDocumentStatusByNameRequest(string url, string documentName)
        {
            var route = ApiRouteAggregate.GetDocuments();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json)
                 .Where(p => p.Name == documentName)
                .FirstOrDefault()
                .Active;
        }

        public bool GetEmailStatusByNameRequest(string url, string emailType)
        {
            var route = ApiRouteAggregate.GetAutoEmails();
            route = $"{url}{route}?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GeneralDto>>(json)
                .Where(p => p.Type == emailType)
                .FirstOrDefault()
                .Active;
        }

        public IPlatformTabApi DeleteDocumentRequest(string url,
            string documentId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.DeleteDocument(documentId);
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IPlatformTabApi CreateDocumentPipe(string url,
            Dictionary<string, string> emailParams, string documentBody,
            string apiKey = null)
        {
            var documentId = GetDocumentsRequest(url, apiKey)
                .Where(p => p.Name == emailParams["name"])
                .DefaultIfEmpty(null)
                .First()
                ?.Id;

            if (documentId != null)
            {
                DeleteDocumentRequest(url, documentId, apiKey);
                PostDocumentRequest(url, emailParams, documentBody, apiKey);
            }
            else
            {
                PostDocumentRequest(url, emailParams, documentBody, apiKey);
            }

            return this;
        }

        public IPlatformTabApi CreateTermsAndConditionPipe(string url,
            Dictionary<string, string> emailParams, string documentBody,
            string apiKey = null)
        {
            var documentId = GetTermsAndConditionsRequest(url, apiKey)
                .Where(p => p.Name == emailParams["name"])
                .DefaultIfEmpty(null)
                .First()
                ?.Id;

            if (documentId == null)
            {
                PostDocumentRequest(url, emailParams, documentBody, apiKey);
            }

            return this;
        }

        public string PostSaveCustomEmailRequest(string url,
           Dictionary<string, string> emailParams,
           string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.SaveEmail();
            route = $"{url}{route}?api_key={_apiKey}";

            var postEmailDto = new
            {
                type = emailParams["type"],
                name = emailParams["name"],
                language = emailParams["language"],
                subject = emailParams["subject"],
                body = emailParams["body"],
                active = true,
            };
            var response = _apiAccess.ExecutePostEntry(route, postEmailDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PutCustomEmailRequest(string url,
            GetCustomEmailsResponse getCustomEmailsResponse,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.SaveEmail();
            route = $"{url}{route}/{getCustomEmailsResponse._id}?api_key={_apiKey}";

            var postEmailDto = new
            {
                type = getCustomEmailsResponse.type,
                name = getCustomEmailsResponse.name,
                language = getCustomEmailsResponse.language,
                subject = getCustomEmailsResponse.subject,
                body = getCustomEmailsResponse.body ?? getCustomEmailsResponse.subject,
                active = getCustomEmailsResponse.active,
            };

            var response = _apiAccess.ExecutePutEntry(route, postEmailDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync()
                .Result;
        }

        public string PutTermsAndConditionRequest(string url, bool termsConditions = true,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PutTermAndCondition();
            route = $"{url}{route}?api_key={_apiKey}";

            var postEmailDto = new
            {
                terms_conditions = termsConditions
            };

            var response = _apiAccess.ExecutePutEntry(route, postEmailDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync()
                .Result;
        }

        public IPlatformTabApi DeleteCustomEmailRequest(string url,
            string customEmailId, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.SaveEmail();
            route = $"{url}{route}/{customEmailId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return this;
        }

        public string PostCreateSystemEmailRequest(string url,
           Dictionary<string, string> emailParams,
           string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.SaveEmail();
            route = $"{url}{route}?api_key={_apiKey}";

            var postEmailDto = new
            {
                type = emailParams["type"],
                language = emailParams["language"],
                subject = emailParams["subject"],
                body = emailParams["body"],
                active = true,
            };
            var response = _apiAccess.ExecutePostEntry(route, postEmailDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PostCreateCustomSystemEmailRequest(string url,
           Dictionary<string, string> emailParams,
           string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.SaveEmail();
            route = $"{url}{route}?api_key={_apiKey}";

            var postEmailDto = new
            {
                type = emailParams["type"],
                language = emailParams["language"],
                subject = emailParams["subject"],
                body = emailParams["body"],
                name = emailParams["name"],
                active = true,
            };
            var response = _apiAccess.ExecutePostEntry(route, postEmailDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PostSendCustomEmailRequest(string url,
            string emailBody, string clientId, string emailName,
            string emailSubject, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PostSendDirectEmail();
            route = $"{url}{route}?api_key={_apiKey}";

            var sendEmailDto = new
            {
                type = "custom",
                name = emailName,
                language = "en",
                subject = emailSubject,
                active = true,
                body = emailBody,
                user_id = clientId
            };

            var response = _apiAccess.ExecutePostEntry(route, sendEmailDto);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public IPlatformTabApi PostDocumentRequest(string url,
           Dictionary<string, string> emailParams, string documentBody,
           string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PostDocument();
            route = $"{url}{route}?api_key={_apiKey}";

            var postDocumentDto = new
            {
                type = emailParams["type"],
                language = emailParams["language"],
                name = emailParams["name"],
                active = true,
                body = documentBody
            };
            var response = _apiAccess.ExecutePostEntry(route, postDocumentDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PostDodRequest(string url,
          Dictionary<string, string> emailParams, string documentBody,
          string apiKey = null, bool checkStatusCode = true)
        {
            var route = ApiRouteAggregate.PostDocument();
            _apiKey = apiKey ?? _apiKey;
            route = $"{url}{route}?api_key={_apiKey}";

            var postDocumentDto = new
            {
                name = emailParams["name"],
                language = emailParams["language"],
                body = documentBody,
                active = true,
                send_by = emailParams["sendBy"],
                deposit_type = emailParams["depositType"],
                type = "dod"
            };
            var response = _apiAccess.ExecutePostEntry(route, postDocumentDto);
            var json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public IPlatformTabApi CreateDodPipe(string url,
         Dictionary<string, string> emailParams, string documentBody)
        {
            var dodExist = GetDodsRequest(url)
                .Any(p => p.Name == emailParams["name"]);

            if (!dodExist)
            {
                PostDodRequest(url, emailParams, documentBody);
            }

            return this;
        }

        public IPlatformTabApi DeleteDod(string url, string dodName)
        {
            string dodId = null;

            // get all DODs ids
            var dod = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetDodsRequest(url)
                .Where(p => p.Name == dodName)
                .FirstOrDefault();

            if (dod != null)
            {
                dodId = dod.Id;

                // delete  DOD by name
                var route = ApiRouteAggregate.DeleteDocument(dodId);
                route = $"{url}{route}?api_key={_apiKey}";
                var response = _apiAccess.ExecuteDeleteEntry(route);
                _apiAccess.CheckStatusCode(route, response);
            }

            return this;
        }

        public IPlatformTabApi DeleteDod(string url, List<string> dodsNames)
        {
            dodsNames.ForEach(p => DeleteDod(url, p));

            return this;
        }

        private IPlatformTabApi PatchDodtStatusRequest(string url, string documentId,
            bool isActive = true)
        {
            var route = ApiRouteAggregate.PatchDocumentStatus(documentId);
            route = $"{url}{route}?is_active={isActive.ToString().ToLower()}&api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IPlatformTabApi ChangeDodStatusPipe(string url, string documentName,
            bool isActive = true)
        {
            var documentId = GetDodsRequest(url)
                .Where(p => p.Name == documentName)
                .FirstOrDefault()
                .Id;

            PatchDodtStatusRequest(url, documentId, isActive);

            return this;
        }

        private IPlatformTabApi PatchDocumentStatusRequest(string url,
            string documentId, bool isActive = true)
        {
            var route = ApiRouteAggregate.PatchDocumentStatus(documentId);
            route = $"{url}{route}?is_active={isActive.ToString().ToLower()}&api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IPlatformTabApi ChangeDocumentStatusPipe(string url, string documentName,
            bool isActive = true)
        {
            var documentId = GetDocumentsRequest(url)
                .Where(p => p.Name == documentName)
                .FirstOrDefault()
                .Id;

            PatchDocumentStatusRequest(url, documentId, isActive);

            return this;
        }

        public IPlatformTabApi PatchEmailStatusRequest(string url, string documentId,
            bool isActive = true)
        {
            var route = ApiRouteAggregate.PatchEmailStatus(documentId);
            route = $"{url}{route}?is_active={isActive.ToString().ToLower()}&api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IPlatformTabApi ChangeEmailStatusPipe(string url, string emailType,
            bool isActive = true)
        {
            var emailId = GetAutoEmailsRequest(url)
                .Where(p => p.Type == emailType)
                .FirstOrDefault()
                .Id;

            PatchEmailStatusRequest(url, emailId, isActive);

            return this;
        }

        public Dictionary<string, string> ParseMailToKeyValuePair(Email email)
        {
            var keyValueParams = new Dictionary<string, string>();
            var emailBodyParams = email.Body.Split(',').ToList();
            var emailSubject = email.Subject;
            keyValueParams.Add("subject", emailSubject);

            if (emailBodyParams == null)
            {
                throw new ArgumentNullException("Email params are empty");
            }

            foreach (var param in emailBodyParams)
            {
                var elements = param.Split(new[] { '=' }, 2);
                keyValueParams.Add(elements.First().Trim(), elements.Last().Trim());
            }

            return keyValueParams;
        }

        public List<Dictionary<string, string>> ParseMailToKeyValuePair(List<Email> emails)
        {
            var emailList = new List<Dictionary<string, string>>();

            foreach (var email in emails)
            {
                emailList.Add(ParseMailToKeyValuePair(email));
            }

            return emailList;
        }

        public List<string> CheckEmailBodyParams(Dictionary<string, string> emailBodyParams)
        {
            var missingKeys = new List<string>();

            if (emailBodyParams != null)
            {
                foreach (var entry in emailBodyParams)
                {
                    if (entry.Value.Contains(entry.Key) || entry.Value.Contains("undefined")
                        || entry.Value.Contains("null"))
                    {
                        missingKeys.Add(entry.Key);
                    }
                }

                return missingKeys;
            }
            else
            {
                return missingKeys;
            }
        }

        public List<Email> GetEmailsRequest(string url, string emailAddress,
            string subject = null)
        {
            var route = $"{url}{ApiRouteAggregate.GetMailByEmailAddress()}" +
                $"?to={emailAddress}";

            List<Email> emails = null;

            try
            {
                for (var i = 0; i < 20; i++)
                {
                    var response = _apiAccess.ExecuteGetEntry(route);
                    var json = response.Content.ReadAsStringAsync().Result;

                    if (json.Contains("mails not found"))
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(2));

                        continue;
                    }

                    emails = JsonConvert.DeserializeObject<List<Email>>(json);

                    if (emails == null)
                    {
                        continue;
                    }

                    break;
                }

                if (emails == null)
                {
                    var exceMessage = ($" Email is empty ," +
                       $" no email received, emailAddress: {emailAddress}, email subject: {subject}");

                    throw new InvalidOperationException(exceMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return emails;
        }

        public List<GetBannersResponse> GetBannersRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetBanners()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetBannersResponse>>(json);
        }

        public IPlatformTabApi DeleteBannerRequest(string url, string bannerId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.DeleteBanner(bannerId)}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IPlatformTabApi DeleteBannerRequest(string url,
            List<string> bannerIds, string apiKey = null)
        {
            foreach (var id in bannerIds)
            {
                DeleteBannerRequest(url, id);
            }

            return this;
        }

        public IPlatformTabApi PostCreateBannerRequest(string url, string bannerName,
            string bannerBody = null, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateBanners()}?api_key={_apiKey}";
            string base64BannerBody = null;

            if (bannerBody == null)
            {
                var bannerBodyParams = new List<string>() { "CLIENT_NAME", "CLIENT_FIRST_NAME ",
                "CLIENT_LAST_NAME", "CLIENT_CURRENCY_SYMBOL", "DATE ", "COMPANY_NAME " };

                var builder = new StringBuilder();

                foreach (var item in bannerBodyParams)
                {
                    var value = "{" + item + "}";

                    builder
                        .Append(value);
                }

                base64BannerBody = builder
                    .ToString()
                    .TrimEnd(',')
                    .EncodeBase64();
            }

            var bannerDto = new
            {
                active = true,
                body = bannerBody?.EncodeBase64() ?? base64BannerBody,
                name = bannerName
            };
            var response = _apiAccess.ExecutePostEntry(route, bannerDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string CreateBannerPipe(string url, string bannerName,
            string bannerBody = null, string apiKey = null)
        {
            var bannerId = _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetBannersRequest(url)
                .Where(p => p.Name == DataRep.AutomationBannerName)
                .FirstOrDefault()?
                .Id;

            if (bannerId != null)
            {
                DeleteBannerRequest(url, bannerId);
            }

            PostCreateBannerRequest(url, bannerName, bannerBody, apiKey);

            return _apiFactory
               .ChangeContext<IPlatformTabApi>()
               .GetBannersRequest(url)
               .Where(p => p.Name == DataRep.AutomationBannerName)
               .FirstOrDefault()?
               .Id;
        }

        public IPlatformTabApi PutBannerInSettingsRequest(string url,
            GetBannersResponse getBannersResponse, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PutBannerInSettings(getBannersResponse.Id)}?api_key={_apiKey}";

            var PutBannerDto = new
            {
                active = true,
                body = getBannersResponse.Body,
                name = getBannersResponse.Name
            };
            var response = _apiAccess.ExecutePutEntry(route, PutBannerDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<Email> FilterEmailBySubject(string url,
            string emailAddress, string subject)
        {
            var emails = GetEmailsRequest(url, emailAddress, subject);

            var emailsBySubject = emails
                .Where(p => p.Subject.Contains(subject, StringComparison.OrdinalIgnoreCase))
                .ToList();

            for (var i = 0; i < 30; i++)
            {
                if (emailsBySubject.Count == 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    emails = GetEmailsRequest(url, emailAddress, subject);

                    emailsBySubject = emails
                        .Where(p => p.Subject.Contains(subject, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (emailsBySubject.Count == 0)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(3));

                        continue;
                    }

                    break;
                }

                break;
            }

            if (emailsBySubject.All(p => p.Subject == null))
            {
                var subjectList = new List<string>();
                var actualSubjectList = new List<string>();
                emails.ForEach(p => subjectList.Add(p.Subject));

                throw new InvalidOperationException($"actual Subject List:" +
                    $" {subjectList.ListToString()}, expected Subject: {subject} ");
            }

            return emailsBySubject;
        }

        public List<Email> FilterEmailByBody(string url,
            string emailAddress, string subject, string body)
        {
            var emails = GetEmailsRequest(url, emailAddress, subject);

            var emailsByBody = emails
                .Where(p => p.Body.Contains(body, StringComparison.OrdinalIgnoreCase))
                .ToList();

            for (var i = 0; i < 30; i++)
            {
                if (emailsByBody.Count == 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    emails = GetEmailsRequest(url, emailAddress, subject);

                    emailsByBody = emails
                        .Where(p => p.Body.Contains(body, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    Thread.Sleep(TimeSpan.FromSeconds(3));

                    continue;
                }

                break;
            }

            if (emailsByBody == null)
            {
                //var subjectList = new List<string>();
                //emails.ForEach(p => subjectList.Add(p.Subject));

                throw new InvalidOperationException("emails By Body is empty");
            }

            return emailsByBody;
        }

        public IPlatformTabApi WaitForNumOfEmails(string url, string emailAddress,
            int expectedNumOfEmails)
        {
            var numOfEmails = GetEmailsRequest(url, emailAddress).Count();

            try
            {
                for (var i = 0; i < 5; i++)
                {
                    if (numOfEmails < expectedNumOfEmails)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                        numOfEmails = GetEmailsRequest(url, emailAddress).Count();

                        continue;
                    }

                    break;
                }

                if (numOfEmails < expectedNumOfEmails)
                {
                    //var exception = new Exception();

                    var exceMessage = ($" expected Num Of Emails: {expectedNumOfEmails}," +
                        $" actual Num Of Emails: {numOfEmails}");

                    throw new Exception(exceMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return this;
        }

        public Email FilterEmailByBodyPipe(string url, string emailAddress, string filter)
        {
            Email filterEmailByBody = null;
            var emails = GetEmailsRequest(url, emailAddress);
            var bodyList = new List<string>();
            emails.ForEach(p => bodyList.Add(p.Body));

            for (var i = 0; i < 25; i++)
            {
                //if (filterEmailByBody != null)
                //{
                //    break;
                //}

                filterEmailByBody = emails
                    .Where(p => p.Body.Contains(filter, StringComparison.OrdinalIgnoreCase))?
                    .FirstOrDefault();

                if (filterEmailByBody == null)
                {
                    Thread.Sleep(500);
                    emails = GetEmailsRequest(url, emailAddress);

                    filterEmailByBody = emails
                        .Where(p => p.Body.Contains(filter, StringComparison.OrdinalIgnoreCase))?
                        .FirstOrDefault();
                }
                else
                {
                    break;
                }
            }

            if (filterEmailByBody == null)
            {
                throw new NullReferenceException($"subject List {bodyList.ListToString()}" +
                   $" not contain email with subject: {filterEmailByBody} email address : {emailAddress}");
            }

            return filterEmailByBody;
        }

        public IPlatformTabApi UpdateEmailTemplatePipe(string url,
            Dictionary<string, string> emailParams, List<string> bodyEmailParams)
        {
            var emailBody = ComposeEmailBody(bodyEmailParams);
            emailParams.Add("body", emailBody);
            PutEmailByIdTemplateRequest(url, emailParams);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
