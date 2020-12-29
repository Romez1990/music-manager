using Core.FileSystem;

namespace Core.Lyrics
{
    public class SongFactory : ISongFactory
    {
        public SongFactory(IFsInfoFactory fsInfoFactory)
        {
            _fsInfoFactory = fsInfoFactory;
        }

        private readonly IFsInfoFactory _fsInfoFactory;

        public ISong CreateSong(IFileElement file)
        {
            var fileInfo = _fsInfoFactory.CreateFileInfo(file.Path);
            return new Song(fileInfo);
        }
    }
}
