using Core.FileSystem;
using Core.FileSystem.Exceptions;
using Core.IocContainer;
using LanguageExt;

namespace Core.FileSystemElement {
    [Service]
    public class FsNodeElementFactory : IFsNodeElementFactory {
        public FsNodeElementFactory(IFsNodeFactory fsNodeFactory) {
            _fsNodeFactory = fsNodeFactory;
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        private const CheckState StartingCheckState = CheckState.Unchecked;
        private const bool StartingIsExpanded = false;

        public Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path) =>
            _fsNodeFactory.CreateDirectory(path)
                .Map(CreateDirectoryElementFromDirectory);

        public Either<FileNotFoundException, IFileElement> CreateFileElement(string path) =>
            _fsNodeFactory.CreateFile(path)
                .Map(CreateFileElementFromFile);

        public IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory) =>
            CreateDirectoryElementFromDirectory(directory, StartingCheckState, StartingIsExpanded,
                ChildrenRetrieval<IFsNodeElement>.Create());

        public IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory, CheckState checkState,
            bool isExpanded, ChildrenRetrieval<IFsNodeElement> childrenRetrieval) =>
            new DirectoryElement(this, directory, checkState, isExpanded, childrenRetrieval);

        public IFileElement CreateFileElementFromFile(IFile file) =>
            CreateFileElementFromFile(file, StartingCheckState);

        public IFileElement CreateFileElementFromFile(IFile file, CheckState checkState) =>
            new FileElement(this, file, checkState);
    }
}
