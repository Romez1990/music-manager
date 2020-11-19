using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Operations.Rename;

namespace Core.Operations.Operation
{
    public class OperationRepository : IOperationRepository
    {
        private readonly ImmutableArray<IOperation> _operations = ImmutableArray.Create(new IOperation[]
        {
            new RenameOperation(),
        });

        public IEnumerable<IOperation> FindAll()
        {
            return _operations;
        }

        public IEnumerable<IOperation> FindAllByNames(IEnumerable<string> names)
        {
            return _operations.Where(operation => names.Contains(operation.Name));
        }
    }
}
