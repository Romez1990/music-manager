using System.Collections.Generic;

namespace Core.Operation {
    public interface IOperationRepository {
        IEnumerable<IOperation> FindAll();
        IEnumerable<IOperation> FindByNames(IEnumerable<string> names);
    }
}
