using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.GoogleSafeBrowsing
{
    /// <summary>
    /// Configuration of Google Safe Browsing API and Client
    /// </summary>
    public class GoogleSafeBrowsingApiConfiguration
    {
        /// <summary>
        /// API URL (official URL by default)
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Client API KEY
        /// </summary>
        public string Apikey { get; set; }

        /// <summary>
        /// Template of threatListUpdates.fetch URL
        /// </summary>
        public string FetchUrlTemplate { get; set; }

        /// <summary>
        /// Identification of current client implementation
        /// </summary>
        /// <remarks>
        /// ClientID and ClientVersion fields should uniquely identify a client implementation, not an individual user, by 
        /// default it has this library name and version.
        /// </remarks>
        public string ClientId { get; set; }

        /// <summary>
        /// Identification of current client implementation
        /// </summary>
        /// <remarks>
        /// ClientID and ClientVersion fields should uniquely identify a client implementation, not an individual user, by 
        /// default it has this library name and version.
        /// </remarks>
        public string ClientVersion { get; set; }

        /// <summary>
        /// Requests the list for a specific geographic location. If not set the server may pick that value based on the user's IP address. Expects ISO 3166-1 alpha-2 format. 
        /// </summary>
        public string Region { get; set; }

        public GoogleSafeBrowsingApiConfiguration()
        {
            BaseUrl = "https://safebrowsing.googleapis.com/v4/";
            FetchUrlTemplate = "{baseurl}threatListUpdates:fetch?key={apikey}";

#if (NETSTANDARD1_0 || NETSTANDARD1_3)
            var assembly = typeof(GoogleSafeBrowsingApiConfiguration).GetTypeInfo().Assembly;
#else
            var assembly = Assembly.GetExecutingAssembly();
#endif
            var assemblyName = assembly.GetName();
            ClientId = assemblyName.Name;
            ClientVersion = assemblyName.Version.ToString(3);
        }
    }
}
