using System.Text.RegularExpressions;

namespace Core.Renaming
{
    public class TrackRenamer : ITrackRenamer
    {
        private readonly Regex _trackRegex =
            new(@"(?<number>\d{1,2})(?:\.| -)? (?<name>.+\.mp3)", RegexOptions.Compiled);


        public string RenameTrack(string trackName, bool isTrackNumberOneDigit)
        {
            var newName = _trackRegex.Replace(trackName, "${number} ${name}");
            if (isTrackNumberOneDigit)
                newName = newName.TrimStart('0');
            return newName;
        }
    }
}
