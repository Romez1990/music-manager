using System.Collections.Generic;
using Core.FileSystem.Exceptions;
using Core.IocContainer;
using LanguageExt;
using static LanguageExt.Prelude;
using DirectoryInfo = System.IO.DirectoryInfo;
using FileInfo = System.IO.FileInfo;

namespace Core.FileSystem {
    [Service]
    public class FsNodeFactory : IFsNodeFactory {
        public FsNodeFactory(INaturalStringComparerFactory naturalStringComparerFactory) {
            _naturalStringComparer = naturalStringComparerFactory.Create();
        }

        private readonly IComparer<string> _naturalStringComparer;

        public Either<DirectoryNotFoundException, IDirectory> CreateDirectory(string path) {
            var directory = CreateDirectoryFromInfo(new DirectoryInfo(path), ChildrenRetrieval<IFsNode>.Create());
            return directory.Exists
                ? Right(directory)
                : Left(new DirectoryNotFoundException(directory.Name));
        }

        public Either<FileNotFoundException, IFile> CreateFile(string path) {
            var file = CreateFileFromInfo(new FileInfo(path));
            return file.Exists
                ? Right(file)
                : Left(new FileNotFoundException(file.Name));
        }

        public IDirectory CreateDirectoryFromInfo(DirectoryInfo info, ChildrenRetrieval<IFsNode> childrenRetrieval) =>
            new Directory(this, _naturalStringComparer, info, childrenRetrieval);

        public IFile CreateFileFromInfo(FileInfo info) =>
            new File(this, info);
    }
}
