using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.CliExample
{
    class Program
    {
        enum Command { SimpleRegex, GoogleSafeBrowsing }

        static async Task Main(string[] args)
        {
            int exitCode = 1;

            try
            {
                var action = Parse(args);
                exitCode = await action();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unexpected error: {e.Message}");
                Console.Error.WriteLine(e);
                exitCode = 1;
            }
        }

        static Func<Task<int>> Parse(string[] args)
        {
            // Common
            var command = Command.SimpleRegex;
            IReadOnlyList<string> urls = Array.Empty<string>();

            // SimpleRegex
            var simpleRegexRulesUrl = "https://raw.githubusercontent.com/MakingSense/safe-browsing/resources/links-blacklist.txt";

            // GoogleSafeBrowsing
            string apikey = null;

            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineCommand("simple-regex", ref command, Command.SimpleRegex, "Test our naive Simple Regex implementation");
                syntax.DefineOption("r|rules", ref simpleRegexRulesUrl, false, "URL of the text file that contains the rules");
                syntax.DefineParameterList("urls", ref urls, "URLs to be verified");

                syntax.DefineCommand("google-safe-browsing", ref command, Command.GoogleSafeBrowsing, "Test our Google Safe Browsing client implementation");
                syntax.DefineOption("k|apikey", ref apikey, true, "Google API KEY");
                syntax.DefineParameterList("urls", ref urls, "URLs to be verified");

                syntax.ValidateRequiredParameters();
            });

            switch (command)
            {
                case Command.SimpleRegex:
                    return () => SimpleRegex.SimpleRegexExample.Run(simpleRegexRulesUrl, urls);
                case Command.GoogleSafeBrowsing:
                    return () => GoogleSafeBrowsing.GoogleSafeBrowsingExample.Run(apikey, urls);
                default: throw new NotImplementedException();
            }
        }
    }
}
