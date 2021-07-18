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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public async Task<TypeResponseObject?> RequestJsonAsync<TypeResponseObject, TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject? requestObject = null,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            string requestContent = JsonSerializer.Serialize(requestObject, this._jsonSerializerOptions);

            // TODO сделать "application/json" в конфиге
            using var httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestContent, requestContentEncoding, "application/json"),
                requestHeaders: requestHeaders);

            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<TypeResponseObject>(responseContentString, this._jsonSerializerOptions);

            return responseObject;
        }

        // request: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> RequestJsonAsync<TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public async Task<TypeResponseObject?> RequestJsonAsync<TypeResponseObject>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            using var httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestHeaders: requestHeaders);

            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            TypeResponseObject? responseObject = JsonSerializer.Deserialize<TypeResponseObject>(responseContentString, this._jsonSerializerOptions);

            return responseObject;
        }

        // get: output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> GetJsonAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // get: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> GetJsonAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestJsonAsync<TypeRequestObject>(
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncondig"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> GetJsonAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncondig = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject, TypeRequestObject>(
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
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncondig"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> PostJsonAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncondig = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestJsonAsync<TypeRequestObject>(
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> PostJsonAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // post: input output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> PostJsonAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeResponseObject : class, new()
            where TypeRequestObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject, TypeRequestObject>(
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> PutJsonAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // put: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> PutJsonAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestJsonAsync<TypeRequestObject>(
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> PutJsonAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject, TypeRequestObject>(
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> DeleteJsonAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // delete: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> DeleteJsonAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestJsonAsync<TypeRequestObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri, requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // delete: input output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> DeleteJsonAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestJsonAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }
    }
}