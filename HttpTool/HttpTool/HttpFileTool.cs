using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace HttpTool
{
    public partial class HttpTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="httpFileParameter"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> UploadFileAsync(
            string requestUri, 
            string fileName, 
            byte[] file,
            Dictionary<string, string>? requestHeaders = null,
            string httpFileParameter = "file")
        {
            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(file);
            // TODO сделать автоопределение ContentType
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            form.Add(fileContent, httpFileParameter, fileName);

            var response = await this.RequestAsync(
                httpMethod: HttpMethod.Post,
                requestUri: requestUri,
                requestHeaders: requestHeaders,
                requestContent: form);

            return response.StatusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="files"></param>
        /// <param name="requestHeaders"></param>
        /// <param name="httpFileParameter"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> UploadFilesAsync(
            string requestUri, 
            IEnumerable<(string, byte[])> files,
            Dictionary<string, string>? requestHeaders = null,
            string httpFileParameter = "files")
        {
            var form = new MultipartFormDataContent();

            foreach ((string fileName, byte[] fileBytes) in files)
            {
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentDisposition = new("form-data");
                fileContent.Headers.ContentDisposition.Name = httpFileParameter;
                fileContent.Headers.ContentDisposition.FileName = fileName;
                // TODO сделать автоопределение ContentType
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                form.Add(fileContent);
            }

            var response = await this.RequestAsync(
                httpMethod: HttpMethod.Post, 
                requestUri: requestUri,
                requestHeaders: requestHeaders,
                requestContent: form);

            return response.StatusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<(string?, byte[])> DownloadFileAsync(string requestUri)
        {
            var httpResponseMessage = await this.RequestAsync(HttpMethod.Get, requestUri);
            var fileName = httpResponseMessage.Content.Headers.ContentDisposition?.FileName;
            var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

            return (fileName, bytes);
        }
    }
}