﻿using MakingSense.SafeBrowsing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MakingSense.SafeBrowsing.SimpleRegex
{
    /// <summary>
    /// Simple implementation of Safe Browsing Checker that uses a list of regular expressions
    /// </summary>
    public class SimpleRegexUrlChecker : IUrlChecker
    {
        private readonly SimpleRegexRules _rules;

        /// <summary>
        /// Create a new instance based on specified rules
        /// </summary>
        /// <param name="rules"></param>
        public SimpleRegexUrlChecker(SimpleRegexRules rules)
        {
            _rules = rules;
        }

        /// <summary>
        /// Create a new instance based on a list of patterns
        /// </summary>
        /// <param name="blacklistPatterns"></param>
        public static SimpleRegexUrlChecker CreateWithFixedList(IEnumerable<string> blacklistPatterns)
        {
            return CreateWithFixedList(blacklistPatterns.Select(x => new Regex(x)));
        }

        /// <summary>
        /// Create a new instance based on a list of regular expressions
        /// </summary>
        /// <param name="blacklist"></param>
        public static SimpleRegexUrlChecker CreateWithFixedList(IEnumerable<Regex> blacklist)
        {
            return new SimpleRegexUrlChecker(new SimpleRegexRules(blacklist));
        }

        /// <inheritdoc />
        public SafeBrowsingStatus Check(string url) =>
            new SafeBrowsingStatus(
                url,
                _rules.Blacklist.Any(r => r.IsMatch(url)) ? ThreatType.Unknow : ThreatType.NoThreat);

        /// <inheritdoc />
        public Task<SafeBrowsingStatus> CheckAsync(string url) =>
            TaskUtilities.FromResult(Check(url));
    }
}
