using MakingSense.SafeBrowsing.SimpleRegex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.CliExample.SimpleRegex
{
    class SimpleRegexExample
    {
        public static async Task<int> Run(string simpleRegexRulesUrl, IReadOnlyList<string> urls)
        {
            var updater = new SimpleRegexRulesHttpUpdater(simpleRegexRulesUrl);
            var checker = new SimpleRegexUrlChecker(updater.Rules);

            // Only for demonstration purposes
            updater.UpdatePeriodically(TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(10));

            // Forcing an update
            await updater.UpdateAsync();

            var results = urls.Select(checker.Check).ToArray();

            if (results.All(x => x.IsSafe))
            {
                Console.Out.WriteLine("All specified URLs seem to be safe.");
                return 0;
            }

            Console.Out.WriteLine("Following URLs seems to be unsafe:");
            foreach (var result in results.Where(x => !x.IsSafe))
            {
                Console.Out.WriteLine($"Threat {result.ThreatType}: {result.Url}");
            }

            return 1;
        }
    }
}
