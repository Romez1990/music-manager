using System;
using System.Text.RegularExpressions;
using LanguageExt;

namespace Core.NamingFormats {
    public interface IRegexMatcher {
        Option<string> Rename(Regex regex, string input, Func<Match, string> onMatch);
    }
}
