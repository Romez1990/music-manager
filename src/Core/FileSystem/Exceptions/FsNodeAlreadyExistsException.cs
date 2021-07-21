using Utils.String;

namespace Core.FileSystem.Exceptions {
    public abstract class FsNodeAlreadyExistsException : FsException {
        protected FsNodeAlreadyExistsException(string fsNodeType, string newFsNodeName)
            : base(GetMessage(fsNodeType, newFsNodeName)) { }

        private static string GetMessage(string fsNodeType, string newFsNodeName) =>
            $"{fsNodeType.Capitalize()} '{newFsNodeName}' already exists";
    }
}
