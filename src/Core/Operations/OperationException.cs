using System;

namespace Core.Operations
{
    public class OperationException : Exception
    {
        public OperationException(string message) : base(message)
        {
        }
    }
}
