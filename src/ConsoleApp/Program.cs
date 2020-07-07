namespace ConsoleApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var containerBuilder = new AppContainerBuilder();
            var container = containerBuilder.Build();
            return 0;
        }
    }
}
