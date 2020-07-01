using System;
using System.Collections.Immutable;

namespace Core.Operation
{
    public interface IOperationRegistry
    {
        ImmutableArray<IOperation> All();

        ImmutableArray<IOperation> Where(Func<IOperation, bool> predicate);
    }
}
