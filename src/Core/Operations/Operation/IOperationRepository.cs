using System.Collections.Generic;

namespace Core.Operations.Operation
{
    public interface IOperationRepository
    {
        IEnumerable<IOperation> FindAll();
        IEnumerable<IOperation> FindAllByNames(IEnumerable<string> names);
    }
}
