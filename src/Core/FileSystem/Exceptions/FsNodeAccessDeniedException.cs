namespace Core.FileSystem.Exceptions {
    public abstract class FsNodeAccessDeniedException : FsException {
        protected FsNodeAccessDeniedException(string fsNodeType, string fsNodeName)
            : base(GetMessage(fsNodeType, fsNodeName)) { }

        private static string GetMessage(string fsNodeType, string fsNodeName) =>
            $"Access denied for {fsNodeType} '{fsNodeName}'";
    }
}
