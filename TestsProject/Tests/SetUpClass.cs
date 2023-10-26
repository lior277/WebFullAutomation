using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.UsersMenuApi.UserMenuApi;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Clients.ClientPage;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings;
using AirSoftAutomationFramework.Objects.ForexObjects.ApiGeneral;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TestsProject.TestsInternals;
using static AirSoftAutomationFramework.Objects.DTOs.CreateOfficeRequest;
using Process = System.Diagnostics.Process;

namespace TestsProject;

[SetUpFixture]
public class MySetUpClass : TestSuitBase
{
    private string _crmUrl = Config.appSettings.CrmUrl;
    private string _apiKey = Config.appSettings.ApiKey;
    private ApplicationFactory _apiFactory = new ApplicationFactory();
    private IApiAccess _apiAccess = new ApiAccess();
    //private IWebDriver _driver;

    private int GetClientsCount()
    {
        var clientsApi = new ClientsApi(_apiFactory, _apiAccess);

        return clientsApi.GetAllClientsRequest(_crmUrl).recordsTotal;
    }

    private void PatchResetBrand(string brandName)
    {
        if (brandName == "qa-auto01")
        {
            brandName = "qa-automation01";
        }

        var url = "http://kube-prod01-deploy.airsoftltd.com/mysql/reset-database";

        var resetProd = new
        {
            @namespace = brandName,
            platform = "mgm"
        };

        if (brandName != "qa-dev-auto")
        {
            var response = _apiAccess.ExecutePatchEntry(url, resetProd);
            _apiAccess.CheckStatusCode(url, response);
        }

        if (brandName.Contains("dev"))
        {
            url = "https://kube-dev01-deploy.airsoftltd.com/mysql/reset-database";

            resetProd = new
            {
                @namespace = brandName,
                platform = "mgm-automation"
            };

            var response = _apiAccess.ExecutePatchEntry(url, resetProd);
            _apiAccess.CheckStatusCode(url, response);
        }
    }

    private void ResetBrandPipe()
    {
        try
        {
            if (GetClientsCount() > 200)
            {
                var brandName = Config.GetBrandName();
                Console.WriteLine($"brand Name From Mongo: {brandName}");
                PatchResetBrand(brandName);
            }
        }
        catch (Exception ex)
        {
            var exceMessage = $"exception: {ex.Message}";
            var exception = new Exception(exceMessage);

            throw exception;
        }
    }

    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        try
        {
            Console.WriteLine("Run Before All Tests");
            ReportSetUp();
            ResetBrandPipe();

            #region office tab
            // office tab
            var officesList = _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .GetOfficesRequest(_crmUrl);

            for (var i = 0; i < officesList.Count; i++)
            {
                var dialer = new Dialer()
                {
                    pbx = "for dialer test", // for dialer test 
                    pbx_type = "Voicespin",
                    pbx_name = officesList[i].city
                };

                officesList[i].dialers = new Dialer[] { dialer };
            }

            officesList
                .ForEach(office => office.allowed_ip_addresses = DataRep.UserAllowedIps);

            officesList
                .Where(p => p.sales_dashboard != null)
                .ToList().ForEach(office => office.sales_dashboard.active = true);

            _apiFactory
                .ChangeContext<IOfficeTabApi>()
                .PutOfficeRequest(_crmUrl, officesList)
                .PutAssignIpsToRequest(_crmUrl);
            #endregion

            // create user admin
            _apiFactory
                .ChangeContext<IUserApi>()
                .CreateUserIfNotExistByNamePipe(_crmUrl,
                    "lior", "@airsoftltd.com");

            #region Super Admin Tub
            // Super Admin Tub
            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutPlatformRequest(_crmUrl)
                .PutCurrenciesRequest(_crmUrl)
                .PutRiskRestrictionsRequest(_crmUrl)
                .PutGroupRestrictionsRequest(_crmUrl)
                .PutChatRequest(_crmUrl);

            var brandRegulation = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetBrandRegulationRequest(_crmUrl);

            if (!Debugger.IsAttached)
            {
                brandRegulation.export_data_email_url = null;
                brandRegulation.admin_email_for_deposit = null;
            }

            DataRep.EmailListForExport.Clear();
            DataRep.EmailListForExport.Add("lior@airsoftltd.com");
            brandRegulation.export_data_email_url = DataRep.EmailListForExport.ToArray();
            brandRegulation.admin_email_for_deposit = DataRep.EmailListForAdminDeposit.ToArray();
            brandRegulation.export_data = true;

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);
            #endregion

            #region general tab
            // Get general settings data
            var generalSettingData = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetRegulationRequest(_crmUrl);

            // put general tab
            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutMaximumDepositRequest(_crmUrl)
                .PutGeneralSettingsRequest(_crmUrl, generalSettingData)
                .PutMarginCallRequest(_crmUrl)
                .PutSuspiciousPnlRequest(_crmUrl);

            // get Sales Status 2
            var salesStatus2 = _apiFactory
                 .ChangeContext<IGeneralTabApi>()
                 .GetSalesStatus2Request(_crmUrl);

            _apiFactory
                .ChangeContext<IGeneralTabApi>()
                .PutSalesStatus2Request(_crmUrl, salesStatus2, null, null);
            #endregion

