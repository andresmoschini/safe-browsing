﻿#if (NETSTANDARD1_0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.Internal
{
    /// <summary>
    /// Custom HttpClient implementation
    /// </summary>
    public class HttpClient : IHttpClient
    {
        private const int BUFFER_SIZE = 1024;

        /// <inheritdoc />
        public async Task<string> GetStringAsync(string url)
        {
            var response = await GetAsync(url);
            var buffer = new byte[BUFFER_SIZE];
            var sb = new StringBuilder();

            using (var stream = response.GetResponseStream())
            {
                bool finish = false;
                while (!finish)
                {
                    var read = await stream.ReadAsync(buffer, 0, BUFFER_SIZE);
                    if (read > 0)
                    {
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, read));
                    }
                    else
                    {
                        finish = true;
                    }
                }
            }

            return sb.ToString();
        }

        private Task<HttpWebResponse> GetAsync(string url)
        {
            var tcs = new TaskCompletionSource<HttpWebResponse>();
            var request = WebRequest.CreateHttp(url);
            request.BeginGetResponse(result =>
            {
                var response = (HttpWebResponse)request.EndGetResponse(result);
                tcs.SetResult(response);
            }, null);
            return tcs.Task;
        }
    }
}
#endif