using Core.IocContainer;

namespace ConsoleApp.Env {
    [Service]
    public class Env : IEnv {
        public string CurrentDirectory => System.Environment.CurrentDirectory;
    }
}
