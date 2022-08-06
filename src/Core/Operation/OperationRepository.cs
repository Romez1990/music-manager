using System.Collections.Generic;
using System.Linq;
using Core.IocContainer;

namespace Core.Operation {
    [Service]
    public class OperationRepository : IOperationRepository {
        public OperationRepository(
            RenameOperation rename
        ) {
            _operations = new IOperation[] {
                rename,
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
