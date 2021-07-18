using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpTool
{
    /// <summary>
    /// 
    /// </summary>
    public partial class HttpTool
    {
        public readonly HttpClient HttpClient;
        private readonly HttpClientHandler _handler = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="defaultHeaders"></param>
        /// <param name="defaultCookies"></param>
        public HttpTool(
            string baseAddress,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null
            )
        {
            _handler = new();
            SetDefaultCookie(defaultCookies);

            HttpClient = new(_handler);
            SetDefaultHeaders(defaultHeaders);

            SetBaseSettings(baseAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public HttpTool(HttpClient client)
        {
            HttpClient = client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        private void SetDefaultHeaders(IDictionary<string, string>? headers)
        {
            if (headers is not null)
            {
                foreach (var (headerName, headerValue) in headers)
                {
                    HttpClient.DefaultRequestHeaders.Add(headerName, headerValue);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookies"></param>
        private void SetDefaultCookie(IEnumerable<Cookie>? cookies)
        {
            if (cookies is not null && _handler is not null)
            {
                CookieContainer cookieContainer = new();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(cookie);
                }

                _handler.CookieContainer = cookieContainer;
                _handler.UseCookies = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseAddress"></param>
        private void SetBaseSettings(string baseAddress)
        {
            var delayServicePoint = ServicePointManager.FindServicePoint(new Uri(baseAddress));
            delayServicePoint.ConnectionLeaseTimeout = 0;

            HttpClient.BaseAddress = new(baseAddress);
            HttpClient.DefaultRequestVersion = new(2, 0);
            HttpClient.Timeout = Timeout.InfiniteTimeSpan;
            HttpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> RequestAsync(HttpRequestMessage httpRequestMessage)
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage);
            return httpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContent"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> RequestAsync(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            HttpContent? requestContent = null)
        {
            using HttpRequestMessage httpRequestMessage = new()
            {
                Method = httpMethod,
                RequestUri = new(requestUri, UriKind.Relative),
                Content = requestContent,
            };

            if (requestHeaders is not null)
            {
                foreach ((var name, var value) in requestHeaders)
                {
                    httpRequestMessage.Headers.Add(name, value);
                }
            }

            return this.RequestAsync(httpRequestMessage);
        }
    }
}
