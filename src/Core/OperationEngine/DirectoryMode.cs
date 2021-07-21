namespace Core.OperationEngine {
    public enum DirectoryMode {
        Album,
        Band,
        Compilation,
    }

    public static class ModeExtension {
        public static DirectoryMode Decrease(this DirectoryMode directoryMode) {
            var modeNumber = (int)directoryMode;
            var newNumber = modeNumber - 1;
            return (DirectoryMode)newNumber;
        }
    }
}
