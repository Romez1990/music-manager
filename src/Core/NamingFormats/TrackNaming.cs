using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core.IocContainer;
using LanguageExt;

namespace Core.NamingFormats {
    [Service]
    public class TrackNaming : ITrackNaming {
        public TrackNaming(IRegexMatcher regexMatcher, INumberNormalizer numberNormalizer) {
            _regexMatcher = regexMatcher;
            _numberNormalizer = numberNormalizer;
        }

        private readonly IRegexMatcher _regexMatcher;
        private readonly INumberNormalizer _numberNormalizer;

        private readonly Regex _regex = new(@"^(?<number>\d+(?:\.\d+)?)?(?:\.| ?-)? ?(?<title>.+)$",
            RegexOptions.Compiled);

        public int FindPrefixLength(IEnumerable<string> trackNames) {
            const char separator = '-';
            const int separatorLength = 1;

            var separatorIndexes = trackNames
                .Map(trackName => {
                    var match = _regex.Match(trackName);
                    return match.Groups["title"].Value;
                })
                .Map(trackTitle => trackTitle.IndexOf(separator))
                .ToArray();

            if (separatorIndexes.Distinct().Count() != 1 ||
                separatorIndexes[0] <= 0) {
                return 0;
            }

            return separatorIndexes[0] + separatorLength;
        }

        public Option<string> Normalize(string trackName, int trackNumber, int trackNumberLength, int prefixLength) =>
            _regexMatcher.Rename(_regex, trackName, match => {
                var title = match.Groups["title"].Value;
                var titleWithoutPrefix = title[prefixLength..].TrimStart();
                var normalizedNumber = _numberNormalizer.Normalize(trackNumber, trackNumberLength);
                return $"{normalizedNumber} {titleWithoutPrefix}";
            });
    }
}
