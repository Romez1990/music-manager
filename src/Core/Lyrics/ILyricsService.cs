using System.Collections.Generic;
using System.Threading.Tasks;
using Core.FileSystemElement;
using Core.OperationEngine;

namespace Core.Lyrics {
    public interface ILyricsService {
        Task<IReadOnlyList<LyricsException>> AddLyrics(IDirectoryElement directory,
            DirectoryMode directoryMode);
    }
}
