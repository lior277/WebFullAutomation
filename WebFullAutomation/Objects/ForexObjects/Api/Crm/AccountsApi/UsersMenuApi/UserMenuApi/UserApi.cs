// Ignore Spelling: Forex Api

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.Ui.Login;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using static AirSoftAutomationFramework.Objects.DTOs.UsersMongoTableDto;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi
{
    public class UserApi : IUserApi
    {
        #region Members    
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IMongoDbAccess _mongoDbAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private string _usersTable = DataRep.UsersTable;
        private IWebDriver _driver;
        #endregion Members    

        public UserApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _mongoDbAccess = mongoDbAccess;
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public string CreateUserForUiPipe(string url, string userName,
            string phone = null, string country = null, string emailPrefix = null,
            string password = null, string role = "admin", string pbxName = null,
            bool affiliate = false, bool active = true, bool addAsChild = false,
            string[] subUsers = null, string salesType = "retention",
            string salesAgentSalaryId = null, GetOfficeResponse officeData = null,
            string apiKey = null)
        {
            var response = PostCreateUserRequest(url, userName, phone,
                country, emailPrefix, password, role, pbxName, affiliate,
                active, addAsChild, subUsers: subUsers, salesType,
                salesAgentSalaryId, officeData, apiKey);

            var actualEmailPerfix = emailPrefix ?? DataRep.EmailPrefix;
            var email = userName + actualEmailPerfix;

            ChangeContext<ILoginPageUi>(_driver)
                .LoginPipe(email, url);

            //GetFingerPrint(email);
            PatchAddFingerPrintPipe(url, email);

            return response;
        }

        public string PostCreateUserRequest(string url, string userName,
            string phone = null, string country = null, string emailPrefix = null,
            string password = null, string role = "admin", string pbxName = null,
            bool affiliate = false, bool active = true, bool addAsChild = true,
            string[] subUsers = null, string salesType = "retention",
            string salesAgentSalaryId = null, GetOfficeResponse officeData = null,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateUser()}?api_key={_apiKey}";
            var actualEmailPerfix = emailPrefix ?? DataRep.EmailPrefix;
            var email = userName + actualEmailPerfix;

            var officeDetails = officeData ?? _apiFactory
                .ChangeContext<IOfficeTabApi>(_driver)
                .GetOfficesByName(url);

            var exc = new Extension()
            {
                pbx_name = pbxName,
                ext_num = DataRep.UserDefaultExtension // from almog
            };

            Extension[] extensions;

            if (pbxName == null)
            {
                extensions = Array.Empty<Extension>();
            }
            else
            {
                extensions = new Extension[] { exc };
            }

            var userDto = new CreateUserRequest
            {
                first_name = userName,
                last_name = userName,
                username = userName,
                phone = phone ?? DataRep.UserDefaultPhone,
                country = country ?? DataRep.UserDefaultCountry,
                email = email,
                password = password ?? DataRep.Password,
                role = role,
                extensions = extensions,
                allowed_ip_addresses = DataRep.UserAllowedIps,
                office = officeDetails._id,
                affiliate = affiliate,
                active = active,
                add_as_child = addAsChild,
                sales_type = salesType,
                sub_users = subUsers ?? Array.Empty<string>(),
                gmt_timezone = officeDetails.gmt_timezone,
                salary_id = salesAgentSalaryId
            };

            var response = _apiAccess.ExecutePostEntry(route, userDto);

            var json = response
              .Content
              .ReadAsStringAsync()
              .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return JsonConvert.DeserializeObject<string>(json);
            }

            return json;
        }

        public List<string> PostCreateUserRequest(string url,
            List<string> userNames, string phone = null, string country = null,
            string emailPrefix = null, string password = null,
            string role = "admin", string pbxName = null, bool affiliate = false,
            bool active = true, bool addAsChild = false,
            string[] subUsers = null, string salesType = "retention",
            string salesAgentSalaryId = null, GetOfficeResponse officeData = null)
        {
            var usersIds = new List<string>();

            foreach (var name in userNames)
            {
                var response = PostCreateUserRequest(url, name, phone,
                country, emailPrefix, password, role, pbxName: pbxName, affiliate,
                active, addAsChild, subUsers, salesType, salesAgentSalaryId,
                officeData);

                usersIds.Add(response);
            }

            return usersIds;
        }

        public string PostCreateWooCommerceUserRequest(string url)
        {
            var username = "woocommerce-agent";

            var WooCommerceUserExist = GetUsersRequest(url)
                .userData
                .Where(p => p.username == username)
                .FirstOrDefault();

            string userId = null;

            if (WooCommerceUserExist == null)
            {
                userId = PostCreateUserRequest(url, username);

                // create ApiKey
                DataRep.ApiKeyOfWooCommerceUser = _apiFactory
                    .ChangeContext<IUserApi>()
                    .PostCreateApiKeyRequest(url, userId);
            }
            else
            {
                userId = GetUsersRequest(url)
                .userData
                .Where(p => p.username == username)
                .FirstOrDefault()
                ._id;

                DataRep.ApiKeyOfWooCommerceUser = 
                    PostCreateApiKeyRequest(url, userId);
            }

            return userId;
        }

        public IUserApi CreateUserIfNotExistByNamePipe(string url, string userName,
            string mailPrefix = null)
        {
            var users = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetActiveUsersRequest(url);

            if (!users.userData.Any(p => p.first_name == userName))
            {
                PostCreateUserRequest(url, userName, emailPrefix: mailPrefix);
            }

            return this;
        }

        public IUserApi PutEditUserRoleRequest(string url,
            string userId, string roleName)
        {
            var route = $"{url}{ApiRouteAggregate.PostCreateUser()}/{userId}?api_key={_apiKey}";

            var user = _apiFactory
                .ChangeContext<IUsersApi>()
                .GetUserByIdRequest(url, userId)
                .GeneralResponse
                .user;

            var json = JsonConvert.SerializeObject(user);
            var dto = JObject.Parse(json);
            dto.Property("last_login").Remove();
            json = JsonConvert.SerializeObject(user);
            var putUserDto = JsonConvert.DeserializeObject<PutUserRequest>(json);
            putUserDto.Role = roleName;
            var response = _apiAccess.ExecutePutEntry(route, putUserDto);
            json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public string PutEditUserRequest(string url,
            GetUserResponse getUserResponse, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateUser()}/{getUserResponse.user._id}?api_key={_apiKey}";
            var json = JsonConvert.SerializeObject(getUserResponse.user);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            dto.Property("allowed_fingerprints").Remove();
            dto.Property("last_update_password").Remove();
            dto.Property("affiliate").Remove();
            dto.Property("last_login").Remove();
            var response = _apiAccess.ExecutePutEntry(route, dto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public IUserApi PutEditUserOfficeRequest(string url, GetUserResponse getUserResponse)
        {
            var user = getUserResponse.user;
            var route = $"{url}{ApiRouteAggregate.PostCreateUser()}/{getUserResponse.user._id}?api_key={_apiKey}";
            var json = JsonConvert.SerializeObject(getUserResponse.user);
            var dto = JObject.Parse(json);
            dto.Property("_id").Remove();
            dto.Property("allowed_fingerprints").Remove();
            dto.Property("last_update_password").Remove();
            dto.Property("affiliate").Remove();
            dto.Property("last_login").Remove();
            var response = _apiAccess.ExecutePutEntry(route, dto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IUserApi PatchAddFingerPrintPipe(string url, string email)
        {
            var fingerPrintFromMongo = GetPendingFingerPrintFromMongo(email);
            var route = $"{url}{ApiRouteAggregate.PatchAddFingerPrint()}?api_key={_apiKey}";

            var addFingerPrintDto = new AddFingerPrintRequest
            {
                email = email,
                fingerprint = fingerPrintFromMongo
            };

            var response = _apiAccess.ExecutePatchEntry(route, addFingerPrintDto);
            var json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IUserApi PatchDeleteOrRestoreUserRequest(string url, string userId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PatchHideUser()}{userId}?api_key={_apiKey}";
            var response = _apiAccess.ExecutePatchEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }
      
        public string PostCreateApiKeyRequest(string uri, string userId,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{uri}{ApiRouteAggregate.PostCreateApiKey()}?api_key={_apiKey}";
            string json;

            var apiKeyDto = new
            {
                id = userId
            };
            var response = _apiAccess.ExecutePostEntry(route, apiKeyDto);
            json = response.Content.ReadAsStringAsync().Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
                return JObject.Parse(json)["key"].ToString();
            }

            return json;
        }

        public string GetPendingFingerPrintFromMongo(string email)
        {
            var mongoDatabase = InitializeMongoClient.ConnectToCrmMongoDb;
            var fingerprint = "";
            Pending_Fingerprints[] pendingFingerPrints = null;
            List<UsersMongoTableDto> mongoUser = null;

            try
            {
                for (var i = 0; i < 28; i++)
                {
                    mongoUser = _mongoDbAccess
                      .SelectAllDocumentsFromTable<UsersMongoTableDto>(mongoDatabase, _usersTable)
                      .Where(p => p.email == email)?
                      .ToList();

                    // check if user exist
                    if (mongoUser != null)
                    {
                        pendingFingerPrints = mongoUser
                            .FirstOrDefault()?
                            .pending_fingerprints;

                        if (pendingFingerPrints.Length == 0)
                        {
                            Thread.Sleep(500);
                        }
                        else
                        {
                            fingerprint = pendingFingerPrints
                                .FirstOrDefault()
                                .fingerprint;
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }

                //for (var i = 0; i < 20; i++)
                //{
                //    if (pendingFingerPrints != null)
                //    {
                //        // check for pending finger print
                //        if (pendingFingerPrints.Length != 0)
                //        {
                //            fingerprint = pendingFingerPrints
                //                .FirstOrDefault()
                //                .fingerprint;

                //            if (fingerprint == null)
                //            {
                //                Thread.Sleep(1000);

                //                continue;
                //            }

                //            break;
                //        }
                //    }
                //}

                if (pendingFingerPrints.Length == 0 || mongoUser == null)
                {
                    throw new NullReferenceException("there is no pending Finger Print");
                }
            }
            catch (Exception ex)
            {
                var exceMessage = ($"Message: {ex?.Message}, user email: {email}");

                throw new Exception(exceMessage);
            }

            return fingerprint;
        }

        public string GetAllowedFingerPrintFromMongo(string email)
        {
            var mongoDatabase = InitializeMongoClient.ConnectToCrmMongoDb;

            return _mongoDbAccess
                .SelectAllDocumentsFromTable<UsersMongoTableDto>(mongoDatabase, _usersTable)
                .Where(p => p.email == email)?
                .FirstOrDefault()
                .allowed_fingerprints
                .FirstOrDefault()?
                .ToString();
        }

        public GetUsersResponse GetUsersRequest(string url,
            bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.GetUsers()}&api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            return JsonConvert.DeserializeObject<GetUsersResponse>(json);
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
