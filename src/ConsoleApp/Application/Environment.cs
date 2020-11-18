namespace ConsoleApp.Application
{
    public class Environment : IEnvironment
    {
        public string CurrentDirectory => System.Environment.CurrentDirectory;
    }
}
