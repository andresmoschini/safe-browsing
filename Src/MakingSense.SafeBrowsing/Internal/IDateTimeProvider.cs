using System;

namespace MakingSense.SafeBrowsing.Internal
{
    /// <summary>
    /// Abstraction to access to current time.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets a System.DateTimeOffset object that is set to the current date and time
        /// on the current computer, with the offset set to the local time's offset from
        /// Coordinated Universal Time (UTC).
        /// </summary>
        /// <returns></returns>
        DateTimeOffset GetCurrentTime();
    }
}