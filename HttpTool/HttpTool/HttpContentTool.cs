using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpTool
{
    public partial class HttpTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="requestContent"></param>
        /// <param name="contentEncoding"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            using HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: HttpMethod.Get,
                requestUri: requestUri,
                requestContent: new StringContent(requestContent ?? "", contentEncoding),
                requestHeaders: requestHeaders);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            using HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestContent: new StringContent(requestContent ?? "", contentEncoding),
                requestHeaders: requestHeaders);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> PutAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            using HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: HttpMethod.Put,
                requestUri: requestUri,
                requestHeaders: requestHeaders,
                requestContent: new StringContent(requestContent ?? "", contentEncoding)
                );

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(
            string requestUri,
            Dictionary<string, string>? requestHeaders = null,
            string? requestContent = null,
            Encoding? contentEncoding = null)
        {
            using HttpResponseMessage httpResponseMessage = await this.RequestAsync(
                httpMethod: HttpMethod.Delete,
                requestUri: requestUri,
                requestHeaders: requestHeaders,
                requestContent: new StringContent(requestContent ?? "", contentEncoding)
                );

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}