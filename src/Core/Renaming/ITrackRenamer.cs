namespace Core.Renaming
{
    public interface ITrackRenamer
    {
        string RenameTrack(string trackName, bool isTrackNumberOneDigit);
    }
}
