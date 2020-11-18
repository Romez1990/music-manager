using System;

namespace ConsoleApp.Application
{
    public interface IConsole
    {
        void Print(string text);
        void Error(Exception e);
    }
}
