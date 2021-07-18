using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HttpTool
{
    public class HttpRequestMessageBuilder
    {
        public string RequestUri { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public HttpMethod HttpMethod { get; set; } = HttpMethod.Get;
        public Version HttpVersion { get; set; } = new(2, 0);

        private HttpVersionPolicy _policy = HttpVersionPolicy.RequestVersionOrLower;
        private Dictionary<string, IEnumerable<string>>? _headers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public HttpRequestMessageBuilder UseRequest(string requestUri)
        {
            RequestUri = requestUri;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public HttpRequestMessageBuilder UseContent(string content)
        {
            Content = content;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public HttpRequestMessageBuilder UseHttpMethod(string method)
        {
            HttpMethod = new HttpMethod(method);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public HttpRequestMessageBuilder UseVersion(string version)
        {
            HttpVersion = new Version(version);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public HttpRequestMessageBuilder UseVersionPolicy(HttpVersionPolicy policy)
        {
            _policy = policy;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public HttpRequestMessageBuilder UseRequestHeaders(Dictionary<string, string> headers)
        {
            foreach ((var key, var value) in headers)
            {
                _headers?.Add(key, new List<string> { value });
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public HttpRequestMessageBuilder UseRequestHeaders(Dictionary<string, IEnumerable<string>> headers)
        {
            _headers = headers;
            return this;
        }

        public HttpRequestMessage Build()
        {
            var httpResponseMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(this.RequestUri, UriKind.Relative),
                Content = new StringContent(Content),
                Method = HttpMethod,
                Version = HttpVersion,
                VersionPolicy = _policy
            };

            if (_headers is not null)
            {
                foreach ((var key, var value) in _headers)
                {
                    httpResponseMessage.Headers.Add(key, value);
                }
            }

            return httpResponseMessage;
        }
    }
}
