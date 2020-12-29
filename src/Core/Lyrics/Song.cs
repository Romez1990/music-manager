using System.IO.Abstractions;
using LanguageExt;
using TagLib;

namespace Core.Lyrics
{
    public class Song : ISong
    {
        public Song(IFileInfo fileInfo)
        {
            _file = File.Create(fileInfo.ToString());
        }

        private readonly File _file;

        public Option<string> Artist =>
            _file.Tag.AlbumArtists is null ? Option<string>.None : _file.Tag.AlbumArtists[0];

        public Option<string> Title => _file.Tag.Title;

        public string Lyrics
        {
            set => _file.Tag.Lyrics = value;
        }

        public void Save()
        {
            _file.Save();
        }
    }
}
