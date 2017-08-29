using MakingSense.SafeBrowsing.GoogleSafeBrowsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.CliExample.GoogleSafeBrowsing
{
    class GoogleSafeBrowsingExample
    {
        public static async Task<int> Run(string apiKey, IReadOnlyList<string> urls)
        {
            var cfg = new GoogleSafeBrowsingApiConfiguration()
            {
                Apikey = apiKey
            };
            var db = new GoogleSafeBrowsingDatabase();
            var updater = new GoogleSafeBrowsingUpdater(cfg, db);
            await updater.UpdateAsync();
            return 1;
        }
    }
}
