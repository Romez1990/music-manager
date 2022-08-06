using LanguageExt;

namespace Core.NamingFormats {
    public interface IAlbumNaming {
        Option<string> NormalizeWithoutNumber(string albumName);
        Option<string> NormalizeWithNumber(string albumName, int albumNumber, int albumNumberLength);
    }
}
