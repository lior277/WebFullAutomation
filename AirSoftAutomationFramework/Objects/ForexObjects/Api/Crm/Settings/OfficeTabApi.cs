// Ignore Spelling: Forex Api Crm

using AirSoftAutomationFramework.Internals.DAL.ApiAccess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccess.ApiRoutes;
using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.Factory;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using static AirSoftAutomationFramework.Objects.DTOs.CreateOfficeRequest;

namespace AirSoftAutomationFramework.Objects.ForexObjects.Api.Crm.Settings
{
    public class OfficeTabApi : IOfficeTabApi
    {
        #region Members  
        private string _apiKey = Config.appSettings.ApiKey;
        //private string _apiRouteGetOffices = Config.GetValue(nameof(Config.ApiRouteGetOffices));
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;

        private Sunday sunday = new Sunday
        {
            from = DataRep.WorkingHoursFrom,
            to = DataRep.WorkingHoursTo
        };

        private Monday monday = new Monday
        {
            from = DataRep.WorkingHoursFrom,
            to = DataRep.WorkingHoursTo
        };

        private Tuesday tuesday = new Tuesday
        {
            from = DataRep.WorkingHoursFrom,
            to = DataRep.WorkingHoursTo
        };

        private Wednesday wednesday = new Wednesday
        {
            from = DataRep.WorkingHoursFrom,
            to = DataRep.WorkingHoursTo
        };

        private Thursday thursday = new Thursday
        {
            from = DataRep.WorkingHoursFrom,
            to = DataRep.WorkingHoursTo
        };

        private Friday friday = new Friday
        {
            from = DataRep.WorkingHoursFrom,
            to = DataRep.WorkingHoursTo
        };

        private Saturday saturday = new Saturday
        {
            from = DataRep.WorkingHoursFrom,
            to = DataRep.WorkingHoursTo
        };

        #endregion Members

        public OfficeTabApi(IApplicationFactory apiFactory, IApiAccess apiAccess)
        {
            _apiFactory = apiFactory;
            _apiAccess = apiAccess;
        }

        public List<GetOfficeResponse> GetOfficesRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetOffices()}?api_key={_apiKey}";

