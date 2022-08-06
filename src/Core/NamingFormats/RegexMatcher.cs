using System;
using System.Text.RegularExpressions;
using Core.IocContainer;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.NamingFormats {
    [Service]
    public class RegexMatcher : IRegexMatcher {
        public Option<string> Rename(Regex regex, string input, Func<Match, string> onMatch) {
            var match = regex.Match(input);
            if (!match.Success)
                return None;
            var result = onMatch(match);
            return CheckIfNotChanged(input, result);
        }

        private Option<string> CheckIfNotChanged(string input, string result) =>
            result == input
                ? None
                : result;
    }
}
