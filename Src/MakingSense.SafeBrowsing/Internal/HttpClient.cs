#if (!NETSTANDARD1_0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.Internal
{
    /// <summary>
    /// Wrapper around System.Net.Http.HttpClient to allow easily implement a compatible version for netstandard1.0
    /// </summary>
    public class HttpClient : IHttpClient
    {

        private static System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();

        /// <inheritdoc />
        public async Task<SimplifiedHttpResponse> GetStringAsync(string url, string ifNoneMatch = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (ifNoneMatch != null)
            {
                request.Headers.TryAddWithoutValidation("If-None-Match", ifNoneMatch);
            }

            var response = await _httpClient.SendAsync(request);

            var newEtag = response.Headers.ETag.Tag;

            if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                return new SimplifiedHttpResponse()
                {
                    NotModified = true,
                    Body = null,
                    Etag = newEtag
                };
            }
            else
            {
                return new SimplifiedHttpResponse()
                {
                    NotModified = false,
                    Body = await _httpClient.GetStringAsync(url),
                    Etag = newEtag
                };
            }
        }

        /// <inheritdoc />
        public async Task<string> PostStringAsync(string url, string body)
        {
            var response = await _httpClient.PostAsync(url, new TextContent(body));
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        class TextContent : HttpContent
        {
            static readonly Encoding Utf8WithoutBom = new UTF8Encoding(false);
            public string ContentAsText { get; private set; }
            public string ContentType { get; private set; }

            public TextContent(string value, string contentType = "text/plain")
            {
                ContentAsText = value;
                ContentType = contentType;
                if (contentType != null)
                {
                    Headers.Add("Content-Type", contentType);
                }
            }

            protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
#if NET40 || NET35
                var writer = new StreamWriter(stream, Utf8WithoutBom, 1024);
                writer.Write(ContentAsText);
#else
                using (var writer = new StreamWriter(stream, Utf8WithoutBom, 1024, true))
                {
                    await writer.WriteAsync(ContentAsText);
                }
#endif
            }

            protected override bool TryComputeLength(out long length)
            {
                length = ContentAsText.Length;
                return true;
            }
        }

    }
}
#endif
