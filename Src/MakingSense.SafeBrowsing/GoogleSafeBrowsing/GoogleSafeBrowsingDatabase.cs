using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.GoogleSafeBrowsing
{
    /// <summary>
    /// Local representation of Google Safe Browsing data
    /// </summary>
    public class GoogleSafeBrowsingDatabase
    {
        /// <summary>
        /// Both the fullHashes.find response and threatListUpdates.fetch response have a minimumWaitDuration field that clients must obey. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the minimumWaitDuration field is not set in the response, clients can update as frequently as they want and send as many threatListUpdates or fullHashes requests as they want.
        /// </para>
        /// <para>
        /// If the minimumWaitDuration field is set in the response, clients cannot update more frequently than the length of the wait duration. For example, if a fullHashes response contains a minimum wait duration of 1 hour, the client must not send send any fullHashes requests until that hour passes, even if the user is visiting a URL whose hash prefix matches the local database. (Note that clients can update less frequently than the minimum wait duration but this may negatively affect protection.)
        /// </para>
        /// </remarks>
        public TimeSpan? MinimumWaitDuration { get; set; } = null;

        /// <summary>
        /// Time of the last update
        /// </summary>
        /// <remarks>
        /// It should be taken into account to make honor minimumWaitDuration.
        /// </remarks>
        public DateTimeOffset? LastUpdate { get; set; } = null;

        // TODO: this state corresponds to only a list, each list could have a different status
        /// <summary>
        /// The state field holds the current client state of the Safe Browsing list. (Client states are returned in the newClientState field of the threatListUpdates.fetch response.) For initial updates, leave the state field empty.
        /// </summary>
        public string LastState { get; set; } = string.Empty;

        public string LASTRESPONSE { get; set; }
    }
}
