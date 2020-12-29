using System.Collections.Generic;

namespace Core.Operations
{
    public interface IOperationRepository
    {
        IEnumerable<IOperation> FindAll();
        IEnumerable<IOperation> FindAllByNames(IEnumerable<string> names);
    }
}
