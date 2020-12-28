using System.Text.RegularExpressions;

namespace Core.Renaming
{
    public class AlbumRenamer : IAlbumRenamer
    {
        private readonly Regex _albumRegex = new(@"(?<year>\d{4}) - (?:.+ - )?(?<name>.+)", RegexOptions.Compiled);

        public string RenameAlbumWithoutNumber(string albumName)
        {
            if (!_albumRegex.IsMatch(albumName))
                return albumName;

            return _albumRegex.Replace(albumName, "${name} (${year})");
        }

        public string RenameAlbumWithNumber(string albumName, int lengthOfNumber, int number)
        {
            var numberString = number.ToString().PadLeft(lengthOfNumber, '0');
            if (!_albumRegex.IsMatch(albumName))
                return albumName;

            return _albumRegex.Replace(albumName, numberString + " ${name} (${year})");
        }
    }
}
