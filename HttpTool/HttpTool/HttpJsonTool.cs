using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpTool
{
    public partial class HttpTool
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // request: input output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public async Task<ResponseType?> RequestJsonAsync<ResponseType, RequestType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestType? requestObject = null,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            string requestContent = JsonSerializer.Serialize(requestObject, this._jsonSerializerOptions);

            // TODO сделать "application/json" в конфиге
            using var httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestContent, requestContentEncoding, "application/json"),
                requestHeaders: requestHeaders);

            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ResponseType>(responseContentString, this._jsonSerializerOptions);

            return responseObject;
        }

        // request: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> RequestJsonAsync<RequestType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            string requestContentString = JsonSerializer.Serialize(requestObject, this._jsonSerializerOptions);

            using HttpResponseMessage responseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestContentString, requestContentEncoding, "application/json"),
                requestHeaders: requestHeaders);

            return responseMessage.StatusCode;
        }

        // request: output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public async Task<ResponseType?> RequestJsonAsync<ResponseType>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            using var httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestHeaders: requestHeaders);

            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            ResponseType? responseObject = JsonSerializer.Deserialize<ResponseType>(responseContentString, this._jsonSerializerOptions);

            return responseObject;
        }

        // get: output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseType?> GetJsonAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            return this.RequestJsonAsync<ResponseType>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // get: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> GetJsonAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            return this.RequestJsonAsync<RequestType>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // get: input, output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncondig"></param>
        /// <returns></returns>
        public Task<ResponseType?> GetJsonAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncondig = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            return this.RequestJsonAsync<ResponseType, RequestType>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncondig);
        }

        // post: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncondig"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> PostJsonAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncondig = null
            )
            where RequestType : class, new()
        {
            return this.RequestJsonAsync<RequestType>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncondig);
        }

        // post: output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseType?> PostJsonAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            return this.RequestJsonAsync<ResponseType>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // post: input output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<ResponseType?> PostJsonAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where ResponseType : class, new()
            where RequestType : class, new()
        {
            return this.RequestJsonAsync<ResponseType, RequestType>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // put: output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseType?> PutJsonAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            return this.RequestJsonAsync<ResponseType>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // put: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> PutJsonAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            return this.RequestJsonAsync<RequestType>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // put: input output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<ResponseType?> PutJsonAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            return this.RequestJsonAsync<ResponseType, RequestType>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // delete: output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseType?> DeleteJsonAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            return this.RequestJsonAsync<ResponseType>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // delete: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> DeleteJsonAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            return this.RequestJsonAsync<RequestType>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri, requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // delete: input output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<ResponseType?> DeleteJsonAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            return this.RequestJsonAsync<ResponseType, RequestType>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }
    }
}