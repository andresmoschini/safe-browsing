using MakingSense.SafeBrowsing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.GoogleSafeBrowsing
{
    /// <summary>
    /// Allow to download GoogleSafeBrowsing rules from HTTP and update them
    /// </summary>
    public class GoogleSafeBrowsingUpdater : IUpdater
    {
        private readonly GoogleSafeBrowsingApiConfiguration _configuration;
        private readonly GoogleSafeBrowsingDatabase _database;
        private readonly IHttpClient _httpClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        /// <summary>
        /// Create a new instance for an already existent set of rules
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="database"></param>
        /// <param name="httpClient">Optional alternative implementation of HttpClient</param>
        /// <param name="dateTimeProvider">Optional alternative implementation of DateTimeProvider</param>
        public GoogleSafeBrowsingUpdater(GoogleSafeBrowsingApiConfiguration configuration, GoogleSafeBrowsingDatabase database, IHttpClient httpClient = null, IDateTimeProvider dateTimeProvider = null)
        {
            _configuration = configuration;
            _database = database;
            _httpClient = httpClient ?? new HttpClient();
            _dateTimeProvider = _dateTimeProvider ?? new DateTimeProvider();
        }

        /// <summary>
        /// Updates rules based on the remote resource taking into account minimumWaitDuration
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            if (_database.LastUpdate.HasValue
                && _database.MinimumWaitDuration.HasValue
                && _dateTimeProvider.GetCurrentTime() <= _database.LastUpdate.Value.Add(_database.MinimumWaitDuration.Value))
            {
                // Do not update because MinimumWaitDuration did not meet
                return;
            }

            await ForceUpdateAsync();
        }

        /// <summary>
        /// Updates rules based on the remote resource ignoring minimumWaitDuration
        /// </summary>
        /// <returns></returns>
        public async Task ForceUpdateAsync()
        {
            var url = _configuration.FetchUrlTemplate
                .Replace("{baseurl}", _configuration.BaseUrl)
                .Replace("{apikey}", _configuration.Apikey);

            // TODO: support more lists and take into account maxUpdateEntries and maxDatabaseEntries
            var body = $@"
{{
  ""client"": {{
    ""clientId"":       ""{_configuration.ClientId}"",
    ""clientVersion"":  ""{_configuration.ClientVersion}""
  }},
  ""listUpdateRequests"": [{{
    ""threatType"":      ""SOCIAL_ENGINEERING"",
    ""platformType"":    ""ANY_PLATFORM"",
    ""threatEntryType"": ""URL"",
    ""state"":           ""{_database.LastState}"",
    ""constraints"": {{
      ""maxUpdateEntries"":      0,
      ""maxDatabaseEntries"":    0,
      ""region"":                ""{_configuration.Region}"",
      ""supportedCompressions"": [""RAW""]
    }}
  }}]
}}
";
            var response = await _httpClient.PostStringAsync(url, body);

            _database.LASTRESPONSE = response;
        }
    }
}
