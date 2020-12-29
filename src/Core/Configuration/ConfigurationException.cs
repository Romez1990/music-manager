using System;

namespace Core.Configuration
{
    public class ConfigurationException : Exception
    {
        protected ConfigurationException(string message) : base(message)
        {
        }
    }
}
