using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Core.Operations
{
    public class OperationRepository : IOperationRepository
    {
        public OperationRepository(RenameOperation rename, LyricsOperation lyricsOperation)
        {
            _operations = ImmutableArray.Create(new IOperation[]
            {
                rename,
                lyricsOperation,
            });
        }

        private readonly ImmutableArray<IOperation> _operations;

        public IEnumerable<IOperation> FindAll() =>
            _operations;

        public IEnumerable<IOperation> FindAllByNames(IEnumerable<string> names) =>
            _operations.Where(operation => names.Contains(operation.Name));
    }
}
