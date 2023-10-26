// Ignore Spelling: Api

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AirSoftAutomationFramework.Internals.DAL.ApiAccess
{
    public class HttpRetryMessageHandler : DelegatingHandler
    {
        public HttpRetryMessageHandler(HttpClientHandler handler) : base(handler) { }


        // Strongly consider limiting the number of retries - "retry forever" is
        // probably not the most user friendly way you could respond to "the
        // network cable got pulled out."
        private const int MaxRetries = 3;

        public HttpRetryMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            try
            {
                for (var i = 0; i < MaxRetries; i++)
                {
                    response = await base.SendAsync(request, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        Thread.Sleep(300);

                        continue;                    
                    }

                    return response;
                }

                return response;
            }
            catch(Exception ex)
            {
                var exceMessage = $"exception: {ex?.Message}, response: {response}";

                throw new Exception(exceMessage);
            }
        }

        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        //    CancellationToken cancellationToken) =>
        //    Policy
        //    .Handle<HttpRequestException>()
        //    .Or<TaskCanceledException>()
        //    .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
        //    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromMilliseconds(300))
        //    .ExecuteAsync(() => base.SendAsync(request, cancellationToken));
    }
}