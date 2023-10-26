// Ignore Spelling: Api

using System.Net.Http;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Internals.DAL.ApiAccess
{
    public interface IApiAccess
    {
        IApiAccess CheckStatusCode(string url, HttpResponseMessage response, HttpRequestMessage request = null);
        HttpResponseMessage ExecuteDeleteEntry(string url);
        HttpResponseMessage ExecuteDeleteEntry(string url, object requestDto, GetLoginResponse loginData = null);
        HttpResponseMessage ExecuteGetEntry(string url, GetLoginResponse loginData = null);
        HttpResponseMessage ExecutePatchEntry(string url, GetLoginResponse loginData = null);
        HttpResponseMessage ExecutePatchEntry(string url, MultipartFormDataContent requestDto, GetLoginResponse loginData = null);
        HttpResponseMessage ExecutePatchEntry(string url, object requestDto, GetLoginResponse loginData = null);
        HttpResponseMessage ExecutePostEntry(string url, MultipartFormDataContent requestDto, GetLoginResponse loginData = null);
        HttpResponseMessage ExecutePostEntry(string apiRoute, object requestDto, GetLoginResponse loginData = null);
        HttpResponseMessage ExecutePutEntry(string apiRoute);
        HttpResponseMessage ExecutePutEntry(string apiRoute, MultipartFormDataContent requestDto);
        HttpResponseMessage ExecutePutEntry(string apiRoute, object requestDto);
        GetLoginResponse GetLoginCookies(HttpResponseMessage response, string url);
    }
}