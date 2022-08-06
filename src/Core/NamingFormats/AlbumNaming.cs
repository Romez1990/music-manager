using System;
using System.Text;
using System.Text.RegularExpressions;
using Core.IocContainer;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.NamingFormats {
    [Service]
    public class AlbumNaming : IAlbumNaming {
        public AlbumNaming(IRegexMatcher regexMatcher, INumberNormalizer numberNormalizer) {
            _regexMatcher = regexMatcher;
            _numberNormalizer = numberNormalizer;
        }

        private readonly IRegexMatcher _regexMatcher;
        private readonly INumberNormalizer _numberNormalizer;

        private readonly Regex _rawNameRegex = new(@"^(?<year>\d+) - (?:.+? - )?(?<title>.+)$");

        private readonly Regex _normalizedNameRegex = new(@"^(?<number>\d{1,3}(?:\.\d{1,3})?)?(?:\.| ?-)? ?(?<title>.+)");

        public Option<string> NormalizeWithoutNumber(string albumName) =>
            Match(albumName, (title, yearOption) => {
                var name = new StringBuilder(title);
                yearOption.IfSome(year => name.Append($" ({year})"));
                return name.ToString();
            });

        public Option<string> NormalizeWithNumber(string albumName, int albumNumber, int albumNumberLength) =>
            Match(albumName, (title, yearOption) => {
                var normalizedNumber = _numberNormalizer.Normalize(albumNumber, albumNumberLength);
                var name = new StringBuilder($"{normalizedNumber} {title}");
                yearOption.IfSome(year => name.Append($" ({year})"));
                return name.ToString();
            });

        private Option<string> Match(string albumName, Func<string, Option<string>, string> onMatch) {
            var methods = new Func<string, Func<string, Option<string>, string>, Option<string>>[] {
                MatchRawName,
                MatchNormalizedName,
            };
            return methods.Tail()
                .Fold(methods.Head()(albumName, onMatch),
                    (result, method) => result.Match(Some, () => method(albumName, onMatch)));
        }

        private Option<string> MatchRawName(string albumName, Func<string, Option<string>, string> onMatch) =>
            _regexMatcher.Rename(_rawNameRegex, albumName, match => {
                var title = match.Groups["title"].Value;
                var year = match.Groups["year"].Value;
                return onMatch(title, year);
            });

        private Option<string> MatchNormalizedName(string albumName, Func<string, Option<string>, string> onMatch) =>
            _regexMatcher.Rename(_normalizedNameRegex, albumName, match => {
                var title = match.Groups["title"].Value;
                return onMatch(title, None);
            });
    }
}
