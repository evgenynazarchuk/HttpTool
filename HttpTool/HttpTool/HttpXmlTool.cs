using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HttpTool.Extension;

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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public async Task<TypeResponseObject?> RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
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
            TypeResponseObject? responseObject = responseContentString.FromXmlToObject<TypeResponseObject>();

            return responseObject;
        }

        // xml input
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
        public async Task<HttpStatusCode> RequestXmlAsync<TypeRequestObject>(
            HttpMethod httpMethod,
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public async Task<TypeResponseObject?> RequestXmlAsync<TypeResponseObject>(
            HttpMethod httpMethod,
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where TypeResponseObject : class, new()
        {
            // TODO сделать чтение параметра Accept из конфига
            // так как могут быть application/xml или text/xml
            HttpTool.AddXmlAcceptHeader(ref requestHeaders);

            using HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: httpMethod,
                requestUri: requestUri,
                requestHeaders: requestHeaders);

            string responseContentString = await httpResponseMessage.Content.ReadAsStringAsync();
            TypeResponseObject? responseObject = responseContentString.FromXmlToObject<TypeResponseObject>();

            return responseObject;
        }

        // get output
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> GetXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null)
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestHeaders: requestHeaders);
        }

        // get input
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> GetXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> GetXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
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
        /// <typeparam name="TypeRequestObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestObject"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContentEncoding"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> PostXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
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
        /// <typeparam name="TypeResponseObject"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public Task<TypeResponseObject?> PostXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
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
        public Task<TypeResponseObject?> PostXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
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
        public Task<TypeResponseObject?> PutXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
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
        public Task<HttpStatusCode> PutXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
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
        public Task<TypeResponseObject?> PutXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
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
        public Task<TypeResponseObject?> DeleteXmlAsync<TypeResponseObject>(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null
            )
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject>(
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
        public Task<HttpStatusCode> DeleteXmlAsync<TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
        {
            return this.RequestXmlAsync<TypeRequestObject>(
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
        public Task<TypeResponseObject?> DeleteXmlAsync<TypeResponseObject, TypeRequestObject>(
            string requestUri,
            TypeRequestObject requestObject,
            Dictionary<string, string>? requestHeaders = null,
            Encoding? requestContentEncoding = null
            )
            where TypeRequestObject : class, new()
            where TypeResponseObject : class, new()
        {
            return this.RequestXmlAsync<TypeResponseObject, TypeRequestObject>(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestObject: requestObject,
                requestHeaders: requestHeaders,
                requestContentEncoding: requestContentEncoding);
        }
    }
}