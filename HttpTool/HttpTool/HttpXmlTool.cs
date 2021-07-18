using HttpTool.Extension;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HttpTool
{
    public partial class HttpTool
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly XmlWriterSettings _xmlSerializationOptions = new()
        {
            // TODO сделать настройки из конфига
            Indent = true,
            OmitXmlDeclaration = true,
            CheckCharacters = false,
            Encoding = Encoding.UTF8
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers"></param>
        private static void AddXmlAcceptHeader(ref Dictionary<string, string>? headers)
        {
            if (headers is not null)
            {
                headers.Add("Accept", "application/xml");
            }
            else
            {
                headers = new();
                headers.Add("Accept", "application/xml");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public async Task<XmlDocument> GetXmlDocumentAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
        {
            var httpResponseMessage = await this.RequestAsync(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestHeaders: requestHeaders);

            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);

            return xmlDocument;
        }

        /// <summary>
        /// xml input output 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public async Task<ResponseType?> RequestXmlAsync<ResponseType, RequestType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            // TODO сделать чтение параметра Accept из конфига
            // так как могут быть application/xml или text/xml
            HttpTool.AddXmlAcceptHeader(ref requestHeaders);

            string requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

            using HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestXmlContentString, requestContentEncoding, mediaType: "application/xml"),
                requestHeaders: requestHeaders);

            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            ResponseType? responseObject = responseContentString.FromXmlToObject<ResponseType>();

            return responseObject;
        }

        // xml input
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
        public async Task<HttpStatusCode> RequestXmlAsync<RequestType>(
            HttpMethod httpMethod,
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            string requestXmlContentString = requestObject.FromObjectToXmlString(this._xmlSerializationOptions);

            HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestContent: new StringContent(requestXmlContentString, requestContentEncoding, "application/xml"),
                requestHeaders: requestHeaders);

            return httpResponseMessage.StatusCode;
        }

        // xml output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public async Task<ResponseType?> RequestXmlAsync<ResponseType>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where ResponseType : class, new()
        {
            // TODO сделать чтение параметра Accept из конфига
            // так как могут быть application/xml или text/xml
            HttpTool.AddXmlAcceptHeader(ref requestHeaders);

            using HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestHeaders: requestHeaders);

            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            ResponseType? responseObject = responseContentString.FromXmlToObject<ResponseType>();

            return responseObject;
        }

        // get output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseType?> GetXmlAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // get input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> GetXmlAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            return this.RequestXmlAsync<RequestType>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // get input output
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
        public Task<ResponseType?> GetXmlAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType, RequestType>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // post: input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RequestType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> PostXmlAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            return this.RequestXmlAsync<RequestType>(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }

        // post: output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ResponseType"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<ResponseType?> PostXmlAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType>(
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
        public Task<ResponseType?> PostXmlAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType, RequestType>(
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
        public Task<ResponseType?> PutXmlAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType>(
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
        public Task<HttpStatusCode> PutXmlAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            return this.RequestXmlAsync<RequestType>(
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
        public Task<ResponseType?> PutXmlAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType, RequestType>(
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
        public Task<ResponseType?> DeleteXmlAsync<ResponseType>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType>(
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
        public Task<HttpStatusCode> DeleteXmlAsync<RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
        {
            return this.RequestXmlAsync<RequestType>(
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
        public Task<ResponseType?> DeleteXmlAsync<ResponseType, RequestType>(
            string requestUri,
            RequestType requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where RequestType : class, new()
            where ResponseType : class, new()
        {
            return this.RequestXmlAsync<ResponseType, RequestType>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }
    }
}