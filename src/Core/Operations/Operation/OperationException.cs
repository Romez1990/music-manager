using System;

namespace Core.Operations.Operation
{
    public class OperationException : Exception
    {
        public OperationException(string message) : base(message)
        {
        }
    }
}
