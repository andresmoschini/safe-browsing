using MakingSense.SafeBrowsing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.Tests
{
    public class DateTimeProviderDouble : IDateTimeProvider
    {
        private Func<DateTimeOffset> _overrideGetCurrentTime = () => DateTimeOffset.UtcNow;
        public void Setup_GetCurrentTime(Func<DateTimeOffset> value) => _overrideGetCurrentTime = value;
        public void Setup_GetCurrentTime(DateTimeOffset value) => Setup_GetCurrentTime(() => value);
        public DateTimeOffset GetCurrentTime() => _overrideGetCurrentTime();
    }
}
