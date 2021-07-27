using System.Collections.Generic;
using System.Linq;
using Core.FileSystemElement;
using LanguageExt;
using TagLib;
using static LanguageExt.Prelude;

namespace Core.Lyrics {
    public class Track : ITrack {
        public Track(IFileElement file) {
            _file = File.Create(file.Path);
        }

        private readonly File _file;

        public Option<IEnumerable<string>> Artists =>
            Optional(_file.Tag.AlbumArtists)
                .Map(albumArtists => (IEnumerable<string>)albumArtists);

        public Option<string> Title => _file.Tag.Title;

        public string Lyrics {
            get => _file.Tag.Lyrics;
            set => _file.Tag.Lyrics = value;
        }

        public void Save() =>
            _file.Save();
    }
}
