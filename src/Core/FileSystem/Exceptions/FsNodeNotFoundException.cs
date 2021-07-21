using Utils.String;

namespace Core.FileSystem.Exceptions {
    public abstract class FsNodeNotFoundException : FsException {
        protected FsNodeNotFoundException(string fsNodeType, string fsNodeName)
            : base(GetMessage(fsNodeType, fsNodeName)) { }

        private static string GetMessage(string fsNodeType, string fsNodeName) =>
            $"{fsNodeType.Capitalize()} '{fsNodeName}' not found";
    }
}
