using Core.FileSystem;
using Core.FileSystem.Exceptions;
using Core.FileSystemElement.Exceptions;
using LanguageExt;

namespace Core.FileSystemElement {
    public class FileElement : FsNodeElementBase<IFile>, IFileElement {
        public FileElement(IFsNodeElementFactory fsNodeElementFactory, IFile file, CheckState checkState)
            : base(file, checkState) {
            _fsNodeElementFactory = fsNodeElementFactory;
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public string Extension => FsNode.Extension;

        public Either<FsException, IFileElement> Rename(string newName) =>
            FsNode.Rename(newName)
                .Map(newFile => {
                    var newFileElement = _fsNodeElementFactory.CreateFileElementFromFile(newFile);
                    InvokeChanged(newFileElement);
                    return newFileElement;
                });

        public IFileElement Uncheck(bool ignoreIfUnchecked = false) =>
            SetCheckState(CheckState.Unchecked, ignoreIfUnchecked);

        public IFileElement Check(bool ignoreIfChecked = false) =>
            SetCheckState(CheckState.Checked, ignoreIfChecked);

        public IFileElement SetCheckState(CheckState checkState, bool ignoreIfSameState = false) {
            if (checkState == CheckState) {
                if (!ignoreIfSameState)
                    throw new CheckStateException(checkState);
                return this;
            }

            var newFile = _fsNodeElementFactory.CreateFileElementFromFile(FsNode, checkState);
            InvokeChanged(newFile);
            return newFile;
        }

        public override string ToString() =>
            $"FileElement {{ Name = {Name}, CheckState = {CheckState} }}";
    }
}
