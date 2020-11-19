using LanguageExt;

namespace Core.Operations.Lyrics
{
    public interface ISong
    {
        Option<string> Artist { get; }
        Option<string> Title { get; }
        string Lyrics { set; }
        void Save();
    }
}
