using System;
using System.Collections.Immutable;
using System.Linq;
using Core.FileRename;

namespace Core.Operation
{
    public class OperationRegistry : IOperationRegistry
    {
        private readonly ImmutableArray<IOperation> _operations = new ImmutableArray<IOperation>
        {
            new Rename(),
        };

        public ImmutableArray<IOperation> All()
        {
            return _operations;
        }

        public ImmutableArray<IOperation> Where(Func<IOperation, bool> predicate)
        {
            return _operations
                .Where(predicate)
                .ToImmutableArray();
        }
    }
}
