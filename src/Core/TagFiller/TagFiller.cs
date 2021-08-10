using Core.FileSystemElement;
using Core.NamingFormats;
using Core.OperationEngine;

namespace Core.TagFiller {
    public class TagFiller {
        public TagFiller(IAlbumNaming albumNaming, ITrackNaming trackNaming) {
            _albumNaming = albumNaming;
            _trackNaming = trackNaming;
        }

        private readonly IAlbumNaming _albumNaming;
        private readonly ITrackNaming _trackNaming;

        public void FillTags(IDirectoryElement directory, DirectoryMode directoryMode) {

        }

        private void FillTagsForAlbum(IDirectoryElement albumDirectory) {

        }
    }
}
