using System;
using System.IO;
using Core.FileSystem.Exceptions;
using LanguageExt;
using SystemPath = System.IO.Path;
using SystemFileNotFoundException = System.IO.FileNotFoundException;

namespace Core.FileSystem {
    public class File : FsNodeBase<FileInfo>, IFile {
        public File(IFsNodeFactory fsNodeFactory, FileInfo info) : base(info) {
            _fsNodeFactory = fsNodeFactory;
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        public string Extension => Info.Extension;

        public override string ParentPath => Info.DirectoryName ?? throw new InvalidOperationException();

        public Either<FsException, IFile> Rename(string newName) {
            var newPath = SystemPath.Combine(ParentPath, newName);
            return MoveFileInfo(newPath, newName)
                .Map(newInfo => {
                    var newFile = _fsNodeFactory.CreateFileFromInfo(newInfo);
                    InvokeChanged(newFile);
                    return newFile;
                });
        }

        private Either<FsException, FileInfo> MoveFileInfo(string newPath, string newName) {
            var newInfo = new FileInfo(Path);
            try {
                newInfo.MoveTo(newPath);
                return newInfo;
            } catch (UnauthorizedAccessException e) {
                return e.Message switch {
                    "Access to the path is denied." => new FileAccessDeniedException(Name),
                    // TODO: have to log this exception
                    var message => new UnknownFileException(message),
                };
            } catch (SystemFileNotFoundException) {
                return new Exceptions.FileNotFoundException(Name);
            } catch (IOException e) {
                return e.Message switch {
                    "Cannot create a file when that file already exists." => new FileAlreadyExistsException(newName),
                    "The process cannot access the file because it is being used by another process." =>
                        new FileIsBeingUsedException(Name),
                    // TODO: have to log this exception
                    var message => new UnknownFileException(message),
                };
            }
        }

        public override string ToString() =>
            $"File {{ Name = {Name} }}";
    }
}
