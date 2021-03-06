﻿using Core.CoreEngine;
using Core.FileSystem;

namespace Core.Operations
{
    public interface IOperation
    {
        string Name { get; }
        string Description { get; }
        OperationResult Perform(IDirectoryElement directory, Mode mode);
    }
}
