using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Actions;
using Core.FileSystem;
using Core.Scanners;
using Action = Core.Actions.Action;

namespace Core.Engines
{
    public class Engine : IEngine
    {
        public Engine(IFileSystemInfoFactory fileSystemInfoFactory, IFsNodeElementFactory fsNodeElementFactory,
            IScanner scanner, IActionsMapper actionsMapper)
        {
            _fileSystemInfoFactory = fileSystemInfoFactory;
            _fsNodeElementFactory = fsNodeElementFactory;
            _scanner = scanner;
            _actionsMapper = actionsMapper;
        }

        private readonly IFileSystemInfoFactory _fileSystemInfoFactory;

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        private readonly IScanner _scanner;

        private readonly IActionsMapper _actionsMapper;

        public IDirectoryElement DirectoryElement { get; private set; }

        private Mode _mode;

        public bool SetDirectory(string path)
        {
            var info = _fileSystemInfoFactory.CreateDirectoryInfo(path);
            var exists = info.Exists;

            if (exists)
            {
                DirectoryElement = _fsNodeElementFactory.CreateDirectoryElement(path);
            }

            return exists;
        }

        public void Scan(Mode mode)
        {
            if (DirectoryElement == null)
                throw new ApplicationException("Cannot scan before set directory");

            if (!DirectoryElement.FsNode.Exists)
                throw new DirectoryNotFoundException("Scanning directory not found");

            _mode = mode;
            _scanner.Scan(DirectoryElement, mode);
        }

        public void PerformActions(IEnumerable<Action> actions)
        {
            actions
                .ToList()
                .ForEach(action => _actionsMapper.Perform(action, DirectoryElement, _mode));
        }
    }
}
