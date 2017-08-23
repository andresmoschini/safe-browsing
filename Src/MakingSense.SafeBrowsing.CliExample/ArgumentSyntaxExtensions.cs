using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine
{
    static class ArgumentSyntaxExtensions
    {
        public static void ValidateRequiredParameters(this ArgumentSyntax syntax)
        {
            var missedArguments = syntax.GetActiveArguments()
                .Where(x => x.IsRequired && !x.IsSpecified);

            if (missedArguments.Any() && !syntax.IsHelpRequested())
            {
                var messages = missedArguments.Select(x =>
                    $"`{string.Join("|", x.GetDisplayNames())}` must be specified.");
                syntax.ReportError(string.Join("\r\n", messages));
            }
        }

        // I had to copy the code because the original method is private
        // See https://github.com/dotnet/corefxlab/blob/e5950f4e145529e12915235e44afdf2eedcbca7c/src/System.CommandLine/System/CommandLine/ArgumentSyntax.cs#L85
        public static bool IsHelpRequested(this ArgumentSyntax syntax) =>
            syntax.RemainingArguments.Any(a => string.Equals(a, @"-?", StringComparison.Ordinal)
            || string.Equals(a, @"-h", StringComparison.Ordinal)
            || string.Equals(a, @"--help", StringComparison.Ordinal));
    }
}
