// Ignore Spelling: Dto json shsec Api exce

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Internals.DAL.ApiAccess
{
    public class ApiAccess : IApiAccess
    {
        private HttpClient _httpClient;
        private CookieContainer _cookieContainer;

        public ApiAccess()
        {
            _cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = _cookieContainer };

            // to fix the error: The SSL connection could not be established
            handler.ServerCertificateCustomValidationCallback +=
                (sender, certificate, chain, errors) => true;

            _httpClient = new HttpClient(new HttpRetryMessageHandler
                (handler));

            _httpClient.DefaultRequestHeaders
                .TryAddWithoutValidation("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36" +
                " (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36");
            //_httpClient = new HttpClient(new HttpRetryHandler(new
            //    HttpClientHandler { CookieContainer = _cookieContainer }));

            _httpClient.DefaultRequestHeaders.Accept.Clear();

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.Timeout = TimeSpan.FromSeconds(20);
        }

        public GetLoginResponse GetLoginCookies(HttpResponseMessage response, string url)
        {
            var cookies = new Dictionary<string, string>();
            var json = response.Content.ReadAsStringAsync().Result;
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(json);
            var shsec = loginResponse.shsec;
            var uri = new Uri(url);
            var loginData = new GetLoginResponse();

            var cookiesList = _cookieContainer
                .GetCookies(uri)
                .Cast<Cookie>()
                .ToList();

            loginData.token = cookiesList.Where(p => p.Name.Equals("token"))
                .FirstOrDefault();

            loginData.token2 = cookiesList.Where(p => p.Name.Equals("token2"))
                .FirstOrDefault();

            loginData.token3 = cookiesList.Where(p => p.Name.Equals("token3"))
                .FirstOrDefault();

            loginData.shsec = shsec;

            return loginData;
        }

        public IApiAccess CheckStatusCode(string url,
            HttpResponseMessage response, HttpRequestMessage request = null)
        {
            try
            {
                response.EnsureSuccessStatusCode();

                return this;
            }
            catch (Exception ex)
            {
                var exceMessage = ($" string Response: {response} Exception Message: {ex?.Message}, request json body: {request}, " +
                    $" request method: {request?.Method} , request uri:{url}, response status code: {response.StatusCode}, response content:" +
                    $" {response.Content.ReadAsStringAsync().Result}, response Headers: {response.Headers}" +
                    $", response Reason Phrase: {response.Content.ReadAsStringAsync().Result} ");

                var exception = new Exception(exceMessage);

                 throw exception;
            }
        }

        public HttpResponseMessage ExecuteGetEntry(string url,
            GetLoginResponse loginData = null)
        {
            SetLoginData(loginData, url);

            return _httpClient.GetAsync(url).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecuteDeleteEntry(string url, object requestDto,
            GetLoginResponse loginData = null)
        {
            SetLoginData(loginData, url);
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0" +
                " (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36");

            request.Content = new StringContent(JsonConvert
                .SerializeObject(requestDto), Encoding.UTF8, "application/json");

            return _httpClient.SendAsync(request).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecuteDeleteEntry(string url)
        {
            return _httpClient.DeleteAsync(url).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePatchEntry(string url,
            object requestDto, GetLoginResponse loginData = null)
        {
            SetLoginData(loginData, url);
            var jsonRequest = JsonConvert.SerializeObject(requestDto);
            var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
   
            return _httpClient.PatchAsync(url, stringContent).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePatchEntry(string url,
            MultipartFormDataContent requestDto, GetLoginResponse loginData = null)
        {
            SetLoginData(loginData, url);

            return _httpClient.PatchAsync(url, requestDto).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePatchEntry(string url,
            GetLoginResponse loginData = null)
        {
            SetLoginData(loginData, url);

            return _httpClient.PatchAsync(url, null).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePostEntry(string url,
            MultipartFormDataContent requestDto, GetLoginResponse loginData = null)
        {
            requestDto.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36");
            SetLoginData(loginData, url);

            return _httpClient.PostAsync(url, requestDto).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePostEntry(string apiRoute, object requestDto,
          GetLoginResponse loginData = null)
        {
            SetLoginData(loginData, apiRoute);
            var jsonRequest = JsonConvert.SerializeObject(requestDto);
            var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            return _httpClient.PostAsync(apiRoute, stringContent).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePutEntry(string apiRoute, object requestDto)
        {
            var jsonRequest = JsonConvert.SerializeObject(requestDto);
            var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            return _httpClient.PutAsync(apiRoute, stringContent).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePutEntry(string apiRoute,
            MultipartFormDataContent requestDto)
        {
            requestDto.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36");

            return _httpClient.PutAsync(apiRoute, requestDto).GetAwaiter().GetResult();
        }

        public HttpResponseMessage ExecutePutEntry(string apiRoute)
        {
            return _httpClient.PutAsync(apiRoute, null).GetAwaiter().GetResult();
        }

        private void SetLoginData(GetLoginResponse loginData, string apiRoute)
        {
            if (loginData != null)
            {
                var host = new Uri(apiRoute).Host;
                var domain = host[(host.LastIndexOf('.', host.LastIndexOf('.') - 1) + 1)..];

                var cookies = loginData.GetType().GetProperties()
                    .Where(p => p.Name.Contains("token"));

                foreach (var cookie in cookies)
                {
                    var value = cookie.GetValue(loginData, null)?.ToString();
                    var ff = value?.Split('=');

                    if (ff != null)
                    {
                        _cookieContainer?.Add(new Cookie($"{cookie.Name}",
                            $"{ff[1]}")
                        {
                            Domain = domain
                        });
                    }
                }

                _httpClient.DefaultRequestHeaders.Add("shsec", loginData.shsec);
            }
        }
    }
}