            // Chrono Tab
            _apiFactory
                .ChangeContext<IChronoTabApi>()
                .PutChronoSettingsRequest(_crmUrl);

            // Psp Tab
            _apiFactory
                .ChangeContext<IPspTabApi>()
                .PostCreateAirsoftSandboxPspRequest(_crmUrl);

            // create psp for deposit page
            _apiFactory
              .ChangeContext<IPspTabApi>()
              .PostCreateBankTransferPspRequest(_crmUrl);

            #region security Tab
            // security Tab
            var loginSection = _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .GetLoginSectionRequest(_crmUrl);

            loginSection.two_factor = false;
            loginSection.disable_ip_verification = true;
            loginSection.attempts = 4;
            loginSection.trade_attempts = 4;

            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .PutLoginSectionRequest(_crmUrl, loginSection);

            _apiFactory
                .ChangeContext<ISecurityTubApi>()
                .PutRecaptchaRequest(_crmUrl);
            #endregion

            #region sales tab
            // Sales Tab
            _apiFactory
                .ChangeContext<ISalesTabApi>()
                // create default saving account
                .PostCreateSavingAccountRequest(_crmUrl, true);

            // for NFT true
            var defaultAccountType = _apiFactory
                .ChangeContext<ISalesTabApi>()
                .GetAccountTypesRequest(_crmUrl)
                .AccountTypeData
                .Where(p => p.AccountTypeDefault.Equals(true))
                .FirstOrDefault();

            defaultAccountType.NftTrading = true;

            _apiFactory
                .ChangeContext<ISalesTabApi>()
                .PutAccountTypeRequest(_crmUrl, defaultAccountType);
            #endregion

            #region Languages
            // update Trading Platform Spanish Language to active
            _apiFactory
                .ChangeContext<ILanguagesTab>()
                .PutTradingLanguagePipe(_crmUrl, "Español", "es");

            // update Trading Platform French Language to active
            _apiFactory
                .ChangeContext<ILanguagesTab>()
                .PutTradingLanguagePipe(_crmUrl, "Français", "fr");
            #endregion

            var Names = new List<string>();

            #region Platform
            // get all DODs ids
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .GetDodsRequest(_crmUrl)
                .ForEach(p => Names.Add(p.Name));

            // delete all DODs of the brand so i can create custom dods
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .DeleteDod(_crmUrl, Names);

            // update Terms And Condition to false
            _apiFactory
                .ChangeContext<IPlatformTabApi>()
                .PutTermsAndConditionRequest(_crmUrl, false);
            #endregion

            // change All Messages Status to close
            _apiFactory
                    .ChangeContext<IBroadcastMessageApi>()
                    .PatchAllMessagesStatusRequest(_crmUrl);

            _apiFactory
                .ChangeContext<IRolesApi>()
                .CreateAdminRoleWithUsersOnlyPipe(_crmUrl)
                .CreateAdminRoleWithDialerPipe(_crmUrl);
        }
        catch (Exception ex)
        {
            var methodName = new StackTrace(ex).GetFrame(0).GetMethod().Name;
            var exceMessage = $" method Name {methodName} " +
                $"in One Time Set Up, exception: {ex.Message}";

            Assert.Inconclusive(exceMessage);

            var exception = new Exception(exceMessage);

            throw exception;
        }
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
        try
        {
            //Console.WriteLine("Run After All Tests");

            // enter the Email For Export in Super Admin Tub
            var brandRegulation = _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .GetBrandRegulationRequest(_crmUrl);

            DataRep.EmailListForExport.RemoveAll(x => !x.Equals("lior@airsoftltd.com"));

            brandRegulation.export_data_email_url = DataRep
                .EmailListForExport.ToArray();

            DataRep.EmailListForAdminDeposit.Clear();

            brandRegulation.admin_email_for_deposit = DataRep
                .EmailListForAdminDeposit.ToArray();

            _apiFactory
                .ChangeContext<ISuperAdminTubApi>()
                .PutRegulationsRequest(_crmUrl, brandRegulation);

            ReportTearDown();
        }
        catch (Exception ex)
        {
            //BeforeTest();
            //AfterTest();
            //ReportTearDown();

            var exceMessage = $"exception: {ex.Message} , in One Time Tear Down";
            var exception = new Exception(exceMessage);

            //throw exception;
        }
        finally
        {
            var brandNameFromMongo = Environment.GetEnvironmentVariable("BrandName");

            if (brandNameFromMongo != null)
            {
                Console.WriteLine("kill Processes");

                Process.GetProcesses()
                    .Where(x => x.ProcessName
                    .Contains("chromedriver", StringComparison.CurrentCultureIgnoreCase))
                    .ToList()
                    .ForEach(x => x.Kill());

                if (!Debugger.IsAttached)
                {
                    Process.GetProcesses()
                        .Where(x => x.ProcessName
                        .Contains("chrome", StringComparison.CurrentCultureIgnoreCase))
                        .ToList()
                        .ForEach(x => x.Kill());
                }
            }
            else
            {
                Process.GetProcesses()
                    .Where(x => x.ProcessName
                    .Contains("chromedriver", StringComparison.CurrentCultureIgnoreCase))
                    .ToList()
                    .ForEach(x => x.Kill());
            }
        }
    }
}