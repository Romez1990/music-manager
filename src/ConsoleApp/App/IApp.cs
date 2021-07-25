using System.Collections.Generic;

namespace ConsoleApp.App {
    public interface IApp {
        int Run(IEnumerable<string> args);
    }
}
