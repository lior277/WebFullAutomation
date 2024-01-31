// Ignore Spelling: Forex Api Usd Crm

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Amazon.Runtime.Internal.Transform;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class GeneralTabApi : IGeneralTabApi
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;
        private IWebDriver _driver;
        #endregion Members

        public GeneralTabApi(IApplicationFactory apiFactory, IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public Dictionary<string, SaleStatusValues> GetSalesStatusRequest(string url,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetSalesStatus()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;
            var tempList = new List<string>();
            var jObject = JObject.Parse(json);
            var salesStatuses = jObject["sales_status_text"];
            salesStatuses.ForEach(x => tempList.Add(x.Parent.ToString()));

            return JsonConvert.DeserializeObject<Dictionary<string, SaleStatusValues>>(tempList.First());
        }

        public List<string> GetSalesStatusFromApiDocRequest(string url, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetSalesStatusFromApiDoc()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;
            var tempList = new List<string>();
            var jObject = JObject.Parse(json);
            var salesStatuses = jObject["sales_status_text"].ToList();
            salesStatuses.ForEach(x => tempList.Add(x.ToString()));

            return tempList;
        }

        public Dictionary<string, string> GetSalesStatus2Request(string url,
            string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetSalesStatus2()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;
            var tempDic = new Dictionary<string, string>();
            var tempList = new List<string>();
            var jObject = JObject.Parse(json);
            var salesStatuses = jObject["sales_status_text"];
            salesStatuses.ForEach(x => tempList.Add(x.Parent.ToString()));
            var ff = JsonConvert.DeserializeObject<Dictionary<string, string>>(tempList.First());

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(tempList.First());
        }

        public string PutSalesStatusRequest(string url,
            Dictionary<string, SaleStatusValues> salesStatusText,
            string newStatusName = null, string oldStatusName = null,
            string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PutSalesStatus();
            route = $"{url}{route}?api_key={_apiKey}";

            // create sale status
            if (newStatusName != null && oldStatusName == null)
            {
                salesStatusText.Add(newStatusName, new SaleStatusValues()
                {
                    answer = true,  
                    color = "#e822c7"
                });
            }

            // edit sales status
            if (newStatusName != null && oldStatusName != null)
            {
                salesStatusText.Remove(oldStatusName);
                salesStatusText.Add(newStatusName, new SaleStatusValues()
                {
                    answer = true,
                    color = "#e822c7"
                });
            }

            // delete sales status
            if (newStatusName == null && oldStatusName != null)
            {
                salesStatusText.Remove(oldStatusName);
            }

            var salesStatus = new
            {
                sales_status_text = salesStatusText,
                new_sales_status = newStatusName,
                old_sales_status = oldStatusName
            };

            var response = _apiAccess.ExecutePutEntry(route, salesStatus);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return response.Content.ReadAsStringAsync().Result;
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public string PutSalesStatus2Request(string url,
            Dictionary<string, string> salesStatusText,
            string newStatusName = null, string oldStatusName = null,
            bool active = true, string apiKey = null, bool checkStatusCode = true)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = ApiRouteAggregate.PutSalesStatus2();
            route = $"{url}{route}?api_key={_apiKey}";


            if (newStatusName != null && oldStatusName == null)
            {
                salesStatusText.Add(newStatusName, "#ef850b");
            }

            if (newStatusName != null && oldStatusName != null)
            {
                salesStatusText.Remove(oldStatusName);
                salesStatusText.Add(newStatusName, "#ef850b");
            }

            if (newStatusName == null && oldStatusName != null)
            {
                salesStatusText.Remove(oldStatusName);
            }

            var salesStatus = new
            {
                active = true,
                sales_status_text = salesStatusText,
                new_sales_status = newStatusName,
                old_sales_status = oldStatusName
            };

            var response = _apiAccess.ExecutePutEntry(route, salesStatus);

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);

                return response.Content.ReadAsStringAsync().Result;
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public Dictionary<string, string> GetSettingBySectionNameRequest(string url,
            List<string> sectionNames)
        {
            var responseList = new Dictionary<string, string>();
            string route;

            foreach (var section in sectionNames)
            {
                if (section.Contains('?'))
                {
                    route = $"{url}{ApiRouteAggregate.GetSetting()}{section}&api_key={_apiKey}";
                }
                else
                {
                    route = $"{url}{ApiRouteAggregate.GetSetting()}{section}?api_key={_apiKey}";
                }

                var response = _apiAccess.ExecuteGetEntry(route);
                _apiAccess.CheckStatusCode(route, response);

                var json = response
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                responseList.Add(section, json);
            }

            return responseList;
        }

        public Dictionary<string, string> PutSettingBySectionNameRequest(string url,
            Dictionary<string, string> sectionNamesAndBodies,
            string apiKey, bool checkStatusCode = true)
        {
            var errorDict = new Dictionary<string, string>();
            string route;
            JObject dto = null;
            string json = null;
            HttpResponseMessage response;

            foreach (var item in sectionNamesAndBodies)
            {
                if (item.Key.Contains('?'))
                {
                    route = $"{url}{ApiRouteAggregate.GetSetting()}{item.Key}&api_key={apiKey}";
                }
                else
                {
                    route = $"{url}{ApiRouteAggregate.GetSetting()}{item.Key}?api_key={apiKey}";
                }

                if (item.Value != "")
                {
                    var token = JToken.Parse(item.Value);

                    if (token is JArray && token.Any())
                    {
                        var tokenStr = JArray.Parse(item.Value).FirstOrDefault();
                        json = JsonConvert.SerializeObject(tokenStr);
                        dto = JObject.Parse(json);
                    }
                    else
                    {
                        dto = JObject.Parse(item.Value);
                    }

                    dto.Property("_id")?.Remove();
                    dto.Property("city")?.Remove();
                    dto.Property("last_update")?.Remove();
                    dto.Property("from_email")?.Remove();
                }

                response = _apiAccess.ExecutePutEntry(route, dto);
                json = response.Content.ReadAsStringAsync().Result;

                if (checkStatusCode)
                {
                    _apiAccess.CheckStatusCode(route, response);
                }

                errorDict.Add(item.Key, json);
            }

            return errorDict;
        }

        public IGeneralTabApi PutRemindAboutDepositRequest(string url,
            double remindAboutDeposit = 0.001)
        {
            var route = $"{url}{ApiRouteAggregate.PutSettingsBySection("remind_about_deposit")}" +
                $"?api_key={_apiKey}";

            var remindAboutDepositDto = new
            {
                remind_about_deposit = remindAboutDeposit
            };

            _apiAccess.ExecutePutEntry(route, remindAboutDepositDto);

            return this;
        }

        public string GetSettingRequest(string url, string apiKey, bool checkStatusCode = true)
        {
            var route = $"{url}{ApiRouteAggregate.GetSetting()}config?api_key={apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response
                .Content
                .ReadAsStringAsync()
                .Result;

            if (checkStatusCode)
            {
                _apiAccess.CheckStatusCode(route, response);
            }

            return json;
        }

        public GetEditCompanyInformationResponse GetEditCompanyInformationRequest(string url)
        {
            var route = ApiRouteAggregate.PutSettingsBySection("company");
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);

            var json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return JsonConvert.DeserializeObject<GetEditCompanyInformationResponse>(json);
        }

        public HttpResponseMessage GetDownloadMainLogoFileRequest(string url, string mainLogoUrl)
        {
            var route = $"{mainLogoUrl}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return response;
        }

        public string GetCountriesRequest(string url, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetCountries()}api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return response.Content.ReadAsStringAsync().Result;
        }

        public IGeneralTabApi PutMaximumDepositRequest(string url,
            int maxDepositUsd = 1000, int maxDepositEur = 1000,
            int maxDepositUsdT = 10000, string apiKey = null)
        {
            var route = $"{ApiRouteAggregate.PutMaximumDeposit()}/maximum_deposit";
            route = $"{url}{route}?api_key={_apiKey}";

            var maximumDepositDto = new
            {
                USD = maxDepositUsd,
                EUR = maxDepositEur,
                GBP = 0,
                BTC = 2,
                USDT = maxDepositUsdT
            };

            var response = _apiAccess.ExecutePutEntry(route, maximumDepositDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IGeneralTabApi PutMinimumDepositRequest(string url,
            int minDepositUsd = 0, int minDepositEur = 0,
            int minDepositUsdT = 0, string apiKey = null)
        {
            var route = $"{ApiRouteAggregate.PutMaximumDeposit()}/minimum_deposit";
            route = $"{url}{route}?api_key={_apiKey}";

            var maximumDepositDto = new
            {
                USD = minDepositUsd,
                EUR = minDepositEur,
                GBP = 0,
                BTC = 2,
                USDT = minDepositUsdT
            };

            var response = _apiAccess.ExecutePutEntry(route, maximumDepositDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IGeneralTabApi PutMarginCallRequest(string url)
        {
            var route = ApiRouteAggregate.PutMarginCall();
            route = $"{url}{route}?api_key={_apiKey}";

            var marginCallDto = new
            {
                margin_call = 1,
                send_email = true,
                send_notification = true
            };
            var response = _apiAccess.ExecutePutEntry(route, marginCallDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IGeneralTabApi PutGeneralSettingsRequest(string url,
            GetRegulationResponse getRegulationResponse)
        {
            var route = ApiRouteAggregate.PutSettingsBySection("general-settings");
            route = $"{url}{route}?api_key={_apiKey}";
            var editDocPartsSection = getRegulationResponse.edit_client_profile.edit_doc_parts;
            var editClientProfileSection = getRegulationResponse.edit_client_profile;

            var editDocParts = new Edit_Doc_Parts
            {
                proof_of_residency = editDocPartsSection.proof_of_residency,
                credit_debit_card_documentation = editDocPartsSection.credit_debit_card_documentation,
                general_dod = editDocPartsSection.general_dod,
                proof_of_identity = editDocPartsSection.proof_of_identity
            };

            var editClientProfile = new EditClientProfile
            {
                first_name = editClientProfileSection.first_name,
                last_name = editClientProfileSection.last_name,
                country = editClientProfileSection.country,
                show_client_name = editClientProfileSection.show_client_name,
                show_available_to_withdrawal = editClientProfileSection.show_available_to_withdrawal,
                show_doc_section = editClientProfileSection.show_doc_section,
                client_export_activity = editClientProfileSection.client_export_activity,
                edit_doc_parts = editDocParts
            };

            var putGeneralSettingRequest = new PutGeneralSettingRequest
            {
                reopen_trade = getRegulationResponse.reopen_trade, // should be removed after eran fix
                delete_bonus = getRegulationResponse.delete_bonus,
                edit_swap = getRegulationResponse.edit_swap,
                delete_trades = getRegulationResponse.delete_trades,
                export_data = getRegulationResponse.export_data,
                mass_trading = getRegulationResponse.mass_trading,
                show_closed_pnl = getRegulationResponse.show_closed_pnl,
                terms_conditions = getRegulationResponse.terms_conditions,
                terms_conditions_url = getRegulationResponse.terms_conditions_url,
                withdrawal_by_wallet = getRegulationResponse.withdrawal_by_wallet,
                export_data_email_url = getRegulationResponse.export_data_email_url ?? DataRep
                .EmailListForExport.ToArray(),

                edit_client_profile = editClientProfile,
                admin_email_for_deposit = getRegulationResponse.admin_email_for_deposit
            };
            var response = _apiAccess.ExecutePutEntry(route, putGeneralSettingRequest);

            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IGeneralTabApi PutEditCompanyInformationRequest(string url)
        {
            var route = ApiRouteAggregate.PutSettingsBySection("company");
            route = $"{url}{route}?api_key={_apiKey}";

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                DataRep.FileNameToUpload);

            // set client password
            var fileContentByteArray = _apiFactory
                .ChangeContext<IFileHandler>(_driver)
                .ConvertToBytesArray(path);

            var form = new MultipartFormDataContent
            {
                { fileContentByteArray, "main_logo", Path.GetFileName(path) },
                { new StringContent("BrandName"), "name" },
                { new StringContent("no-reply@airsoftltd.com"), "email" },
                { new StringContent("new"), "target" },
                { new StringContent("home"), "redirect" }
            };

            var response = _apiAccess.ExecutePutEntry(route, form);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IGeneralTabApi PutGeneralDodRequest(string url)
        {
            var route = ApiRouteAggregate.PutSettingsBySection("dod");
            route = $"{url}{route}?api_key={_apiKey}";

            var path = Path.Combine(Path.GetDirectoryName(Assembly
                .GetExecutingAssembly().Location), DataRep.PdfFileNameToUpload);

            var multipartFormDataContent = _apiFactory
                .ChangeContext<IFileHandler>()
                .UploadFilePipe(path, "en", "application/pdf");

            var response = _apiAccess.ExecutePutEntry(route, multipartFormDataContent);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IGeneralTabApi PutSuspiciousPnlRequest(string url,
            int suspiciousPercentage = 4000, int blockUserPercentage = 1,
            string[] exportEmails = null, bool blockUser = false)
        {
            var route = ApiRouteAggregate.PutSuspiciousPnl();
            route = $"{url}{route}?api_key={_apiKey}";

            var putSuspiciousPnlNewDto = new
            {
                percentage = suspiciousPercentage,
                admin_emails = exportEmails ?? Array.Empty<string>(),
                block_user = blockUser,
                block_user_percentage = blockUserPercentage.ToString()
            };
            var response = _apiAccess.ExecutePutEntry(route, putSuspiciousPnlNewDto);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public GetSuspiciousProfit GetSuspiciousPnlRequest(string url)
        {
            var route = ApiRouteAggregate.PutSuspiciousPnl();
            route = $"{url}{route}?api_key={_apiKey}";

            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            var json = response
               .Content
               .ReadAsStringAsync()
               .Result;

            return JsonConvert.DeserializeObject<GetSuspiciousProfit>(json);
        }

        public T ChangeContext<T>(IWebDriver driver = null) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }

        public IGeneralTabApi PutTermAndConditionRequest(string url, bool TermAndCondition = false)
        {
            throw new NotImplementedException();
        }
    }
}
