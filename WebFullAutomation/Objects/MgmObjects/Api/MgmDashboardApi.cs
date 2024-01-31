


// Ignore Spelling: Api mongo

using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using Newtonsoft.Json;
using System.Collections.Generic;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess;
using AirSoftAutomationFramework.Internals.DAL.ApiAccsess.ApiRoutes;
using AirSoftAutomationFramework.Internals.Factorys;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public class MgmDashboardApi : IMgmDashboardApi
    {
        private readonly IApplicationFactory _apiFactory;
        private IApiAccess _apiAccess;

        public MgmDashboardApi(IApplicationFactory apiFactory,
            IApiAccess apiAccess, IMongoDbAccess mongoDbAccess)
        {
            _apiFactory = apiFactory;
            _apiAccess = apiAccess;
        }

        public List<GetBannersResponse> GetBrandsRequest(string url,
            GetLoginResponse loginData)
        {
            var route = ApiRouteAggregate.GetBrands();
            route = url + route;
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<GetBannersResponse>>(json);
        }

        public GetBrandsActivitiesResponse GetBrandsActivitiesRequest(string url,
           GetLoginResponse loginData)
        {
            var route = $"{url}{ApiRouteAggregate.GetBrands()}/activities";
            var response = _apiAccess.ExecuteGetEntry(route, loginData);
            _apiAccess.CheckStatusCode(route, response);
            var json = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<GetBrandsActivitiesResponse>(json);
        }

        public IMgmDashboardApi PostUpdateBrandsRequest(string url, string brandId,
            GetLoginResponse loginData)
        {
            var route = url + ApiRouteAggregate.PostUpdateBrands();

            var UpdateBrandsRequest = new
            {
                brands = new List<string> { brandId }
            };

            var response = _apiAccess.ExecutePostEntry(route, UpdateBrandsRequest, loginData);
            _apiAccess.CheckStatusCode(route, response);

            return this;
        }
    }
}
