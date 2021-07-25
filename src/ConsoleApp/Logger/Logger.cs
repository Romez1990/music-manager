using System;
using Core.IocContainer;

namespace ConsoleApp.Logger {
    [Service]
    public class Logger : ILogger {
        public void Info(string input) =>
            Console.WriteLine(input);

        public void Error(Exception e) =>
            Console.WriteLine(e.Message);
    }
}
