using System;

namespace ConsoleApp.Logger {
    public interface ILogger {
        void Info(string input);
        void Error(Exception e);
    }
}
