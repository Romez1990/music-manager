using Core.FileSystemElement;
using Core.IocContainer;
using Core.OperationEngine;

namespace Core.Operation {
    [Service(ToSelf = true)]
    public class TagsOperation : IOperation {
        public string Name => "Tags";

        public string Description => "Fills ID3 tags based on ";

        public OperationResult Perform(IDirectoryElement directory, DirectoryMode directoryMode) {
            throw new System.NotImplementedException();
        }
    }
}
