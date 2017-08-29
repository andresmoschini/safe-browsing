#pragma warning disable 1998 // Because I want to generate tasks from synchronous code
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.Tests
{
    public class HttpClientDouble : IHttpClient
    {
        private Func<string, string, SimplifiedHttpResponse> _overrideGetString = (a, b) => new SimplifiedHttpResponse() { Body = string.Empty };
        private Func<string, string, string> _overridePostString = (a, b) => string.Empty;

        /// <summary>
        /// Set an exception for GetStringAsync
        /// </summary>
        /// <param name="exception"></param>
        public void Setup_GetString(Exception exception) =>
            _overrideGetString = (url, ifNoneMatch) => throw exception;

        /// <summary>
        /// Set a result for GetStringAsync
        /// </summary>
        /// <param name="func"></param>
        public void Setup_GetString(string responseBody) =>
            _overrideGetString = (url, ifNoneMatch) => new SimplifiedHttpResponse()
            {
                Body = responseBody
            };

        /// <summary>
        /// Set a result for GetStringAsync
        /// </summary>
        /// <param name="result"></param>
        public void Setup_GetString(SimplifiedHttpResponse result) =>
            _overrideGetString = (url, ifNoneMatch) => result;

        /// <summary>
        /// Override GetStringAsync behavior
        /// </summary>
        /// <param name="func"></param>
        public void Setup_GetString(Func<SimplifiedHttpResponse> func) =>
            _overrideGetString = (url, ifNoneMatch) => func();

        /// <summary>
        /// Override GetStringAsync behavior
        /// </summary>
        /// <param name="func"></param>
        public void Setup_GetString(Func<string, SimplifiedHttpResponse> func) =>
            _overrideGetString = (url, ifNoneMatch) => func(url);

        /// <summary>
        /// Override GetStringAsync behavior
        /// </summary>
        /// <param name="func"></param>
        public void Setup_GetString(Func<string, string, SimplifiedHttpResponse> func) =>
            _overrideGetString = func;

        public async Task<SimplifiedHttpResponse> GetStringAsync(string url, string ifNoneMatch = null) =>
            _overrideGetString(url, ifNoneMatch);

        /// <summary>
        /// Set an exception for PostStringAsync
        /// </summary>
        /// <param name="exception"></param>
        public void Setup_PostString(Exception exception) =>
            _overridePostString = (url, body) => throw exception;

        /// <summary>
        /// Set a result for PostStringAsync
        /// </summary>
        /// <param name="func"></param>
        public void Setup_PostString(string responseBody) =>
            _overridePostString = (url, body) => responseBody;

        /// <summary>
        /// Override PostStringAsync behavior
        /// </summary>
        /// <param name="func"></param>
        public void Setup_PostString(Func<string> func) =>
            _overridePostString = (url, body) => func();

        /// <summary>
        /// Override PostStringAsync behavior
        /// </summary>
        /// <param name="func"></param>
        public void Setup_PostString(Func<string, string> func) =>
            _overridePostString = (url, body) => func(url);

        /// <summary>
        /// Override PostStringAsync behavior
        /// </summary>
        /// <param name="func"></param>
        public void Setup_PostString(Func<string, string, string> func) =>
            _overridePostString = func;

        public async Task<string> PostStringAsync(string url, string body = null) =>
            _overridePostString(url, body);
    }
}
#pragma warning restore 1998