            try
            {
                var response = _apiAccess.ExecuteGetEntry(route);
                _apiAccess.CheckStatusCode(route, response);
                var json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<List<GetOfficeResponse>>(json);
            }
            catch (Exception ex)
            {
                var exceMessage = ($"inner exeption: {ex.Message}");

                throw new Exception(exceMessage);
            }
        }

        public GetOfficeResponse GetOfficesByName(string url,
           string officeByName = "Main Office", string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var officeDetails = new Dictionary<string, string>();
            var route = $"{url}{ApiRouteAggregate.GetOffices()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            var Offices =
                JsonConvert.DeserializeObject<List<GetOfficeResponse>>(json);

            return Offices?
                .Where(p => p.city.Contains(officeByName))
                .FirstOrDefault();
        }

        public string GetLeaderBoardRequest(string url, string officeId)
        {
            var officeDetails = new Dictionary<string, string>();
            var route = url + ApiRouteAggregate.GetLeader(officeId);
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return response.Content.ReadAsStringAsync().Result;
        }

        public IOfficeTabApi PostCreateOfficeRequest(string url, string officeCity,
            string pbxName = null, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateOffice()}?api_key={_apiKey}";
            var workingHours = new Working_Hours
            {
                active = true,
                block_user = false,
                send_alert_to = new string[] { "Auto@Auto.Auto" },
                sunday = sunday,
                monday = monday,
                tuesday = tuesday,
                wednesday = wednesday,
                thursday = thursday,
                friday = friday,
                saturday = saturday,
            };

            var salesDashboard = new Sales_Dashboard
            {
                active = false,
                price = 0
            };

            HttpResponseMessage response = null;

            var dialer = new Dialer()
            {
                pbx = DataRep.PbxNameAutomation, // for dialer test // from almog
                pbx_type = DataRep.PbxType, // from almog,
                pbx_name = pbxName ?? TextManipulation.RandomString()
            };

            var city = officeCity;
            var dialers = new Dialer[] { dialer };
            var allowedIpAddresses = DataRep.UserAllowedIps;
            var gmtTimezone = "01:00";

            // only super admin has sales Dashboard
            if (apiKey == null)
            {
                var officeDtoSuper = new CreateOfficeRequest
                {
                    sales_dashboard = salesDashboard,
                    city = city,
                    dialers = dialers,
                    allowed_ip_addresses = allowedIpAddresses,
                    gmt_timezone = gmtTimezone,
                    working_hours = workingHours
                };

                response = _apiAccess.ExecutePostEntry(route, officeDtoSuper);
            }
            else
            {
                var officeDtoNotSuper = new
                {
                    city = city,
                    allowed_ip_addresses = allowedIpAddresses,
                    gmt_timezone = gmtTimezone,
                    dialers = dialers,
                    working_hours = workingHours
                };

                response = _apiAccess.ExecutePostEntry(route, officeDtoNotSuper);
            }
            try
            {
                _apiAccess.CheckStatusCode(route, response);
            }
            catch (Exception ex)
            {
                var exceMessage = ($"inner exception: {ex.Message}");

                throw new Exception(exceMessage);
            }

            return this;
        }

        public IOfficeTabApi PutOfficeRequest(string url,
            GetOfficeResponse getOfficeResponses, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.GetOffices()}/{getOfficeResponses._id}?api_key={_apiKey}";

            var workingHours = new Working_Hours()
            {
                active = getOfficeResponses.working_hours.active,
                block_user = getOfficeResponses.working_hours.block_user,
                send_alert_to = getOfficeResponses.working_hours.send_alert_to ??
                new string[] { "Auto@Auto.Auto" },

                sunday = getOfficeResponses.working_hours.sunday ?? sunday,
                monday = getOfficeResponses.working_hours.monday ?? monday,
                tuesday = getOfficeResponses.working_hours.tuesday ?? tuesday,
                wednesday = getOfficeResponses.working_hours.wednesday ?? wednesday,
                thursday = getOfficeResponses.working_hours.thursday ?? thursday,
                friday = getOfficeResponses.working_hours.friday ?? friday,
                saturday = getOfficeResponses.working_hours.saturday ?? saturday,
            };

            HttpResponseMessage response = null;

            var city = getOfficeResponses.city;
            var allowed_ip_addresses = getOfficeResponses.allowed_ip_addresses;
            var gmt_timezone = getOfficeResponses.gmt_timezone;
            var dialers = getOfficeResponses.dialers;
            var sales_dashboard = getOfficeResponses.sales_dashboard;
            var working_hours = workingHours;

            if (apiKey == null) // super admin
            {
                var officeDtoSuperAdmin = new GetOfficeResponse()
                {
                    city = city,
                    allowed_ip_addresses = allowed_ip_addresses,
                    gmt_timezone = getOfficeResponses.gmt_timezone,
                    dialers = dialers,
                    sales_dashboard = sales_dashboard,
                    working_hours = working_hours
                };

                var json = JsonConvert.SerializeObject(officeDtoSuperAdmin);
                var dto = JObject.Parse(json);
                dto.Property("_id").Remove();
                response = _apiAccess.ExecutePutEntry(route, dto);
                json = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                var officeDtoNotSuper = new
                {
                    city = city,
                    allowed_ip_addresses = allowed_ip_addresses,
                    gmt_timezone = getOfficeResponses.gmt_timezone,
                    dialers = dialers,
                    working_hours = working_hours
                };

                response = _apiAccess.ExecutePutEntry(route, officeDtoNotSuper);
            }

            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IOfficeTabApi PutOfficeRequest(string url,
            List<GetOfficeResponse> getOfficeResponses)
        {
            foreach (var office in getOfficeResponses)
            {
                PutOfficeRequest(url, office);
            }

            return this;
        }

        public IOfficeTabApi DeleteOfficeByIdRequest(string url,
           string officeId, string apiKey = null)
        {
            _apiKey = apiKey ?? _apiKey;
            var route = $"{url}{ApiRouteAggregate.PostCreateOffice()}/{officeId}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IOfficeTabApi PutAssignIpsToRequest(string url)
        {
            foreach (var office in GetOfficesRequest(url))
            {
                // overwrite
                var route = $"{url}{ApiRouteAggregate.GetOffices()}/assign-ip/add/{office._id}?api_key={_apiKey}";
                var response = _apiAccess.ExecutePutEntry(route);
                _apiAccess.CheckStatusCode(route, response);
            }

            return this;
        }

        public List<GetTrunksRequest.TrunkData> GetTrunkRequest(string url)
        {
            var route = $"{url}{ApiRouteAggregate.GetTrunks()}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            var json = response.Content.ReadAsStringAsync().Result;
            _apiAccess.CheckStatusCode(route, response);

            return JsonConvert.DeserializeObject<List<GetTrunksRequest.TrunkData>>(json);
        }

        public bool CheckIfAutomationTrunkExist(string url)
        {
            var trunks = GetTrunkRequest(url)
                .SelectMany(o => o.trunks);

            return trunks.Any(p => p.name == DataRep.AutomationTrunkName);
        }

        public IOfficeTabApi PostCreateTrunkPipe(string url,
           string trunkName = null, string officeId = null,
           string pbxName = null, string trunkNumber = "5")
        {
            if (!CheckIfAutomationTrunkExist(url))
            {
                var route = ApiRouteAggregate.PostCreateTrunk();
                route = $"{url}{route}?api_key={_apiKey}";

                var trunk = new Trunk
                {
                    Name = trunkName ?? DataRep.AutomationTrunkName,
                    Number = trunkNumber
                };

                var postCreateTrunkDto = new
                {
                    office = officeId,
                    pbx_name = pbxName,
                    trunks = new Trunk[] { trunk }
                };

                var response = _apiAccess.ExecutePostEntry(route, postCreateTrunkDto);
                var json = response.Content.ReadAsStringAsync().Result;
                _apiAccess.CheckStatusCode(route, response);
            }

            for (var i = 0; i < 20; i++)
            {
                if (!CheckIfAutomationTrunkExist(url))
                {
                    Thread.Sleep(300);

                    continue;
                }

                break;
            }

            return this;
        }

        public GetTrunkResponse GetTrunks(string url, string trunkName)
        {
            var route = ApiRouteAggregate.PostCreateTrunk();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetTrunkResponse>>(json)
             .Where(p => p.Trunks.Select(e => e.Name == trunkName)
             .FirstOrDefault())
             .FirstOrDefault();
        }

        public IOfficeTabApi DeleteTrunkByIdRequest(string url,
            string trunkId)
        {
            var route = $"{ApiRouteAggregate.PostCreateTrunk()}/{trunkId}";
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public IOfficeTabApi PostCreateDialerRequest(string url,
         string officeId)
        {
            var route = ApiRouteAggregate.PostCreateDialer();
            route = $"{url}{route}?api_key={_apiKey}";

            var dialer = new
            {
                office = officeId,
                ip = "00000",
                user = "Automation",
                password = "Automation"
            };

            var response = _apiAccess.ExecutePostEntry(route, dialer);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public List<GetDialersResponse> GetDialers(string url)
        {
            var route = ApiRouteAggregate.PostCreateDialer();
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteGetEntry(route);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetDialersResponse>>(json);
        }

        public IOfficeTabApi DeleteDialer(string url, string dialerId)
        {
            var route = $"{ApiRouteAggregate.PostCreateDialer()}/{dialerId}";
            route = $"{url}{route}?api_key={_apiKey}";
            var response = _apiAccess.ExecuteDeleteEntry(route);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }

        public T ChangeContext<T>(IWebDriver driver) where T : class
        {
            return _apiFactory.ChangeContext<T>();
        }
    }
}
