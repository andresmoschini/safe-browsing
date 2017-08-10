using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.Internal
{
    static class TaskUtilities
    {
        public static Task<T> FromResult<T>(T result)
#if (HAVE_ASYNC)
            => Task.FromResult(result);
#else
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(result);
            return tcs.Task;
        }
#endif

        private static Task _completedTask = null;
        public static Task CompletedTask => _completedTask ?? (_completedTask = FromResult(true));
    }
}
