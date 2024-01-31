// Ignore Spelling: Api

using System;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Objects.MgmObjects.Api
{
    public interface IMgmDashboardApi
    {
        List<GetBannersResponse> GetBrandsRequest(string url,
            GetLoginResponse loginData);

        GetBrandsActivitiesResponse GetBrandsActivitiesRequest(string url,
           GetLoginResponse loginData);

        IMgmDashboardApi PostUpdateBrandsRequest(string url, string brandId,
            GetLoginResponse loginData);
    }
}