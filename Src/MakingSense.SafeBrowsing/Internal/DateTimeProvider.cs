using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.Internal
{

    /// <inheritdoc />
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc />
        public DateTimeOffset GetCurrentTime()
        {
            return DateTimeOffset.Now;
        }
    }
}
