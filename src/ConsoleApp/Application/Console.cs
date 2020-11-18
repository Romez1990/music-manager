using System;
using static System.Console;

namespace ConsoleApp.Application
{
    public class Console : IConsole
    {
        public void Print(string text)
        {
            WriteLine(text);
        }

        public void Error(Exception e)
        {
            Print($"Error: {e.Message}");
        }
    }
}
