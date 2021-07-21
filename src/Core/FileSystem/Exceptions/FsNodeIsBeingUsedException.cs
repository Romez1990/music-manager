using Utils.String;

namespace Core.FileSystem.Exceptions {
    public abstract class FsNodeIsBeingUsedException : FsException {
        protected FsNodeIsBeingUsedException(string fsNodeType, string fsNodeName)
            : base(GetMessage(fsNodeType, fsNodeName)) { }

        private static string GetMessage(string fsNodeType, string fsNodeName) =>
            $"{fsNodeType.Capitalize()} '{fsNodeName}' is being used by another process";
    }
}
