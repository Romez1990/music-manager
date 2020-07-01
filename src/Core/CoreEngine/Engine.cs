using System;
using System.Collections.Generic;
using System.Linq;
using Core.FileScanner;
using Core.FileSystem;
using Core.Operation;

namespace Core.CoreEngine
{
    public class Engine : IEngine
    {
        public Engine(IFsNodeElementFactory fsNodeElementFactory, IScanner scanner)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            _scanner = scanner;
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        private readonly IScanner _scanner;

        public IDirectoryElement DirectoryElement { get; private set; }

        public bool SetDirectory(string path)
        {
            var directoryElement = _fsNodeElementFactory.CreateDirectoryElement(path);
            var exists = directoryElement.FsNode.Exists;

            if (exists)
            {
                DirectoryElement = directoryElement;
            }

            return exists;
        }

        private Mode _mode;

        public void Scan(Mode mode)
        {
            CheckDirectoryElement();

            _mode = mode;
            _scanner.Scan(DirectoryElement, mode);
        }

        public void PerformOperations(IEnumerable<IOperation> operations)
        {
            CheckDirectoryElement();
            CheckCheckState();

            operations
                .ToList()
                .ForEach(operation => operation.Perform(DirectoryElement, _mode));
        }

        private void CheckDirectoryElement()
        {
            if (DirectoryElement == null)
                throw new ApplicationException("Cannot scan before set directory");
        }

        private void CheckCheckState()
        {
            if (DirectoryElement.CheckState == CheckState.Unchecked)
                throw new ApplicationException("Directory is unchecked");
        }
    }
}
