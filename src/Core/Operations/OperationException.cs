using System;

namespace Core.Operations
{
    public class OperationException : Exception
    {
        protected OperationException(string message) : base(message)
        {
        }
    }
}
