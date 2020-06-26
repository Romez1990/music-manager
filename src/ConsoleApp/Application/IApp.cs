namespace ConsoleApp.Application
{
    public interface IApp
    {
        void Configure(string[] args);

        int Run();
    }
}
