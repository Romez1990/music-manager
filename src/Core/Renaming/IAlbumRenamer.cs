namespace Core.Renaming
{
    public interface IAlbumRenamer
    {
        string RenameAlbumWithoutNumber(string albumName);
        string RenameAlbumWithNumber(string albumName, int lengthOfNumber, int number);
    }
}
