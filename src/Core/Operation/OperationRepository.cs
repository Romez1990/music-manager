using System.Collections.Generic;
using System.Linq;
using Core.IocContainer;

namespace Core.Operation {
    [Service]
    public class OperationRepository : IOperationRepository {
        public OperationRepository(
            RenameOperation rename,
            LyricsOperation lyrics,
            TagsOperation tags
        ) {
            _operations = new IOperation[] {
                rename,
                lyrics,
                tags,
            };
        }

        private readonly IReadOnlyList<IOperation> _operations;

        public IEnumerable<IOperation> FindAll() =>
            _operations;

        public IEnumerable<IOperation> FindByNames(IEnumerable<string> names) =>
            _operations.Where(operation => names.Contains(operation.Name))
                .ToArray();
    }
}
