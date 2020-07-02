using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class FsNodeElementFactory : IFsNodeElementFactory
    {
        public FsNodeElementFactory(IFsNodeFactory fsNodeFactory)
        {
            _fsNodeFactory = fsNodeFactory;
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        public IDirectoryElement CreateDirectoryElement(
            string path,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler)
        {
            return new DirectoryElement(this, _fsNodeFactory.InstantiateDirectory(path), checkStateChangeHandler);
        }

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler)
        {
            return new DirectoryElement(this, directory, checkStateChangeHandler);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler)
        {
            return new FileElement(file, checkStateChangeHandler);
        }
    }
}
