// Ignore Spelling: Api Crm Forex

using System.Linq;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.AccountsApi.Roles;
using OpenQA.Selenium;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.CampaignsApi.Dashboard
{
    public class CreateEditRoleApi : ICreateEditRoleApi
    {
        #region Members
        private IWebDriver _driver;
        private IApiAccess _apiAccess;
        private string _apiKey = Config.appSettings.ApiKey;
        private readonly IApplicationFactory _apiFactory;
        #endregion Members

        public CreateEditRoleApi(IApplicationFactory apiFactory,
            IWebDriver driver, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _driver = driver;
            _apiAccess = apiAccess;
        }

        public ICreateEditRoleApi PostAffiliateRoleWithNoPermissionsRequest(string uri)
        {
            var roleExist = _apiFactory
                .ChangeContext<IRolesApi>(_driver)
                .GetAffiliateRolesRequest(uri)
                .Any(p => p.Name == DataRep.AffiliateWithNoPermissionRole);

            if (!roleExist)
            {
                var route = $"{uri}{ApiRouteAggregate.PostAffiliateRole()}?api_key={_apiKey}";

                var affiliateRole = new
                {
                    role_name = DataRep.AffiliateWithNoPermissionRole,
                    _id = "",
                    affiliate_permissions = System.Array.Empty<string>(),
                };

                var response = _apiAccess.ExecutePostEntry(route, affiliateRole);
                _apiAccess.CheckStatusCode(route, response);
            }

            return this;
        }

        public ICreateEditRoleApi PostAffiliateRoleWithAllPermissionsRequest(string uri)
        {
            var roleExist = _apiFactory
                .ChangeContext<IRolesApi>(_driver)
                .GetAffiliateRolesRequest(uri)
                .Any(p => p.Name == DataRep.AffiliateWithAllPermissionRole);

            if (!roleExist)
            {
                var route = $"{uri}{ApiRouteAggregate.PostAffiliateRole()}?api_key={_apiKey}";

                var affiliateRole = new
                {
                    role_name = DataRep.AffiliateWithAllPermissionRole,
                    _id = "",
                    affiliate_permissions = new string[] { "show_all_deposits", "show_deposits_amount",
                        "show_sales_status", "show_deposit_date", "show_client_email", "show_client_phone",
                        "create_login_link" },
                };

                var response = _apiAccess.ExecutePostEntry(route, affiliateRole);
                _apiAccess.CheckStatusCode(route, response);
            }

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>(_driver);
        }
    }
}
